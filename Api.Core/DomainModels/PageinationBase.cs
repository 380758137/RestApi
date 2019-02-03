using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Core.DomainModels
{
    public class PageinationBase
    {
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 0;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string OrderBy { get; set; }

        public int MaxPageSize { get; set; } = 100;

        //public PageinationBase(int pageSize,int pageIndex,int maxPageSize)
        //{
        //    PageIndex = pageIndex;
        //    PageSize = pageSize;
        //    MaxPageSize = maxPageSize;
        //}

        //public PageinationBase Clone()
        //{
        //    PageinationBase pageinationBase = new PageinationBase();
        //    return pageinationBase;
        //}
    }
}
