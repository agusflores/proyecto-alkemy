using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.PagedListDTO;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OngProject.Test.Core.Controllers
{
    public class ContactsControllerTests
    {
        // Do not use real data instead mock business logic in order to return Ok - 200 status code.
        [Fact]
        public async void GetAllContacts_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var contactsBusinessMock = new Mock<IContactsBusiness>();

            contactsBusinessMock
                .Setup(m => m.GetAllContacts())
                .Returns(Task.FromResult<List<ContactDTO>>(new List<ContactDTO>() { new ContactDTO { Name = "test01", Phone = 08009999, Email = "test@gmail.com", Message = "test01message" } }.ToList()));

            var sut = new ContactsController(contactsBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllContacts();

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        // Test in which there is no Members in database.
        [Fact]
        public async void GetAllContacts_OnFail_ReturnsStatusCode404()
        {
            // Arrange
            var contactsBusinessMock = new Mock<IContactsBusiness>();

            contactsBusinessMock
                .Setup(m => m.GetAllContacts())
                .Returns(Task.FromResult<List<ContactDTO>>(null));

            var sut = new ContactsController(contactsBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllContacts();

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void CreateContact_OnSucess_ReturnsStatusCode200()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var sendGridMock = new Mock<ISendGridBusiness>();
            IContactsBusiness contactsBussiness = new ContactsBusiness(unitOfWork, sendGridMock.Object);

            var sut = new ContactsController(contactsBussiness);

            // Act
            // ContactCreateDTO with a set name and email leads to success.
            var result = await sut.PostContacts(new ContactCreateDTO { Name = "test01", Phone = 08009999, Email = "test@gmail.com", Message = "test01message" });

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CreateContact_OnFail_ReturnsStatusCode404()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var sendGridMock = new Mock<ISendGridBusiness>();
            IContactsBusiness contactsBussiness = new ContactsBusiness(unitOfWork, sendGridMock.Object);

            var sut = new ContactsController(contactsBussiness);

            // Act
            // ContactCreateDTO without a set name and email leads to success.
            var result = await sut.PostContacts(new ContactCreateDTO { });

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void CreateContact_OnSuccess_SaveAContactInDatabase()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var sendGridMock = new Mock<ISendGridBusiness>();
            IContactsBusiness contactsBussiness = new ContactsBusiness(unitOfWork, sendGridMock.Object);

            var sut = new ContactsController(contactsBussiness);

            // Act
            await sut.PostContacts(new ContactCreateDTO { Name = "unique_test_name", Phone = 44444, Email = "testEmail@gmail.com", Message = "uniqueTest" }); // Id = 5
            // Id = 5 matchs new contact created above ↑↑↑
            var result = await unitOfWork.ContactsRepository.GetById(5);

            // Assert
            result
                .Name
                .Should()
                .Be("unique_test_name");
        }

    }
}
