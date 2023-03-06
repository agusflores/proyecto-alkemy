using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.DataAccess;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OngProject.Test.Core.Controllers
{
    public class UsersControllerTests
    {
        [Fact]    
        public async void GetAllUsers_OnSucess_ReturnsStatusCode200()
        {
            //arrange
            var userBusinessMock = new Mock<IUsersBusiness>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGridBusiness = new Mock<ISendGridBusiness>();
            userBusinessMock
                .Setup(u => u.GetAll())
                .Returns(Task.FromResult<List<ViewUserDTO>>(new List<ViewUserDTO>() { new ViewUserDTO { FirstName = "FirstNameTest01", LastName = "LastNameTest01", Email = "EmailTest01@gmail.com", Photo = "PhotoTest01.url", RoleId = 1 } }.ToList()));

       
            var userController = new UserController(userBusinessMock.Object, authBusiness.Object, sendGridBusiness.Object);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();

            //act
            var result = await userController.GetAllUsers();

            //assert 
            result
               .Should()
               .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetAllUsers_OnFail_ReturnsStatusCode404()
        {
            //arrange
            var userBusinessMock = new Mock<IUsersBusiness>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGridBusiness = new Mock<ISendGridBusiness>();
            userBusinessMock
                .Setup(u => u.GetAll())
                .Returns(Task.FromResult<List<ViewUserDTO>>(null));


            var userController = new UserController(userBusinessMock.Object, authBusiness.Object, sendGridBusiness.Object);
            userController.ControllerContext = new ControllerContext();
            userController.ControllerContext.HttpContext = new DefaultHttpContext();

            //act
            var result = await userController.GetAllUsers();

            //assert 
            result
               .Should()
               .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void RemoveUser_OnSucess_ReturnsStatusCode200()
        {
            //arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var ongDbCOntext = new TestHelper().GetOngDbContext();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGrid = new Mock<ISendGridBusiness>();
            IUsersBusiness userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness.Object, amazonS3Mock.Object);

            var usercontroller = new UserController(userBusiness, authBusiness.Object, sendGrid.Object);

            // act
            var result = await usercontroller.RemoveUser(1); // Id = 1

            // assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void RemoveUser_OnFail_ReturnsStatusCode404()
        {
            //arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var ongDbCOntext = new TestHelper().GetOngDbContext();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGrid = new Mock<ISendGridBusiness>();
            IUsersBusiness userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness.Object, amazonS3Mock.Object);

            var usercontroller = new UserController(userBusiness, authBusiness.Object, sendGrid.Object);

            // act
            var result = await usercontroller.RemoveUser(10); // Id = 10

            // assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void UpdateUser_OnSucess_ReturnsStatusCode200()
        {
            //arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var ongDbCOntext = new TestHelper().GetOngDbContext();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGrid = new Mock<ISendGridBusiness>();
            IUsersBusiness userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness.Object, amazonS3Mock.Object);

            var usercontroller = new UserController(userBusiness, authBusiness.Object, sendGrid.Object);

            // act
            var result = await usercontroller.UpdateUser(1, new UserUpdateDTO { FirstName = "newUserFirstName01", LastName = "newUserLastName01", Email = "user01@email.com", Password = "user01password", Photo = "user01photo.jpg" }); 

            // assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void UpdateUser_OnFail_ReturnsStatusCode404()
        {
            //arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var ongDbCOntext = new TestHelper().GetOngDbContext();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGrid = new Mock<ISendGridBusiness>();
            IUsersBusiness userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness.Object, amazonS3Mock.Object);

            var usercontroller = new UserController(userBusiness, authBusiness.Object, sendGrid.Object);

            // act
            var result = await usercontroller.UpdateUser(10, new UserUpdateDTO { FirstName = "newUserFirstName01", LastName = "newUserLastName01", Email = "user01@email.com", Password = "user01password", Photo = "user01photo.jpg" });

            // assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void FindUserById_OnSucess_ReturnsStatusCode200()
        {
            //arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var ongDbCOntext = new TestHelper().GetOngDbContext();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGrid = new Mock<ISendGridBusiness>();
            IUsersBusiness userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness.Object, amazonS3Mock.Object);

            var usercontroller = new UserController(userBusiness, authBusiness.Object, sendGrid.Object);

            // act
            var result = await usercontroller.GetUserById(1); // Id = 1


            // assert
            result
                .Should()
                .BeOfType<OkObjectResult>();     
        }

        [Fact]
        public async void FindUserById_OnFail_ReturnsStatusCode404()
        {
            //arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var ongDbCOntext = new TestHelper().GetOngDbContext();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            var authBusiness = new Mock<IAuthBusiness>();
            var sendGrid = new Mock<ISendGridBusiness>();
            IUsersBusiness userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness.Object, amazonS3Mock.Object);

            var usercontroller = new UserController(userBusiness, authBusiness.Object, sendGrid.Object);

            // act
            var result = await usercontroller.GetUserById(10); // Id = 10 


            // assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }
    }
}
