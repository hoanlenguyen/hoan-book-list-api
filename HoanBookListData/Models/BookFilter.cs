using HoanBookListData.Models.Paging;
using Newtonsoft.Json;

namespace HoanBookListData.Models
{
    public class BookFilter : ISortable
    {
        public string Title { get; set; }

        public string MainGenre { get; set; }

        public string Author { get; set; }

        public decimal? Rate { get; set; }

        public string SortBy { get; set; }

        public bool IsAsc { get; set; } = true;
    }
}