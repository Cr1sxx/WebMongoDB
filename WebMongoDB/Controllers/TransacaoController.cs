﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using WebMongoDB.Models;

namespace WebMongoDB.Controllers
{
    public class TransacaoController : Controller
    {
        // GET: Transacao
        public async Task<IActionResult> Index()
        {
            ContextMongodb dbContext = new ContextMongodb();
            return View(await dbContext.Transacao.Find(t=> true).ToListAsync());
        }

        // GET: Transacao/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ContextMongodb dbContext = new ContextMongodb();
            var transacao = await dbContext.Transacao.Find(t => t.Id == id).FirstOrDefaultAsync();

            if (transacao == null)
            {
                return NotFound();
            }

            return View(transacao);
        }

        // GET: Transacao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Amount,Category")] Transacao transacao)
        {
            if (ModelState.IsValid)
            {
                ContextMongodb dbContext = new ContextMongodb();
                transacao.Id = Guid.NewGuid();
                await dbContext.Transacao.InsertOneAsync(transacao);
                return RedirectToAction(nameof(Index));
            }
            return View(transacao);
        }

        // GET: Transacao/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContextMongodb dbContext = new ContextMongodb();
            var transacao = await dbContext.Transacao.Find(t => t.Id == id ).FirstOrDefaultAsync();
            if (transacao == null)
            {
                return NotFound();
            }
            return View(transacao);
        }

        // POST: Transacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Data,Amount,Category")] Transacao transacao)
        {
            if (id != transacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ContextMongodb dbContext = new ContextMongodb();

                    await dbContext.Transacao.ReplaceOneAsync(m => m.Id == transacao.Id, transacao);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransacaoExists(transacao.Id))
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
            return View(transacao);
        }

        // GET: Transacao/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContextMongodb dbContext = new ContextMongodb();
            var transacao = await dbContext.Transacao.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (transacao == null)
            {
                return NotFound();
            }

            return View(transacao);
        }

        // POST: Transacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ContextMongodb dbContext = new ContextMongodb();

            await dbContext.Transacao.DeleteOneAsync(t => t.Id == id);
            return RedirectToAction(nameof(Index));
        }

        private bool TransacaoExists(Guid id)
        {
            ContextMongodb dbContext = new ContextMongodb();
            var transacao = dbContext.Transacao.Find(t => t.Id == id).Any();

            return transacao;
        }

        //Soma de transações
        public ActionResult Sum()
        {
            ContextMongodb dbContext = new ContextMongodb();
            var transacao = dbContext.Transacao.Find(_ => true).ToList();
            var total = transacao.Sum(t => t.Amount);

            var totalTransacao = new SomTransacao
            {
                Total = total
            };

            dbContext.SomaTransacao.InsertOne(totalTransacao);
            
            ViewBag.Total = total;
            return View(transacao);
        }

        //Transações: Filtro por data
        public ActionResult FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            var dbContext = new ContextMongodb();

            if (startDate == null || endDate == null)
            {
                ViewBag.ErrorMessage = "Por favor, informe as datas de início e fim.";
                return View(new List<Transacao>());
            }

            var transacoes = dbContext.Transacao
                .Find(t => t.Data >= startDate && t.Data <= endDate)
                .ToList();

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            return View(transacoes);
        }

        //Transações: Filtro por categoria
        public ActionResult FilterByCategory(string category)
        {
            var dbContext = new ContextMongodb();

            if (string.IsNullOrWhiteSpace(category))
            {
                ViewBag.ErrorMessage = "Por favor, informe uma categoria.";
                return View(new List<Transacao>());
            }

            var transacoes = dbContext.Transacao
                .Find(t => t.Category.ToLower() == category.ToLower())
                .ToList();
            
            ViewBag.Category = category;

            return View(transacoes);
        }


    }
}
