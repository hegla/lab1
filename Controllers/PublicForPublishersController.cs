﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sem2Lab1SQLServer;

namespace Sem2Lab1SQLServer.Controllers
{
    public class PublicForPublishersController : Controller
    {
        private readonly gameindustryContext _context;

        public PublicForPublishersController(gameindustryContext context)
        {
            _context = context;
        }

        // GET: PublicForPublishers
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Publishers");
            ViewBag.PubName = name;
            ViewBag.PubId = id;
            var gameindustryContext = _context.Publications.Where(x => x.PublisherId == id).Include(p => p.Game).Include(p => p.Publisher);
            return View(await gameindustryContext.ToListAsync());
        }

        // GET: PublicForPublishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publications = await _context.Publications
                .Include(p => p.Game)
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.PublicationId == id);
            if (publications == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "GamesForPublic", new { id = publications.GameId });
        }

        // GET: PublicForPublishers/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Contacts");
            return View();
        }

        // POST: PublicForPublishers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublicationId,GameId,PublisherId")] Publications publications)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Name", publications.GameId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Contacts", publications.PublisherId);
            return View(publications);
        }

        // GET: PublicForPublishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publications = await _context.Publications.FindAsync(id);
            if (publications == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Name", publications.GameId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Contacts", publications.PublisherId);
            return View(publications);
        }

        // POST: PublicForPublishers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PublicationId,GameId,PublisherId")] Publications publications)
        {
            if (id != publications.PublicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublicationsExists(publications.PublicationId))
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
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Name", publications.GameId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Contacts", publications.PublisherId);
            return View(publications);
        }

        // GET: PublicForPublishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publications = await _context.Publications
                .Include(p => p.Game)
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.PublicationId == id);
            if (publications == null)
            {
                return NotFound();
            }

            return View(publications);
        }

        // POST: PublicForPublishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publications = await _context.Publications.FindAsync(id);
            _context.Publications.Remove(publications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublicationsExists(int id)
        {
            return _context.Publications.Any(e => e.PublicationId == id);
        }
    }
}
