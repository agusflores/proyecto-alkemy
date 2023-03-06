using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace OngProject.Test.Core.Controllers
{
    public class AuthControllerTest
    {

        #region AuthRegister_ShouldSaveAUser_OnSuccess
        [Fact]
        public async void AuthRegister_ShouldSaveAUser_OnSuccess()
        {
            //Arrange
            var helper = new TestHelper();
            var config = new ConfigurationBuilder().Build();
            var authBusiness = new AuthBusiness(config);
            var unitOfWork = helper.GetUnitOfWork();
            var ongDbCOntext = helper.GetOngDbContext();
            var amazon = new Mock<IAmazonS3Client>();
            var sendGrid = new Mock<ISendGridBusiness>();
            var userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness, amazon.Object);

            var newUser = new UserRegisterDTO()
            {
                FirstName = "User",
                LastName = "Test",
                Email = "usertest@mail.com",
                Password = "123"
            };
            sendGrid.Setup(x => x.WelcomeEmail(newUser.Email));
            var userController = new UserController(userBusiness, authBusiness, sendGrid.Object);

            //Act
            userController.CreateUser(newUser);
            var savedUser = await unitOfWork.UserRepository.GetById(7);

            //Assert
            savedUser.Email.Should().Be("usertest@mail.com");
        }
        #endregion

        #region AuthRegister_NotFound
        [Fact]
        public async void AuthRegister_NotFound()
        {
            //Arrange
            var helper = new TestHelper();
            var config = new ConfigurationBuilder().Build();
            var authBusiness = new AuthBusiness(config);
            var unitOfWork = helper.GetUnitOfWork();
            var ongDbCOntext = helper.GetOngDbContext();
            var amazon = new Mock<IAmazonS3Client>();
            var sendGrid = new Mock<ISendGridBusiness>();
            var userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness, amazon.Object);
            var userController = new UserController(userBusiness, authBusiness, sendGrid.Object);

            // Act
            var result = userController.CreateUser(new UserRegisterDTO { }) as IActionResult;

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region AuthRegister_FirstAndLastName_IsInvalid
        [Fact]
        public async void AuthRegister_FirstAndLastName_IsInvalid()
        {
            //Arrange
            var newUser = new UserRegisterDTO()
            {
                Email = "usertest@mail.com",
                Password = "123"
            };

            //Act
            var validateContext = new ValidationContext(newUser);
            var resultsList = new List<ValidationResult>();

            var result = Validator.TryValidateObject(newUser, validateContext, resultsList, true);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region Auth_ValidateUser
        [Fact]
        public async void Auth_ValidateUser()
        {
            // Arranger
            var helper = new TestHelper();
            var config = new ConfigurationBuilder().Build();
            var authBusiness = new AuthBusiness(config);
            var unitOfWork = helper.GetUnitOfWork();
            var ongDbCOntext = helper.GetOngDbContext();
            var amazon = new Mock<IAmazonS3Client>();
            var userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness, amazon.Object);

            var user = new User
            {
                Id = 1,
                FirstName = "UserFirstName01",
                LastName = "UserLastName01",
                Email = "user01@email.com",
                Password = "thisIsmyPassword",
                Photo = "user01photo.jpg",
                RoleId = 2,
                IsDeleted = false,
                LastModified = DateTime.UtcNow
            };

            // Act
            var validateUser = userBusiness.ValidateUser(user, "thisIsmyPassword");

            // Assert
            Assert.True(validateUser.IsCompletedSuccessfully);
        }
        #endregion

        #region Auth_InvalidateUser
        [Fact]
        public async void Auth_InvalidateUser()
        {
            // Arranger
            var helper = new TestHelper();
            var config = new ConfigurationBuilder().Build();
            var authBusiness = new AuthBusiness(config);
            var unitOfWork = helper.GetUnitOfWork();
            var ongDbCOntext = helper.GetOngDbContext();
            var amazon = new Mock<IAmazonS3Client>();
            var userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness, amazon.Object);

            var user = new User
            {
                Id = 1,
                FirstName = "UserFirstName01",
                LastName = "UserLastName01",
                Email = "user01@email.com",
                Password = "thisIsmyPassword",
                Photo = "user01photo.jpg",
                RoleId = 2,
                IsDeleted = false,
                LastModified = DateTime.UtcNow
            };

            // Act
            var validateUser = userBusiness.ValidateUser(user, "ThisIsOtherPassword");

            // Assert
            Assert.False(validateUser.IsFaulted);
        }
        #endregion

        #region AuthLogin_SuccessfulLogin
        [Fact]
        public async void AuthLogin_SuccessfulLogin()
        {
            //Arrange
            var helper = new TestHelper();
            var config = new ConfigurationBuilder().Build();
            var authBusiness = new AuthBusiness(config);
            var unitOfWork = helper.GetUnitOfWork();
            var ongDbCOntext = helper.GetOngDbContext();
            var amazon = new Mock<IAmazonS3Client>();
            var sendGrid = new Mock<ISendGridBusiness>();
            var userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness, amazon.Object);

            await ongDbCOntext.Database.EnsureDeletedAsync();
            await ongDbCOntext.SaveChangesAsync();

            var dto = new UserLoginDTO
            {
                Email = "usertest@mail.com",
                Password = "123"
            };
            var userController = new UserController(userBusiness, authBusiness, sendGrid.Object);

            // Act
            var result = userController.LoginUser(dto);
            var expectedResult = typeof(OkResult);

            // Assert
            Assert.True(result.IsCompletedSuccessfully);
        }
        #endregion

        #region AuthLogin_NotFound
        [Fact]
        public async void AuthLogin_NotFound()
        {
            //Arrange
            var helper = new TestHelper();
            var config = new ConfigurationBuilder().Build();
            var authBusiness = new AuthBusiness(config);
            var unitOfWork = helper.GetUnitOfWork();
            var ongDbCOntext = helper.GetOngDbContext();
            var amazon = new Mock<IAmazonS3Client>();
            var sendGrid = new Mock<ISendGridBusiness>();
            var userBusiness = new UsersBusiness(unitOfWork, ongDbCOntext, authBusiness, amazon.Object);

            await ongDbCOntext.Database.EnsureDeletedAsync();
            await ongDbCOntext.SaveChangesAsync();

            var dto = new UserLoginDTO
            {
                Email = "usertest@mail.com",
                Password = "123"
            };
            var userController = new UserController(userBusiness, authBusiness, sendGrid.Object);

            // Act
            var result = userController.LoginUser(dto) as IActionResult;
            
            // Assert
            Assert.Null(result);
        }
        #endregion
        
    }
}
