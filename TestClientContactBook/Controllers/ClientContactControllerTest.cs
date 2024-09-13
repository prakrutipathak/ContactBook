using AutoFixture;
using ClientApplicationContactBook.Controllers;
using ClientApplicationContactBook.Infrastructure;
using ClientApplicationContactBook.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestClientContactBook.Controllers
{
    public class ClientContactControllerTest
    {
        //Index
        [Fact]
        public void Index_ReturnsContacts()
        {
            //Arrange
            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
              var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(response);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index() as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index_ReturnsNoContacts()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(response);

            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index() as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        //Index 1
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachAndLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1(null,null,1,2,"asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsNoContacts_WhenSerachAndLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>();
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true,Data=3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1(null, null, 1, 2, "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachAndLetterIsNotNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1('f', "fir", 1, 2, "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachIsNullAndLetterIsNotNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1('f', " ", 1, 2, "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachIsNotNullAndLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1(null, "fir", 1, 2, "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachAndLetterIsNotNull_TotalCountIsZeo()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 0 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1('f', "fir", 1, 2, "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Index1_ReturnsContacts_WhenSerachAndLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 11 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Index1('f', "fir", 6, 6, "asc") as RedirectToActionResult;
            //Assert
            Assert.Equal("Index1", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        //Favourite
        [Fact]
        public void Favourite_ReturnsContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Favourite( null, 1, 2) as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Favourite_ReturnsNoContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>();
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Favourite( null, 1, 2) as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Favourite_ReturnsContacts_WhenLetterIsNotNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Favourite('f', 1, 2) as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Favourite_ReturnsContacts_WhenLetterIsNotNull_TotalCountIsZero()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 0 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Favourite('f', 1, 2) as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Favourite_ReturnsContacts_WhenLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 11 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Favourite('f', 6, 6) as RedirectToActionResult;
            //Assert
            Assert.Equal("Index1", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        //Test cases for Create() httpget
        [Fact]
        public void Create_ReturnsViewResult_WithCountryList_WhenCountryExists()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var expectedResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        public void Create_ReturnsViewResult_WithEmptyCountryList_WhenCountryDoesNotExists()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = false,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = false,
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedStateResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        //Create POST
        //Test cases for Create()
        [Fact]
        public void Create_RedirectToActionResult_WhenContactSavedSuccessfully_WhenFileISNotNull()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };

            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1",LastName="lastname",StateId=1,CountryId=1,States= expectedState ,Countries=expectedCountry, File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg") };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal("Contact Saved Successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_RedirectToActionResult_WhenContactSavedSuccessfully_WhenFileISNull()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };

            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry};
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal("Contact Saved Successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResultWithErrorMessage_WhenResponseIsNotSuccess()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
           
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsRedirectToActionResult_WhenResponseIsNotSuccess()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Something went wrong try after some time";
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResult_WithContactList_WhenModelStateIsInvalid()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var expectedResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedResponseState = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "firstname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponseState);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Create_ReturnsViewResult_WithEmptyCountryandStateList_WhenModelStateIsInvalid()
        {
            //Arrange

            var expectedResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = false,
            };
            var expectedResponseState = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = false,
            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "firstname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseState);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        //Test case for Details
        [Fact]
        public void Details_ReturnsViewResult_WhenDetailsObtainedSuccessfully()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Data = expectedContacts,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = null,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal("Something went wrong.Please try after sometime.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        //Test case for Delete()
        [Fact]
        public void Delete_ReturnsViewResult_WhenDetailsObtainedSuccessfully()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };

            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Data = expectedContacts,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = null,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal("Something went wrong.Please try after sometime.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

       //Delete Confirm
        [Fact]
        public void DeleteConfirm_ReturnsRedirectToAction_WhenDeletedSuccessfully()
        {
            // Arrange
            var id = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = "Success",
                Success = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedServiceResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        public void DeleteConfirm_ReturnsRedirectToAction_WhenDeletionFailed()
        {
            // Arrange
            var id = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = "Error",
                Success = false
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedServiceResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        //Edit HttpGet
        [Fact]
        public void Edit_ReturnsViewResult_WhenContactDetailsObtainedSuccessfully_WithCountryAndStateList()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };

            var contactId = 1;
            var expectedProducts = new AddContactViewModel {  ContactId=1, FirstName="FirstName 1"};
            
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedResponseForCountries = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedResponseForStates = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedServiceResponse = new ServiceResponse<AddContactViewModel>
            {
                Success = true,
                Data = expectedProducts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseForCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseForStates);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as ViewResult;
            var model = actual.Model as AddContactViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.NotNull(model);
            Assert.NotNull(model.Countries);
            Assert.NotNull(model.States);
            Assert.Equal(expectedCountry.Count, model.Countries.Count());
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResult_WhenContactDetailsObtainedSuccessfully_WithCountriesEmptyList()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new AddContactViewModel {  ContactId=1, FirstName="FirstName 1"};
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedResponseForCountries = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = false,
            };
            var expectedResponseForState = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = false,
            };
            var expectedServiceResponse = new ServiceResponse<AddContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseForCountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseForState);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as ViewResult;
            var model = actual.Model as AddContactViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.NotNull(model);
            Assert.Empty(model.Countries);
            Assert.Empty(model.States);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            var contactId = 1;
            var expectedProducts = new AddContactViewModel {  ContactId=1, FirstName="FirstName 1"};
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<AddContactViewModel>
            {
                Success = false,
                Data = expectedProducts,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange
          
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<AddContactViewModel>
            {
                Success = true,
                Data = null,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<AddContactViewModel>
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
             var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new   ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal("Something went wrong.Please try after sometime.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<AddContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        //Edit HttpPost
        //Test cases for Edit(ProductViewModel viewModel)
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNull()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNotNull()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry , File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg") };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNotNull_ExtesnionISpdf()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry, File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.pdf") };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNullAndRemoveImageHiddenIsTrue()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname",RemoveImageHidden="true", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResultWithErrorMessage_WhenResponseIsNotSuccess()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenResponseIsNotSuccess()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IImageUpload>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Something went wrong try after some time";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("Index1", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>
            {
                new ContactsCountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new ContactsCountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<ContactsStateViewModel>
            {
                new ContactsStateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new ContactsStateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("ProductDescription", "Product description is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResult_WhenModelStateIsInvalid_WhenStateAndCountryIsNull()
        {
            //Arrange
            var expectedCountry = new List<ContactsCountryViewModel>{};
            var expectedState = new List<ContactsStateViewModel>{};
            var expectedCountryResponse = new ServiceResponse<IEnumerable<ContactsCountryViewModel>>
            {
                Success = false,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<ContactsStateViewModel>>
            {
                Success = false,
            };
            var viewModel = new AddContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, States = expectedState, Countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockImage = new Mock<IImageUpload>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
             .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("ProductDescription", "Product description is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsCountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactsStateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }






    }
}
