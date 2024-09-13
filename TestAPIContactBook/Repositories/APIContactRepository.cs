using APIContactBook.Data;
using APIContactBook.Data.Implementation;
using APIContactBook.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIContactBook.Repositories
{
    public class APIContactRepository
    {
        [Fact]
        public void GetContact_WhenContactIsNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Contacts, Times.Once);

        }
        [Fact]
        public void GetContact_WhenContactIsNotNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Contact>()
            {
              new Contact { ContactId = 1, FirstName = "Contact 1" },
                new Contact { ContactId = 2, FirstName = "Contact 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull_SearchIsNull()
        {
            char? letter = null;
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull_SearchIsNotNull()
        {
            char? letter = null;
            string search = "c";
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull_SearchIsNull()
        {
            char? letter = null;
            var contacts = new List<Contact>
            {

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull_SearchIsNotNull()
        {
            char? letter = null;
            var contacts = new List<Contact>
            {

            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            string search = "abc";
            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull_SearchIsNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, null);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull_SearchIsNotNull()
        {
            char? letter = 'c';
            string search = "abc";
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull_SearchIsNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, null);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull_SearchIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact>
            {

            }.AsQueryable();
            string search = "abc";
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCount_WhenContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<Contact>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCount_WhenContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExists_LetterIsNull_SearchIsNull()
        {
            string sortOrder = "asc";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExists_LetterIsNull_SearchIsNotNull()
        {
            string sortOrder = "asc";
            string search = "C";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);

            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists_LetterIsNull_SearchIsNull()
        {
            string sortOrder = "asc";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists_LetterIsNull_SearchIsNotNull()
        {
            string search = "con";
            string sortOrder = "asc";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, null, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter_SearchIsNull()
        {
            char letter = 'c';
            string sortOrder = "desc";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter_SearchIsNotNull()
        {
            char letter = 'c';
            string sortOrder = "asc";
            string search = "con";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter_SearchIsNull()
        {
            char letter = 'c';
            string sortOrder = "desc";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter_SearchIsNotNull()
        {
            char letter = 'c';
            string sortOrder = "desc";
            string search = "c";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(1, 2, letter, search, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsCorrectContacts_WhenContactsExists()
        {
            string sortOrder = "desc";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedFavouriteContacts(1, 2, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsEmptyList_WhenNoContactsExists()
        {
            string sortOrder = "asc";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedFavouriteContacts(1, 2, null, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsCorrectContacts_WhenContactsExistsWithLetter()
        {
            string sortOrder = "desc";

            char letter = 'c';
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedFavouriteContacts(1, 2, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsEmptyList_WhenNoContactsExistsWithLetter()
        {
            char letter = 'c';
            string sortOrder = "asc";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedFavouriteContacts(1, 2, letter, sortOrder);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetAll_ReturnsContacts_WhenContactsExists()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact{  ContactId = 1,
                FirstName = "C1"},
                new Contact{ ContactId = 2,
                FirstName = "C2"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.GetEnumerator()).Returns(contacts.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

        [Fact]
        public void GetAll_ReturnsContacts_WhenContactsNotExists()
        {
            //Arrange
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.GetEnumerator()).Returns(contacts.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.GetEnumerator(), Times.Once);

        }
        [Fact]
        public void GetAllFavourite_ReturnsContacts_WhenContactsExists()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact{  ContactId = 1,
                FirstName = "C1", Favourite = true},
                new Contact{ ContactId = 2,
                FirstName = "C2", Favourite = true},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavourite();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }

        [Fact]
        public void GetAllFavourite_ReturnsContacts_WhenContactsNotExists()
        {
            //Arrange
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavourite();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }

        [Fact]
        public void InsertContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.InsertContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void InsertContact_ReturnsFalse()
        {
            //Arrange
            Contact contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.InsertContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void UpdateContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.UpdateContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void UpdateContact_ReturnsFalse()
        {
            //Arrange
            Contact contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.UpdateContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void DeleteContact_ReturnsTrue()
        {
            //Arrange
            var id = 1;
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "C1"
            };

            var contacts = new List<Contact> { contact }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.DeleteContact(id);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Contacts, Times.Once);
            mockAbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsFalse()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.DeleteContact(id);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void ContactExists_ReturnsTrue()
        {
            //Arrange
            var phone = "1234567890";
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "Contact 1", ContactNumber="1234567890"},
                new Contact { ContactId = 2, FirstName = "Contact 2", ContactNumber="9876543216" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsFalse()
        {
            //Arrange
            var phone = "1234567890";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExistsIdName_ReturnsFalse()
        {
            //Arrange
            var phone = "1234567890";
            var id = 1;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(id, phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExistsIdName_ReturnsTrue()
        {
            //Arrange
            var phone = "1234567890";
            var id = 3;
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "Contact 1", ContactNumber="1234567890" },
                new Contact { ContactId = 2, FirstName = "Contact 2" , ContactNumber="9876543219"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(id, phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

    }
}
