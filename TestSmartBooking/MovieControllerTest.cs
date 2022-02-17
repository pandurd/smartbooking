using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartBooking.Controllers;
using SmartBooking.Models;
using SmartBooking.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSmartBooking
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetAvailableMovies()
        {
            //Arrange
            var res = new MoviesResult() 
                        { Movies = new List<MovieCard> 
                            { 
                                new MovieCard { Title = "TestMovie", Year = 2008 },
                                new MovieCard { Title = "TMovie", Year = 2022 }
                            } 
                        };

            var mockService = new Mock<IMovieService>();
            var mockLogger = new Mock<ILogger<MoviesController>>();
            mockService.Setup(repo => repo.GetAvailableMovies()).ReturnsAsync(res);
            var controller = new MoviesController(mockService.Object, mockLogger.Object);

            //Act
            var result = await controller.GetAvailableMovies();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var ObjectInRes = JsonConvert.DeserializeObject<MoviesResult>((string)okResult.Value);
            var movies = ObjectInRes.Movies.ToList();
            Assert.AreEqual(2, movies.Count);
        }
    }
}