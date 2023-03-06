using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.NewsDTO;
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
    public class NewsControllerTests
    {
        // Do not use real data instead mock business logic in order to return Ok - 200 status code.
        [Fact]
        public async void GetAllNews_OnSuccess_ReturnsStatusCode200()
        {
            var newsBusinessMock = new Mock<INewsBusiness>();

            newsBusinessMock
                .Setup(m => m.GetAllNews())
                .Returns(Task.FromResult<IQueryable<GetNewsDto>>(new List<GetNewsDto>() { new GetNewsDto { Content = "test text",
                    Name = "test name", Image = "test image" } }.AsQueryable()));

            var sut = new NewsController(newsBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllNews(1, 1);

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        // Test in which there is no News in database.
        [Fact]
        public async void GetAllNews_OnFail_ReturnsStatusCode404()
        {
            // Arrange
            var newsBusinessMock = new Mock<INewsBusiness>();

            newsBusinessMock
                .Setup(m => m.GetAllNews())
                .Returns(Task.FromResult<IQueryable<GetNewsDto>>(null));

            var sut = new NewsController(newsBusinessMock.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await sut.GetAllNews(1, 1);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }


        // Use real data from in-memory database
        [Fact]
        public async void CreateNews_OnSuccess_ReturnsPagedListDTO_OF_getNewsDto()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = (OkObjectResult)await sut.GetAllNews(1, 2);

            // Assert
            result
                .Value
                .Should()
                .BeOfType<PagedListDTO<GetNewsDto>>();
        }


        [Fact]
        public async void CreateNews_OnSucess_ReturnsStatusCode200()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);

            //act
            var result = await sut.CreateNews(new InserNewDto { CategoryId = 1, Content = "Content test", Name = "Test name" });

            // Assert
            result
                .Should()
                .BeOfType<OkObjectResult>();
        }


        [Fact]
        public async void CreateNews_OnFail_ReturnsStatusCode404()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);
            InserNewDto insertNewDto = null;

            // Act
            // InserNewDto null leads to fail.
            var result = await sut.CreateNews(insertNewDto);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void RemoveNews_OnSuccess_SoftDeleteANewsFromDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);

            // Act
            await sut.RemoveNews(1); // Id = 1

            // News { Id = 1, Name = "News01", Image = "url_img", Content = "this is a content - news 01", CategoryId = 2, IsDeleted = false, LastModified = DateTime.UtcNow }
            var result = await unitOfWork.NewsRepository.GetById(1);

            // Assert
            result
                .IsDeleted
                .Should()
                .BeTrue();
        }

        [Fact]
        public async void UpdateNews_OnSuccess_UpdateANewsInDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);

            // Act
            await sut.UpdateNews(1, new InserNewDto { Content = "Test Update Content" }); // Id = 1
            // News { Id = 1, Name = "News01", Image = "url_img", Content = "this is a content - news 01", CategoryId = 2, IsDeleted = false, LastModified = DateTime.UtcNow }
            var result = await unitOfWork.NewsRepository.GetById(1);

            // Assert
            result
                .Content
                .Should()
                .Be("Test Update Content");
        }


        [Fact]
        public async void GetNewsById_OnSuccess_GetANewsFromDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);

            // Act

            var result = (OkObjectResult)await sut.GetNewById(1);
            // Assert
            result
                .Value
                .Should()
                .BeOfType<GetNewsDto>();
        }

        [Fact]
        public async void GetCommentsByNewsId_OnSuccess_GetAListOfCommentsFromDatabase()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);

            // Act
            var result = (OkObjectResult)await sut.GetNewByIdComments(1);

            // Assert
            result
                .Value
                .Should()
                .BeOfType<List<CommentGetDto>>();
        }

        [Fact]
        public async void GetCommentsByNewsId_OnSucces_ReturnAnEmptyList()
        {
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            INewsBusiness newsBussiness = new NewsBusiness(unitOfWork, amazonS3Mock.Object);

            var sut = new NewsController(newsBussiness);

            // Act
            //There are no comments with newsId = 4
            var result = (OkObjectResult)await sut.GetNewByIdComments(4);

            // Assert
            result
                .Value
                .Should()
                .BeOfType<List<CommentGetDto>>();
        }
    }
}
