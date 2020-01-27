using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, GetBookDetailsResponse>();
            CreateMap<PostBooksRequest, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InInventory, opt => opt.MapFrom(_ => true));
            CreateMap<Book, BookSummaryItem>();
        }
    }
}
