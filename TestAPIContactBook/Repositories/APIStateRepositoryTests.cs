using APIContactBook.Data.Implementation;
using APIContactBook.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIContactBook.Models;

namespace TestAPIContactBook.Repositories
{
    public class APIStateRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsStates_WhenStatesExist()
        {
            var statesList = new List<State>
            {
              new State{ StateId=1, StateName="State 1", CountryId = 1 },
              new State{ StateId=2, StateName="State 2", CountryId = 1},
             }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();

            mockDbSet.As<IQueryable<State>>().Setup(c => c.GetEnumerator()).Returns(statesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);

            //Act
            var actual = target.GetAllStates();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoStatesExist()
        {
            var statesList = new List<State>().AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(c => c.GetEnumerator()).Returns(statesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllStates();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.GetEnumerator(), Times.Once);

        }
        [Fact]
        public void GetStatesByCountryId_ReturnStates()
        {
            //Arrange
            var id = 1;
            var statesList = new List<State>
            {
              new State{ StateId=1, StateName="State 1", CountryId = 1 },
              new State{ StateId=2, StateName="State 2", CountryId = 1},
             }.AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(statesList.AsQueryable().Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(statesList.AsQueryable().Expression);
            mockAbContext.SetupGet(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetStatesByCountryId(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.States, Times.Once);

        }
        [Fact]
        public void GetStatesByCountryId_WhenStatesIsNull()
        {
            //Arrange
            var id = 1;
            var statesList = new List<State>().AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(statesList.AsQueryable().Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(statesList.AsQueryable().Expression);
            mockAbContext.SetupGet(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetStatesByCountryId(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Provider, Times.Exactly(2));
            mockDbSet.As<IQueryable<State>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.States, Times.Once);
        }

    }
}
