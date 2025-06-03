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
    public class DeeplinksController : Controller
    {
        private readonly DB_Context _context;

        public DeeplinksController(DB_Context context)
        {
            _context = context;
        }

        // GET: Deeplinks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deeplinks.OrderByDescending(e=>e.CreatedOn).ToListAsync());
        }

        // GET: Deeplinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deeplink = await _context.Deeplinks
                .FirstOrDefaultAsync(m => m.DeeplinkId == id);
            if (deeplink == null)
            {
                return NotFound();
            }

            return View(deeplink);
        }

        // GET: Deeplinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deeplinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeeplinkId,ImageUrl,CountryName,CityName1,Price1,CityName2,Price2,CityName3,Price3,Adults,Children,Infant,DepartureDate,ReturnDate,Origin,Destination,CabinClass,FlightType,Adults2,Children2,Infant2,DepartureDate2,ReturnDate2,Origin2,Destination2,CabinClass2,FlightType2,Adults3,Children3,Infant3,DepartureDate3,ReturnDate3,Origin3,Destination3,CabinClass3,FlightType3")] Deeplink deeplink)
        {
            if (ModelState.IsValid)
            {
                deeplink.IsActive = true;
                deeplink.CreatedOn = DateTime.UtcNow;
                _context.Add(deeplink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deeplink);
        }

        // GET: Deeplinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deeplink = await _context.Deeplinks.FindAsync(id);
            if (deeplink == null)
            {
                return NotFound();
            }
            return View(deeplink);
        }

        // POST: Deeplinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeeplinkId,ImageUrl,CountryName,CityName1,Price1,CityName2,Price2,CityName3,Price3,Adults,Children,Infant,DepartureDate,ReturnDate,Origin,Destination,IsActive,CabinClass,FlightType,Adults2,Children2,Infant2,DepartureDate2,ReturnDate2,Origin2,Destination2,CabinClass2,FlightType2,Adults3,Children3,Infant3,DepartureDate3,ReturnDate3,Origin3,Destination3,CabinClass3,FlightType3,CreatedOn")] Deeplink deeplink)
        {
            if (id != deeplink.DeeplinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deeplink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeeplinkExists(deeplink.DeeplinkId))
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
            return View(deeplink);
        }

        // GET: Deeplinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deeplink = await _context.Deeplinks
                .FirstOrDefaultAsync(m => m.DeeplinkId == id);
            if (deeplink == null)
            {
                return NotFound();
            }

            return View(deeplink);
        }

        // POST: Deeplinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deeplink = await _context.Deeplinks.FindAsync(id);
            if (deeplink != null)
            {
                deeplink.IsActive = false;
                _context.Deeplinks.Update(deeplink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeeplinkExists(int id)
        {
            return _context.Deeplinks.Any(e => e.DeeplinkId == id);
        }
    }
}
