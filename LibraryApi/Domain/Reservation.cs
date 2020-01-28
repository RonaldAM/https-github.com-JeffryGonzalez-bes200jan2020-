using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Domain
{

    public enum ReservationStatus {  Pending, Approved, Cancelled }
    public class Reservation
    {
        public int Id { get; set; }
        public string For { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime ReservationCreated { get; set; }
        public string Books { get; set; } // "1,2,3,4"
    }
}
