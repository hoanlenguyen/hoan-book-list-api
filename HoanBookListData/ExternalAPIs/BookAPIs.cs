using HoanBookListData.Enums;
using HoanBookListData.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace HoanBookListData.ExternalAPIs
{
    public static class BookAPIs
    {
        private static string apiKey = "VEU9aE13Fuo1phA2RPDRHcILrprps3PG";

        public static IEnumerable<string> GetBookGenres()
        {
            Type myType = typeof(BookGenre);
            FieldInfo[] fields = myType.GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields.Select(x => x.GetValue(null).ToString());
        }

        public static async Task<List<Book>> GetBooks(DateTime? date = null)
        {
            var client = new HttpClient();
            var list = new List<Book>();
            var genres = GetBookGenres();
            date ??= DateTime.UtcNow;
            var dateStr = date?.ToString("yyyy-MM-dd");
            foreach (var genre in genres)
            {
                var url = $"https://api.nytimes.com/svc/books/v3/lists/{dateStr}/{genre}.json?api-key={apiKey}";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var books = JObject.Parse(json)["results"]["books"].ToObject<List<Book>>();
                    if (books.Count > 0)
                    {
                        foreach (var book in books)
                        {
                            book.MainGenre = genre;
                            book.Meta = new MongoDbConnection.BaseEntities.Meta();
                            book.Meta.CreatedAt = DateTime.UtcNow.AddHours(7);
                            book.SubGenres.AddRange(GenreList.GetRandomGenres());
                            book.Rate = GetRandomNumber(3, 5);
                            book.Rate = GetRandomNumber(3, 5);
                            book.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(book.Title.ToLower());
                        }
                        list.AddRange(books);
                    }
                }
            }
            return list;
        }

        private static decimal GetRandomNumber(double minimum, double maximum, Random random = null)
        {
            random ??= new Random();
            var value = random.NextDouble() * (maximum - minimum) + minimum;
            return (decimal)Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
        }
    }
}