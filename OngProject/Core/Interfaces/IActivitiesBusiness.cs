using OngProject.Core.Models.DTOs.ActivitiesDTO;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IActivitiesBusiness
    {
        public Task<List<ActivitiesDisplayDTO>> GetAllActivities();
        public Task<Activities> GetActivitiesById(int id);
        public Task<ActivitiesDisplayDTO> CreateActivities(ActivitiesCreateDTO activitiesDto);
        public Task<bool> RemoveActivities(int id);
        public Task<Activities> UpdateActivities(int id, ActivitiesCreateDTO activities);
 
    }
}
