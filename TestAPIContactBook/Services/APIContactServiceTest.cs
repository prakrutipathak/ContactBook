using APIContactBook.Data.Contract;
using APIContactBook.Dtos;
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
    public class APIContactServiceTest
    {
        [Fact]
        public void GetContacts_ReturnList_WhenNoContactExist()
        {
            //Arrange
            var mockRepository = new Mock<IContactRepository>();
            var target = new ContactService(mockRepository.Object);
            //Act
            var actual = target.GetContacts();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found!", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        public void GetContacts_ReturnsContactsList_WhenContactsExist()
        {
            //Arrange
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName }).ToList(),
            };
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(c => c.GetAll()).Returns(contacts);
            var target = new ContactService(mockRepository.Object);

            //Act
            var actual = target.GetContacts();

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockRepository.Verify(c => c.GetAll(), Times.Once);
            Assert.Equal(contacts.Count, actual.Data.Count()); // Ensure the counts are equal

            for (int i = 0; i < contacts.Count; i++)
            {
                Assert.Equal(contacts[i].ContactId, actual.Data.ElementAt(i).ContactId);
                Assert.Equal(contacts[i].FirstName, actual.Data.ElementAt(i).FirstName);
                Assert.Equal(contacts[i].LastName, actual.Data.ElementAt(i).LastName);
            }
        }
        [Fact]
        public void GetAllFavourite_ReturnList_WhenNoContactExist()
        {
            //Arrange
            var mockRepository = new Mock<IContactRepository>();
            var target = new ContactService(mockRepository.Object);
            //Act
            var actual = target.GetAllFavourite();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("No record found!", actual.Message);
            Assert.False(actual.Success);
        }
        [Fact]
        public void GetAllFavourite_ReturnsContactsList_WhenContactsExist()
        {
            //Arrange
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName }).ToList(),
            };
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(c => c.GetAllFavourite()).Returns(contacts);
            var target = new ContactService(mockRepository.Object);

            //Act
            var actual = target.GetAllFavourite();

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockRepository.Verify(c => c.GetAllFavourite(), Times.Once);
            Assert.Equal(contacts.Count, actual.Data.Count()); // Ensure the counts are equal

            for (int i = 0; i < contacts.Count; i++)
            {
                Assert.Equal(contacts[i].ContactId, actual.Data.ElementAt(i).ContactId);
                Assert.Equal(contacts[i].FirstName, actual.Data.ElementAt(i).FirstName);
                Assert.Equal(contacts[i].LastName, actual.Data.ElementAt(i).LastName);
            }
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, null, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull_SearchIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            string search = "abc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, null, search, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull_SearchIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, null, null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull_SearchIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "abc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, null, search, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, null, search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull_SearchIsNull()
        {

            // Arrange
            string sortOrder = "asc";
            var contacts = new List<Contact>

        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, null, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull_SearchIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            string search = "abc";
            var contacts = new List<Contact>

        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, search, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull_SearchIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, null, sortOrder)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull_SearchIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            string search = "abc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, search, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, search, sortOrder), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull_SearchIsNull()
        {
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(null, null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(null, null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(null, null), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            string search = "abc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(null, search)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(null, search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(null, search), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            var letter = 'c';
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(letter, null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(letter, null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(letter, null), Times.Once);
        }
        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            var letter = 'c';
            string search = "abc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(letter, search)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(letter, search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(letter, search), Times.Once);
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            string sortOrder = "desc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "desc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            string sortOrder = "asc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                Image = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            var letter = 'x';
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNull()
        {
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContactFavourite(null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContactFavourite(null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContactFavourite(null), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNotNull()
        {
            var letter = 'c';
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContactFavourite(letter)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContactFavourite(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContactFavourite(letter), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsContact_WhenContactExist()
        {
            // Arrange
            var contactId = 1;
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }

            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(contactId)).Returns(contact);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsNoRecord_WhenNoContactsExist()
        {
            // Arrange
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(contactId)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsContactSavedSuccessfully_WhenContactisSaved()
        {
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,

            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(true);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Contact saved successfully", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsSomethingWentWrong_WhenContactisNotSaved()
        {
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(false);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong after sometime", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact already exists.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);

        }
        [Fact]
        public void AddContact_ReturnsEnterValidDate_WhenDateIsNotProper()
        {
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                BirthDate = new DateTime(2027, 5, 15, 10, 30, 0),
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Enter valid date", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);

        }

        [Fact]
        public void ModifyContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contactId = 1;
            var contact = new Contact()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contactId, contact.ContactNumber)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact Exists!", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contactId, contact.ContactNumber), Times.Once);
        }
        [Fact]
        public void ModifyContact_ReturnsEnterValidDate_WhenContactBirthdateIsNotProper()
        {
            var contactId = 1;
            var contact = new Contact()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                BirthDate= new DateTime(2027,5,15,10,30,0),
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contactId, contact.ContactNumber)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Enter valid date", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contactId, contact.ContactNumber), Times.Once);
        }
        [Fact]
        public void ModifyContact_ReturnsSomethingWentWrong_WhenContactNotFound()
        {
            var contactId = 1;
            var existingContact = new Contact()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1

            };

            var updatedContact = new Contact()
            {
                ContactId = contactId,
                FirstName = "C1"
            };


            var mockRepository = new Mock<IContactRepository>();
            //mockRepository.Setup(r => r.ContactExists(contactId, updatedContact.ContactNumber)).Returns(false);
            mockRepository.Setup(r => r.GetContact(updatedContact.ContactId)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(existingContact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong after sometime", actual.Message);
            //mockRepository.Verify(r => r.ContactExists(contactId, updatedContact.ContactNumber), Times.Once);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsUpdatedSuccessfully_WhenContactModifiedSuccessfully()
        {

            //Arrange
            var existingContact = new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1

            };

            var updatedContact = new Contact
            {
                ContactId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.ContactId, updatedContact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(true);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Contact Updated successfully", actual.Message);

            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);


            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }
        [Fact]
        public void ModifyContact_ReturnsError_WhenContactModifiedFails()
        {

            //Arrange
            var existingContact = new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                Image = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };

            var updatedContact = new Contact
            {
                ContactId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.ContactId, updatedContact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(false);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong after sometime", actual.Message);
            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(contactId)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Contact Deleted Successfully", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_SomethingWentWrong_WhenDeletionFailed()
        {
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(contactId)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(contactId), Times.Once);
        }

    }
}
