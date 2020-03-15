using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElcomManage.Data;
using ElcomManage.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElcomManage.Controllers
{
    public class StockLocationsController : Controller
    {
        private readonly ElcomDb _context;

        public StockLocationsController(ElcomDb context)
        {
            _context = context;
        }

        // GET: StockLocations
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.StockLocation.ToListAsync());
        }

        // GET: StockLocations/Details/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLocation = await _context.StockLocation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockLocation == null)
            {
                return NotFound();
            }

            return View(stockLocation);
        }

        // GET: StockLocations/Create
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StockLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Create(StockLocation stockLocation)
        {
            
            if (ModelState.IsValid)
            { 
                ICollection<Stock> newStocks = new List<Stock>();
                var Products = _context.Products.ToList();
                foreach (var Product in Products)
                {
                    newStocks.Add(new Stock
                    {
                        ProductId = Product.Id,
                        Quantity = 0
                        
                    });
                }
                stockLocation.Stock = newStocks;
                _context.Add(stockLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockLocation);
        }

        // GET: StockLocations/Edit/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLocation = await _context.StockLocation.FindAsync(id);
            if (stockLocation == null)
            {
                return NotFound();
            }
            return View(stockLocation);
        }

        // POST: StockLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,InHouse")] StockLocation stockLocation)
        {
            if (id != stockLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockLocationExists(stockLocation.Id))
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
            return View(stockLocation);
        }

        // GET: StockLocations/Delete/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockLocation = await _context.StockLocation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockLocation == null)
            {
                return NotFound();
            }

            return View(stockLocation);
        }

        // POST: StockLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockLocation = await _context.StockLocation.FindAsync(id);
            _context.StockLocation.Remove(stockLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockLocationExists(int id)
        {
            return _context.StockLocation.Any(e => e.Id == id);
        }
    }
}
