using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISTP_LABA_3.Data;
using ISTP_LABA_3.Models;
using System.Globalization;

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
        public async Task<IActionResult> Index(DateTime dateFilter,string clNameFilter, string bnNameFilter, string otNameFilter, string sortOrder)
        {
            ViewData["otNameFilter"] = otNameFilter;
            ViewData["clNameFilter"] = clNameFilter;
            ViewData["bnNameFilter"] = bnNameFilter;
            ViewData["dateFilter"] = dateFilter;

            ViewData["OfferNameSortParam"] = sortOrder == "Offer Name" ? "offer_name_desc" : "Offer Name";
            ViewData["BankNameSortParam"] = sortOrder == "Bank Name" ? "bank_name_desc" : "Bank Name";
            ViewData["ClientNameSortParam"] = sortOrder == "Client Name" ? "client_name_desc" : "Client Name";
            ViewData["PercentageSortParam"] = sortOrder == "Percentage" ? "percentage_desc" : "Percentage";
            ViewData["SumSortParam"] = sortOrder == "Sum" ? "sum_desc" : "Sum";
            ViewData["SigningDateSortParam"] = sortOrder == "Signing date" ? "signing_date_desc" : "Signing date";
            ViewData["FinishDateSortParam"] = sortOrder == "Finish date" ? "finish_date_desc" : "Finish date";

            var contracts = from c in _context.Contracts.Include(c => c.Client).Include(c => c.Offer).Include(c => c.Offer.Bank).Include(c => c.Offer.Condition).Include(c => c.Offer.OfferType)
                            select c;

            if (!String.IsNullOrEmpty(clNameFilter))
            {
                contracts = contracts.Where(c => c.Client.Name.Contains(clNameFilter));
            }
            if (!String.IsNullOrEmpty(bnNameFilter))
            {
                contracts = contracts.Where(c => c.Offer.Bank.Name.Contains(bnNameFilter));
            }
            if (!String.IsNullOrEmpty(otNameFilter))
            {
                contracts = contracts.Where(c => c.Offer.OfferType.Name.Contains(otNameFilter));
            }

            if (dateFilter != DateTime.MinValue)
            {
                contracts = contracts.Where(c => c.FinishDate < dateFilter);
            }

            switch (sortOrder)
            {
                case "offer_name_desc":
                    contracts = contracts.OrderByDescending(c => c.Offer.Name);
                    break;
                case "Offer Name":
                    contracts = contracts.OrderBy(c => c.Offer.Name);
                    break;
                case "bank_name_desc":
                    contracts = contracts.OrderByDescending(c => c.Offer.Bank.Name);
                    break;
                case "Bank Name":
                    contracts = contracts.OrderBy(c => c.Offer.Bank.Name);
                    break;
                case "client_name_desc":
                    contracts = contracts.OrderByDescending(c => c.Client.Name);
                    break;
                case "Client Name":
                    contracts = contracts.OrderBy(c => c.Client.Name);
                    break;
                case "percentage_desc":
                    contracts = contracts.OrderByDescending(c => c.Offer.Percentage);
                    break;
                case "Percentage":
                    contracts = contracts.OrderBy(c => c.Offer.Percentage);
                    break;
                case "sum_desc":
                    contracts = contracts.OrderByDescending(c => c.Sum);
                    break;
                case "Sum":
                    contracts = contracts.OrderBy(c => c.Sum);
                    break;
                case "signing_date_desc":
                    contracts = contracts.OrderByDescending(c => c.SigningDate);
                    break;
                case "Signing date":
                    contracts = contracts.OrderBy(c => c.SigningDate);
                    break;
                case "finish_date_desc":
                    contracts = contracts.OrderByDescending(c => c.FinishDate);
                    break;
                case "Finish date":
                    contracts = contracts.OrderBy(c => c.FinishDate);
                    break;
                default:
                    break;

            }
            var bankAppContext = _context.Contracts.Include(c => c.Client).Include(c => c.Offer).Include(c=>c.Offer.Bank).Include(c=>c.Offer.OfferType);
            return View(await contracts.AsNoTracking().ToListAsync());
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
                .Include(c => c.Offer).Include(c=>c.Offer.OfferType).Include(c=>c.Offer.Bank)
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
        public async Task<IActionResult> Create([Bind("ContractID,OfferID,ClientID,SigningDate,FinishDate")] Contract contract, int oid, string sum, DateTime sgndate, DateTime fnshdate)
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
            float sumf = float.Parse(sum, CultureInfo.InvariantCulture.NumberFormat);
            contract.Sum = sumf;
            if ((ModelState.IsValid) && (int)sumf<=max && (int)sumf>=min && fnshdate>sgndate)
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "Name", contract.ClientID);
            ViewData["OfferID"] = new SelectList(_context.Offers, "OfferID", "Name", contract.OfferID);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractID,OfferID,ClientID,SigningDate,FinishDate")] Contract contract, string sum)
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

            float sumf = float.Parse(sum, CultureInfo.InvariantCulture.NumberFormat);
            contract.Sum = sumf;
            if (ModelState.IsValid && contract.SigningDate<contract.FinishDate 
                && max>=(int)contract.Sum && min<=(int)contract.Sum)
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "Name", contract.ClientID);
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
