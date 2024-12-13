using Microsoft.AspNetCore.Mvc;
using Models;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionStorage _transactionStorage;

        public TransactionController(ITransactionStorage transactionStorage)
        {
            _transactionStorage = transactionStorage ?? throw new ArgumentNullException(nameof(transactionStorage));
        }

        // Получить все транзакции
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions(CancellationToken cancellationToken)
        {
            var transactions = await _transactionStorage.GetAllTransactionsAsync(cancellationToken);
            return Ok(transactions);
        }

        // Создать новую транзакцию
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction, CancellationToken cancellationToken)
        {
            if (transaction == null)
                return BadRequest("Transaction cannot be null.");

            await _transactionStorage.AddTransactionAsync(transaction, cancellationToken);
            return CreatedAtAction(nameof(GetAllTransactions), new { id = transaction.Id }, transaction);
        }
    }
}