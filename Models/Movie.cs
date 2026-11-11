using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace MovieTicketMVC.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên phim")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Tên phim phải từ 2 đến 200 ký tự")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá vé")]
        [Range(typeof(decimal), "0", "9999999999999999",
            ErrorMessage = "Giá vé phải lớn hơn hoặc bằng 0")]
        public decimal BasePrice { get; set; }

        [Required]
        [Range(30, 300,
            ErrorMessage = "Thời lượng phải từ 30 đến 300 phút")]
        public int DurationMinutes { get; set; }

        public string? Genre { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
            = new List<Ticket>();
    }
}