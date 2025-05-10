namespace Library.ViewModel
{
    public class BookDetailsViewModel
    {
        public string Title { get; set; } = null!;
        public bool? IsFree { get; set; }
        public string? BookFile { get; set; }
        public string? BookImage { get; set; }
        public string? BookDescription { get; set; }
        public int CategoryId { get; set; }

        public List<AuthorViewModel> Authors { get; set; } = new();
    }

    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = null!;
        public string? AuthorImage { get; set; }
        public string? AboutAuthor { get; set; }
    }

}
