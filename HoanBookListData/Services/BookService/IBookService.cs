using HoanBookListData.Models;
using HoanBookListData.Models.Paging;
using System.Collections.Generic;

namespace HoanBookListData.Services
{
    public interface IBookService
    {
        public List<Book> Get();
        public Book Get(string id);
        public bool LikeBook(string userId, string bookId, bool like);
        public bool BookmarkBook(string userId, string bookId, bool bookmark);
        public PagingResult GetUiList(string userId, PagingRequest request, BookFilter filter);
    }
}