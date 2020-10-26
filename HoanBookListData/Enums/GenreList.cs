using System;
using System.Collections.Generic;
using System.Linq;

namespace HoanBookListData.Enums
{
    public static class GenreList
    {
        public static List<string> Names = new List<string> {
            "Novel",
            "Humor",
            "Fantasy",
            "Fiction",
            "Drama",
            "History"
        };

        public static List<string> GetRandomGenres(Random random = null)
        {
            random ??= new Random();
            var max = Names.Count;
            if (max == 0)
                return null;

            var count = random.Next(1, max);
            int index;
            var result = new List<string>();
            for (int i = 0; i < count; i++)
            {
                index = random.Next(1, max);
                result.Add(Names[index]);
            }
            return result.Distinct().ToList();
        }
    }
}