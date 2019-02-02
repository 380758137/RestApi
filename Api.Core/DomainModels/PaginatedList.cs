using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api.Core.DomainModels
{
    public class PaginatedList<T>:List<T> where T :class
    {
        public PageinationBase PageinationBase { get; }

        public int TotalItemsCount { get; set; }

        public int PageCount => TotalItemsCount / PageinationBase.PageSize + (TotalItemsCount % PageinationBase.PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PageinationBase.PageIndex > 0;

        public bool HasNext => PageinationBase.PageIndex < PageCount - 1;

        public PaginatedList(int pageIndex,int pageSize,int toltalItemsCount,IEnumerable<T> data)
        {
            PageinationBase = new PageinationBase
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            TotalItemsCount = toltalItemsCount;
            AddRange(data);
        }

        //public static PaginatedList<T> Create(IQueryable<T> source,int pageIndex,int pageSize)
        //{
        //    var count = source.Count();
        //    var items = source.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        //    return new PaginatedList<T>(pageIndex, pageSize, count, items);
        //}
    }
}
