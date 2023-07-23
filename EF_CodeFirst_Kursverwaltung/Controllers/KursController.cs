using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_CodeFirst_Kursverwaltung.Models;
using System.Text;

namespace EF_CodeFirst_Kursverwaltung.Controllers
{
    public class KursController : Controller
    {
        private readonly KursContext _context;

        public KursController(KursContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var kursliste = await _context.Kurse.ToListAsync();

            return View(kursliste);


        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Kurse == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurse.FirstOrDefaultAsync(m => m.Id == id);

            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }


        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Starttermin,Dauer")] Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kurs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kurs);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Kurse == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurse.FindAsync(id);
            if (kurs == null)
            {
                return NotFound();
            }
            return View(kurs);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Starttermin,Dauer")] Kurs kurs)
        {
            if (id != kurs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {             
                _context.Update(kurs);
                await _context.SaveChangesAsync();              
                return RedirectToAction(nameof(Index));
            }
            return View(kurs);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Kurse == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurse.FirstOrDefaultAsync(m => m.Id == id);

            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Kurse == null)
            {
                return Problem("Entity set 'KursContext.Kurse'  is null.");
            }
            var kurs = await _context.Kurse.FindAsync(id);
            if (kurs != null)
            {
                _context.Kurse.Remove(kurs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KursExists(int id)
        {
          return (_context.Kurse?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       
        [HttpPost]
        public IActionResult Upload(IFormFile csvupluad)
        {
            StreamReader streamReader = new StreamReader(csvupluad.OpenReadStream());
            string zeile = null;
            Kurs neuerKurs = null;

            while ((zeile = streamReader.ReadLine()) != null)
            {
                if (Kurs.Parse(zeile, out neuerKurs))
                {
                    _context.Kurse.Add(neuerKurs);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public FileResult Download()
        {
            string csv = string.Empty;

            foreach (Kurs einKurs in _context.Kurse)
            {
                csv += string.Format("{0};{1};{2};{3}", einKurs.Id, einKurs.Name, einKurs.Starttermin.ToShortDateString(), einKurs.Dauer);
                csv += Environment.NewLine;
            }

            return File(Encoding.Unicode.GetBytes(csv), "Text/Csv", "Kurse.csv");
        }
    }
}
