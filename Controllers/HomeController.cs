using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElcomManage.Models;
using ElcomManage.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElcomManage.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ElcomDb _context;

        public HomeController(ElcomDb context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN,PUNETOR BAZE,PUNETOR TERENI")]
        public IActionResult Index()
        {

            ViewData["ProductCategoriesQuantityBaze1"] =  _context.Stock.Where(s => s.StockLocation.Id == 1).GroupBy(s => s.Product.ProductCategory.Name).Select(s => new CategoryQuantity
            {
       
                CategoryName = s.Key,
                Quantity = s.Sum(c => c.Quantity)
            }).OrderBy(p => p.CategoryName).ToList();

            ViewData["ProductCategoriesQuantityBaze2"] = _context.Stock.Where(s => s.StockLocation.Id == 2).GroupBy(s => s.Product.ProductCategory.Name).Select(s => new CategoryQuantity
            {

                CategoryName = s.Key,
                Quantity = s.Sum(c => c.Quantity)
            }).OrderBy(p => p.CategoryName).ToList();

            ViewData["ProductCategoriesQuantityPikap1"] = _context.Stock.Where(s => s.StockLocation.Id == 3).GroupBy(s => s.Product.ProductCategory.Name).Select(s => new CategoryQuantity
            {

                CategoryName = s.Key,
                Quantity = s.Sum(c => c.Quantity)
            }).OrderBy(p => p.CategoryName).ToList();


            return View();
        }

        [Authorize(Roles = "ADMIN,PUNETOR BAZE,PUNETOR TERENI")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(string productCategoryName, int stockLocationId)
        {

            
            ViewData["listaEProduktevePrejDatabazes"] = _context.Stock.Where(s => s.StockLocationId == stockLocationId && s.Product.ProductCategory.Name==productCategoryName).GroupBy(s => s.Product.Name).Select(p => new ProductQuantity
            {
                ProductName = p.Key,
                Quantity = p.Sum(s => s.Quantity)
            }).ToList();
 
            //foreach (var Produkt in listaEProduktevePrejDatabazes)
            //{
            //    var ProductQuantity = _context.Stock.Where(p => p.ProductId == Produkt.Id).Sum(p => p.Quantity);
            //    produktet.Add(new ProductQuantity
            //    {
            //        Name = Produkt.Name,
            //        Quantity = ProductQuantity
            //    });
            //}

            return View();
        }
    }
}
