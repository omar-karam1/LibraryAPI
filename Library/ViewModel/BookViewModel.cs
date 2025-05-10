namespace Library.ViewModel
{
    public class BookViewModel
    {
        public int BookId {  get; set; }    
        public string Title { get; set; } = null!;


        public bool? IsFree { get; set; }

        public string? BookFile { get; set; }

        public string? BookImage { get; set; }

        public string? BookDescription { get; set; }
        public int CategoryId { get; set; }
        public double AverageRating { get; set; }
    }
}
