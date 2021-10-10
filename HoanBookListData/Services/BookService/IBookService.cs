using HoanBookListData.Models;
using HoanBookListData.Models.Paging;
using System.Collections.Generic;

namespace HoanBookListData.Services
{
    public interface IBookService
    {
        List<Book> Get();
        Book Get(string id);
        bool LikeBook(string userId, string bookId, bool like);
        bool BookmarkBook(string userId, string bookId, bool bookmark);
        PagingResult GetUiList(string userId, PagingRequest request, BookFilter filter);
        string GetConnectionString();
    }
}