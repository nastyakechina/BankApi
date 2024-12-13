using Microsoft.AspNetCore.Mvc;
using Models;
using Storage;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CoinController : ControllerBase
    {
        private readonly ICoinStorage _coinStorage;

        public CoinController(ICoinStorage coinStorage)
        {
            _coinStorage = coinStorage ?? throw new ArgumentNullException(nameof(coinStorage));
        }

        // Добавление новой валюты
        // [HttpPost]
        // public async Task<IActionResult> AddCoinAsync([FromBody] Coin coin, CancellationToken cancellationToken)
        // {
        //     if (coin == null || string.IsNullOrEmpty(coin.Name) || coin.Course <= 0)
        //     {
        //         return BadRequest("Неверные данные для валюты.");
        //     }
        //
        //     await _coinStorage.AddCoinAsync(coin, cancellationToken);
        //     return CreatedAtAction(nameof(GetCoinAsync), new { name = coin.Name }, coin);
        // }
        
        
        // [HttpPost]
        // public async Task<IActionResult> AddCoin([FromBody] Coin coin, CancellationToken cancellationToken)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);
        //
        //     await _coinStorage.AddCoinAsync(coin, cancellationToken);
        //     return Ok();
        // }
        
        [HttpPost]
        public async Task<IActionResult> AddCoinAsync([FromBody] Coin coin, CancellationToken cancellationToken)
        {
            try
            {
                await _coinStorage.AddCoinAsync(coin, cancellationToken);
                return CreatedAtAction(nameof(GetCoinAsync), new { name = coin.Name }, coin);
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                return Conflict($"Валюта с именем '{coin.Name}' уже существует.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }



        

        // Получение валюты по имени
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCoinAsync(string name, CancellationToken cancellationToken)
        {
            var coin = await _coinStorage.GetCoinAsync(name, cancellationToken);

            if (coin == null)
            {
                return NotFound($"Валюта с именем {name} не найдена.");
            }

            return Ok(coin);
        }
        
        
        
    }
}