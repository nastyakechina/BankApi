using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models;

namespace Storage
{
    public interface ICoinStorage
    {
        Task AddCoinAsync(Coin coin, CancellationToken cancellationToken);
        Task<Coin> GetCoinAsync(string name, CancellationToken cancellationToken);
    }
}