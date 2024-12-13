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
    public class CoinAmountController : ControllerBase
    {
        private readonly IWalletStorage _walletStorage;

        public CoinAmountController(IWalletStorage walletStorage)
        {
            _walletStorage = walletStorage ?? throw new ArgumentNullException(nameof(walletStorage));
        }

        // Получить баланс всех монет в кошельке
        [HttpGet("{id}/balance")]
        public async Task<IActionResult> GetBalance(Guid id, CancellationToken cancellationToken)
        {
            var coinAmounts = await _walletStorage.GetCoinAmountsIdAsync(id, cancellationToken);
            if (coinAmounts == null || coinAmounts.Count == 0)
                return NotFound("No coin amounts found for this wallet.");
            
            return Ok(coinAmounts);
        }

        // Получить баланс конкретной монеты в кошельке
        [HttpGet("{id}/{coinName}/balance")]
        public async Task<IActionResult> GetCoinAmount(Guid id, string coinName, CancellationToken cancellationToken)
        {
            var coinAmount = await _walletStorage.GetCoinAmountAsync(id, coinName, cancellationToken);
            if (coinAmount == null)
                return NotFound($"Coin '{coinName}' not found in wallet.");
            
            return Ok(coinAmount);
        }

        // Добавить сумму монеты в кошелек
        [HttpPost("{id}/balance")]
        public async Task<IActionResult> AddCoinAmount(Guid id, [FromBody] CoinAmount coinAmount, CancellationToken cancellationToken)
        {
            if (coinAmount == null)
                return BadRequest("CoinAmount cannot be null.");

            coinAmount.WalletId = id; // Устанавливаем WalletId из параметра маршрута
            await _walletStorage.AddCoinAmountAsync(coinAmount, cancellationToken);
            return CreatedAtAction(nameof(GetCoinAmount), new { id = coinAmount.WalletId, coinName = coinAmount.CoinName }, coinAmount);
        }

        // Обновить сумму монеты в кошельке
        [HttpPut("{id}/balance")]
        public async Task<IActionResult> UpdateCoinAmount(Guid id, [FromBody] CoinAmount updatedCoinAmount, CancellationToken cancellationToken)
        {
            updatedCoinAmount.WalletId = id; // Устанавливаем WalletId из параметра маршрута
            await _walletStorage.UpdateCoinAmountAsync(updatedCoinAmount, cancellationToken);
            return NoContent();
        }
    }
}