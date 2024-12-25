using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using redriver_backend.Models;
using redriver_test.Data;
using redriver_test.Dtos.Book;
using redriver_test.models;

namespace redriver_test.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _db;

        public BookService(IMapper mapper, DataContext db)
        {
            _mapper = mapper;
            _db = db;
        }
        public async Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto newBook)
        {

            var serviceResponse = new ServiceResponse<List<GetBookDto>>();

            try
            {
                _db.Books.Add(_mapper.Map<BookModel>(newBook));
                await _db.SaveChangesAsync();


                serviceResponse.Data = await _db.Books.Select(b => _mapper.Map<GetBookDto>(b)).ToListAsync();
                serviceResponse.Message = $"{newBook.Title} added successfully";

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> DeleteBook(Guid id)
        {
            var serviceResponse = new ServiceResponse<List<GetBookDto>>();

            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Book with id: {id} not found";
            }
            else
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
                serviceResponse.Data = _db.Books.Select(propertyManager => _mapper.Map<GetBookDto>(propertyManager)).ToList();
            }
            serviceResponse.Data = _db.Books.Select(propertyManager => _mapper.Map<GetBookDto>(book)).ToList();


            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> GetAllBooks()
        {
            var serviceResponse = new ServiceResponse<List<GetBookDto>>();

            var dbBooks = await _db.Books.ToListAsync();
            var bookList = _mapper.Map<List<GetBookDto>>(dbBooks);
            serviceResponse.Data = bookList;
            return serviceResponse;
        }


        public async Task<ServiceResponse<GetBookDto>> GetSingleBook(Guid id)
        {
           var serviceResponse = new ServiceResponse<GetBookDto>();
            var dBBook = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
            serviceResponse.Data = _mapper.Map<GetBookDto>(dBBook);

            if (dBBook == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Book not found";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetBookDto>> UpdateBook(Guid id, UpdateBookDto updatedBook)
        {
            var serviceResponse = new ServiceResponse<GetBookDto>();

            var bookToUpdate = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            try
            {
                if(!string.IsNullOrEmpty(updatedBook.Title))
                {
                    bookToUpdate!.Title = updatedBook.Title;
                }
                if(!string.IsNullOrEmpty(updatedBook.Author))
                {
                    bookToUpdate!.Author = updatedBook.Author;
                }
                if(!string.IsNullOrEmpty(updatedBook.Genre))
                {
                    bookToUpdate!.Genre = updatedBook.Genre;
                }
                if(!string.IsNullOrEmpty(updatedBook.Publisher))
                {
                    bookToUpdate!.Publisher = updatedBook.Publisher;
                }
                if (!string.IsNullOrEmpty(updatedBook.PublishDate))
                {
                    bookToUpdate!.PublishDate = updatedBook.PublishDate;
                }
                if(!string.IsNullOrEmpty(updatedBook.Description))
                {
                    bookToUpdate!.Description = updatedBook.Description;
                }
                if(!string.IsNullOrEmpty(updatedBook.ISBN))
                {
                    bookToUpdate!.ISBN = updatedBook.ISBN;
                }
                if(!string.IsNullOrEmpty(updatedBook.Language))
                {
                    bookToUpdate!.Language = updatedBook.Language;
                }
                if(updatedBook.Pages != 0)
                {
                    bookToUpdate!.Pages = updatedBook.Pages;
                }

                serviceResponse.Data = _mapper.Map<GetBookDto>(bookToUpdate);
                serviceResponse.Message = $"{bookToUpdate!.Title} updated successfully";
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}