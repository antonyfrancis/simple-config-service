using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Sample.AspNet5.Web.Models;

namespace Sample.AspNet5.Web.Controllers
{
    public class HomeController : Controller
    {
	    private readonly IOptions<AppSettings> _options;

	    public HomeController(IOptions<AppSettings> options)
	    {
		    _options = options;
	    }

	    // GET: /<controller>/
		public IActionResult Index()
        {
            return Content(_options.Options.SiteTitle);
        }
    }
}
