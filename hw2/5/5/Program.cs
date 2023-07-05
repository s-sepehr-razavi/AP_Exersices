using System.Net.Http.Headers;
// 2:30h (my code got deleted :/ ) + 0:30 debug
namespace _5
{

    class StockMarket
    {
        public string MarketName;
        public int[] StockValues;

        public StockMarket(string marketName, int[] StockValues)
        {
            this.MarketName = marketName;
            this.StockValues = StockValues;
        }

    }

    class Company
    {
        string CompanyName;
        public List<StockMarket> WorkingMarkets;

        public List<List<int>> BuyStockMethods(int[] dailyBuys, int TotalCash)
        {
            List<List<int>> list = new List<List<int>>();
            BuyStockMethods(dailyBuys, TotalCash, list);
            return list;
        }

        public int BestPeriods()
        {
            int[] profits = new int[WorkingMarkets.Count];

            for (int i = 0; i < profits.Length; i++)
            {
                StockMarket stock = WorkingMarkets[i];
                for (int j = 0; j < stock.StockValues.Length; j++)
                {
                    profits[i] += stock.StockValues[j] * should_i_buy(stock.StockValues, j, stock.StockValues.Length);
                } 
            }

            return profits.Max();
        }

        private int should_i_buy(int[] arr, int i, int n)
        {

            if (i == 0 && arr[i] < arr[i + 1])
            {
                return -1;
            }

            try
            {

            if (arr[i] <= arr[i - 1] && arr[i] < arr[i + 1])
            {
                return -1;
            }
            }
            catch (Exception)
            {

                
            }

            if (i > 0)
            {
                if (i == n - 1 && arr[i - 1] < arr[i])
                {
                    return 1;
                }

                try
                {

                if (arr[i] > arr[i + 1] && arr[i - 1] <= arr[i])
                {
                    return 1;
                }

                }
                catch (Exception)
                {
                    
                }
            }

            return 0;
        }



        private void BuyStockMethods(int[] dailyBuys, int TotalCash, List<List<int>> list, Stack<int> buys = null, int j = 0)
        {
            if (buys == null)
            {
                buys= new Stack<int>();                           
            }
            if (TotalCash < 0)
            {
                return;
            }

            if (TotalCash == 0)
            {
                List<int> toAdd = new List<int>();
                foreach (var item in buys)
                {
                    toAdd.Add(item);
                }
                list.Add(toAdd);
            }

            for (int i = j; i < dailyBuys.Length; i++)
            {
                int item = dailyBuys[i];
                buys.Push(item);
                BuyStockMethods(dailyBuys, TotalCash - item, list, buys, j);
                buys.Pop();
                j++;
            }
            
        } 
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();
            List<List<int>> x = company.BuyStockMethods(new int[] { 2, 5, 3, 6 }, 10);
            foreach (var item in x)
            {
                foreach (var xx in item)
                {
                    Console.Write(xx + " ");
                }

                Console.WriteLine();
            }

            StockMarket ny = new StockMarket("ny", new int[] { 100, 180, 260, 310, 40, 535, 695 });
            StockMarket hk = new StockMarket("hk", new int[] { 4, 2, 2, 2, 4 });
            company.WorkingMarkets = new List<StockMarket> { ny, hk };
            Console.WriteLine(company.BestPeriods());
        }
    }
}