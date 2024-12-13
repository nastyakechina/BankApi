using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models;

namespace Storage;

public interface IWalletStorage
{
    Task<List<Wallet>> GetAllWalletsAsync(CancellationToken cancellationToken);
    Task UpdateWalletAsync(Guid walletId, Wallet updatedWallet, CancellationToken cancellationToken);

    Task<CoinAmount> GetCoinAmountAsync(Guid walletId, string coinName, CancellationToken cancellationToken);

    Task AddCoinAmountAsync(CoinAmount coinAmount, CancellationToken cancellationToken);
    Task UpdateCoinAmountAsync(CoinAmount updatedCoinAmount, CancellationToken cancellationToken);
    Task<List<CoinAmount>> GetCoinAmountsIdAsync(Guid walletId, CancellationToken cancellationToken);
    Task AddWalletAsync(Wallet wallet, CancellationToken cancellationToken);
    Task<Wallet> GetWalletByIdAsync(Guid id, CancellationToken cancellationToken);

    //Task DeleteWalletAsync(Guid walletId, CancellationToken cancellationToken);
}