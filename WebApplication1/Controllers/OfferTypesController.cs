using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISTP_LABA_3.Data;
using ISTP_LABA_3.Models;

namespace WebApplication1.Controllers
{
    public class OfferTypesController : Controller
    {
        private readonly BankAppContext _context;

        public OfferTypesController(BankAppContext context)
        {
            _context = context;
        }

        // GET: OfferTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfferTypes.ToListAsync());
        }

        // GET: OfferTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offerType = await _context.OfferTypes
                .FirstOrDefaultAsync(m => m.OfferTypeID == id);
            if (offerType == null)
            {
                return NotFound();
            }

            return View(offerType);
        }

        // GET: OfferTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfferTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfferTypeID,Name,Description")] OfferType offerType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offerType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offerType);
        }

        // GET: OfferTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offerType = await _context.OfferTypes.FindAsync(id);
            if (offerType == null)
            {
                return NotFound();
            }
            return View(offerType);
        }

        // POST: OfferTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OfferTypeID,Name,Description")] OfferType offerType)
        {
            if (id != offerType.OfferTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offerType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferTypeExists(offerType.OfferTypeID))
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
            return View(offerType);
        }

        // GET: OfferTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offerType = await _context.OfferTypes
                .FirstOrDefaultAsync(m => m.OfferTypeID == id);
            if (offerType == null)
            {
                return NotFound();
            }

            return View(offerType);
        }

        // POST: OfferTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offerType = await _context.OfferTypes.FindAsync(id);
            _context.OfferTypes.Remove(offerType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferTypeExists(int id)
        {
            return _context.OfferTypes.Any(e => e.OfferTypeID == id);
        }
    }
}
