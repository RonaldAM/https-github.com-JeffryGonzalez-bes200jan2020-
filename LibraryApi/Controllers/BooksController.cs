using LibraryApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using LibraryApi.Mappers;
using LibraryApi.Services;

namespace LibraryApi.Controllers
{
    public class BooksController : Controller
    {
        LibraryDataContext Context;
        IMapBooks BooksMapper;

        public BooksController(LibraryDataContext context, IMapBooks booksMapper)
        {
            Context = context;
            BooksMapper = booksMapper;
        }


        /// <summary>
        /// Gives you all the books that are currently in inventory
        /// </summary>
        /// <returns>uh, those books</returns>
        IQueryable<Book> GetBooksInInventory()
        {
            return Context.Books
                .Where(b => b.InInventory);
        }

        [HttpPut("/books/{id:int}/genre")]
        public async Task<IActionResult> UpdateTheGenre(int id, [FromBody] string genre)
        {
            bool madeChange = await BooksMapper.UpdateGenreFor(id, genre);
            if(madeChange)
            {
                return NoContent();
            } else
            {
                return NotFound();
            }
        }

        [HttpDelete("/books/{id:int}")]
        public async Task<IActionResult> RemoveBookFromInventory(int id)
        {
            await BooksMapper.Remove(id);
            return NoContent();
        }


        /// <summary>
        /// Add a book to the inventory
        /// </summary>
        /// <param name="bookToAdd">Information about the book you want to add</param>
        /// <returns></returns>
        [HttpPost("/books")]
        [Produces("application/json")]
        [ValidateModel]
        public async Task<ActionResult<GetBookDetailsResponse>> AddABook([FromBody] PostBooksRequest bookToAdd)
        {
            GetBookDetailsResponse response = await BooksMapper.Add(bookToAdd);
            return CreatedAtRoute("books#getbookbyid", new { id = response.Id }, response);
        }


        [HttpGet("/books/{id:int}", Name = "books#getbookbyid")]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookById(int id)
        {
            var response = await BooksMapper.GetBookById(id);

            return this.Maybe(response);
        }

        /// <summary>
        /// Provides a list of all the books in our inventory   
        /// </summary>
        /// <param name="genre">If you'd like to filter by genre, use this. Otherwise all books will be returned</param>
        /// <returns>A List of Books</returns>
        /// <response code="200">Returns all of your books.</response>
        [HttpGet("/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetBooksResponse>> GetAllBooks([FromQuery] string genre = "all")
        {
            GetBooksResponse response = await BooksMapper.GetBooks(genre);
            return Ok(response); // for right now.
        }
    }
}
