using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.TestimonialDTO;
using OngProject.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OngProject.Test.Core.Controllers
{
    public class TestimonialsControllerTest
    {

        // Comprueba que devuelva la lista correctamente
        [Fact]
        public async void GetAllTestimonials_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var testimonialBusinessMock = new Mock<ITestimonialsBusiness>();

            testimonialBusinessMock.Setup(s => s.GetAll())
                                   .Returns(Task.FromResult<IQueryable<TestimonialsDTO>>(new List<TestimonialsDTO>() { new TestimonialsDTO { Content = "Test" } }.AsQueryable()));


            var controller = new TestimonialsController(testimonialBusinessMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var result = await controller.GetAllTestimonials(1, 1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

        }

        // Comprueba que si no existe una lista, devuelva un NotFound

        [Fact]
        public async void GetAllTestimonials_OnSuccess_ReturnsStatusCode404()
        {
            // Arrange
            var testimonialBusinessMock = new Mock<ITestimonialsBusiness>();
            testimonialBusinessMock.Setup(s => s.GetAll()).Returns(Task.FromResult<IQueryable<TestimonialsDTO>>(null));

            var controller = new TestimonialsController(testimonialBusinessMock.Object)
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await controller.GetAllTestimonials(1, 1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

        }

        // Comprueba la creacion correctamante de un nuevo Testimonio

        [Fact]

        public async void CreateTestimonials_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness= new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var testimonialInsertDto = new TestimonialInsertDto { Content = "contenido", Name = "testimonio1" };
            var controller = new TestimonialsController(testimonialsBusiness);
            // Act
            var result = await controller.CreateTestimonials(testimonialInsertDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        // Comprueba que devuelva un NotFound cuando el Dto llega vacio


        [Fact]
        public async void CreateTestimonials_OnSuccess_ReturnsStatusCode404()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            TestimonialInsertDto testimonialInsertDto = null;
            var controller = new TestimonialsController(testimonialsBusiness);
            // Act
            var result = await controller.CreateTestimonials(testimonialInsertDto);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        // Comprueba que se elimine exitosamente un testiminio con un id existente

        [Fact]
        public async void DeleteTestimonials_OnSuccess_ReturnStatusCode200()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var controller = new TestimonialsController(testimonialsBusiness);

            // Act
            await controller.RemoveTestimonials(2);
            var result = await unitOfWork.TestimonialsRepository.GetById(2);
            // Assert
            result.IsDeleted.Should().BeTrue();

        }

        // Comprueba que se retorne un NotFound si se intenta eliminar un Testimonio con un Id inexistente en la base de datos

        [Fact]
        public async void DeleteTestimonials_OnSuccess_ReturnStatusCode404()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var controller = new TestimonialsController(testimonialsBusiness);

            // Act
            var result = await controller.RemoveTestimonials(2000);
            // Assert
            result.Should().BeOfType<NotFoundResult>();

        }

        // Comprueba que se realice con existo la Actualizacion de un testimonio cuando se envia correctamente el Id de un usuario
        // en este caso comprueba que el cambio del contenido del testimonio, sea el correcto.

        [Fact]
        public async void UpdateTestimonials_OnSuccess_ReturnStatusCode200()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var controller = new TestimonialsController(testimonialsBusiness);
            var TestimonialUpdateDto = new TestimonialUpdateDto { Content = "El contenido fue modificado"};

            // Act
            await controller.UpdateTestimonials(1, TestimonialUpdateDto);
            var result = await unitOfWork.TestimonialsRepository.GetById(1);

            // Assert
            result.Content.Should().Be("El contenido fue modificado");

        }

        // comprueba que no se pueda realizar la actualizacion de un Testimonio inexistente por su Id

        [Fact]
        public async void UpdateTestimonials_OnSuccess_ReturnStatusCode404()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var controller = new TestimonialsController(testimonialsBusiness);
            var TestimonialUpdateDto = new TestimonialUpdateDto { Content = "El contenido fue modificado" };

            // Act
            var result = await controller.UpdateTestimonials(3231, TestimonialUpdateDto);
            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();

        }

        // Comprueba la existencia del testimonio por el Id, retornando un 200

        [Fact]

        public async void GetByIdTestimonials_OnSuccess_ReturnStatusCode200()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var controller = new TestimonialsController(testimonialsBusiness);

            // Act
            var result = await controller.GetTestimonialsById(3);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        // Comprueba que retorne un NotFound cuando el testimonio es inexistente por su Id

        [Fact]
        public async void GetByIdTestimonials_OnSuccess_ReturnStatusCode404()
        {
            // Arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            ITestimonialsBusiness testimonialsBusiness = new TestimonialsBusiness(unitOfWork, amazonS3Mock.Object);

            var controller = new TestimonialsController(testimonialsBusiness);

            // Act
            var result = await controller.GetTestimonialsById(23451);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
