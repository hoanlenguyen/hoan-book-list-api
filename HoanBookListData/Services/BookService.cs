using HoanBookListData.Extensions;
using HoanBookListData.ExternalAPIs;
using HoanBookListData.Models;
using HoanBookListData.Models.Paging;
using HoanBookListData.MongoDb;
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

        private const string _collectionName = "Books";

        private readonly IMongoCollection<Book> _books;

        public BookService(MongoDbContext mongoDb)
        {
            ConnectionString = mongoDb.ConnectionString;

            _books = mongoDb.Database.GetCollection<Book>(_collectionName);
        }

        public string GetConnectionString() => ConnectionString;

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public IEnumerable<Book> GetByCategory(string genre) =>
           _books.Find<Book>(book => book.MainGenre == genre).ToEnumerable();

        public IEnumerable<Book> FindByNamekey(string key) =>
           _books.AsQueryable().Where(x => x.BookName.Contains(key));

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

        public PagingResult PageIndexingItems(PagingRequest request, BookFilter filter)
        {
            if (request.CurrentPage == null || request.ItemsPerPage == null)
            {
                var result = FilterMongoCollection(filter);
                return new PagingResult
                {
                    Items = result.Items,
                    MaxItemCount = result.Count
                };
            }
            var skipItems = (request.CurrentPage.Value - 1) * request.ItemsPerPage.Value;

            var (item, count) = FilterMongoCollection(filter, skipItems, request.ItemsPerPage.Value);

            //var maxPage = (int)Math.Ceiling((double)count / request.ItemsPerPage.Value);

            return new PagingResult
            {
                CurrentPage = request.CurrentPage,
                ItemsPerPage = request.ItemsPerPage,
                MaxItemCount = count,
                Items = item
            };
        }

        private (IEnumerable<string> Items, int Count) FilterMongoCollection(BookFilter filter, int? skip = null, int? take = null)
        {
            var items = _books.AsQueryable()
                              .WhereIf(filter.BookName.HasValue(), x => x.BookName.Contains(filter.BookName))
                              .WhereIf(filter.Author.HasValue(), x => x.Author.Contains(filter.Author))
                              .Where(x => filter.Rate == null || x.Rate >= filter.Rate)

                              .OrderBy(filter.SortFieldName, filter.IsAscending)
                              .Select(x => x.Author)
                              ;

            var count = items.Count<string>();

            if (skip.HasValue)
                items = items.Skip(skip.Value);

            if (take.HasValue)
                items = items.Take(take.Value);

            return (items, count);
        }
    }
}