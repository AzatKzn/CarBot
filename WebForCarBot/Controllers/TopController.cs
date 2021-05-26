using CarBot.DBContexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForCarBot.Controllers
{
	public class TopController : Controller
	{
		public IActionResult Users(AppDbContext context)
		{
			//ViewBag.Users = context.Users.OrderByDescending(x => x.TestDrivesCount).Take(10).ToList();
			//return View();
			return Redirect("~/");
		}
	}
}
