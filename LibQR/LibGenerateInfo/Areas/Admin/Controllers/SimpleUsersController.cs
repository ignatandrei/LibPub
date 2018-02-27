using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibQRDAL.Models;

namespace LibGenerateInfo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SimpleUsersController : Controller
    {
        private readonly QRContext _context;

        public SimpleUsersController(QRContext context)
        {
            _context = context;
        }

        // GET: Admin/SimpleUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.SimpleUser
                .Where(it=>!it.IsAdmin)
                .ToListAsync());
        }
        public async Task<IActionResult> IndexAdmin()
        {
            return View("Index",await _context.SimpleUser
                .Where(it => it.IsAdmin)
                .ToListAsync() );
        }
        

        // GET: Admin/SimpleUsers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simpleUser = await _context.SimpleUser
                .SingleOrDefaultAsync(m => m.Iduser == id);
            if (simpleUser == null)
            {
                return NotFound();
            }

            return View(simpleUser);
        }

        // GET: Admin/SimpleUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SimpleUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Iduser,Name,Email,Password,ConfirmedByEmail,IsAdmin")] SimpleUser simpleUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(simpleUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(simpleUser);
        }

        // GET: Admin/SimpleUsers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simpleUser = await _context.SimpleUser.SingleOrDefaultAsync(m => m.Iduser == id);
            if (simpleUser == null)
            {
                return NotFound();
            }
            return View(simpleUser);
        }

        // POST: Admin/SimpleUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Iduser,Name,Email,Password,ConfirmedByEmail,IsAdmin")] SimpleUser simpleUser)
        {
            if (id != simpleUser.Iduser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(simpleUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SimpleUserExists(simpleUser.Iduser))
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
            return View(simpleUser);
        }

        // GET: Admin/SimpleUsers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simpleUser = await _context.SimpleUser
                .SingleOrDefaultAsync(m => m.Iduser == id);
            if (simpleUser == null)
            {
                return NotFound();
            }

            return View(simpleUser);
        }

        // POST: Admin/SimpleUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var simpleUser = await _context.SimpleUser.SingleOrDefaultAsync(m => m.Iduser == id);
            _context.SimpleUser.Remove(simpleUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SimpleUserExists(long id)
        {
            return _context.SimpleUser.Any(e => e.Iduser == id);
        }
    }
}
