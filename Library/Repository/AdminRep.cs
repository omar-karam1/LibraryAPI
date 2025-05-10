using Library.Models;
using Library.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Library.Repository
{
    public class AdminRep: IAdminRep
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminRep(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            
        }

        public async Task UploadBookAsync(UploadBookViewModel model)
        {
            IFormFile? file = model.BookFile;
            if (file == null || file.Length == 0)
                throw new Exception("No book file uploaded.");

           
            var folderName = model.IsFree == true ? "books/free" : "books/paid";
            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

         
            var uniqueBookFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var bookFilePath = Path.Combine(uploadsFolder, uniqueBookFileName);
            using (var stream = new FileStream(bookFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

          
            string? imagePath = null;
            if (model.BookImage != null && model.BookImage.Length > 0)
            {
                var imageFolder = Path.Combine(_env.WebRootPath, "books/images");
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                var uniqueImageName = Guid.NewGuid() + Path.GetExtension(model.BookImage.FileName);
                var imageFullPath = Path.Combine(imageFolder, uniqueImageName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await model.BookImage.CopyToAsync(stream);
                }

                imagePath = $"/books/images/{uniqueImageName}";
            }

           
            var book = new Book
            {
                Title = model.Title,
                CategoryId = (int)model.CategoryId,
                Price = model.Price ?? 0,
                IsFree = !(model.IsFree ?? false),
                FilePath = $"/{folderName}/{uniqueBookFileName}",
                CreatedDate = DateTime.Now,
                BookImage = imagePath,
                BookDescription = model.BookDescription
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> UpdateBookAsync(int bookId, UploadBookViewModel model)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
                return false;

            if (!string.IsNullOrEmpty(model.Title))
                book.Title = model.Title;

            if (model.CategoryId > 0)
                book.CategoryId = (int)model.CategoryId;

            if (model.Price.HasValue)
                book.Price = model.Price;

            if (model.IsFree.HasValue)
                book.IsFree = model.IsFree.Value;

            if (!string.IsNullOrEmpty(model.BookDescription))
                book.BookDescription = model.BookDescription;

            if (model.BookImage != null && model.BookImage.Length > 0)
            {
              
                if (!string.IsNullOrEmpty(book.BookImage))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, book.BookImage.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                        File.Delete(oldImagePath);
                }

                var imageFolder = Path.Combine(_env.WebRootPath, "books/images");
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                var uniqueImageName = Guid.NewGuid() + Path.GetExtension(model.BookImage.FileName);
                var newImagePath = Path.Combine(imageFolder, uniqueImageName);
                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await model.BookImage.CopyToAsync(stream);
                }

                book.BookImage = $"/books/images/{uniqueImageName}";
            }

    
            if (model.BookFile != null && model.BookFile.Length > 0)
            {
            
                if (!string.IsNullOrEmpty(book.FilePath))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, book.FilePath.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                var folderName = (bool)book.IsFree ? "books/free" : "books/paid";
                var bookFolder = Path.Combine(_env.WebRootPath, folderName);
                if (!Directory.Exists(bookFolder))
                    Directory.CreateDirectory(bookFolder);

                var uniqueBookFileName = Guid.NewGuid() + Path.GetExtension(model.BookFile.FileName);
                var newBookPath = Path.Combine(bookFolder, uniqueBookFileName);
                using (var stream = new FileStream(newBookPath, FileMode.Create))
                {
                    await model.BookFile.CopyToAsync(stream);
                }

                book.FilePath = $"/{folderName}/{uniqueBookFileName}";
            }

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }
        public async Task<bool> AddNewCategorieAsync(string name)
        {
            Category c = new Category();
            c.CategoryName = name;
            await _context.Categories.AddAsync(c);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddNewAdminAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return false;

            user.UserType = "Admin";
            await _context.SaveChangesAsync(); 
            return true;
        }


        public async Task<string> SaveAuthorImageAsync(IFormFile profileImage)
        {
            if (profileImage == null)
                return "/image/Authors/Author.jpg";

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profileImage.FileName)}";
            var filePath = Path.Combine("wwwroot/image/Authors", fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }


            return $"/image/Authors/{fileName}";
        }


        public async Task<bool> AddNewAuthorAsync(AuthorٍViewModel model)
        {

            var imagePath = await SaveAuthorImageAsync(model.AuthorImage);
               
                var author = new Author
                {
                    Name = model.Name,
                    AboutAuthor = model.AboutAuthor,
                    AuthorImage = imagePath
                };

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return true;
            
           
        }

        public async Task<bool> UpdateAuthorAsync(int authorId, AuthorٍViewModel model)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
            if (author == null) return false;

           
            if (!string.IsNullOrWhiteSpace(model.Name))
                author.Name = model.Name;

            
            if (!string.IsNullOrWhiteSpace(model.AboutAuthor))
                author.AboutAuthor = model.AboutAuthor;

           
            if (model.AuthorImage != null)
            {
                string image =await SaveAuthorImageAsync(model.AuthorImage);
                     author.AuthorImage = image;
            }

            _context.Authors.Update(author);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> AddAuthorsToBookAsync(AddAuthorsToBookViewModel model)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.BookId == model.BookId);

            if (book == null) return false;

            var authors = await _context.Authors
                .Where(a => model.AuthorIds.Contains(a.AuthorId))
                .ToListAsync();

            foreach (var author in authors)
            {
                if (!book.Authors.Contains(author)) 
                    book.Authors.Add(author);
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<BookAuthorLinkViewModel>> GetAllBookAuthorLinksAsync()
        {
            return await _context.Books
                .Include(b => b.Authors)
                .SelectMany(b => b.Authors.Select(a => new BookAuthorLinkViewModel
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    AuthorId = a.AuthorId,
                    AuthorName = a.Name
                }))
                .ToListAsync();
        }



        public async Task<bool> UpdateCategoryAsync(int id, string NewCategory)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return false;

            category.CategoryName = NewCategory;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return false;

        
            if (!string.IsNullOrEmpty(book.BookImage))
            {
                var imageFullPath = Path.Combine(_env.WebRootPath, book.BookImage.TrimStart('/'));
                if (File.Exists(imageFullPath))
                    File.Delete(imageFullPath);
            }

      
            if (!string.IsNullOrEmpty(book.FilePath))
            {
                var fileFullPath = Path.Combine(_env.WebRootPath, book.FilePath.TrimStart('/'));
                if (File.Exists(fileFullPath))
                    File.Delete(fileFullPath);
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Author?> GetAuthorByIdAsync(int id )
        {
            var authors = await _context.Authors.FirstOrDefaultAsync(a=>a.AuthorId==id);
            return authors;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
                return false;
            if(!string.IsNullOrEmpty(author.AuthorImage))
            File.Delete(author.AuthorImage);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAuthorFromBookAsync(int bookId, int authorId)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null) return false;

            var author = book.Authors.FirstOrDefault(a => a.AuthorId == authorId);
            if (author == null) return false;

            book.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return true;
        }




    }
}
