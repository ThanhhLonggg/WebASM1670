using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebASM1670.Data;
using WebASM1670.Models;
using WebASM1670.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.Include(b => b.Category).ToListAsync();
        return View(books);
    }
  


    public IActionResult Create()
    {
        var categories = _context.Categories.ToList();
        var viewModel = new BookModel
        {
            Categories = categories
        };
        return View(viewModel);
    }
   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookModel bookModel)
    {
            if (ModelState.IsValid)
        {
            string uniqueFileName = ProcessUploadedFile(bookModel);
            Book book = new()
            {
                Title = bookModel.Title,
                Quantity = bookModel.Quantity,
                Price = bookModel.Price,
                CategoryId = bookModel.CategoryId,
                Image = uniqueFileName
            };

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        bookModel.Categories = _context.Categories.ToList();
        return View(bookModel);
    }


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
            Categories = _context.Categories.ToList(),
            ExistingImage = book.Image
        };

        return View(viewModel);
    }


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
                    if (bookModel.ExistingImage != null)
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath, "Uploads", bookModel.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    book.Image = ProcessUploadedFile(bookModel);
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

    private string ProcessUploadedFile(BookModel bookModel)
    {
        string uniqueFileName = null;
        string path = Path.Combine(_hostEnvironment.WebRootPath, "images");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if (bookModel.Image != null)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + bookModel.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                bookModel.Image.CopyTo(fileStream);
            }
        }

        return uniqueFileName;
    }
}
