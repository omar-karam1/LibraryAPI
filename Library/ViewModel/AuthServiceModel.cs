namespace Library.ViewModel
{
    public class AuthServiceModel
    {
        public bool IsAuthenticated { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public string? Roles { get; set; }
        public string ProfileImageUrl { get; set; }
        public int id { get; set; }
    }
}
