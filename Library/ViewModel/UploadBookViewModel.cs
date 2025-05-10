namespace Library.ViewModel
{
    public class UploadBookViewModel
    {
        public string? Title { get; set; } = null!;

        public int? CategoryId { get; set; }

        public decimal? Price { get; set; }

        public bool? IsFree { get; set; }

        public IFormFile? BookFile { get; set; }

        public IFormFile? BookImage { get; set; }

        public string? BookDescription { get; set; }
    }
}
