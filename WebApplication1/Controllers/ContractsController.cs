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
    public class ContractsController : Controller
    {
        private readonly BankAppContext _context;

        public ContractsController(BankAppContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index()
        {
            var bankAppContext = _context.Contracts.Include(c => c.Client).Include(c => c.Offer).Include(c=>c.Offer.Bank);
            return View(await bankAppContext.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Offer)
                .FirstOrDefaultAsync(m => m.ContractID == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "Name");
            ViewData["OfferID"] = new SelectList(_context.Offers, "OfferID", "Name");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractID,OfferID,ClientID,Sum,SigningDate,FinishDate")] Contract contract, int oid, int sum, DateTime sgndate, DateTime fnshdate)
        {
            var max = (from c in _context.Offers
                     where (c.OfferID == oid)
                     select c.Condition.MaxSum).Max();

            var min = (from c in _context.Offers
                       where (c.OfferID == oid)
                       select c.Condition.MinSum).Min();

            contract.OfferID = oid;
            contract.FinishDate = fnshdate;
            contract.SigningDate = sgndate;
            if ((ModelState.IsValid) && sum<=max && sum>=min && fnshdate>sgndate)
            {
                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "Name", contract.ClientID);
            ViewData["OfferID"] = new SelectList(_context.Offers, "OfferID", "Name", contract.OfferID);
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientType", contract.ClientID);
            ViewData["OfferID"] = new SelectList(_context.Offers, "OfferID", "Name", contract.OfferID);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractID,OfferID,ClientID,Sum,SigningDate,FinishDate")] Contract contract)
        {
            if (id != contract.ContractID)
            {
                return NotFound();
            }

            var max = (from c in _context.Offers
                       where (c.OfferID == contract.OfferID)
                       select c.Condition.MaxSum).Max();

            var min = (from c in _context.Offers
                       where (c.OfferID == contract.OfferID)
                       select c.Condition.MinSum).Min();
            if (ModelState.IsValid && contract.SigningDate<contract.FinishDate 
                && max>contract.Sum && min<contract.Sum)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.ContractID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientType", contract.ClientID);
            ViewData["OfferID"] = new SelectList(_context.Offers, "OfferID", "Name", contract.OfferID);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Offer)
                .FirstOrDefaultAsync(m => m.ContractID == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.ContractID == id);
        }
    }
}
