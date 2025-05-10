namespace Library.ViewModel
{
    public class AddAuthorsToBookViewModel
    {
        public int BookId { get; set; }
        public List<int> AuthorIds { get; set; } = new();
    }
}
