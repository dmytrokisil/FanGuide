using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanGuide;
using FanGuide.Models;
using Microsoft.AspNetCore.Http;
using FanGuide.Enums;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace FanGuide.Controllers
{
    public class SportsmenController : Controller
    {
        private readonly DataBaseContext _context;

        public SportsmenController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Sportsmen
        public async Task<IActionResult> Index(string name, string surname, string teamName, string sportName, int ageFrom, int ageTo,
            SportsmanSort sort = SportsmanSort.SurnameAsc)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");

            IQueryable<Sportsman> dataBaseContext = _context.Sportsmen
                .Include(c => c.Team)
                .ThenInclude(c => c.Sport);

            if (!String.IsNullOrEmpty(name))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(surname))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Surname.Contains(surname));
            }

            if (!String.IsNullOrEmpty(teamName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Team.Name.Contains(teamName));
            }

            if (!String.IsNullOrEmpty(sportName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Team.Sport.Name.Contains(sportName));
            }

            if (ageTo != 0)
            {
                dataBaseContext = dataBaseContext.Where(x => x.Age <= ageTo);
            }

            dataBaseContext = dataBaseContext.Where(x => x.Age >= ageFrom);

            switch (sort)
            {
                case SportsmanSort.NameAsc:
                    dataBaseContext = dataBaseContext.OrderBy(c => c.Name);
                    break;
                case SportsmanSort.NameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(c => c.Name);
                    break;
                case SportsmanSort.SurnameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(c => c.Surname);
                    break;
                case SportsmanSort.TeamAsc:
                    dataBaseContext = dataBaseContext.OrderBy(c => c.Team.Name);
                    break;
                case SportsmanSort.TeamDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(c => c.Team.Name);
                    break;
                case SportsmanSort.AgeAsc:
                    dataBaseContext = dataBaseContext.OrderBy(c => c.Age);
                    break;
                case SportsmanSort.AgeDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(c => c.Age);
                    break;
                case SportsmanSort.WeightAsc:
                    dataBaseContext = dataBaseContext.OrderBy(c => c.Weight);
                    break;
                case SportsmanSort.WeightDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(c => c.Weight);
                    break;
                default:
                    dataBaseContext = dataBaseContext.OrderBy(c => c.Surname);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(SportsmanSort)).Cast<SportsmanSort>()
              .Select(x => new SelectListItem
              {
                  Text = x.GetType()
          .GetMember(x.ToString())
          .FirstOrDefault()
          .GetCustomAttribute<DisplayAttribute>()?
          .GetName(),
                  Value = x.ToString(),
                  Selected = (x == sort)
              }).ToList();

            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            ViewBag.Name = name;
            ViewBag.Surname = surname;
            ViewBag.TeamName = teamName;
            ViewBag.SportName = sportName;
            ViewBag.AgeFrom = ageFrom;
            ViewBag.AgeTo = ageTo;

            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Sportsmen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var sportsman = await _context.Sportsmen
                .Include(s => s.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportsman == null)
            {
                return NotFound();
            }

            return View(sportsman);
        }

        // GET: Sportsmen/Create
        public IActionResult Create()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Sportsmen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sportsman sportsman)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (ModelState.IsValid)
            {
                _context.Add(sportsman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", sportsman.TeamId);
            return View(sportsman);
        }

        // GET: Sportsmen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var sportsman = await _context.Sportsmen.FindAsync(id);
            if (sportsman == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", sportsman.TeamId);
            return View(sportsman);
        }

        // POST: Sportsmen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sportsman sportsman)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id != sportsman.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportsman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportsmanExists(sportsman.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", sportsman.TeamId);
            return View(sportsman);
        }

        // GET: Sportsmen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var sportsman = await _context.Sportsmen
                .Include(s => s.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportsman == null)
            {
                return NotFound();
            }

            return View(sportsman);
        }

        // POST: Sportsmen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sportsman = await _context.Sportsmen.FindAsync(id);
            _context.Sportsmen.Remove(sportsman);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportsmanExists(int id)
        {
            return _context.Sportsmen.Any(e => e.Id == id);
        }
    }
}
