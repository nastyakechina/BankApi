using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public class TransactionStorage : ITransactionStorage
    {
        private readonly IDatabaseStorage<Transaction> _databaseStorage;

        public TransactionStorage(IDatabaseStorage<Transaction> databaseStorage)
        {
            _databaseStorage = databaseStorage;
        }

        // Получение всех транзакций
        public async Task<List<Transaction>> GetAllTransactionsAsync(CancellationToken cancellationToken)
        {
            var transactions = await _databaseStorage.GetListAsync("1 = 1", null, cancellationToken); 
            return new List<Transaction>(transactions);
        }

        // Добавление новой транзакции
        public async Task AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            await _databaseStorage.AddAsync(transaction, cancellationToken);
        }
    }
}



/*using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public class TransactionStorage : ITransactionStorage
    {
        private readonly IStorageFile<Transaction> _storageFile;

        public TransactionStorage()
        {
            _storageFile = new StorageFile<Transaction>("../../data/Transaction.json", nameof(Transaction));
        }
        public TransactionStorage(IStorageFile<Transaction> storageFile)
        {
            _storageFile = storageFile;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(CancellationToken cancellationToken)
        {
            return await _storageFile.GetAllAsync(cancellationToken);
        }

        public async Task AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            await _storageFile.AddAsync(transaction, cancellationToken);
        }

        /*public async Task DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            await _storageFile.DeleteAsync(transaction => transaction.Id == transactionId, cancellationToken);
        }

        public async Task UpdateTransactionAsync(Guid transactionId, Transaction updatedTransaction, CancellationToken cancellationToken)
        {
            await _storageFile.UpdateAsync(transaction => transaction.Id == transactionId, updatedTransaction, cancellationToken);
        }*/
