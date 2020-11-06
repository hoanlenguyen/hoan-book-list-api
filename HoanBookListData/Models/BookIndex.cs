namespace HoanBookListData.Models
{
    public class BookIndex
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string MainGenre { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string BookCoverUrl { get; set; }

        public decimal Rate { get; set; }

        public bool IsLiked { get; set; } = false;

        public int TotalLikes { get; set; }

        public bool IsBookmarked { get; set; } = false;
    }
}