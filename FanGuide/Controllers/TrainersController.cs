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
    public class TrainersController : Controller
    {
        private readonly DataBaseContext _context;

        public TrainersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index(string name, string surname, string teamName, int ageFrom, int ageTo,
            TrainerSort sort = TrainerSort.NameAsc)
        {
            IQueryable<Trainer> dataBaseContext = _context.Trainers
                .Include(t => t.Team);

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

            if (ageTo != 0)
            {
                dataBaseContext = dataBaseContext.Where(x => x.Age <= ageTo);
            }

            dataBaseContext = dataBaseContext.Where(x => x.Age >= ageFrom);

            switch (sort)
            {
                case TrainerSort.NameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Name);
                    break;
                case TrainerSort.TeamAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Team.Name);
                    break;
                case TrainerSort.TeamDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Team.Name);
                    break;
                case TrainerSort.AgeAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Age);
                    break;
                case TrainerSort.AgeDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Age);
                    break;
                default:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Name);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(TrainerSort)).Cast<TrainerSort>()
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
            ViewBag.AgeFrom = ageFrom;
            ViewBag.AgeTo = ageTo;

            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamId,Id,Name,Surname,Age")] Trainer trainer)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", trainer.TeamId);
            return View(trainer);
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", trainer.TeamId);
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,Id,Name,Surname,Age")] Trainer trainer)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id != trainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", trainer.TeamId);
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.User = ControllerContext.HttpContext.Session.GetString("Name");
            if (id == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainers
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }
    }
}
