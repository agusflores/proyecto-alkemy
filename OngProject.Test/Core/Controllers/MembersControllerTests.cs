using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
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
    public class MembersControllerTests
    {
        // Do not use real data instead mock business logic in order to return Ok - 200 status code.
        [Fact]
        public async void GetAllMembers_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var membersBusinessMock = new Mock<IMembersBusiness>();

            membersBusinessMock
                .Setup(m => m.GetAllMembers())
                .Returns(Task.FromResult<IQueryable<MembersDTO>>(new List<MembersDTO>() { new MembersDTO { Description = "test" } }.AsQueryable()));

            var sut = new MembersController(membersBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllMembers(1, 1);

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        // Test in which there is no Members in database.
        [Fact]
        public async void GetAllMembers_OnFail_ReturnsStatusCode404()
        {
            // Arrange
            var membersBusinessMock = new Mock<IMembersBusiness>();

            membersBusinessMock
                .Setup(m => m.GetAllMembers())
                .Returns(Task.FromResult<IQueryable<MembersDTO>>(null));

            var sut = new MembersController(membersBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllMembers(1, 1);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        // Use real data from in-memory database
        [Fact]
        public async void CreateMember_OnSuccess_ReturnsPagedListDTO_OF_MembersDTO()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = (OkObjectResult)await sut.GetAllMembers(1, 2);

            // Assert
            result
                .Value
                .Should()
                .BeOfType<PagedListDTO<MembersDTO>>();
        }

        [Fact]
        public async void CreateMember_OnSucess_ReturnsStatusCode200()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);

            // Act
            // MemberDTO with a set name leads to success.
            var result = await sut.CreateMember(new MembersDTO { Description = "Test", Name = "Test_212" });

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CreateMember_OnFail_ReturnsStatusCode404()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);

            // Act
            // MemberDTO without a set name leads to fail.
            var result = await sut.CreateMember(new MembersDTO {  });

            // Assert
            result
                .Should()
                .BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async void CreateMember_OnSuccess_SaveAMemberInDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);

            // Act
            await sut.CreateMember(new MembersDTO { Name = "unique_test_name" }); // Id = 6
            // Id = 6 matchs new member created above ↑↑↑
            var result = await unitOfWork.MembersRepository.GetById(6);

            // Assert
            result
                .Name
                .Should()
                .Be("unique_test_name");
        }
        
        [Fact]
        public async void RemoveMember_OnSuccess_SoftDeleteAMemberFromDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);

            // Act
            await sut.RemoveMember(5); // Id = 5
            // Members { Id = 5, Name = "Members05", Image = "url_image_members05", Description = "Description members05", FacebookUrl = "url_facebook_members05", InstagramUrl = "url_instagram_members05", LinkedinUrl = "url_linkedIn_members05", IsDeleted = false, LastModified = DateTime.UtcNow }
            var result = await unitOfWork.MembersRepository.GetById(5);

            // Assert
            result
                .IsDeleted
                .Should()
                .BeTrue();
        }
        
        [Fact]
        public async void UpdateMember_OnSuccess_UpdateAMemberInDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);

            // Act
            await sut.UpdateMembers(5, new MembersUpdateDTO { Description = "new_description_modified" }); // Id = 5
            // Members { Id = 5, Name = "Members05", Image = "url_image_members05", Description = "Description members05", FacebookUrl = "url_facebook_members05", InstagramUrl = "url_instagram_members05", LinkedinUrl = "url_linkedIn_members05", IsDeleted = false, LastModified = DateTime.UtcNow }
            var result = await unitOfWork.MembersRepository.GetById(5);

            // Assert
            result
                .Description
                .Should()
                .Be("new_description_modified");
        }
        
        [Fact]
        public async void GetMemberById_OnSuccess_GetAMemberFromDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IMembersBusiness membersBussiness = new MembersBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new MembersController(membersBussiness);

            // Act
            await sut.GetMemberById(5); // Id = 5
            // Members { Id = 5, Name = "Members05", Image = "url_image_members05", Description = "Description members05", FacebookUrl = "url_facebook_members05", InstagramUrl = "url_instagram_members05", LinkedinUrl = "url_linkedIn_members05", IsDeleted = false, LastModified = DateTime.UtcNow }
            var result = await unitOfWork.MembersRepository.GetById(5);

            // Assert
            result
                .Description
                .Should()
                .NotBeNull();

            result
                .Id
                .Should()
                .Be(5);
        }

        
    }
}
