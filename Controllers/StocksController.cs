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
    public class StocksController : Controller
    {
        private readonly ElcomDb _context;

        public StocksController(ElcomDb context)
        {
            _context = context;
        }

        // GET: Stocks
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Index()
        {
            var elcomDb = _context.Stock.Include(s => s.Product).Include(s => s.StockLocation);
            return View(await elcomDb.ToListAsync());
        }

        // GET: Stocks/Details/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .Include(s => s.Product)
                .Include(s => s.StockLocation)
                .FirstOrDefaultAsync(m => m.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["StockLocationId"] = new SelectList(_context.StockLocation, "Id", "Id");
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Create([Bind("StockId,ProductId,StockLocationId,Quantity")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stock.ProductId);
            ViewData["StockLocationId"] = new SelectList(_context.StockLocation, "Id", "Id", stock.StockLocationId);
            return View(stock);
        }

        // GET: Stocks/Edit/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stock.ProductId);
            ViewData["StockLocationId"] = new SelectList(_context.StockLocation, "Id", "Id", stock.StockLocationId);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int id, [Bind("StockId,ProductId,StockLocationId,Quantity")] Stock stock)
        {
            if (id != stock.StockId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.StockId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stock.ProductId);
            ViewData["StockLocationId"] = new SelectList(_context.StockLocation, "Id", "Id", stock.StockLocationId);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stock = await _context.Stock
                .Include(s => s.Product)
                .Include(s => s.StockLocation)
                .FirstOrDefaultAsync(m => m.StockId == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _context.Stock.FindAsync(id);
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.StockId == id);
        }
    }
}
