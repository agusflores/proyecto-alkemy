using OngProject.Core.Models.DTOs.NewsDTO;
using OngProject.Entities;

namespace OngProject.Core.Mapper
{
    public class NewsMapper
    {
        public static GetNewsDto NewsToGetNewsDTO(News news)
        {
            GetNewsDto getNewsDtoDto = new()
            {
                Name = news.Name,
                Content = news.Content,
                Image = news.Image,
            };
            return getNewsDtoDto;
        }
    }
}
