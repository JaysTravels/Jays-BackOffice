using Jays_BackOffice.Context;
using Jays_BackOffice.DB_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jays_BackOffice.Controllers
{
    [Authorize]
    public class ManualPaymentsController : Controller
    {
        private readonly DB_Context _context;

        public ManualPaymentsController(DB_Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.ManulPayments.OrderByDescending(e => e.PaymentId).ToListAsync());
        }

        // GET: ManualPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manualPayment = await _context.ManulPayments
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (manualPayment == null)
            {
                return NotFound();
            }

            return View(manualPayment);
        }

        // GET: ManualPayments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManualPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,Amount,FirstName,LastName,Email,PhoneNumber,Address,City,PostalCode,Country,CreatedOn,PaymentStatus,BookingRef")] ManualPayment manualPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manualPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manualPayment);
        }

        // GET: ManualPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manualPayment = await _context.ManulPayments.FindAsync(id);
            if (manualPayment == null)
            {
                return NotFound();
            }
            return View(manualPayment);
        }

        // POST: ManualPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,Amount,FirstName,LastName,Email,PhoneNumber,Address,City,PostalCode,Country,CreatedOn,PaymentStatus,BookingRef")] ManualPayment manualPayment)
        {
            if (id != manualPayment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manualPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManualPaymentExists(manualPayment.PaymentId))
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
            return View(manualPayment);
        }

        // GET: ManualPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manualPayment = await _context.ManulPayments
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (manualPayment == null)
            {
                return NotFound();
            }

            return View(manualPayment);
        }

        // POST: ManualPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manualPayment = await _context.ManulPayments.FindAsync(id);
            if (manualPayment != null)
            {
                _context.ManulPayments.Remove(manualPayment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManualPaymentExists(int id)
        {
            return _context.ManulPayments.Any(e => e.PaymentId == id);
        }
    }
}
