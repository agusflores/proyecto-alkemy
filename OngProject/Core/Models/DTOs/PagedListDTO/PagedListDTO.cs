using OngProject.Core.Helper;
using OngProject.Entities;
using System.Collections.Generic;

namespace OngProject.Core.Models.DTOs.PagedListDTO
{
    public class PagedListDTO<T>
    {
        
        public string PreviousPage { get; set; }
        public string NextPage { get; set; }
        public int currentPage { get; set; }
        public int totalPage { get; set; }
        public int pageSize { get; set; }
        public PagedListHelper<T> Values { get; set; }
        
        public PagedListDTO(PagedListHelper<T> entity, string host, string path)
        {
            pageSize = entity.PageSize;
            totalPage = entity.TotalPage;
            currentPage = entity.CurrentPage;            
            PreviousPage = entity.HasPrevious ? $"https://{host}{path}?numberPage={entity.CurrentPage - 1}&quantityPage={entity.PageSize}" : null;
            NextPage = entity.HasNext ? $"https://{host}{path}?numberPage={entity.CurrentPage + 1}&quantityPage={entity.PageSize}" : null;            
            Values = entity;
        }
    }
}
