using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbConnection.BaseEntities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoanBookListData.Models
{
    public class Book : MongoMetaEntity
    {
        [BsonElement("Name")]
        [JsonProperty("title")]
        public string Title { get; set; }

        public string MainGenre { get; set; }

        public List<string> SubGenres { get; set; } = new List<string>();

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("book_image")]
        public string BookCoverUrl { get; set; }

        public decimal Rate { get; set; }

        public int TotalLikes { get; set; }
    }

    public static class BookExtensions
    {
        public static BookIndex ToBookIndex(this Book book)
        {
            return new BookIndex
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                MainGenre = book.MainGenre,
                Publisher = book.Publisher,
                BookCoverUrl = book.BookCoverUrl,
                Rate = book.Rate,
                TotalLikes = book.TotalLikes
            };
        }

        public static IEnumerable<BookIndex> ToBookIndex(this IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                yield return book.ToBookIndex();
            }
        }
    }
}