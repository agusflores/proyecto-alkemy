using OngProject.Core.Models.DTOs.CategoriesDTO;
using OngProject.Entities;

namespace OngProject.Core.Mapper
{
    public class CategoriesMapper
    {
        public Category CreateCategoryByDTO(CreateCategoriesDTO categoriesDTO)
        {
            Category category = new()
            {
                Name = categoriesDTO.Name,
                Description = categoriesDTO.Description,
            };
            return category;
        }

        public GetCategoriesDTO GetCategories(Category category)
        {
            GetCategoriesDTO categoriesDTO = new()
            {
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            };
            return categoriesDTO;
        }
    }
}
