using System;
using System.Collections;

namespace HoanBookListData.Models.Paging
{
    public class PagingResult
    {
        public long MaxItemCount { get; set; }

        public int? CurrentPage { get; set; }

        public int? ItemsPerPage { get; set; }

        public IEnumerable Items { get; set; }

        public bool HasNextPage => CurrentPage != null && ItemsPerPage != null ?
                                    MaxItemCount >= CurrentPage * ItemsPerPage
                                    : false;

        public bool HasPreviousPage => CurrentPage.GetValueOrDefault() > 1;

        public int? MaxPage => ItemsPerPage != null ? (int)Math.Ceiling((double)MaxItemCount / ItemsPerPage.Value) : 1;
    }
}