using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using DTS.HelpDesk.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories
{
    public interface IArticleRepository
    {
        #region non-async, blocking

        /// <summary>
        /// Gets all Articles
        /// </summary>
        /// <value>
        /// 
        /// </value>
        IQueryable<Article> All { get; }


        /// <summary>
        /// All Articles with included properties
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        IQueryable<Article> AllIncluding(params Expression<Func<Article, object>>[] includeProperties);


        /// <summary>
        /// Get Article using Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Article Find(string id);


        /// <summary>
        /// Add Article
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(Article item);


        /// <summary>
        /// Update Article
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(Article item);


        /// <summary>
        /// Delete Article
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(string id);


        /// <summary>
        /// Save Article
        /// </summary>
        void Save();


        #endregion


        #region async

        /// <summary>
        /// Async get all Articles.
        /// </summary>
        /// <value>
        /// All asynchronous.
        /// </value>
        Task<List<Article>> AllAsync();



        /// <summary>
        /// Async find Article.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Article> FindAsync(string id);

        /// <summary>
        /// Async add Article
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<int> AddAysnc(Article item);


        /// <summary>
        /// Async update Article 
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<int> UpdateAsync(Article item);


        /// <summary>
        /// Async delete Article
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string id);

        #endregion

    }

    public class ArticleRepository : BaseRepository, IArticleRepository
    {
        public ArticleRepository(ILogger logger, HelpDeskContext context) : base(logger, context) { }

        #region non-async, blocking

        /// <summary>
        /// Gets all Articles
        /// </summary>
        public IQueryable<Article> All
        {
            get { return _context.Articles.AsQueryable(); }
        }

        /// <summary>
        /// All Articles with included properties
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        public IQueryable<Article> AllIncluding(params Expression<Func<Article, object>>[] includeProperties)
        {
            var query = _context.Articles.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        /// <summary>
        /// Get Article using Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Article Find(string id)
        {
            return _context.Articles.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Add Article
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(Article item)
        {
            _context.Articles.Add(item);
        }

        /// <summary>
        /// Update Article
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(Article item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete Article
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(string id)
        {
            var toDelete = Find(id);
            if (toDelete != null)
            {
                _context.Articles.Remove(toDelete);
            }
        }


        /// <summary>
        /// Save Article
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion


        #region async


        /// <summary>
        /// Async get all Articles.
        /// </summary>
        /// <value>
        /// All asynchronous.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Article>> AllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        /// <summary>
        /// Async find Article.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Article> FindAsync(string id)
        {
            return await _context.Articles.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Async add Article
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<int> AddAysnc(Article item)
        {
            _context.Articles.Add(item);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Async update Article
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Article item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Async delete Article
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(string id)
        {
            var toDelete = FindAsync(id);
            if (toDelete != null)
            {
                _context.Entry(toDelete).State = EntityState.Deleted;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        #endregion


    }
}