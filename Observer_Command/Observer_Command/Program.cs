using System;
using System.Collections.Generic;

// Observer interface
interface IObserver
{
    void Update(double price);
}

// Subject class
class StockTicker
{
    private Dictionary<string, double> prices = new Dictionary<string, double>();
    private List<IObserver> observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void SetPrice(string stockName, double price)
    {
        if (prices.ContainsKey(stockName))
        {
            if (prices[stockName] != price)
            {
                prices[stockName] = price;
                NotifyObservers(stockName, price);
            }
        }
        else
        {
            prices.Add(stockName, price);
            NotifyObservers(stockName, price);
        }
    }

    public void RemoveStock(string stockName)
    {
        if (prices.ContainsKey(stockName))
        {
            prices.Remove(stockName);
            RemoveObserver(new StockDisplay(stockName));
            Console.WriteLine("Stock {0} removed", stockName);
        }
        else
        {
            Console.WriteLine("Stock {0} not found", stockName);
        }
    }

    private void NotifyObservers(string stockName, double price)
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(price);
        }
    }

    public void PrintPrices()
    {
        Console.WriteLine("Current stock prices:");
        foreach (var price in prices)
        {
            Console.WriteLine("{0}: {1}", price.Key, price.Value);
        }
    }

    public List<IObserver> GetObservers()
    {
        return observers;
    }
}

// Concrete observer class
class StockDisplay : IObserver
{
    private string stockName;

    public StockDisplay(string stockName)
    {
        this.stockName = stockName;
    }

    public string StockName
    {
        get { return stockName; }
    }

    public void Update(double price)
    {
        Console.WriteLine("{0} price update: {1}", stockName, price);
    }
}

// Command interface
interface ICommand
{
    void Execute();
}

// Concrete command class for adding a new stock observer
class AddStockCommand : ICommand
{
    private StockTicker stockTicker;

    public AddStockCommand(StockTicker stockTicker)
    {
        this.stockTicker = stockTicker;
    }

    public void Execute()
    {
        Console.WriteLine("Enter stock name:");
        string stockName = Console.ReadLine();
        StockDisplay display = new StockDisplay(stockName);
        stockTicker.AddObserver(display);
        Console.WriteLine("Stock {0} added", stockName);
    }
}

// Concrete command class for removing a stock observer
class RemoveStockCommand : ICommand
{
    private StockTicker stockTicker;

    public RemoveStockCommand(StockTicker stockTicker)
    {
        this.stockTicker = stockTicker;
    }

    public void Execute()
    {
        Console.WriteLine("Enter stock name to remove:");
        string stockName = Console.ReadLine();
        stockTicker.RemoveStock(stockName);
    }
}

// Concrete command class for setting a stock price
class SetPriceCommand : ICommand
{
    private StockTicker stockTicker;

    public SetPriceCommand(StockTicker stockTicker)
    {
        this.stockTicker = stockTicker;
    }

    public void Execute()
    {
        Console.WriteLine("Enter stock name:");
        string stockName = Console.ReadLine();
        Console.WriteLine("Enter stock price:");
        double stockPrice = Double.Parse(Console.ReadLine());
        stockTicker.SetPrice(stockName, stockPrice);
    }
}

// Invoker class
class StockCommander
{
    private ICommand addStockCommand;
    private ICommand removeStockCommand;
    private ICommand setPriceCommand;
    public StockCommander(ICommand addStockCommand, ICommand removeStockCommand, ICommand setPriceCommand)
    {
        this.addStockCommand = addStockCommand;
        this.removeStockCommand = removeStockCommand;
        this.setPriceCommand = setPriceCommand;
    }

    public void AddStock()
    {
        addStockCommand.Execute();
    }

    public void RemoveStock()
    {
        removeStockCommand.Execute();
    }

    public void SetPrice()
    {
        setPriceCommand.Execute();
    }
}

// Client class
class Program
{
    static void Main(string[] args)
    {
        // create the subject
        StockTicker stockTicker = new StockTicker();
        // create the commands
        AddStockCommand addStockCommand = new AddStockCommand(stockTicker);
        RemoveStockCommand removeStockCommand = new RemoveStockCommand(stockTicker);
        SetPriceCommand setPriceCommand = new SetPriceCommand(stockTicker);

        // create the invoker
        StockCommander stockCommander = new StockCommander(addStockCommand, removeStockCommand, setPriceCommand);

        // loop to accept commands
        while (true)
        {
            Console.WriteLine("Enter command: (a)dd stock, (r)emove stock, (s)et price, (p)rint prices, (q)uit");
            string command = Console.ReadLine();

            switch (command.ToLower())
            {
                case "a":
                    stockCommander.AddStock();
                    break;
                case "r":
                    stockCommander.RemoveStock();
                    break;
                case "s":
                    stockCommander.SetPrice();
                    break;
                case "p":
                    stockTicker.PrintPrices();
                    break;
                case "q":
                    return;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}