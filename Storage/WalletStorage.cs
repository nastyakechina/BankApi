using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public class WalletStorage : IWalletStorage
    {
        private readonly IDatabaseStorage<Wallet> _walletRepository;
        private readonly IDatabaseStorage<CoinAmount> _coinAmountRepository;
        
        public WalletStorage(IDatabaseStorage<Wallet> walletRepository, IDatabaseStorage<CoinAmount> coinAmountRepository)
        {
            _walletRepository = walletRepository;
            _coinAmountRepository = coinAmountRepository;
        }

        public async Task<List<Wallet>> GetAllWalletsAsync(CancellationToken cancellationToken)
        {
            var wallets = await _walletRepository.GetListAsync("1 = 1", null, cancellationToken);  // Возвращаем все записи
            return new List<Wallet>(wallets);
        }

        public async Task UpdateWalletAsync(Guid walletId, Wallet updatedWallet, CancellationToken cancellationToken)
        {
            var whereClause = "id = @walletId"; 
            
            var parameters = new
            {
                walletId, 
                updatedWallet.Name 
            };
            
            await _walletRepository.UpdateAsync(whereClause, parameters, updatedWallet, cancellationToken);
        }

        public async Task<List<CoinAmount>> GetCoinAmountsIdAsync(Guid walletId, CancellationToken cancellationToken)
        {
            var coinAmounts = await _coinAmountRepository.GetListAsync($"WalletId = '{walletId}'", null, cancellationToken);
            return new List<CoinAmount>(coinAmounts);
        }
        

        public async Task<CoinAmount> GetCoinAmountAsync(Guid walletId, string coinName, CancellationToken cancellationToken)
        {
            var coinAmounts = await GetCoinAmountsIdAsync(walletId, cancellationToken);
            return coinAmounts.FirstOrDefault(ca => ca.CoinName == coinName);
        }

        public async Task AddCoinAmountAsync(CoinAmount coinAmount, CancellationToken cancellationToken)
        {
            await _coinAmountRepository.AddAsync(coinAmount, cancellationToken);
        }

        public async Task UpdateCoinAmountAsync(CoinAmount updatedCoinAmount, CancellationToken cancellationToken)
        {
            var whereClause = "walletid = @walletId AND CoinName = @coinname";
            
            var parameters = new
            {
                walletId = updatedCoinAmount.WalletId, 
                coinName = updatedCoinAmount.CoinName, 
            };

            await _coinAmountRepository.UpdateAsync(whereClause, parameters, updatedCoinAmount, cancellationToken);
        }

        public async Task AddWalletAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            await _walletRepository.AddAsync(wallet, cancellationToken);
        }
        public async Task<Wallet> GetWalletByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var whereClause = "id = @id"; // Убедитесь, что имя столбца соответствует базе данных
            var parameters = new { id }; // Параметр для запроса
            
            var wallets = await _walletRepository.GetListAsync(whereClause, parameters, cancellationToken);
            return wallets.FirstOrDefault(); // Возвращаем первый найденный кошелек или null
        }
    }
}





/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public class WalletStorage : IWalletStorage
    {
        private readonly IStorageFile<Wallet> _walletStorageFile;
        private readonly IStorageFile<CoinAmount> _coinAmountStorageFile;

        public WalletStorage()
        {
            _walletStorageFile = new StorageFile<Wallet>("../../data/wallet.json", nameof(Wallet));
            _coinAmountStorageFile = new StorageFile<CoinAmount>("../../data/coinAmounts.json", nameof(CoinAmount));
        }

        public WalletStorage(IStorageFile<Wallet> walletStorageFile, IStorageFile<CoinAmount> coinAmountStorageFile)
        {
            _walletStorageFile = walletStorageFile;
            _coinAmountStorageFile = coinAmountStorageFile;
        }

        public async Task<List<Wallet>> GetAllWalletsAsync(CancellationToken cancellationToken)
        {
            return await _walletStorageFile.GetAllAsync(cancellationToken);
        }

        public async Task UpdateWalletAsync(Guid walletId, Wallet updatedWallet, CancellationToken cancellationToken)
        {
            await _walletStorageFile.UpdateAsync(wallet => wallet.id == walletId, updatedWallet, cancellationToken);
        }

        public async Task<List<CoinAmount>> GetCoinAmountsIdAsync(Guid walletId, CancellationToken cancellationToken)
        {
            var allCoinAmounts = await _coinAmountStorageFile.GetAllAsync(cancellationToken);
            return allCoinAmounts.Where(ca => ca.WalletId == walletId).ToList();
        }

        public async Task<CoinAmount> GetCoinAmountAsync(Guid walletId, string coinName, CancellationToken cancellationToken)
        {
            var coinAmounts = await GetCoinAmountsIdAsync(walletId, cancellationToken);
            return coinAmounts.FirstOrDefault(ca => ca.Coin.Name == coinName);
        }

        public async Task AddCoinAmountAsync(CoinAmount coinAmount, CancellationToken cancellationToken)
        {
            await _coinAmountStorageFile.AddAsync(coinAmount, cancellationToken);
        }

        public async Task UpdateCoinAmountAsync(CoinAmount updatedCoinAmount, CancellationToken cancellationToken)
        {
            await _coinAmountStorageFile.UpdateAsync(ca => ca.WalletId == updatedCoinAmount.WalletId && ca.Coin.Name == updatedCoinAmount.Coin.Name, updatedCoinAmount, cancellationToken);
        }
public async Task AddWalletAsync(Wallet wallet, CancellationToken cancellationToken)
{
  await _walletStorageFile.AddAsync(wallet, cancellationToken);
}
}
}*/
