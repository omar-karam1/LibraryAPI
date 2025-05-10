using Library.Models;
using Library.Repository;
using Library.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {


        private readonly IAdminRep _AdminRep;
        public AdminController(IAdminRep _AdminRep)
        {
            this._AdminRep = _AdminRep;
        }

        [HttpPost("upload-book")]
        public async Task<IActionResult> UploadBook([FromForm] UploadBookViewModel model)
        {
            try
            {
                if (model == null || model.BookImage == null || model.BookImage.Length == 0)
                    return BadRequest("Please fill all required fields and upload an image.");


                if (model.BookFile == null)
                    return BadRequest("Please upload the book file.");

                await _AdminRep.UploadBookAsync(model);
                return Ok("Book uploaded successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-book/{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromForm] UploadBookViewModel model)
        {
           
        

            var result = await _AdminRep.UpdateBookAsync(bookId,model);
            if (!result)
                return NotFound("Book not found.");

            return Ok("Book updated successfully.");
        }

        [HttpDelete("delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _AdminRep.DeleteBookAsync(id);
            if (!result)
                return NotFound("book not found.");

            return Ok("Deleted successfully.");
        }

        [HttpPost("AddAddAuthor")]
        public async Task<IActionResult> AddAddAuthor([FromForm] AuthorٍViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            var result = await _AdminRep.AddNewAuthorAsync(model);

            if (!result)
                return StatusCode(500, "Failed to add author");

            return Ok("Author added successfully");
        }

        [HttpGet("AuthorByID")]
        public async Task<IActionResult> AuthorByID(int id)
        {
            var authors = await _AdminRep.GetAuthorByIdAsync(id);
            return Ok(authors);
        }

        [HttpPut("update-author/{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromForm] AuthorٍViewModel model)
        {
            var result = await _AdminRep.UpdateAuthorAsync(id, model); 

            if (!result)
                return NotFound("المؤلف غير موجود أو لم يتم إرسال أي بيانات للتعديل.");

            return Ok("تم تعديل بيانات المؤلف بنجاح.");
        }

        [HttpDelete("delete-author/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _AdminRep.DeleteAuthorAsync(id);

            if (!result)
                return NotFound("المؤلف غير موجود.");

            return Ok("تم حذف المؤلف بنجاح.");
        }


        [HttpPost("AddAuthorsToBook")]
        public async Task<IActionResult> AddAuthorsToBook([FromBody] AddAuthorsToBookViewModel model)
        {
            var result = await _AdminRep.AddAuthorsToBookAsync(model);
            if (!result)
                return NotFound("Book not found or invalid authors.");

            return Ok("Authors added to book successfully.");
        }

        [HttpGet("all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _AdminRep.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost("AddNewCategorie")]
        public async Task<ActionResult> AddNewCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            bool isSaved = await _AdminRep.AddNewCategorieAsync(categoryName);

            if (!isSaved)
            {
                return BadRequest("Failed to save the category.");
            }

            return Ok("Category saved successfully.");
        }

        [HttpPut("AddNewAdmin")]
        public async Task<ActionResult> AddNewAdmin(int UserId)
        {
            if (!await _AdminRep.AddNewAdminAsync(UserId))
                return BadRequest("Failed to Add the Admin");
            return Ok("Admin added");
        }


        


        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory(int CategoryID, string NewCategory)
        {
            var result = await _AdminRep.UpdateCategoryAsync(CategoryID, NewCategory);

            if (!result)
                return BadRequest("The category could not be updated.");

            return Ok("Updated successfully.");
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _AdminRep.DeleteCategoryAsync(id);
            if (!result)
                return NotFound("category not found.");

            return Ok("Deleted successfully.");
        }


        [HttpGet("all-links")]
        public async Task<IActionResult> GetAllBookAuthorLinks()
        {
            var links = await _AdminRep.GetAllBookAuthorLinksAsync();
            return Ok(links);
        }

        [HttpDelete("unlink")]
        public async Task<IActionResult> RemoveAuthorFromBook(int bookId, int authorId)
        {
            var result = await _AdminRep.RemoveAuthorFromBookAsync(bookId, authorId);
            if (!result)
                return NotFound("لم يتم العثور على الكتاب أو المؤلف.");

            return Ok("تم إلغاء الربط بنجاح.");
        }


    }
}
