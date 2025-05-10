using Library.Repository;
using Library.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRep _bookRep;
        public BookController(IBookRep bookRepository)
        {
            _bookRep = bookRepository;
        }


        [HttpGet("allBooks")]
        public async Task<ActionResult<List<BookViewModel>>> GetAllBooks()
        {
            var books = await _bookRep.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _bookRep.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<List<BookViewModel>>> GetByCategory(int categoryId)
        {
            var books = await _bookRep.GetBooksByCategoryAsync(categoryId);
            if (books == null || !books.Any())
                return NotFound($"No books found in category {categoryId}.");
            return Ok(books);
        }

        [HttpGet("by-author/{authorId}")]
        public async Task<ActionResult<List<BookViewModel>>> GetBooksByAuthor(int authorId)
        {
            var books = await _bookRep.GetBooksByAuthorIdAsync(authorId);
            if (books == null || !books.Any())
                return NotFound($"No books found for author with ID {authorId}.");
            return Ok(books);
        }


        [HttpGet("details/{bookId}")]
        public async Task<ActionResult<BookDetailsViewModel>> GetBookDetailsWithAuthors(int bookId)
        {
            var bookDetails = await _bookRep.GetBookDetailsWithAuthorsAsync(bookId);

            if (bookDetails == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(bookDetails);
        }

        [Authorize]
        [HttpPost("add-review")]
        public async Task<IActionResult> AddReview([FromBody] ReviewViewModel model)
        {
            var result = await _bookRep.AddReviewAsync(model);

            if (!result)
                return BadRequest(new { message = "Failed to add review" });

            return Ok(new { message = "Review added successfully" });
        }

        [HttpGet("reviews/{bookId}")]
        public async Task<IActionResult> GetReviewsForBook(int bookId)
        {
            var reviews = await _bookRep.GetReviewsForBookAsync(bookId);

            if (reviews == null || !reviews.Any())
                return NotFound(new { message = "No reviews found for this book" });

            return Ok(reviews);
        }

    }



}
