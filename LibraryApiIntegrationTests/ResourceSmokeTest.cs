using LibraryApi;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class ResourceSmokeTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient Client;

        public ResourceSmokeTest(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }


        [Theory]
        [InlineData("/books")]
        [InlineData("/books/1")]

        public async Task GetAllResourcesAndSeeIfTheyAreAlive(string url)
        {
            var response = await Client.GetAsync(url);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetBookOne()
        {
            var response = await Client.GetAsync("/books/1");
            var content = await response.Content.ReadAsAsync<GetBookResponse>();

            Assert.Equal("Walden", content.title);
            Assert.Equal("Threau", content.author);
        }

        [Fact]
        public async Task CanAddABook()
        {
            var bookToAdd = new BookAddRequest
            {
                author = "Goodall",
                genre = "non-fiction",
                title = "Gorillas are RAD!",
                numberOfPages = 222
            };

            var response = await Client.PostAsJsonAsync("/books", bookToAdd);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var location = response.Headers.Location.LocalPath;

            var getItResponse = await Client.GetAsync(location);
            var responseData = await getItResponse.Content.ReadAsAsync<GetBookResponse>();
            Assert.Equal(bookToAdd.author, responseData.author);
            // etc. etc.
        }
    }


    public class GetBookResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int numberOfPages { get; set; }
    }

    public class BookAddRequest
    {

        public string author { get; set; }
        public string genre { get; set; }
        public string title { get; set; }
        public int numberOfPages { get; set; }
    }

}
