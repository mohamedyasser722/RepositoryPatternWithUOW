using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Api.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IBaseRepository<Author> _AuthorsRepository;

        public AuthorsController(IBaseRepository<Author> AuthorsRepository)
        {
            _AuthorsRepository = AuthorsRepository;
        }

        // Synchronous GET by ID
        [HttpGet("GetById/id")]
        public IActionResult GetById(int id)
        {
            var author = _AuthorsRepository.GetById(id);
            return author == null ? NotFound() : Ok(author);
        }

        // Asynchronous GET by ID
        [HttpGet("GetByIdAsync/id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var author = await _AuthorsRepository.GetByIdAsync(id);
            return author == null ? NotFound() : Ok(author);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var authors = _AuthorsRepository.GetAll();
            return Ok(authors);
        }
    }
}
