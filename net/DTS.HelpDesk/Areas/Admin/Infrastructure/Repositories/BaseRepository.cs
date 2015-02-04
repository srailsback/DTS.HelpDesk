using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories
{
    public class BaseRepository
    {
        protected readonly HelpDeskContext _context;
        protected readonly ILogger _logger;

        public BaseRepository(ILogger logger, HelpDeskContext context)
        {
            this._logger = logger;
            this._context = context;
        }


        //public void Detach(object entity)
        //{
        //    ((IObjectContextAdapter)_context).ObjectContext.Detach(entity);
        //}

    }
}