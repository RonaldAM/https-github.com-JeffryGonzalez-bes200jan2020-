using LibraryApi.Domain;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace LibraryApi.Mappers
{
    public class EfBookMapper : IMapBooks
    {
        LibraryDataContext Context;
        IMapper Mapper;

        public EfBookMapper(LibraryDataContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        private IQueryable<Book> GetBooksInInventory()
        {
            return Context.Books.Where(b => b.InInventory);
        }

        public async Task<GetBookDetailsResponse> GetBookById(int id)
        {
            // From Book -> GetBookDetailsResponse
            return await GetBooksInInventory()
                .Where(b => b.Id == id)
                .AsNoTracking()
                .Select(b => Mapper.Map<GetBookDetailsResponse>(b)).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateGenreFor(int id, string genre)
        {
            var book = await GetBooksInInventory().SingleOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return false;
            }
            else
            {
                book.Genre = genre;
                await Context.SaveChangesAsync();

                return true;
            }

        }

        public async Task Remove(int id)
        {
            var book = await GetBooksInInventory().SingleOrDefaultAsync(b => b.Id == id);
            if (book != null)
            {
                book.InInventory = false;
                await Context.SaveChangesAsync();
            }
        }

        public async Task<GetBookDetailsResponse> Add(PostBooksRequest bookToAdd)
        {
            var book = Mapper.Map<Book>(bookToAdd);

            Context.Books.Add(book);
            await Context.SaveChangesAsync();

            return Mapper.Map<GetBookDetailsResponse>(book);
        }

        public async Task<GetBooksResponse> GetBooks(string genre)
        {

            var books = GetBooksInInventory();

            if (genre != "all")
            {
                books = books.Where(b => b.Genre == genre);
            }

            var booksListItems = await books.Select(b => Mapper.Map<BookSummaryItem>(b))
                .AsNoTracking()
                .ToListAsync();

            var response = new GetBooksResponse
            {
                Data = booksListItems,
                Genre = genre,
                Count = booksListItems.Count()
            };

            return response;
        }
    }
}
