using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.OrganizationDTO;
using OngProject.Core.Models.DTOs.PagedListDTO;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OngProject.Test.Core.Controllers
{
    public class OrganizationsControllerTests
    {
        // Do not use real data instead mock business logic in order to return Ok - 200 status code.
        [Fact]
        public async void GetAllOrganizations_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var organizationsBusinessMock = new Mock<IOrganizationsBusiness>();

            organizationsBusinessMock
                .Setup(m => m.GetAllOrganization())
                .Returns(Task.FromResult<List<GetOrganizationDto>>(new List<GetOrganizationDto>() { new GetOrganizationDto { Name = "test" } }.ToList()));

            var sut = new OrganizationsController(organizationsBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllOrganization();

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        // Test in which there is no Members in database.
        [Fact]
        public async void GetAllOrganizations_OnFail_ReturnsStatusCode404()
        {
            // Arrange
            var organizationsBusinessMock = new Mock<IOrganizationsBusiness>();

            organizationsBusinessMock
                .Setup(m => m.GetAllOrganization())
                .Returns(Task.FromResult<List<GetOrganizationDto>>(new List<GetOrganizationDto> { }));

            var sut = new OrganizationsController(organizationsBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllOrganization();

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void CreateOrganization_OnSucess_ReturnsStatusCode200()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationsBussiness = new OrganizationsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new OrganizationsController(organizationsBussiness);

            // Act
            // ContactCreateDTO with a set name and email leads to success.
            var result = await sut.InsertOrganization(new CreateOrganizationDTO { Name = "test01", Address = "addressTest01", Phone = 08009999, Email = "test@gmail.com", WelcomeText = "WelcomeTextTest01", AboutUsText = "AboutTextTest01", FacebookUrl = "www.facebookTest.com", InstagramUrl = "www.instagramTest.com", LinkedinUrl = "www.linkedInTest.com" });

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CreateOrganization_OnFail_ReturnsStatusCode404()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationsBussiness = new OrganizationsBusiness(unitOfWork,amazonS3Mock.Object);

            var sut = new OrganizationsController(organizationsBussiness);

            // Act
            // ContactCreateDTO without a set name and email leads to success.
            var result = await sut.InsertOrganization(new CreateOrganizationDTO { });

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void CreateOrganization_OnSuccess_SaveAContactInDatabase()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationsBussiness = new OrganizationsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new OrganizationsController(organizationsBussiness);

            // Act
            await sut.InsertOrganization(new CreateOrganizationDTO { Name = "test02", Address = "addressTest02", Phone = 08001234, Email = "test@gmail.com", WelcomeText = "WelcomeTextTest02", AboutUsText = "AboutTextTest02", FacebookUrl = "www.facebookTest.com", InstagramUrl = "www.instagramTest.com", LinkedinUrl = "www.linkedInTest.com" });
            // Id = 5 matchs new contact created above ↑↑↑
            var result = await unitOfWork.OrganizationRepository.GetById(5);

            // Assert
            result
                .Name
                .Should()
                .Be("test02");
        }


        [Fact]
        public async void RemoveOrganizations_OnSuccess_Return_StatusCode200_Datos_Validos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationsBussiness = new OrganizationsBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new OrganizationsController(organizationsBussiness);
            //Actuar-act
            var result = await controlador.DeleteOrganization(4);//ID=4

            //confirmar-assert
            result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async void RemoveOrganizations_OnFail_Return_StatusCode404NotFound()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationsBussiness = new OrganizationsBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new OrganizationsController(organizationsBussiness);
            //Actuar-act
            var result = await controlador.DeleteOrganization(9);

            //confirmar-assert
            result.Should().BeOfType<NotFoundResult>();
        }      

        [Fact]
        public async void GetOrganizationById_OnSuccess_Return_StatusCode200()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationsBussiness = new OrganizationsBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new OrganizationsController(organizationsBussiness);

            //Actuar-act
            var result = (OkObjectResult)await controlador.GetByIdOrganization(4);//ID=4

            //confirmar-assert
            result.Value.Should().BeOfType<Organization>();
        }

        [Fact]
        public async void GetOrganizationsById_OnFail_Return_StatusCode404NotFound()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IOrganizationsBusiness organizationBussiness = new OrganizationsBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new OrganizationsController(organizationBussiness);

            //Actuar-act
            var result = await controlador.GetByIdOrganization(9);//ID=6

            //confirmar-assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
