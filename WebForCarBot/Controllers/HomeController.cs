using CarBot.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebForCarBot.Models;

namespace WebForCarBot.Controllers
{
	public class HomeController : Controller
	{	
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Commands()
		{
			return Redirect("~/");
		}

		public IActionResult Contacts()
		{
			return Redirect("~/");
		}
	}
}
