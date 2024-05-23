using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Api.Interfaces;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Consts;
using RepositoryPatternWithUOW.Core.Dtos;
using RepositoryPatternWithUOW.Core.Models;
using System.Linq.Expressions;

namespace RepositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //private readonly IBaseRepository<Book> _BooksRepository;

        private readonly IUnitOfWork _unitOfWork;

        //public BooksController(IBaseRepository<Book> BooksRepository)
        //{
        //    _BooksRepository = BooksRepository;
        //}

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetById/id")]
        public IActionResult GetById(int id)
        {
            var book = _unitOfWork.Books.GetById(id);

            return book == null ? NotFound() : Ok(book);
        }

        //Api/Controllers/GetAll ---> i want to take this route
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var books = _unitOfWork.Books.GetAll();

            return Ok(books);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name ,[FromQuery] string[] includes)
        {
            var book = _unitOfWork.Books.Find(b => b.Title == name, includes);

            return book == null ? NotFound() : Ok(book);
        }

        [HttpGet("GetAllWithAuthors")]
        public IActionResult GetAllWithAuthors(string name, [FromQuery] string[] includes)
        {
            var book = _unitOfWork.Books.FindAll(b => b.Title.Contains(name), includes);

            return book == null ? NotFound() : Ok(book);
        }

        [HttpGet("GetOrdered")]
        public IActionResult GetOrdered(int? skip, int? take, string orderBy = "Id", string orderByDirection = OrderBy.Ascending, [FromQuery]string[] includes = null)
        {
            Expression<Func<Book, object>> orderByExpression = GetOrderByExpression(orderBy);
            var books = _unitOfWork.Books.FindAll(b => true, take, skip, orderByExpression, orderByDirection,includes);

            return books == null ? NotFound() : Ok(books);
        }

        private Expression<Func<Book, object>> GetOrderByExpression(string orderBy)
        {
            switch (orderBy.ToLower())
            {
                case "title":
                    return b => b.Title;
                case "author":
                    return b => b.Author;
                // Default case
                case "id":
                default:
                    return b => b.BookId;
            }
        }

        [HttpPost("AddOne")]
        public IActionResult AddOne([FromQuery] BookDto book)
        {
            Book newBook = new Book
            {
                Title = book.Title,
                AuthorId = book.AuthorId
            };

            var AddedBook = _unitOfWork.Books.Add(newBook);

            return AddedBook == null ? NotFound() : Ok(newBook);
        }

        [HttpPost("AddMany")]
        public IActionResult AddMany(IEnumerable<BookDto> books)
        {
            if (books == null || !books.Any())
            {
                return BadRequest("Books collection is null or empty.");
            }

            var newBooks = books.Select(book => new Book
            {
                Title = book.Title,
                AuthorId = book.AuthorId
            }).ToList();

            var addedBooks = _unitOfWork.Books.AddRange(newBooks);

            if (addedBooks == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding books to the database.");
            }

            return Ok(addedBooks);
        }

    }
}

