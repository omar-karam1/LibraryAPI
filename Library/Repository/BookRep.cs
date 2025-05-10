using Library.Models;
using Library.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Library.Repository
{
    public class BookRep: IBookRep
    {
        private readonly AppDbContext _context;
      

        public BookRep(AppDbContext context)
        {
            _context = context;
            
        }

       


        public async Task<List<BookViewModel>> GetAllBooksAsync()
        {
            var books = await _context.Books
                .Select(b => new BookViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    IsFree = b.IsFree,
                    BookFile = b.FilePath,
                    BookImage = b.BookImage,
                    BookDescription = b.BookDescription,
                    CategoryId = b.CategoryId,
                    AverageRating = b.Reviews.Any(r => r.Rating.HasValue)
                        ? b.Reviews.Where(r => r.Rating.HasValue).Average(r => (double)r.Rating.Value)
                        : 0
                })
                .ToListAsync();

            return books;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors.ToListAsync();
            return authors;
        }

        public async Task<List<BookViewModel>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .Where(b => b.CategoryId == categoryId)
                .Select(b => new BookViewModel
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    IsFree = b.IsFree,
                    BookFile = b.FilePath,
                    BookImage = b.BookImage,
                    BookDescription = b.BookDescription,
                    CategoryId = b.CategoryId
                })
                .ToListAsync();
        }


        public async Task<List<BookViewModel>> GetBooksByAuthorIdAsync(int authorId)
        {
            var books = await _context.Authors
                .Where(a => a.AuthorId == authorId)
                .SelectMany(a => a.Books) 
                .Select(book => new BookViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    IsFree = book.IsFree,
                    BookFile = book.FilePath,
                    BookImage = book.BookImage,
                    BookDescription = book.BookDescription,
                    CategoryId = book.CategoryId
                  
                })
                .ToListAsync();

            return books;
        }


        public async Task<BookDetailsViewModel?> GetBookDetailsWithAuthorsAsync(int bookId)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .Where(b => b.BookId == bookId)
                .Select(b => new BookDetailsViewModel
                {
                    Title = b.Title,
                    IsFree = b.IsFree,
                    BookFile = b.FilePath,
                    BookImage = b.BookImage,
                    BookDescription = b.BookDescription,
                    CategoryId = b.CategoryId,
                    Authors = b.Authors.Select(a => new AuthorViewModel
                    {
                        AuthorId = a.AuthorId,
                        Name = a.Name,
                        AuthorImage = a.AuthorImage,
                        AboutAuthor = a.AboutAuthor
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return book;
        }


        public async Task<bool> AddReviewAsync(ReviewViewModel model)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == model.UserId);
            if (!userExists)
            {
                return false; 
            }

            var review = new Review
            {
                BookId = model.BookId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText,
                CreatedDate =  DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<List<ReviewDisplayViewModel>> GetReviewsForBookAsync(int bookId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.User) 
                .Select(r => new ReviewDisplayViewModel
                {
                    UserId = r.UserId,
                    UserName = r.User.Name,  
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    CreatedDate = (DateTime)r.CreatedDate
                })
                .ToListAsync();

            return reviews;
        }


    }
}
