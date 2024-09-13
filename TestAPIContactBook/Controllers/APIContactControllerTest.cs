using APIContactBook.Controllers;
using APIContactBook.Dtos;
using APIContactBook.Models;
using APIContactBook.Services.Contract;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPIContactBook.Controllers
{
    public class APIContactControllerTest
    {
        [Fact]
        public void GetAllContacts_ReturnsOkWithContacts_WhenContactsExists()
        {
            //Arrange
            
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContacts()).Returns(response);

            //Act
            var actual = target.GetAllContacts() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContacts(), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenNoContactExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = new List<ContactDto>(),

            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContacts()).Returns(response);

            //Act
            var actual = target.GetAllContacts() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContacts(), Times.Once);
        }
        [Fact]
        public void GetAllFavourite_ReturnsOkWithContacts_WhenContactsExists()
        {
            //Arrange

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllFavourite()).Returns(response);

            //Act
            var actual = target.GetAllFavourite() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllFavourite(), Times.Once);
        }

        [Fact]
        public void GetAllFavourite_ReturnsNotFound_WhenNoContactExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = new List<ContactDto>(),

            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllFavourite()).Returns(response);

            //Act
            var actual = target.GetAllFavourite() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllFavourite(), Times.Once);
        }
        [Fact]
        public void GetContactById_ReturnsOk()
        {

            var contactId = 1;
            var contact = new Contact
            {
                ContactId = contactId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<ContactDto>
            {
                Success = true,
                Data = new ContactDto
                {
                    ContactId = contactId,
                    FirstName = contact.FirstName
                }
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(contactId)).Returns(response);

            //Act
            var actual = target.GetContactById(contactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsNotFound()
        {

            var contactId = 1;
            var contact = new Contact
            {
                ContactId = contactId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<ContactDto>
            {
                Success = false,
                Data = new ContactDto
                {
                    ContactId = contactId,
                    FirstName = contact.FirstName
                }
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(contactId)).Returns(response);

            //Act
            var actual = target.GetContactById(contactId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(contactId), Times.Once);
        }
        [Fact]
        public void AddContact_ReturnsBadRequest_WhenModelIsInValid()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            target.ModelState.AddModelError("ContactNumber", "ContactNumber is required");
            //Act

            var actual = target.AddContact(addContactDto) as BadRequestResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.False(target.ModelState.IsValid);
        }


        [Fact]
        public void AddContact_ReturnsOk_WhenContactIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.AddContact(addContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenContactIsNotAdded()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.AddContact(addContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);

        }
        [Fact]
        public void Edit_ReturnsBadRequest_WhenModelIsInValid()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            target.ModelState.AddModelError("ContactNumber", "ContactNumber is required");
            //Act

            var actual = target.Edit(updateContactDto) as BadRequestResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.False(target.ModelState.IsValid);
        }


        [Fact]
        public void Edit_ReturnsOk_WhenContactIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.Edit(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void Edit_ReturnsBadRequest_WhenContactIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.Edit(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void DeleteConfirmed_ReturnsOkResponse_WhenContactDeletedSuccessfully()
        {

            var contactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(contactId)).Returns(response);

            //Act

            var actual = target.DeleteConfirmed(contactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsBadRequest_WhenContactNotDeleted()
        {

            var contactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(contactId)).Returns(response);

            //Act

            var actual = target.DeleteConfirmed(contactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(contactId), Times.Once);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsBadRequest_WhenContactIsLessThanZero()
        {

            var contactId = 0;

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);

            //Act

            var actual = target.DeleteConfirmed(contactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data", actual.Value);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, null, page, pageSize) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "tac";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
               // Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, null, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, null, sortOrder), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
               // Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, search, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, search, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "desc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
               // Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedFavouriteContacts(null, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "desc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
               // Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedFavouriteContacts(letter, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedFavouriteContacts(null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedFavouriteContacts(page, pageSize, null, sortOrder), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
            {
               new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                 new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder)).Returns(response);

            //Act
            var actual = target.GetPaginatedFavouriteContacts(letter, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedFavouriteContacts(page, pageSize, letter, sortOrder), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
             {
            new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
            new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, null)).Returns(response);

            //Act
            var actual = target.GetContactsCount(null, null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, null), Times.Once);
        }
        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
             {
            new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
            new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };
            string search = "dev";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, search)).Returns(response);

            //Act
            var actual = target.GetContactsCount(null, search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
             {
            new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
            new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, null)).Returns(response);

            //Act
            var actual = target.GetContactsCount(letter, null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, null), Times.Once);
        }
        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
             {
            new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
            new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };
            string search = "dev";
            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, search)).Returns(response);

            //Act
            var actual = target.GetContactsCount(letter, search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, null)).Returns(response);

            //Act
            var actual = target.GetContactsCount(letter, null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            string search = "dev";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, search)).Returns(response);

            //Act
            var actual = target.GetContactsCount(letter, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, null)).Returns(response);

            //Act
            var actual = target.GetContactsCount(null, null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };
            string search = "dev";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, search)).Returns(response);

            //Act
            var actual = target.GetContactsCount(null, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
             {
            new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
            new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactFavourite(null)).Returns(response);

            //Act
            var actual = target.TotalContactFavourite(null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContactFavourite(null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
             {
            new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
            new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactFavourite(letter)).Returns(response);

            //Act
            var actual = target.TotalContactFavourite(letter) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContactFavourite(letter), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactFavourite(letter)).Returns(response);

            //Act
            var actual = target.TotalContactFavourite(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContactFavourite(letter), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactFavourite(null)).Returns(response);

            //Act
            var actual = target.TotalContactFavourite(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContactFavourite(null), Times.Once);
        }





    }
}
