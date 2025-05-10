namespace Library.ViewModel
{
    public class AuthorٍViewModel
    {
        public string Name { get; set; } = null!;

        public IFormFile? AuthorImage { get; set; }

        public string? AboutAuthor { get; set; }
    }
}
