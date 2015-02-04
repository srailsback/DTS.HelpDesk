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
    public interface IFAQRepository
    {
        #region non-async, blocking

        /// <summary>
        /// Gets all FAQs
        /// </summary>
        /// <value>
        /// 
        /// </value>
        IQueryable<FAQ> All { get; }


        /// <summary>
        /// All FAQs with included properties
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        IQueryable<FAQ> AllIncluding(params Expression<Func<FAQ, object>>[] includeProperties);


        /// <summary>
        /// Get FAQ using Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        FAQ Find(string id);


        /// <summary>
        /// Add FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(FAQ item);


        /// <summary>
        /// Update FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(FAQ item);


        /// <summary>
        /// Delete FAQ
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(string id);


        /// <summary>
        /// Save FAQ
        /// </summary>
        void Save();


        #endregion


        #region async

        /// <summary>
        /// Async get all FAQs.
        /// </summary>
        /// <value>
        /// All asynchronous.
        /// </value>
        Task<List<FAQ>> AllAsync();



        /// <summary>
        /// Async find FAQ.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<FAQ> FindAsync(string id);

        /// <summary>
        /// Async add FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<int> AddAysnc(FAQ item);


        /// <summary>
        /// Async update FAQ 
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<int> UpdateAsync(FAQ item);


        /// <summary>
        /// Async delete FAQ
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string id);

        #endregion

    }

    public class FAQRepository : BaseRepository, IFAQRepository {
        public FAQRepository(ILogger logger, HelpDeskContext context) : base(logger, context) { }

        #region non-async, blocking

        /// <summary>
        /// Gets all FAQs
        /// </summary>
        public IQueryable<FAQ> All
        {
            get { return _context.FAQs.AsQueryable(); }
        }

        /// <summary>
        /// All FAQs with included properties
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        public IQueryable<FAQ> AllIncluding(params Expression<Func<FAQ, object>>[] includeProperties)
        {
            var query = _context.FAQs.AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        /// <summary>
        /// Get FAQ using Id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public FAQ Find(string id)
        {
            return _context.FAQs.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Add FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(FAQ item)
        {
            _context.FAQs.Add(item);
        }

        /// <summary>
        /// Update FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(FAQ item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete FAQ
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(string id)
        {
            var toDelete = Find(id);
            if (toDelete != null)
            {
                _context.FAQs.Remove(toDelete);
            }
        }


        /// <summary>
        /// Save FAQ
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
        /// Async get all FAQs.
        /// </summary>
        /// <value>
        /// All asynchronous.
        /// </value>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<FAQ>> AllAsync()
        {
            return await _context.FAQs.ToListAsync();
        }

        /// <summary>
        /// Async find FAQ.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<FAQ> FindAsync(string id)
        {
            return await _context.FAQs.SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Async add FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<int> AddAysnc(FAQ item)
        {
            _context.FAQs.Add(item);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Async update FAQ
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(FAQ item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Async delete FAQ
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