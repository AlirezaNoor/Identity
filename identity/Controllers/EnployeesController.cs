using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using identity.Models;
using identity._Context;

namespace identity.Controllers
{
    public class EnployeesController : Controller
    {
        private readonly MyContext _context;

        public EnployeesController(MyContext context)
        {
            _context = context;
        }

        // GET: Enployees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enploye.ToListAsync());
        }

        // GET: Enployees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enployees = await _context.Enploye
                .FirstOrDefaultAsync(m => m.id == id);
            if (enployees == null)
            {
                return NotFound();
            }

            return View(enployees);
        }

        // GET: Enployees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,city,lastname,Gender")] Enployees enployees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enployees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enployees);
        }

        // GET: Enployees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enployees = await _context.Enploye.FindAsync(id);
            if (enployees == null)
            {
                return NotFound();
            }
            return View(enployees);
        }

        // POST: Enployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,city,lastname,Gender")] Enployees enployees)
        {
            if (id != enployees.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enployees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnployeesExists(enployees.id))
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
            return View(enployees);
        }

        // GET: Enployees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enployees = await _context.Enploye
                .FirstOrDefaultAsync(m => m.id == id);
            if (enployees == null)
            {
                return NotFound();
            }

            return View(enployees);
        }

        // POST: Enployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enployees = await _context.Enploye.FindAsync(id);
            _context.Enploye.Remove(enployees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnployeesExists(int id)
        {
            return _context.Enploye.Any(e => e.id == id);
        }
    }
}
