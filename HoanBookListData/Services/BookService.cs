﻿using HoanBookListData.Extensions;
using HoanBookListData.ExternalAPIs;
using HoanBookListData.Models;
using HoanBookListData.Models.Paging;
using HoanBookListData.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoanBookListData.Services
{
    public class BookService
    {
        private string ConnectionString { get; }

        private const string _BookName = "Books";

        private const string _UserBookName = "UserBooks";

        private readonly IMongoCollection<Book> _books;

        private readonly IMongoCollection<UserBook> _userBooks;

        public BookService(MongoDbContext mongoDb)
        {
            ConnectionString = mongoDb.ConnectionString;

            _books = mongoDb.Database.GetCollection<Book>(_BookName);

            _userBooks = mongoDb.Database.GetCollection<UserBook>(_UserBookName);
        }

        public string GetConnectionString() => ConnectionString;

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            return _books.Find<Book>(book => book.Id == id).FirstOrDefault();
        }

        public IEnumerable<Book> GetByCategory(string genre) =>
           _books.Find<Book>(book => book.MainGenre == genre).ToEnumerable();

        public IEnumerable<Book> FindByNamekey(string key) =>
           _books.AsQueryable().Where(x => x.Title.Contains(key));

        public Book Create(Book book)
        {
            book.Id = null;
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);

        public async Task<bool> BulkInsert(DateTime? date = null)
        {
            var books = await BookAPIs.GetBooks(date);
            _books.InsertMany(books);
            return true;
        }

        public bool BulkDelete()
        {
            _books.DeleteMany(x => true);
            return true;
        }

        public bool BulkUpdate(string oldName, string newName)
        {
            var filter = Builders<Book>.Filter.Eq(x => x.Author, oldName);
            var update = Builders<Book>.Update.Set(x => x.Author, newName);
            _books.UpdateMany(filter, update);
            return true;
        }

        public bool LikeBook(string userId, string bookId, bool like)
        {
            var book = _books.Find(x => x.Id == bookId).FirstOrDefault();
            if (book == null)
                return false;

            var userBook = _userBooks.Find(x => x.UserId == userId && x.BookId == userId)
                                         .FirstOrDefault();

            if (userBook != null)
            {
                userBook.IsLiked = like;
                _userBooks.ReplaceOne(x => x.Id == userBook.Id, userBook);
            }
            else
            {
                _userBooks.InsertOne(new UserBook
                {
                    UserId = userId,
                    BookId = bookId,
                    IsLiked = like
                });
            }

            if (like)
                book.TotalLikes++;
            else
                _ = book.TotalLikes > 0 ? book.TotalLikes-- : 0;

            _books.ReplaceOne(x => x.Id == book.Id, book);

            return true;
        }

        public bool BookmarkBook(string userId, string bookId, bool bookmark)
        {
            var book = _books.Find(x => x.Id == bookId).FirstOrDefault();
            if (book == null)
                return false;

            var userBook = _userBooks.Find(x => x.UserId == userId && x.BookId == userId)
                                         .FirstOrDefault();

            if (userBook != null)
            {
                userBook.IsBookmarked = bookmark;
                _userBooks.ReplaceOne(x => x.Id == userBook.Id, userBook);
            }
            else
            {
                _userBooks.InsertOne(new UserBook
                {
                    UserId = userId,
                    BookId = bookId,
                    IsBookmarked = bookmark
                });
            }

            return true;
        }

        public PagingResult PageIndexingItems(string userId, PagingRequest request, BookFilter filter)
        {
            if (request.CurrentPage == null || request.ItemsPerPage == null)
            {
                var result = FilterBookCollection(filter, userId);
                return new PagingResult
                {
                    Items = result.Items,
                    MaxItemCount = result.Count
                };
            }

            var skipItems = (request.CurrentPage.Value - 1) * request.ItemsPerPage.Value;

            var (item, count) = FilterBookCollection(filter, userId, skipItems, request.ItemsPerPage.Value);

            return new PagingResult
            {
                CurrentPage = request.CurrentPage,
                ItemsPerPage = request.ItemsPerPage,
                MaxItemCount = count,
                Items = item
            };
        }

        private (IEnumerable<BookIndex> Items, int Count) FilterBookCollection(BookFilter filter, string userId = null, int? skip = null, int? take = null)
        {
            var items = _books.AsQueryable()
                              .WhereIf(filter.Title.HasValue(), x => x.Title.Contains(filter.Title))
                              .WhereIf(filter.Author.HasValue(), x => x.Author.Contains(filter.Author))
                              .Where(x => filter.Rate == null || x.Rate >= filter.Rate);

            if (filter.SortBy.HasValue())
            {
                filter.SortBy = char.ToUpper(filter.SortBy[0]) + filter.SortBy.Substring(1);
                items = items.OrderBy(filter.SortBy, filter.IsAsc);
            }

            var count = items.Count<Book>();

            if (skip.HasValue)
                items = items.Skip(skip.Value);

            if (take.HasValue)
                items = items.Take(take.Value);

            if (userId.HasValue())
            {
                return (ConvertToBookIndex(userId, items), count);
            }

            return (items.ToBookIndex(), count);
        }

        private IEnumerable<BookIndex> ConvertToBookIndex(string userId, IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                var userBook = _userBooks.Find(x => x.UserId == userId && x.BookId == book.Id)
                                         .FirstOrDefault();

                var bookIndex = book.ToBookIndex();

                if (userBook != null)
                {
                    bookIndex.IsBookmarked = userBook.IsBookmarked;
                    bookIndex.IsLiked = userBook.IsLiked;
                }

                yield return bookIndex;
            }
        }
    }
}