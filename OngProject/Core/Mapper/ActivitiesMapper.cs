using OngProject.Core.Models.DTOs.ActivitiesDTO;
using OngProject.Entities;
using System.Collections.Generic;

namespace OngProject.Core.Mapper
{
    public class ActivitiesMapper
    {
        public Activities FromActivitiesCreateDtoToActivities(ActivitiesCreateDTO activitiesDto)
        {
            var activities = new Activities
            {
                Name = activitiesDto.Name,
                Content = activitiesDto.Content,
            };
            return activities;
        }

        public ActivitiesDisplayDTO FromActivitiesToActivitiesDisplayDto(Activities activities)
        {
            var dto = new ActivitiesDisplayDTO
            {
                Name = activities.Name,
                Content = activities.Content,
                Image = activities.Image
            };
            return dto;
        }

        public Activities UpdateActivities(Activities currentActivities, ActivitiesCreateDTO activitiesToUpdate, string newImageActivity)
        {
            currentActivities.Name = activitiesToUpdate.Name;
            currentActivities.Image = newImageActivity;
            currentActivities.Content = activitiesToUpdate.Content;
            return currentActivities;
        }

        
    }
}
