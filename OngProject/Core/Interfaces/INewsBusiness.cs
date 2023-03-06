using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.NewsDTO;
using OngProject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface INewsBusiness
    {
        Task<IQueryable<GetNewsDto>> GetAllNews();
        Task<GetNewsDto> GetNewsById(int id);
        Task<IEnumerable<CommentGetDto>> GetNewsByIdComments(int id);
        Task<News> CreateNews(InserNewDto newDto);
        public Task<bool> RemoveNews(int id);
        public Task<GetNewsDto> UpdateNews(int id, InserNewDto newsDto);
    }
}
