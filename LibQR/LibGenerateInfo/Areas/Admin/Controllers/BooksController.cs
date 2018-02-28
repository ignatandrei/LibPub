using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibQRDAL.Models;
using LibInfoBook;
using LibTimeCreator;

namespace ibGenerateInfo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly QRContext _context;

        public BooksController(QRContext context)
        {
            _context = context;
        }

        // GET: Admin/Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .SingleOrDefaultAsync(m => m.Idbook == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Admin/Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string tinRead)
        {
            var idBooks = tinRead.Split(Environment.NewLine);
            foreach (var id in idBooks)
            {
                var exists =await _context.Book.FirstOrDefaultAsync(it => it.IdtinRead == id);
                if(exists != null)
                {
                    _context.Book.Remove(exists);                    
                }
                var book = new Book();
                book.IdtinRead = id;
                book.IsCorrect = true;
                try
                {
                    var b = new InfoBook(id);
                    await b.GetInfoFromId();
                    var val = b.Validate(null).FirstOrDefault();
                    book.Title = b.Title;
                    book.Creator = b.Creator;
                    book.Identifier = b.Identifier;
                    if(val != null)
                    {
                        book.IsCorrect = false;
                        book.ErrorMessage = val.ErrorMessage;
                    }
                    else
                    {
                        var l = Guid.NewGuid();//.ToString("N");
                        book.UniqueLink= l.ToString("N");
                    }
                }
                catch (Exception ex)
                {
                    book.ErrorMessage = ex.Message;
                    book.IsCorrect = false;
                }
                _context.Add(book);
                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            
        }

        // GET: Admin/Books/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return NotFound();
            var book = await _context.Book.SingleOrDefaultAsync(m => m.Idbook == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Idbook,IdtinRead,Title,Creator,Identifier,IsCorrect,ErrorMessage")] Book book)
        {
            if (id != book.Idbook)
            {
                return NotFound();
            }
            return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Idbook))
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
            return View(book);
        }

        // GET: Admin/Books/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return NotFound();
            var book = await _context.Book
                .SingleOrDefaultAsync(m => m.Idbook == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            return NotFound();
            var book = await _context.Book.SingleOrDefaultAsync(m => m.Idbook == id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.Idbook == id);
        }
    }
}
