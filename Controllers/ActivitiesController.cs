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
    public class ActivitiesController : Controller
    {
        private readonly ElcomDb _context;

        public ActivitiesController(ElcomDb context)
        {
            _context = context;
        }

        // GET: Activities
        [Authorize(Roles = "ADMIN,PUNETOR BAZE,PUNETOR TERENI")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activities.ToListAsync());
        }

    }
}
