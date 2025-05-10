namespace Library.ViewModel
{
    public class ReviewDisplayViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }  // إذا عندك اسم مستخدم
        public byte? Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
