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
    public class StadiaController : Controller
    {
        private readonly DataBaseContext _context;

        public StadiaController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Stadia
        public async Task<IActionResult> Index()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            var dataBaseContext = _context.Stadiums.Include(s => s.HomeTeam);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Stadia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums
                .Include(s => s.HomeTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // GET: Stadia/Create
        public IActionResult Create()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Stadia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,StadiumCapacity,HomeTeamId")] Stadium stadium)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (ModelState.IsValid)
            {
                _context.Add(stadium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", stadium.HomeTeamId);
            return View(stadium);
        }

        // GET: Stadia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", stadium.HomeTeamId);
            return View(stadium);
        }

        // POST: Stadia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,StadiumCapacity,HomeTeamId")] Stadium stadium)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id != stadium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadiumExists(stadium.Id))
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
            ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Name", stadium.HomeTeamId);
            return View(stadium);
        }

        // GET: Stadia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadiums
                .Include(s => s.HomeTeam)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stadium = await _context.Stadiums.FindAsync(id);
            _context.Stadiums.Remove(stadium);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadiumExists(int id)
        {
            return _context.Stadiums.Any(e => e.Id == id);
        }
    }
}
