using Microsoft.AspNetCore.Http;
using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models.DTOs.CommentsDTO;
using OngProject.Core.Models.DTOs.UserDTO;
using OngProject.DataAccess;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Core.Business
{
    public class CommentsBusiness : ICommentsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OngDbContext _dbContext;

        public CommentsBusiness(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, OngDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteComments(Comments comments)
        {
            try
            {
                await _unitOfWork.CommentsRepository.Delete(comments);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<CommentGetDto>> GetAll()
        {
            var comments = await _unitOfWork.CommentsRepository.GetAll();
            var commentOrd = comments.OrderByDescending(x => x.LastModified);
            IEnumerable<Comments> ordenby = commentOrd;

            List<CommentGetDto> listComment = new();
            
            foreach (var comment in ordenby)
            {
                CommentGetDto commentDto = new()
                {
                    Body = comment.Body
                };
                listComment.Add(commentDto);
            }

            return listComment;
        }

        public async Task<Comments> GetById(int id)
        {
            var comments = await _unitOfWork.CommentsRepository.GetById(id);

            return comments;
        }

        public async Task<Comments> Insert(CommentCreateDTO commentCreateDTO)
        {
            var mapper = new CommentsMapper();
            var comment = mapper.CommentCreateDTOToComments(commentCreateDTO);

            var user = await _unitOfWork.UserRepository.GetById(comment.UserId);

            if(user != null)
            {
                var news = await _unitOfWork.NewsRepository.GetById(comment.NewsId);

                if (news != null)
                {
                    await _unitOfWork.CommentsRepository.Insert(comment);
                    _unitOfWork.SaveChanges();
                    return comment;
                }

                return null;
            }
            return null;
        }

        public async Task<Comments> Update(int id, CommentUpdateDto Comments)
        {
            try
            {
                var existing = await _unitOfWork.CommentsRepository.GetById(id);

                if (existing == null)
                    throw new Exception("Comment Not Found.");

                GetUser(out int userId, out string userRole);

                if (existing.UserId != userId && userRole != "Admin")
                    throw new Exception("Operation Forbidden.");

                existing.Body = Comments.Body;

                _unitOfWork.CommentsRepository.Update(existing);
                _unitOfWork.SaveChanges();

                return existing;
            }
            catch (Exception er)
            { 
                throw new Exception(er.Message);
            }
        }

        private void GetUser(out int userId, out string userRole)
        {
            try
            {
                userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
    }
}
