using OngProject.Core.Interfaces;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace OngProject.Core.Helper
{
    public class PagedListHelper<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPage);

        public PagedListHelper(List<T> Items,int count, int pageNumber, int pageSize)
        {            
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(Items);
        }

        public static PagedListHelper<T> Create(IQueryable<T> sourse, int pageNumber, int pageSize)
        {
            var count = sourse.Count();
            var items = sourse.Skip((pageNumber -1)* pageSize).Take(pageSize).ToList();
            return new PagedListHelper<T>(items,count, pageNumber, pageSize);
        }
    }

}
