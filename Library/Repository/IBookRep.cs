using Library.Models;
using Library.ViewModel;

namespace Library.Repository
{
    public interface IBookRep
    {
       
        Task<List<BookViewModel>> GetAllBooksAsync();
        Task<List<Author>> GetAllAuthorsAsync();
        Task<List<BookViewModel>> GetBooksByCategoryAsync(int categoryId);
        Task<List<BookViewModel>> GetBooksByAuthorIdAsync(int authorId);
        Task<BookDetailsViewModel?> GetBookDetailsWithAuthorsAsync(int bookId);
        Task<bool> AddReviewAsync(ReviewViewModel model);
        Task<List<ReviewDisplayViewModel>> GetReviewsForBookAsync(int bookId);
    }
}
