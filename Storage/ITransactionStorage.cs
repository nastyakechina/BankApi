using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models;
namespace Storage;

public interface ITransactionStorage
{
    Task<List<Transaction>> GetAllTransactionsAsync(CancellationToken cancellationToken);
    Task AddTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    //Task DeleteTransactionAsync(Guid transactionId, CancellationToken cancellationToken);
    //Task UpdateTransactionAsync(Guid transactionId, Transaction updatedTransaction, CancellationToken cancellationToken);
}