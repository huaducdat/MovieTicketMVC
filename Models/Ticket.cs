using System.ComponentModel.DataAnnotations;

namespace MovieTicketMVC.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày chiếu")]
        [DataType(DataType.DateTime)]
        public DateTime ShowDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Tên phải từ 3 đến 100 ký tự")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(0[0-9]{9}|\+84[0-9]{9})$",
            ErrorMessage = "Số điện thoại Việt Nam không hợp lệ")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue,
            ErrorMessage = "Vui lòng chọn phim")]
        public int MovieId { get; set; }

        [Required]
        [Range(1, 10,
            ErrorMessage = "Số ghế phải từ 1 đến 10")]
        public int SeatCount { get; set; } = 1;

        public Movie? Movie { get; set; }
    }
}