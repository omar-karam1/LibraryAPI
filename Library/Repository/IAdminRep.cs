using Library.Models;
using Library.ViewModel;

namespace Library.Repository
{
    public interface IAdminRep
    {
        Task UploadBookAsync(UploadBookViewModel model);
        Task<bool> UpdateBookAsync(int BookId, UploadBookViewModel model);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<bool> AddNewCategorieAsync(string name);
        Task<bool> AddNewAdminAsync(int id);
        Task<bool> AddNewAuthorAsync(AuthorٍViewModel model);
        Task<bool> AddAuthorsToBookAsync(AddAuthorsToBookViewModel model);

        Task<bool> UpdateCategoryAsync(int id, string NewCategory);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> DeleteBookAsync(int id);
        Task<bool> UpdateAuthorAsync(int authorId, AuthorٍViewModel model);
        Task<bool> DeleteAuthorAsync(int id);
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<List<BookAuthorLinkViewModel>> GetAllBookAuthorLinksAsync();
        Task<bool> RemoveAuthorFromBookAsync(int bookId, int authorId);
    }
}
