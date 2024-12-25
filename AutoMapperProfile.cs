using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using redriver_test.Dtos.Book;
using redriver_test.models;

namespace redriver_test
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Book
            CreateMap<BookModel, GetBookDto>();
            CreateMap<AddBookDto, BookModel>();
            CreateMap<UpdateBookDto, BookModel>();


            // Quotes
        }
    }
}