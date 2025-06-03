using Jays_BackOffice.Context;
using Jays_BackOffice.DB_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jays_BackOffice.Controllers
{
    [Authorize]
    public class BookingInfosController : Controller
    {
        private readonly DB_Context _context;

        public BookingInfosController(DB_Context context)
        {
            _context = context;
        }
        // GET: BookingInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookingInfo.OrderByDescending(e => e.AutoId).ToListAsync());
        }

        // GET: BookingInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingInfo = await _context.BookingInfo
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (bookingInfo == null)
            {
                return NotFound();
            }

            return View(bookingInfo);
        }

        // GET: BookingInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookingInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutoId,PnrNumber,FirstName,LastName,TotalAmount,SessionId,PaymentStatus,Error,CreatedOn,BookingRef,SentEmail")] BookingInfo bookingInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookingInfo);
        }

        // GET: BookingInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingInfo = await _context.BookingInfo.FindAsync(id);
            if (bookingInfo == null)
            {
                return NotFound();
            }
            return View(bookingInfo);
        }

        // POST: BookingInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutoId,PnrNumber,FirstName,LastName,TotalAmount,SessionId,PaymentStatus,Error,CreatedOn,BookingRef,SentEmail")] BookingInfo bookingInfo)
        {
            if (id != bookingInfo.AutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingInfoExists(bookingInfo.AutoId))
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
            return View(bookingInfo);
        }

        // GET: BookingInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingInfo = await _context.BookingInfo
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (bookingInfo == null)
            {
                return NotFound();
            }

            return View(bookingInfo);
        }

        // POST: BookingInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingInfo = await _context.BookingInfo.FindAsync(id);
            if (bookingInfo != null)
            {
                _context.BookingInfo.Remove(bookingInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingInfoExists(int id)
        {
            return _context.BookingInfo.Any(e => e.AutoId == id);
        }
    }
}
