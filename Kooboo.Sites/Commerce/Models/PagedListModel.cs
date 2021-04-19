using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models
{
    public class PagedListModel<T>
    {
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public long PageCount { get; set; }
        public List<T> List { get; set; }

        public void SetPageInfo(PagingQueryModel paging, long count)
        {
            PageIndex = paging.Index;
            PageSize = paging.Size;
            PageCount = count / PageSize + (count % PageSize > 0 ? 1 : 0);
            if (PageCount == 0) PageCount = 1;
            if (PageIndex > PageCount) PageIndex = PageCount;
        }

        public long GetOffset()
        {
            return (PageIndex - 1) * PageSize;
        }
    }
}
