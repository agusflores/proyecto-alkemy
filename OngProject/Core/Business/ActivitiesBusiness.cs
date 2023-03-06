using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs.ActivitiesDTO;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Business
{
    public class ActivitiesBusiness : IActivitiesBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAmazonS3Client _amazonClient;

        public ActivitiesBusiness(IUnitOfWork unitOfWork, IAmazonS3Client amazonClient)
        {
            _unitOfWork = unitOfWork;
            _amazonClient = amazonClient;
        }

        public async Task<ActivitiesDisplayDTO> CreateActivities(ActivitiesCreateDTO activitiesDto)
        {
            if (activitiesDto != null)
            {
                var mapper = new ActivitiesMapper();
                var activity = mapper.FromActivitiesCreateDtoToActivities(activitiesDto);

                if(activitiesDto != null)
                {
                    activity.Image = await _amazonClient.UploadObject(activitiesDto.Image);
                }

                await _unitOfWork.ActivitiesRepository.Insert(activity);
                _unitOfWork.SaveChanges();

                var dto = mapper.FromActivitiesToActivitiesDisplayDto(activity);

                return dto;
            }
            return null;
        }

        public async Task<Activities> GetActivitiesById(int id)
        {
            var activityById = await _unitOfWork.ActivitiesRepository.GetById(id);
            return activityById;
        }

        public async Task<List<ActivitiesDisplayDTO>> GetAllActivities()
        {
            List<ActivitiesDisplayDTO> listActivities = new();
            var activities = await _unitOfWork.ActivitiesRepository.GetAll();
            foreach(var activity in activities)
            {
                listActivities.Add(new ActivitiesDisplayDTO { Name = activity.Name, Content = activity.Content, Image = activity.Image });
            }
            return listActivities;
        }

        public async Task<bool> RemoveActivities(int id)
        {
           Activities activity= await _unitOfWork.ActivitiesRepository.GetById(id);
            if (activity == null) return false;
            bool result = await _unitOfWork.ActivitiesRepository.Delete(activity);
            _unitOfWork.SaveChanges();
            return result;
        }

        public async Task<Activities> UpdateActivities(int id, ActivitiesCreateDTO activitiesToUpdate)
        {
            ActivitiesMapper mapper = new();
            var curretActivity = await GetActivitiesById(id);            
            var newImageActivity = await _amazonClient.UploadObject(activitiesToUpdate.Image);
            var ActivityUpdated = mapper.UpdateActivities(curretActivity,activitiesToUpdate, newImageActivity);

            _unitOfWork.ActivitiesRepository.Update(ActivityUpdated);
            _unitOfWork.SaveChanges();

            return ActivityUpdated;

        }
    }
}
