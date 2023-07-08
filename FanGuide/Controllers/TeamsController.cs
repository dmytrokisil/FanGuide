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

namespace FanGuide.Controllers
{
    public class TeamsController : Controller
    {
        private readonly DataBaseContext _context;

        public TeamsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            var dataBaseContext = _context.Teams.Include(t => t.Sport);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Sport)
                .Include(c => c.Trainers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,CreateDate,SportId")] Team team)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");

            var dbTeam = await _context.Teams.FirstOrDefaultAsync(c => c.Name == team.Name && c.SportId == team.SportId);

            if (dbTeam is not null)
            {
                ModelState.AddModelError("Name", "Team name already taken for this sport");
            }

            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", team.SportId);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", team.SportId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,CreateDate,SportId")] Team team)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id != team.Id)
            {
                return NotFound();
            }

            var dbTeam = await _context.Teams.FirstOrDefaultAsync(c => c.Name == team.Name && c.SportId == team.SportId && c.Id != id);

            if (dbTeam is not null)
            {
                ModelState.AddModelError("Name", "Team name already taken for this sport");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
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
            ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", team.SportId);
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Sport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            var mathes = _context.Matches.Where(c => c.VisitorTeamId == id || c.HomeTeamId == id);
            var stadiums = _context.Stadiums.Where(c => team.Id == c.HomeTeamId);
            var sportsmen = _context.Sportsmen.Where(c => team.Id == c.TeamId);
            var trainers = _context.Trainers.Where(c => team.Id == c.TeamId);
            _context.Matches.RemoveRange(mathes);
            _context.RemoveRange(stadiums);
            _context.RemoveRange(sportsmen);
            _context.RemoveRange(trainers);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
