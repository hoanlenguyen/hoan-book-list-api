using HoanBookListData.Models.BaseEntities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoanBookListData.Models
{
    public class Book : MongoEntity
    {
        [BsonElement("Name")]
        [JsonProperty("title")]
        public string BookName { get; set; }

        public string MainGenre { get; set; }

        public List<string> SubGenres { get; set; } = new List<string>();

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("book_image")]
        public string BookCoverUrl { get; set; }

        public decimal Rate { get; set; }
    }
}