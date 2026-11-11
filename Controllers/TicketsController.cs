using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTicketMVC.Data;
using MovieTicketMVC.Models;

namespace MovieTicketMVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // DANH SÁCH VÉ
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Movie)
                .OrderByDescending(t => t.ShowDate)
                .ToListAsync();

            return View(tickets);
        }

        // NẠP DANH SÁCH PHIM VÀO DROPDOWN
        private async Task LoadMovies(int? selectedId = null)
        {
            var movies = await _context.Movies
                .OrderBy(m => m.Title)
                .ToListAsync();

            ViewBag.Movies = new SelectList(
                movies,
                "Id",
                "Title",
                selectedId
            );
        }

        // HIỂN THỊ FORM THÊM VÉ
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadMovies();

            var ticket = new Ticket
            {
                ShowDate = DateTime.Now,
                SeatCount = 1
            };

            return View(ticket);
        }

        // LƯU VÉ MỚI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ShowDate,CustomerName,CustomerPhone,MovieId,SeatCount")]
            Ticket ticket)
        {
            bool movieExists = await _context.Movies
                .AnyAsync(m => m.Id == ticket.MovieId);

            if (!movieExists)
            {
                ModelState.AddModelError(
                    "MovieId",
                    "Phim được chọn không tồn tại"
                );
            }

            if (ModelState.IsValid)
            {
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm vé thành công!";

                return RedirectToAction(nameof(Index));
            }

            await LoadMovies(ticket.MovieId);
            return View(ticket);
        }

        // HIỂN THỊ FORM CHỈNH SỬA
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _context.Tickets
                .FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            await LoadMovies(ticket.MovieId);

            return View(ticket);
        }

        // CẬP NHẬT VÉ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,ShowDate,CustomerName,CustomerPhone,MovieId,SeatCount")]
            Ticket input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            bool movieExists = await _context.Movies
                .AnyAsync(m => m.Id == input.MovieId);

            if (!movieExists)
            {
                ModelState.AddModelError(
                    "MovieId",
                    "Phim được chọn không tồn tại"
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadMovies(input.MovieId);
                return View(input);
            }

            var ticket = await _context.Tickets
                .FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.ShowDate = input.ShowDate;
            ticket.CustomerName = input.CustomerName;
            ticket.CustomerPhone = input.CustomerPhone;
            ticket.MovieId = input.MovieId;
            ticket.SeatCount = input.SeatCount;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật vé thành công!";

            return RedirectToAction(nameof(Index));
        }

        // TRANG XÁC NHẬN XÓA
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Movie)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // XÓA VÉ KHỎI DATABASE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets
                .FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa vé thành công!";

            return RedirectToAction(nameof(Index));
        }
    }
}