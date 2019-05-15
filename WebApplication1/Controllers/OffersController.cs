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
    public class OffersController : Controller
    {
        private readonly BankAppContext _context;

        public OffersController(BankAppContext context)
        {
            _context = context;
        }

        // GET: Offers
        public async Task<IActionResult> Index(string sortOrder, string OfferTypeFilter, string NameFilter, string BankNameFilter, int searchInt)
        {
            ViewData["NameFilter"] = NameFilter;
            ViewData["BankNameFilter"] = BankNameFilter;
            ViewData["OfferTypeFilter"] = OfferTypeFilter;
            ViewData["IntFilter"] = searchInt;

            ViewData["NameSortParam"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["PercentageSortParam"] = sortOrder == "Percentage" ? "percentage_desc" : "Percentage";
            ViewData["BankSortParam"] = sortOrder == "Bank" ? "bank_desc" : "Bank";
            ViewData["MaxSortParam"] = sortOrder == "Max" ? "max_desc" : "Max";
            ViewData["MinSortParam"] = sortOrder == "Min" ? "min_desc" : "Min";

            var offers = from o in _context.Offers.Include(o => o.Bank).Include(o => o.Condition).Include(o => o.OfferType)
            select o;

            if (!String.IsNullOrEmpty(NameFilter))
            {
                offers = offers.Where(o => o.Name.Contains(NameFilter) || o.Condition.Mark.Contains(NameFilter));
            }

            if (!String.IsNullOrEmpty(BankNameFilter))
            {
                offers = offers.Where(o => o.Bank.Name.Contains(BankNameFilter));
            }

            if (!String.IsNullOrEmpty(OfferTypeFilter))
            {
                offers = offers.Where(o => o.OfferType.Name.Contains(OfferTypeFilter));
            }

            if (searchInt>0)
            {
                offers = offers.Where(o => o.Condition.MaxSum >= searchInt && o.Condition.MinSum <= searchInt);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    offers = offers.OrderByDescending(o => o.Name);
                    break;
                case "Percentage":
                    offers = offers.OrderBy(o => o.Percentage);
                    break;
                case "percentage_desc":
                    offers = offers.OrderByDescending(o => o.Percentage);
                    break;
                case "Name":
                    offers = offers.OrderBy(o => o.Name);
                    break;
                case "Bank":
                    offers = offers.OrderBy(o => o.Bank.Name);
                    break;
                case "bank_desc":
                    offers = offers.OrderByDescending(o => o.Bank.Name);
                    break;
                case "Max":
                    offers = offers.OrderBy(o => o.Condition.MaxSum);
                    break;
                case "max_desc":
                    offers = offers.OrderByDescending(o => o.Condition.MaxSum);
                    break;
                case "Min":
                    offers = offers.OrderBy(o => o.Condition.MinSum);
                    break;
                case "min_desc":
                    offers = offers.OrderByDescending(o => o.Condition.MinSum);
                    break;
                default:
                    offers = offers.OrderBy(o => o.Name);
                    break;
            }

            var bankAppContext = _context.Offers.Include(o => o.Bank).Include(o => o.Condition).Include(o => o.OfferType);
            return View(await offers.AsNoTracking().ToListAsync());
        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Bank)
                .Include(o => o.Condition)
                .Include(o => o.OfferType)
                .FirstOrDefaultAsync(m => m.OfferID == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["BankID"] = new SelectList(_context.Banks, "BankID", "Name");
            ViewData["ConditionID"] = new SelectList(_context.Conditions, "ConditionID", "Mark");
            ViewData["OfferTypeID"] = new SelectList(_context.OfferTypes, "OfferTypeID", "Name");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfferID,BankID,ConditionID,OfferTypeID,Name,Percentage")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BankID"] = new SelectList(_context.Banks, "BankID", "Address", offer.BankID);
            ViewData["ConditionID"] = new SelectList(_context.Conditions, "ConditionID", "Mark", offer.ConditionID);
            ViewData["OfferTypeID"] = new SelectList(_context.OfferTypes, "OfferTypeID", "Description", offer.OfferTypeID);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["BankID"] = new SelectList(_context.Banks, "BankID", "Address", offer.BankID);
            ViewData["ConditionID"] = new SelectList(_context.Conditions, "ConditionID", "Mark", offer.ConditionID);
            ViewData["OfferTypeID"] = new SelectList(_context.OfferTypes, "OfferTypeID", "Description", offer.OfferTypeID);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OfferID,BankID,ConditionID,OfferTypeID,Name,Percentage")] Offer offer)
        {
            if (id != offer.OfferID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.OfferID))
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
            ViewData["BankID"] = new SelectList(_context.Banks, "BankID", "Address", offer.BankID);
            ViewData["ConditionID"] = new SelectList(_context.Conditions, "ConditionID", "Mark", offer.ConditionID);
            ViewData["OfferTypeID"] = new SelectList(_context.OfferTypes, "OfferTypeID", "Description", offer.OfferTypeID);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Bank)
                .Include(o => o.Condition)
                .Include(o => o.OfferType)
                .FirstOrDefaultAsync(m => m.OfferID == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.OfferID == id);
        }
    }
}
