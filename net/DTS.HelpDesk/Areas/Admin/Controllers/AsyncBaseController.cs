using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AsyncBaseController : AsyncController
    {

        protected readonly ILogger _logger;



        public AsyncBaseController(ILogger logger) 
        { 
        }
    }
}