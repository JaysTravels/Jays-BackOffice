using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jays_BackOffice.Context;
using Jays_BackOffice.DB_Models;
using Microsoft.AspNetCore.Authorization;

namespace Jays_BackOffice.Controllers
{
    [Authorize]
    public class ActiveUsersController : Controller
    {
        private readonly DB_Context _context;

        public ActiveUsersController(DB_Context context)
        {
            _context = context;
        }

        // GET: ActiveUsers
        public async Task<IActionResult> Index()
        {

            return View(await _context.ActiveUser.Where(e => e.LastActive > DateTime.UtcNow.AddMinutes(-5)).ToListAsync());
        }

        // GET: ActiveUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeUser = await _context.ActiveUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activeUser == null)
            {
                return NotFound();
            }

            return View(activeUser);
        }

        // GET: ActiveUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActiveUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SessionId,UserId,IpAddress,LastActive")] ActiveUser activeUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activeUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activeUser);
        }

        // GET: ActiveUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeUser = await _context.ActiveUser.FindAsync(id);
            if (activeUser == null)
            {
                return NotFound();
            }
            return View(activeUser);
        }

        // POST: ActiveUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,UserId,IpAddress,LastActive")] ActiveUser activeUser)
        {
            if (id != activeUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activeUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiveUserExists(activeUser.Id))
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
            return View(activeUser);
        }

        // GET: ActiveUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeUser = await _context.ActiveUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activeUser == null)
            {
                return NotFound();
            }

            return View(activeUser);
        }

        // POST: ActiveUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activeUser = await _context.ActiveUser.FindAsync(id);
            if (activeUser != null)
            {
                _context.ActiveUser.Remove(activeUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActiveUserExists(long id)
        {
            return _context.ActiveUser.Any(e => e.Id == id);
        }
    }
}
