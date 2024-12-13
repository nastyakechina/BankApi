using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Presenter;
using Models;
using Storage;

namespace View
{
    public class Menu
    {
        private readonly ILogic _logic;
        private readonly CoinStorage _coinStorage;
        private readonly WalletStorage _walletStorage;
        private readonly TransactionStorage _transactionStorage;

        public Menu(CoinStorage coinStorage, WalletStorage walletStorage, TransactionStorage transactionStorage)
        {
            _coinStorage = coinStorage;
            _walletStorage = walletStorage;
            _transactionStorage = transactionStorage;
            _logic = new Logic(coinStorage, walletStorage, transactionStorage); 
        }

        public async Task StartMenuAsync(CancellationToken cancellationToken)
        {
            var menuOptions = new Dictionary<string, Func<CancellationToken, Task>>
            {
                { "1", AddCurrencyAsync },
                { "2", DepositAsync },
                { "3", ConvertAsync },
                { "4", ViewBalanceAsync },
                { "5", ViewHistoryAsync },
                { "0", ExitAsync }
            };

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                DisplayMainOptions(); 

                string choice = Console.ReadLine();
                if (menuOptions.TryGetValue(choice, out var selectedAction))
                {
                    await selectedAction.Invoke(cancellationToken); // Вызываем выбранный метод
                }
                else
                {
                    Console.WriteLine("> Неверный ввод. Пожалуйста, введите число от 0 до 5.");
                }
            }
        }

        private void DisplayMainOptions()
        {
            Console.WriteLine("\n> Выберите действие:");
            Console.WriteLine("1. Добавить валюту");
            Console.WriteLine("2. Пополнить кошелек");
            Console.WriteLine("3. Конвертировать валюту");
            Console.WriteLine("4. Посмотреть баланс");
            Console.WriteLine("5. Просмотреть историю транзакций");
            Console.WriteLine("0. Выйти из программы");
        }

        private async Task AddCurrencyAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Введите название валюты:");
            string currency = Console.ReadLine();

            Console.WriteLine("> Введите курс валюты:");
            if (decimal.TryParse(Console.ReadLine(), out decimal rate))
            {
                try
                {
                    await _logic.AddNewCoinAsync(new Coin(currency, rate), cancellationToken);
                    Console.WriteLine($"> Валюта {currency} успешно добавлена.");
                }
                catch (WalletException ex)
                {
                    Console.WriteLine($"> Ошибка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("> Ошибка: некорректный курс валюты.");
            }
        }

        private async Task DepositAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Введите валюту для пополнения:");
            string currency = Console.ReadLine();

            Console.WriteLine("> Введите сумму для пополнения:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                try
                {
                    await _logic.DepositWalletAsync(currency, amount, cancellationToken);
                    Console.WriteLine($"> Кошелек успешно пополнен на {amount} {currency}.");
                }
                catch (WalletException ex)
                {
                    Console.WriteLine($"> Ошибка при пополнении: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("> Ошибка: некорректная сумма.");
            }
        }

        private async Task ConvertAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Введите валюту для конвертации:");
            string curFrom = Console.ReadLine();

            Console.WriteLine("> Введите целевую валюту:");
            string curTo = Console.ReadLine();

            Console.WriteLine("> Введите сумму для конвертации:");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                try
                {
                    decimal convertedAmount = await _logic.ConversionCoinAsync(curFrom, curTo, amount, cancellationToken);
                    Console.WriteLine($"> Конвертация выполнена. Вы получили {convertedAmount} {curTo}.");
                }
                catch (WalletException ex)
                {
                    Console.WriteLine($"> Ошибка при конвертации: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"> Непредвиденная ошибка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("> Ошибка: некорректная сумма.");
            }
        }

        private async Task ViewBalanceAsync(CancellationToken cancellationToken)
        {
            try
            {
                var balance = await _logic.GetBalanceAsync(cancellationToken);
                Console.WriteLine("> Текущий баланс:");
                foreach (var entry in balance)
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }
            catch (WalletException ex)
            {
                Console.WriteLine($"> Ошибка при получении баланса: {ex.Message}");
            }
        }

        private async Task ViewHistoryAsync(CancellationToken cancellationToken)
        {
            try
            {
                List<Transaction> history = await _logic.GetHistoryAsync(cancellationToken);

                if (history.Count > 0)
                {
                    Console.WriteLine("> История транзакций:");
                    foreach (var transaction in history)
                    {
                        Console.WriteLine($" {transaction.Type} | Сумма: {transaction.Amount}");
                    }
                }
                else
                {
                    Console.WriteLine("> История пуста.");
                }
            }
            catch (WalletException ex)
            {
                Console.WriteLine($"> Ошибка при получении истории транзакций: {ex.Message}");
            }
        }

        private Task ExitAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("> Завершение работы...");
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
