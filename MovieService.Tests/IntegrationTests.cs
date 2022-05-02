using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieService.WebAPI;
using MovieService.WebAPI.Data;
using MovieService.WebAPI.SeedData;
using Newtonsoft.Json;
using Xunit;

namespace MovieService.Tests
{
    public class IntegrationTests
    {
        private TestServer _server;

        public HttpClient Client { get; private set; }

        public IntegrationTests()
        {
            SetUpClient();
        }

        private async Task SeedData()
        {
            var createForm0 = GenerateMovieCreateForm("Movie Title 1", "DramaKey", DateTime.Parse("05.03.2019", new CultureInfo("en")));
            var response0 = await Client.PostAsync("/api/movies", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));

            var createForm1 = GenerateMovieCreateForm("Movie Title 2", "ComedyKey", DateTime.Parse("01.23.2019", new CultureInfo("en")));
            var response1 = await Client.PostAsync("/api/movies", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));

            var createForm2 = GenerateMovieCreateForm("Movie Title 3", "HorrorKey", DateTime.Parse("05.10.2019", new CultureInfo("en")));
            var response2 = await Client.PostAsync("/api/movies", new StringContent(JsonConvert.SerializeObject(createForm2), Encoding.UTF8, "application/json"));

            var createForm3 = GenerateMovieCreateForm("Movie Title 4", "ComedyKey", DateTime.Parse("02.12.2020", new CultureInfo("en")));
            var response3 = await Client.PostAsync("/api/movies", new StringContent(JsonConvert.SerializeObject(createForm3), Encoding.UTF8, "application/json"));
        }

        private MovieForm GenerateMovieCreateForm(string movieName, string category, DateTime releaseDate)
        {
            return new MovieForm
            {
                Title = movieName,
                Category = category,
                ReleaseDate = releaseDate
            };
        }

        [Fact]
        public async Task Test1()
        {
            await SeedData();
            
            Client.DefaultRequestHeaders.Clear();
            var response = await Client.GetAsync("/api/movies/1");
            response.StatusCode.Should().BeEquivalentTo(200);

            var movieDefault = JsonConvert.DeserializeObject<Movie>(response.Content.ReadAsStringAsync().Result);
            movieDefault.Title.Should().Be("Movie Title 1");

            movieDefault.Category.Should().Be("Drama");
        }


        [Fact]
        public async Task Test2()
        {   
            await SeedData();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("it"));
            var response1 = await Client.GetAsync("/api/movies/1");
            response1.StatusCode.Should().BeEquivalentTo(200);

            var movieIt = JsonConvert.DeserializeObject<Movie>(response1.Content.ReadAsStringAsync().Result);
            movieIt.Title.Should().Be("Movie Title 1");

            movieIt.Category.Should().Be("Dramma");
        }

        [Fact]
        public async Task Test3()
        {
            await SeedData();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));
            var response0 = await Client.GetAsync("/api/movies/1");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var movie = JsonConvert.DeserializeObject<Movie>(response0.Content.ReadAsStringAsync().Result);
            movie.Title.Should().Be("Movie Title 1");

            movie.Category.Should().Be("Драма");
        }

        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var context = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(DatabaseContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}
