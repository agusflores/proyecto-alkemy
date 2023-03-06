using OngProject.Core.Models.DTOs;
using OngProject.Entities;

namespace OngProject.Core.Mapper
{
    public class CommentsMapper
    {
        public static CommentGetDto CommentsToCommentsDTO(Comments comments)
        {
            CommentGetDto commentGetDto = new ()
            {
                Body = comments.Body,
             
            };
            return commentGetDto;
        }

        public Comments CommentCreateDTOToComments(CommentCreateDTO comment)
        {
            var comments = new Comments()
            {
                Body = comment.Body,
                NewsId = comment.NewsId,
                UserId = comment.UserId
            };
            return comments;
        }
    }
}
