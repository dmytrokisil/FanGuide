using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FanGuide;
using FanGuide.Models;
using FanGuide.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace FanGuide.Controllers
{
    public class MatchesController : Controller
    {
        private readonly DataBaseContext _context;

        public MatchesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index(string teamName, string sportName, string sportsmanName, DateTime dateFrom, DateTime dateTo, 
            MatchSort sort = MatchSort.DateAsc)
        
        {
            IQueryable<Match> dataBaseContext = _context.Matches
                .Include(m => m.HomeTeam)
                .ThenInclude(c => c.Sportsmen)
                .Include(c => c.HomeTeam)
                .ThenInclude(c => c.Sport)
                .Include(m => m.VisitorTeam)
                .ThenInclude(c => c.Sportsmen);

            if (!String.IsNullOrEmpty(teamName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.HomeTeam.Name.Contains(teamName) || x.VisitorTeam.Name.Contains(teamName));
            }

            if (!String.IsNullOrEmpty(sportsmanName))
            {
                dataBaseContext = dataBaseContext.Where(x =>
                x.HomeTeam.Sportsmen.Any(c => c.Surname.Contains(sportsmanName) || c.Name.Contains(sportsmanName)) ||
                x.VisitorTeam.Sportsmen.Any(c => c.Surname.Contains(sportsmanName) || c.Name.Contains(sportsmanName)));
            }

            if (!String.IsNullOrEmpty(sportName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.HomeTeam.Sport.Name.Contains(sportName));
            }

            if (dateTo.Year == 1)
            {
                dateTo = DateTime.Now.AddDays(1);
            }

            dataBaseContext = dataBaseContext.Where(x => x.Date >= dateFrom);
            dataBaseContext = dataBaseContext.Where(x => x.Date <= dateTo);

            switch (sort)
            {
                case MatchSort.HomeTeamNameAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.HomeTeam.Name);
                    break;
                case MatchSort.HomeTeamNameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.HomeTeam.Name);
                    break;
                case MatchSort.VisitorTeamNameAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.VisitorTeam.Name);
                    break;
                case MatchSort.VisitorTeamNameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.VisitorTeam.Name);
                    break;
                case MatchSort.DateDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Date);
                    break;
                default:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Date);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(MatchSort)).Cast<MatchSort>()
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
            ViewBag.TeamName = teamName;
            ViewBag.SportsmanName = sportsmanName;
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            ViewBag.SportName = sportName;

            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.HomeTeam)
                .ThenInclude(c => c.Sport)
                .Include(m => m.VisitorTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name");
            ViewData["VisitorTeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,HomeTeamId,VisitorTeamId,HomeTeamScore,VisitorTeamScore")] Match match)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (match.HomeTeamId == match.VisitorTeamId)
            {
                ModelState.AddModelError("HomeTeamId", "Error");
            }

            var homeTeam = await _context.Teams.FirstOrDefaultAsync(c => c.Id == match.HomeTeamId);
            var visitorTeam = await _context.Teams.FirstOrDefaultAsync(c => c.Id == match.VisitorTeamId);

            if (homeTeam.SportId != visitorTeam.SportId)
            {
                ModelState.AddModelError("HomeTeamId", "Invalid team sport type");
            }

            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", match.HomeTeamId);
            ViewData["VisitorTeamId"] = new SelectList(_context.Teams, "Id", "Name", match.VisitorTeamId);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", match.HomeTeamId);
            ViewData["VisitorTeamId"] = new SelectList(_context.Teams, "Id", "Name", match.VisitorTeamId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,HomeTeamId,VisitorTeamId,HomeTeamScore,VisitorTeamScore")] Match match)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id != match.Id)
            {
                return NotFound();
            }

            if (match.HomeTeamId == match.VisitorTeamId)
            {
                ModelState.AddModelError("HomeTeamId", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
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
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", match.HomeTeamId);
            ViewData["VisitorTeamId"] = new SelectList(_context.Teams, "Id", "Name", match.VisitorTeamId);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.HomeTeam)
                .ThenInclude(c => c.Sport)
                .Include(m => m.VisitorTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
