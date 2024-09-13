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
    public class APIStateServiceTests
    {

        [Fact]
        public void GetStates_ReturnsOk_WhenStatesExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var states = new List<State>
            {
                new State { StateId = 1, StateName = "State1", CountryId = 1, Country = new Country{ CountryId=1, CountryName="India" } },
                new State { StateId = 2, StateName = "State2", CountryId = 2, Country = new Country{ CountryId=1, CountryName="India" } }
            };
            mockStateRepository.Setup(c => c.GetAllStates()).Returns(states);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetAllStates();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal(states.Count, actual.Data.Count());
            mockStateRepository.Verify(c => c.GetAllStates(), Times.Once);

        }

        [Fact]
        public void GetStates_ReturnsNotFound_WhenNoStatesExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            mockStateRepository.Setup(c => c.GetAllStates()).Returns((List<State>)(null));

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetAllStates();

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("No record found!", actual.Message);
            mockStateRepository.Verify(c => c.GetAllStates(), Times.Once);
        }
        [Fact]
        public void GetStatesByCountryId_ReturnsOk_WhenStatesExist()
        {
            // Arrange
            var countryId = 1;
            var mockStateRepository = new Mock<IStateRepository>();
            var states = new List<State>
            {
                new State { StateId = 1, StateName = "State1", CountryId = 1, Country = new Country{ CountryId=1, CountryName="India" } },
                new State { StateId = 2, StateName = "State2", CountryId = 2, Country = new Country{ CountryId=1, CountryName="India" } }
            };
            mockStateRepository.Setup(c => c.GetStatesByCountryId(countryId)).Returns(states);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal(states.Count, actual.Data.Count());
            mockStateRepository.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);

        }

        [Fact]
        public void GetStatesByCountryId_ReturnsNotFound_WhenNoStatesExist()
        {
            // Arrange
            var countryId = 1;
            var mockStateRepository = new Mock<IStateRepository>();
            mockStateRepository.Setup(c => c.GetStatesByCountryId(countryId)).Returns((List<State>)(null));

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetStatesByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("No record found!", actual.Message);
            mockStateRepository.Verify(c => c.GetStatesByCountryId(countryId), Times.Once);
        }
    }
}
