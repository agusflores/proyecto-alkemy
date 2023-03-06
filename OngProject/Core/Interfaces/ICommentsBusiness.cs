using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.CommentsDTO;
using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface ICommentsBusiness
    {
        Task<IEnumerable<CommentGetDto>> GetAll();
        Task<Comments> GetById(int id);
        Task<Comments> Insert(CommentCreateDTO commentCreateDTO);
        Task<bool> DeleteComments(Comments comments);
        Task<Comments> Update(int id, CommentUpdateDto Comments);
    }
}
