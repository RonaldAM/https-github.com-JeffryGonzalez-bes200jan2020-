using LibraryApi.Domain;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController : Controller
    {

        LibraryDataContext Context;

        public ReservationsController(LibraryDataContext context)
        {
            Context = context;
        }

        [HttpPost("/reservations")]
        [ValidateModel]
        public async Task<ActionResult> AddAReservation([FromBody] PostReservationRequest request)
        {
            // 1. Map It to a Reservation
            var reservation = new Reservation
            {
                For = request.For,
                Books = string.Join(',', request.Books),
                ReservationCreated = DateTime.Now,
                Status = ReservationStatus.Pending
            };
            // 2. Add it to the database
            Context.Reservations.Add(reservation);
            await Context.SaveChangesAsync();
            // 3. That that reservation and map it to a reservation response.
            // 4. return a 201 with a location and attach a GetReservationItemResponse
            var response = MapIt(reservation);
            return CreatedAtRoute("reservations#getbyid", new { id = response.Id }, response);
        }

        [HttpGet("/reservations/{id:int}", Name ="reservations#getbyid")]
        public async Task<ActionResult<GetReservationItemResponse>> GetById(int id)
        {
            return Ok(); // come back to this.
        }

        private GetReservationItemResponse MapIt(Reservation reservation)
        {
            var response = new GetReservationItemResponse
            {
                Id = reservation.Id,
                For = reservation.For,
                ReservationCreated = DateTime.Now,
                Status = reservation.Status, // this is going to not work quite right. We'll fix that.
                Books = reservation.Books.Split(',')
                    .Select(id => Url.ActionLink("GetBookById", "Books", new { id = id})).ToList()
            };

            return response;

        }
    }
}
