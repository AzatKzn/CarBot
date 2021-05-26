using CarBot.DBContexts;
using CarBot.DbSetExtensions;
using CarBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForCarBot.Controllers
{
	public class GroupRaceController : Controller
	{
		public IActionResult Result(int id, AppDbContext context)
		{
			var groupRaces = context.GroupRaces.Where(x => x.Id == id).FirstOrDefault();

			if (groupRaces == null)
			{
				return Redirect("~/");
			}
			var participantsQuery = context.GroupRaceParticipant.Where(x => x.GroupRace.Id == groupRaces.Id).Include(x => x.User).Include(x => x.UserCar).ThenInclude(x => x.Auto);
			var participants = participantsQuery.ToList();
			var isFinished = groupRaces.IsFinished.HasValue && groupRaces.IsFinished.Value;

			if (isFinished)
			{
				participants = participants.OrderBy(x => x.Place).ToList();			
			}

			ViewBag.Participants = participants;
			ViewBag.Id = groupRaces.Id;
			ViewBag.Division = groupRaces.RaceDivision.GetDisplayName();
			ViewBag.IsFinished = participants.Any(x => x.Place != 0);
			return View();
		}

		public IActionResult List()
		{
			return Redirect("~/");
		}
	}
}
