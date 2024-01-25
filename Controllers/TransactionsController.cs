using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.DAL;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using ExpenseTracker.Services;

namespace ExpenseTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ExpenseTrackerContext _context;
        private readonly ICsvParserService _csvParserService;

        public TransactionsController(ExpenseTrackerContext context, ICsvParserService csvParserService)
        {
            _context = context;
            _csvParserService = csvParserService;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(int id)
        {
            //test
            var transactions = await _context.Transactions.ToListAsync();

            foreach (var trans in transactions)
            {
                //Load sample data
                var sampleData = new TransactionSortingMLModel.ModelInput()
                {
                    Col0 = trans.Notes,
                };

                //Load model and predict output
                trans.CategoryId = (int)TransactionSortingMLModel.Predict(sampleData).PredictedLabel;
            }
            await _context.SaveChangesAsync();
            //test ml


            ViewBag.AccountId = id;
            ViewBag.Account = _context.Accounts.Find(id);

            //TODO: FIND A MORE EFFICIENT WAY TO DO THIS. IT TAKES SO LONG
            var expenseTrackerContext = _context.Transactions.Where(x => x.AccountId == id).Include(x => x.Category).Include(x => x.AccountUsers);

            return View(await expenseTrackerContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.AccountUsers)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public async Task<IActionResult> CreateAsync(int accountId)
        {
            ViewBag.AccountDetails = _context.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
            ViewBag.AccountUsersId = await _context.AccountUsers.Where(x => x.AccountId == accountId).ToListAsync();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,AccountId,Date,isReoccuring,Notes,AccountUsersId,Amount")] Transaction transaction)
        {
            if (transaction is not null)
            {
                try
                {
                    var account = _context.Accounts.Find(transaction.AccountId);
                    if (account != null)
                    {
                        account.Total += transaction.Amount; //+ a negative is subtracing
                        _context.Update(account);
                    }
                    _context.Add(transaction);                   
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    ViewBag.AccountDetails = _context.Accounts.Where(x => x.Id == transaction.AccountId).FirstOrDefault();
                    ViewBag.AccountUsersId = await _context.AccountUsers.Where(x => x.AccountId == transaction.AccountId).ToListAsync();
                    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                    return View(transaction);
                }
                return RedirectToAction("Index", new { id = transaction.AccountId });
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", transaction.AccountId);
            ViewData["AccountUsersId"] = new SelectList(_context.AccountUsers, "Id", "Id", transaction.AccountUsersId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,AccountId,Date,isReoccuring,Notes,AccountUsersId")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", transaction.AccountId);
            ViewData["AccountUsersId"] = new SelectList(_context.AccountUsers, "Id", "Id", transaction.AccountUsersId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.AccountUsers)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task UploadAsync(IFormFile formData, int accountId)
        {
            var transactions = _csvParserService.GetWFTransactions(formData, accountId);
            await _context.Transactions.BulkInsertOptimizedAsync(transactions);
            await _context.SaveChangesAsync();
        }
    }
    public class JSONData
    {
        public IFormFile formData { get; set; }
        public int accountId { get; set; }
    }

    
}
