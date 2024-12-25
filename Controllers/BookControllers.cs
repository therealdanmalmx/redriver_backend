using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using redriver_test.Dtos.Book;
using redriver_test.models;
using redriver_test.Services.BookService;

namespace redriver_test.controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetBookDto>>> Get()
        {
            return Ok(await _bookService.GetAllBooks());
        }

        [HttpPost]
        public async Task<ActionResult<List<GetBookDto>>> Post(AddBookDto book)
        {
            return Ok(await _bookService.AddBook(book));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDto>> Get(Guid id)
        {
            return Ok(await _bookService.GetSingleBook(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetBookDto>> Put(Guid id, UpdateBookDto updatedBook)
        {
            return Ok(await _bookService.UpdateBook(id, updatedBook));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<GetBookDto>>> Delete(Guid id)
        {
            return Ok(await _bookService.DeleteBook(id));
        }
    }
}