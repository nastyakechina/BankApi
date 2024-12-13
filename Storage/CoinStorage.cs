using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public class CoinStorage : ICoinStorage
    {
        private readonly IDatabaseStorage<Coin> _databaseStorage;
        
        public CoinStorage(IDatabaseStorage<Coin> databaseStorage)
        {
            _databaseStorage = databaseStorage;
        }

        // Добавление монеты
        public async Task AddCoinAsync(Coin coin, CancellationToken cancellationToken)
        {
            await _databaseStorage.AddAsync(coin, cancellationToken);
        }

        // Получение монеты по имени
        public async Task<Coin> GetCoinAsync(string name, CancellationToken cancellationToken)
        {
            var coins = await _databaseStorage.GetListAsync("name = @name", new { name }, cancellationToken);
            return coins.FirstOrDefault();
        }
    }
}



/*using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Storage
{
    public class CoinStorage : ICoinStorage
    {
       
        IStorageFile<Coin> _storageFile;

        public CoinStorage()
        {
            _storageFile = new StorageFile<Coin>("../../data/Coin.json","Coin.json");
        }

        public CoinStorage(IStorageFile<Coin> storageFile)
        {
            _storageFile = storageFile;
        }
        public async Task AddCoinAsync(Coin coin, CancellationToken cancellationToken)
        {
            await _storageFile.AddAsync(coin, cancellationToken);
        }

        public async Task<Coin> GetCoinAsync(string name, CancellationToken cancellationToken)
        {
            var coins = await _storageFile.GetAllAsync(cancellationToken);
            foreach (var coin in coins)
            {
                if (coin.Name == name)
                {
                    return coin;
                }
            }
            return null;
        }
    }
}*/