using HoanBookListData.Models.Paging;
using Newtonsoft.Json;

namespace HoanBookListData.Models
{
    public class BookFilter : ISortable
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BookName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MainGenre { get; set; }

        //public List<string> SubGenres { get; set; } = new List<string>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string Publisher { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Rate { get; set; }

        public string SortFieldName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAscending { get; set; } = true;
    }
}