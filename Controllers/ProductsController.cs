using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElcomManage.Data;
using ElcomManage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ElcomManage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ElcomDb _context;
       

        public ProductsController(ElcomDb context)
        {
            _context = context;
       
        }

        // GET: Products
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Index()
        {
            var elcomDb = _context.Products.Include(p => p.ProductCategory);
            return View(await elcomDb.ToListAsync());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ProductCategoryId")] Product product)
        {

            if (ModelState.IsValid)
            {
                ICollection<Stock> newStocks = new List<Stock>();
                var StockLocations = await _context.StockLocation.ToListAsync();
                foreach (var StockLocation in StockLocations)
                {
                    newStocks.Add(new Stock
                    {
                        Quantity = 0,
                        StockLocationId = StockLocation.Id,
                    });
                }
                product.Stock = newStocks;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Name");
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Name", product.ProductCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ProductCategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Name", product.ProductCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [Authorize(Roles = "ADMIN,PUNETOR BAZE,PUNETOR TERENI")]
        public IActionResult Load()

        {
            ViewData["Products"] = new SelectList(_context.Set<Product>().OrderBy(p => p.Name), "Id", "Name");

            if(User.IsInRole("ADMIN") || User.IsInRole("PUNETOR BAZE")) { 
            
            ViewData["StockLocations"] = new SelectList(_context.Set<StockLocation>().OrderBy(s => s.Name), "Id", "Name");
            }

            else if(User.IsInRole("PUNETOR TERENI"))
            {
                ViewData["StockLocations"] = new SelectList(_context.Set<StockLocation>().Where(s => s.InBase==false && s.InHouse==true).OrderBy(s => s.Name), "Id", "Name");
            }


            return View();
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,PUNETOR BAZE,PUNETOR TERENI")]
        public async Task<IActionResult> Load(LoadProduct lp)
        {
            if (ModelState.IsValid)
            {
                string produktet = "Produktet: ";
                foreach (var Load in lp.Loads)
                {
                    //Loaded Product
                    var Produkti = await _context.Products.SingleOrDefaultAsync(p => p.Id == Load.ProductId);
                    produktet += Produkti.Name + "-" + Load.Quantity + " cope, ";
                    //Source Stock
                    var SourceStock = await _context.Stock.Include(s => s.StockLocation).SingleOrDefaultAsync(s => s.ProductId == Load.ProductId && s.StockLocationId == lp.SourceId);

                    if (SourceStock.StockLocation.InHouse)
                    {
                        if(SourceStock.Quantity-Load.Quantity<0)
                        {
                            ModelState.AddModelError(string.Empty, "Nuk ke sasi te "+Produkti.Name+" mjaftueshem ne "+SourceStock.StockLocation.Name);
                            return View(lp);
                        }
                        SourceStock.Quantity -= Load.Quantity;
                    }
                    //Destination Stock
                    var DestinationStock = await _context.Stock.SingleOrDefaultAsync(s => s.ProductId == Load.ProductId && s.StockLocationId == lp.DestinationId);
                    DestinationStock.Quantity += Load.Quantity;
                }

                var SourceLocation = await _context.StockLocation.Where(s => s.Id == lp.SourceId).SingleOrDefaultAsync();
                var DestiantionLocation = await _context.StockLocation.Where(s => s.Id == lp.DestinationId).SingleOrDefaultAsync();

                Activity activity = new Activity
                {
                    Date = DateTime.Now,
                    Comment = User.Identity.Name + " ka bere aranzhim te ketyre produkteve nga "+SourceLocation.Name+" ne "+DestiantionLocation.Name+"." + produktet + ".Komenti:" + lp.Coment
                };
                         _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Home");
                
            }

            ViewData["Products"] = new SelectList(_context.Set<Product>().OrderBy(p => p.Name), "Id", "Name");

            if (User.IsInRole("ADMIN") || User.IsInRole("PUNETOR BAZE"))
            {

                ViewData["StockLocations"] = new SelectList(_context.Set<StockLocation>().OrderBy(p => p.Name), "Id", "Name");
            }

            else if (User.IsInRole("PUNETOR TERENI"))
            {
                ViewData["StockLocations"] = new SelectList(_context.Set<StockLocation>().Where(s => s.InBase == false && s.InHouse == true).OrderBy(p => p.Name), "Id", "Name");
            }

            return View(lp);
    }
       
      
    }
}
