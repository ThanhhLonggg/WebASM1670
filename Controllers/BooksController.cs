using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebASM1670.Data;
using WebASM1670.Models;
using WebASM1670.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public BooksController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    // GET: Books
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.Include(b => b.Category).ToListAsync();
        return View(books);
    }

    // GET: Books/Create
    public IActionResult Create()
    {
        var categories = _context.Categories.ToList();
        var viewModel = new BookModel
        {
            Categories = categories
        };
        return View(viewModel);
    }

    // POST: Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookModel bookModel)
    {
        if (ModelState.IsValid)
        {
            var book = new Book
            {
                Title = bookModel.Title,
                Quantity = bookModel.Quantity,
                Price = bookModel.Price,
                CategoryId = bookModel.CategoryId
            };

            if (bookModel.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + bookModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                bookModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                book.Image = uniqueFileName;
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        bookModel.Categories = _context.Categories.ToList();
        return View(bookModel);
    }

    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        ViewBag.Categories = _context.Categories.ToList();

        var viewModel = new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Quantity = book.Quantity,
            Price = book.Price,
            CategoryId = book.CategoryId,
            Categories = _context.Categories.ToList()
        };

        return View(viewModel);
    }

    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookModel bookModel)
    {
        if (id != bookModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);

                book.Title = bookModel.Title;
                book.Quantity = bookModel.Quantity;
                book.Price = bookModel.Price;
                book.CategoryId = bookModel.CategoryId;

                if (bookModel.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + bookModel.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    bookModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    book.Image = uniqueFileName;
                }

                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookModel.Id))
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
        bookModel.Categories = _context.Categories.ToList();
        return View(bookModel);
    }

    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

}
