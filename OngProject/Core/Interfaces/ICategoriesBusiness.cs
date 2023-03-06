using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.CategoriesDTO;
using OngProject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface ICategoriesBusiness
    {
        public Task<IQueryable<GetNameCategoriesDTO>> GetAllCategories();
        public Task<GetCategoriesDTO> GetCategoryById(int id);
        public Task<GetCategoriesDTO> CreateCategory(CreateCategoriesDTO category);
        public Task<bool> RemoveCategory(int id);
        public Task<Category> UpdateCategory(int id, CategoryUpdateDto categoryDTO);
    }
}
