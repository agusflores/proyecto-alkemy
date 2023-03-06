
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Business;
using OngProject.Core.Interfaces;
using OngProject.Core.Models.DTOs.ActivitiesDTO;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OngProject.Test.Core.Controllers
{
    public class ActivitiesControllerTest
    {
        [Fact]
        public async void GetAllActivities_OnSuccess_RetornaLista_de_actividadesDisplayDto_Cuando_hay_Datos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador =  new ActivitiesController(activitiesBussiness);

            //Actuar-act
            var result = (OkObjectResult) await controlador.GetAllActivities();

            //confirmar-assert
            result.Value.Should().BeOfType<List<ActivitiesDisplayDTO>>();
        }
        [Fact]
        public async void CreateActivities_OnSuccess_Return_StatusCode200_Con_Una_Actividad_Cuando_Los_Datos_Son_Validos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
            ActivitiesCreateDTO activitiesCreateDTO = new ActivitiesCreateDTO
            {
                Name="Activity 05",
                Content= "this is Activities 05"
            };
            //Actuar-act
            var result = (OkObjectResult)await controlador.CreateActivities(activitiesCreateDTO);

            //confirmar-assert
            result.Value.Should().BeOfType<ActivitiesDisplayDTO>();
        }
        [Fact]
        public async void CreateActivities_OnFail_Return_StatusCode404NotFound_Cuando_Los_Datos_No_Son_Validos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
            ActivitiesCreateDTO activitiesCreateDTO = new ActivitiesCreateDTO();
            activitiesCreateDTO = null;
            //Actuar-act
            var result = await controlador.CreateActivities(activitiesCreateDTO);

            //confirmar-assert
            result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async void RemoveActivities_OnSuccess_Return_StatusCode200_Cuando_Hay_Datos_Validos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
            //Actuar-act
            var result = await controlador.RemoveActivities(4);//ID=4

            //confirmar-assert
            result.Should().BeOfType<OkResult>();
        }
        [Fact]
        public async void RemoveActivities_OnFail_Return_StatusCode404NotFound_Cuando_Noo_Hay_Datos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
            //Actuar-act
            var result = await controlador.RemoveActivities(5);//ID=4

            //confirmar-assert
            result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async void UpdateActivities_OnSuccess_Return_StatusCode200_Con_Un_ActivitiesDisplayDto_Cuando_Hay_Datos_Validos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
            ActivitiesCreateDTO activitiesDisplayDTO = new ActivitiesCreateDTO
            {
                Name = "Activity 04",
                Content = "this is Activities 05"
            };
            //Actuar-act
            var result = (OkObjectResult) await controlador.UpdateActivities(4,activitiesDisplayDTO);//ID=4

            //confirmar-assert
            result.Value.Should().BeOfType<ActivitiesDisplayDTO>();
        }
        [Fact]
        public async void UpdateActivities_OnFail_Return_StatusCode404NotFound_Cuando_No_Encuentra_ID()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
            ActivitiesCreateDTO activitiesDisplayDTO = new ActivitiesCreateDTO
            {
                Name = "Activity 04",
                Content = "this is Activities 05"
            };
            //Actuar-act
            var result = (NotFoundObjectResult)await controlador.UpdateActivities(5, activitiesDisplayDTO);//ID=5

            //confirmar-assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void GetActivitiesById_OnSuccess_Return_StatusCode200_Con_Una_ActivitiesDisplayDto_Cuando_los_Datos_Son_Validos()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);
           
            //Actuar-act
            var result = (OkObjectResult)await controlador.GetActivitiesById(4);//ID=4

            //confirmar-assert
            result.Value.Should().BeOfType<ActivitiesDisplayDTO>();
        }

        [Fact]
        public async void GetActivitiesById_OnFail_Return_StatusCode404NotFound_Cuando_No_Encuentra_ID()
        {
            //preparando-arrange
            IUnitOfWork unitOfWork = new TestHelper().GetUnitOfWork();
            var amazonS3Mock = new Mock<IAmazonS3Client>();
            IActivitiesBusiness activitiesBussiness = new ActivitiesBusiness(unitOfWork, amazonS3Mock.Object);
            var controlador = new ActivitiesController(activitiesBussiness);

            //Actuar-act
            var result = await controlador.GetActivitiesById(6);//ID=6

            //confirmar-assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
