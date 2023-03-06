using OngProject.Core.Interfaces;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Mapper;
using System;
using OngProject.Core.Models.DTOs.NewsDTO;

namespace OngProject.Core.Business
{
    public class NewsBusiness : INewsBusiness
    {
        private readonly IAmazonS3Client _amazonS3Client;
        private readonly IUnitOfWork _unitOfWork;
        public NewsBusiness(IUnitOfWork unitOfWork, IAmazonS3Client amazonS3Client)
        {
            _unitOfWork = unitOfWork;
            _amazonS3Client = amazonS3Client;
        }
        public async Task<News> CreateNews(InserNewDto newDto)
        {
            if (newDto != null)
            {
                string imgUrl = await _amazonS3Client.UploadObject(newDto.Image);

                News news = new News
                {
                    Name = newDto.Name,
                    Content = newDto.Content,
                    Image = imgUrl,
                    CategoryId = newDto.CategoryId
                };
                await _unitOfWork.NewsRepository.Insert(news);
                _unitOfWork.SaveChanges();

                return news;
            }
            return null;
        }

        public async Task<IQueryable<GetNewsDto>> GetAllNews()
        {
            List<News> listNews = (List<News>) await _unitOfWork.NewsRepository.GetAll();
            List<GetNewsDto> listNewsDto = new ();
            foreach (News nws in listNews)
            {
                listNewsDto.Add(NewsMapper.NewsToGetNewsDTO(nws));
            }
            IQueryable<GetNewsDto> list = listNewsDto.AsQueryable();
            return list;
        }

        public async Task<GetNewsDto> GetNewsById(int id)
        {
           var newsById = await _unitOfWork.NewsRepository.GetById(id);
            var newsDto = NewsMapper.NewsToGetNewsDTO(newsById);
            return newsDto;
        }

        public async Task<IEnumerable<CommentGetDto>> GetNewsByIdComments(int id)
        {
            var comments = await _unitOfWork.CommentsRepository.GetAll();
            var commentForNews = comments.Where(x=>x.NewsId==id);
            
            List<CommentGetDto> listComment = new();
            foreach (var comment in commentForNews)
            {
                var commentDto = CommentsMapper.CommentsToCommentsDTO(comment);
                listComment.Add(commentDto);
            }
            return listComment;
        }

        public async Task<bool> RemoveNews(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetById(id);
            if(news != null)
            {
                try
                {
                    await _unitOfWork.NewsRepository.Delete(news);
                    _unitOfWork.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            return false;
        }

        public async Task<GetNewsDto> UpdateNews(int id, InserNewDto newsDto)
        {
            News ExistNews = await _unitOfWork.NewsRepository.GetById(id);
            if (ExistNews == null) throw new Exception("News Not Found.");

            ExistNews.Name= newsDto.Name;
            ExistNews.Content = newsDto.Content;
            ExistNews.Image = await _amazonS3Client.UploadObject(newsDto.Image);
            ExistNews.CategoryId = newsDto.CategoryId;
            ExistNews.LastModified = DateTime.UtcNow;
            
             _unitOfWork.NewsRepository.Update(ExistNews);
             _unitOfWork.SaveChanges();
            GetNewsDto GetNewDto = NewsMapper.NewsToGetNewsDTO(ExistNews);
            return GetNewDto;
        }
    }
}
