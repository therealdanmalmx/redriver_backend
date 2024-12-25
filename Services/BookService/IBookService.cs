using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using redriver_backend.Models;
using redriver_test.controllers;
using redriver_test.Dtos.Book;
using redriver_test.models;

namespace redriver_test.Services.BookService
{
    public interface IBookService
    {
        Task<ServiceResponse<List<GetBookDto>>> GetAllBooks();
        Task<ServiceResponse<GetBookDto>> GetSingleBook(Guid id);
        Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto newBook);
        Task<ServiceResponse<GetBookDto>> UpdateBook(Guid id, UpdateBookDto updatedBook);
        Task<ServiceResponse<List<GetBookDto>>> DeleteBook(Guid id);
    }
}