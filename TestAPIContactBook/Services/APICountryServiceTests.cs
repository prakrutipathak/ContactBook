using APIContactBook.Data.Contract;
using APIContactBook.Models;
using APIContactBook.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIContactBook.Services
{
    public class APICountryServiceTests
    {
        [Fact]
       
        public void GetCountries_ReturnsCountries_WhenCountriesExist()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { CountryId = 1, CountryName = "Country1"},
                new Country { CountryId = 2, CountryName = "Country2"}
            };

            var mockRepository = new Mock<ICountryRepository>();
            mockRepository.Setup(r => r.GetAllCountries()).Returns(countries);

            var countryService = new CountryService(mockRepository.Object);

            // Act
            var actual = countryService.GetCountries();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(countries.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAllCountries(), Times.Once);
        }

        [Fact]
        public void GetCountries_Returns_WhenNoCountriesExist()
        {
            // Arrange
            var countries = new List<Country>();


            var mockRepository = new Mock<ICountryRepository>();
            mockRepository.Setup(r => r.GetAllCountries()).Returns(countries);

            var countryService = new CountryService(mockRepository.Object);

            // Act
            var actual = countryService.GetCountries();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAllCountries(), Times.Once);
        }
    }
}
