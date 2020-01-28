using LibraryApi;
using LibraryApi.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using System.Net.Http;
using LibraryApi.Models;

namespace LibraryApiIntegrationTests
{
    public class MakingAReservation : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> Factory;

        public MakingAReservation(CustomWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
        }

        [Fact]
        public async Task ServiceIsCalled()
        {
            var mockReservationProcessor = new Mock<ISendMessagesToTheReservationProcessor>();

            var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<ISendMessagesToTheReservationProcessor>(mockReservationProcessor.Object);
                });
            }).CreateClient();

            var reservation = new FakeReservation { For = "Sam", Books = new string[] { "1", "42" } };
            var response = await client.PostAsJsonAsync("/reservations", reservation);

            var responseReservation = await response.Content.ReadAsAsync<FakeReservationResponse>();

            mockReservationProcessor.Verify(p => p.SendForProcessing(It.IsAny<GetReservationItemResponse>()), Times.Once());
            var firstCall = mockReservationProcessor.Invocations[0].Arguments[0] as GetReservationItemResponse;
            Assert.Equal("Sam", firstCall.For);
        }
    }


    public class FakeReservation
    {
        public string For { get; set; }
        public string[] Books { get; set; }
    }


    public class FakeReservationResponse
    {
        public int id { get; set; }
        public string For { get; set; }
        public string status { get; set; }
        public DateTime reservationCreated { get; set; }
        public string[] books { get; set; }
    }

}
