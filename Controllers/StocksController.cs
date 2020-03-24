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
        public async Task<IActionResult> Index(int stockLocationId)
        {
            var elcomDb =await _context.Stock.Include(s => s.Product).Include(s => s.StockLocation).Where(s => s.StockLocationId==stockLocationId).ToListAsync();

            if(elcomDb==null)
            {
                return NotFound();
            }

            return View(elcomDb);
        }

        //// GET: Stocks/Details/5
        //[Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var stock = await _context.Stock
        //        .Include(s => s.Product)
        //        .Include(s => s.StockLocation)
        //        .FirstOrDefaultAsync(m => m.StockId == id);
        //    if (stock == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(stock);
        //}

        //// GET: Stocks/Create
        //[Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        //public IActionResult Create()
        //{
        //    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
        //    ViewData["StockLocationId"] = new SelectList(_context.StockLocation, "Id", "Name");
        //    return View();
        //}

        //// POST: Stocks/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        //public async Task<IActionResult> Create([Bind("StockId,ProductId,StockLocationId,Quantity")] Stock stock)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(stock);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", stock.ProductId);
        //    ViewData["StockLocationId"] = new SelectList(_context.StockLocation, "Id", "Id", stock.StockLocationId);
        //    return View(stock);
        //}

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
            ViewData["ProductId"] = new SelectList(_context.Products.Where(p => p.Id==stock.ProductId), "Id", "Name", stock.ProductId);
            ViewData["StockLocationId"] = new SelectList(_context.StockLocation.Where(s => s.Id==stock.StockLocationId), "Id", "Name", stock.StockLocationId);
            return View(stock);
        }

        

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int id, Stock stock)
        {
            if (id != stock.StockId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Produkti =await _context.Products.SingleOrDefaultAsync(p => p.Id == stock.ProductId);
                    var StockLocation =await _context.StockLocation.SingleOrDefaultAsync(s => s.Id == stock.StockLocationId);

                    _context.Update(stock);

                    var activity = new Activity
                    {
                        Comment = User.Identity.Name + " ka ndryshuar produktin " + Produkti.Name + " ne stokun " + StockLocation.Name + " ne sasine " + stock.Quantity,
                        Date=DateTime.Now
                    };
                    _context.Activities.Add(activity);
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
                return RedirectToAction("Index","Home");
            }
            ViewData["ProductId"] = new SelectList(_context.Products.Where(p => p.Id == stock.ProductId), "Id", "Id", stock.ProductId);
            ViewData["StockLocationId"] = new SelectList(_context.StockLocation.Where(s => s.Id == stock.StockLocationId), "Id", "Id", stock.StockLocationId);
            return View(stock);
        }

        //// GET: Stocks/Delete/5
        //[Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var stock = await _context.Stock
        //        .Include(s => s.Product)
        //        .Include(s => s.StockLocation)
        //        .FirstOrDefaultAsync(m => m.StockId == id);
        //    if (stock == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(stock);
        //}

        //// POST: Stocks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var stock = await _context.Stock.FindAsync(id);
        //    _context.Stock.Remove(stock);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool StockExists(int id)
        {
            return _context.Stock.Any(e => e.StockId == id);
        }
    }
}
