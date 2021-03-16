using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketingSystem.Controllers
{
	public class HomeController : Controller
	{
		private TicketContext context;
		public HomeController(TicketContext ctx) => context = ctx;
		public IActionResult Index(string id)
		{
			var filters = new Filters(id);
			ViewBag.Filters = filters;
			ViewBag.Sprints = context.Sprints.ToList();
			ViewBag.Points = context.Points.ToList();
			ViewBag.Statuses = context.Statuses.ToList();

			IQueryable<Ticket> query = context.Tickets.Include(t => t.Sprint).Include(t => t.Point).Include(t => t.Status);
			if (filters.HasSprint)
			{
				query = query.Where(t => t.SprintId == filters.SprintId);
			}
			if (filters.HasPoint)
			{
				query = query.Where(t => t.PointId == filters.PointId);
			}
			if (filters.HasStatus)
			{
				query = query.Where(t => t.StatusId == filters.StatusId);
			}
			var tasks = query.OrderBy(t => t.Status).ToList();
			return View(tasks);
		}

		[HttpGet]
		public IActionResult Add()
		{
			ViewBag.Sprints = context.Sprints.ToList();
			ViewBag.Points = context.Points.ToList();
			ViewBag.Statuses = context.Statuses.ToList();

			return View();
		}

		[HttpPost]
		public IActionResult Add(Ticket task)
		{
			if (ModelState.IsValid)
			{
				context.Tickets.Add(task);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				ViewBag.Sprints = context.Sprints.ToList();
				ViewBag.Points = context.Points.ToList();
				ViewBag.Statuses = context.Statuses.ToList();
				return View(task);
			}
		}

		[HttpPost]
		public IActionResult Filter(string[] filter)
		{
			string id = string.Join('-', filter);
			return RedirectToAction("Index", new { ID = id });
		}

		[HttpPost]
		public IActionResult Edit([FromRoute] string id, Ticket selected)
		{
			if (selected.StatusId == null)
			{
				context.Tickets.Remove(selected);
			}
			else
			{
				string newStatusId = selected.StatusId;
				selected = context.Tickets.Find(selected.Id);
				selected.StatusId = newStatusId;
				context.Tickets.Update(selected);
			}
			context.SaveChanges();

			return RedirectToAction("Index", new { ID = id });
		}
	}
}
