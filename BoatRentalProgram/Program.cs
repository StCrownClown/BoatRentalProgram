using System;
using System.Collections.Generic;
using System.Linq;

namespace BoatRentalProgram
{
    class Program
    {
        public static List<Boat> boats = new List<Boat>
        {
            new Boat("Tiny", 5, 5),
            new Boat("Medium", 10, 8),
            new Boat("Large", 15, 12)
        };

        static void Main(string[] args)
        {
            string input = "";
            bool isValid = false;
            bool again = true;
            int number = 0;
            List<Boat> result = new List<Boat>();

            while (again)
            {
                ValidateInput(ref input, ref number, ref isValid);

                CalculationBoat(number, ref result);

                PrintResult(ref result);

                PlayAgain(ref again, ref isValid);
            }
        }

        public static void PrintResult(ref List<Boat> result)
        {
            var query = result.GroupBy(d => new { d.size })
                .Select(g => new
                {
                    g.Key.size,
                    count = g.Count(),
                    cost = g.Sum(d => d.cost)
                });

            var response = query.ToList();

            response.ForEach(i => Console.Write("{0}\t", i));
        }

        public static List<Boat> CalculationBoat(int number, ref List<Boat> result)
        {

            while (number > 0)
            {
                Boat findMostCost = boats.OrderByDescending(o => o.costPerSeat)
                    .FirstOrDefault(b => b.costPerSeat == boats.Max(a => a.costPerSeat));

                Boat findSmallest = boats.OrderBy(o => o.seat)
                    .FirstOrDefault(b => b.seat == boats.Min(a => a.seat));

                Boat largest = boats.OrderByDescending(a => a.seat).FirstOrDefault();

                if (number <= findSmallest.seat)
                {
                    result.Add(findSmallest);
                    number -= findSmallest.seat;
                }
                else
                {
                    Boat minCostminSize = null;
                    if (number > largest.seat)
                    {
                        minCostminSize = boats.OrderBy(o => o.costPerSeat)
                            .ThenBy(a => a.seat)
                            .FirstOrDefault(b => b.seat < number && b != findMostCost);
                    }
                    else
                    {
                        minCostminSize = boats.OrderBy(o => o.costPerSeat)
                            .ThenBy(a => a.seat)
                            .FirstOrDefault(b => b.seat > number);
                    }

                    if (minCostminSize == findMostCost)
                    {
                        Boat lastItem = result[result.Count - 1];
                        result.Remove(lastItem);
                        number += lastItem.seat;

                        Boat findLarger = boats.OrderByDescending(o => o.seat)
                            .FirstOrDefault(l => l != findMostCost && l.seat > lastItem.seat);
                        result.Add(findLarger);
                        number -= findLarger.seat;
                    }
                    else
                    {
                        result.Add(minCostminSize);
                        number -= minCostminSize.seat;
                    }
                }
            }

            return result;
        }

        public IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 100)
        {
            for (int i = 0; i < locations.Count; i += nSize)
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
        }

        public class Boat
        {
            public Boat(string size, int seat, int cost)
            {
                this.size = size;
                this.seat = seat;
                this.cost = cost;
            }

            public string size { get; set; }
            public int seat { get; set; }
            public int cost { get; set; }
            public float costPerSeat => cost / seat;
        }

        static readonly Func<string, int> _parseInt = i => Convert.ToInt32(i);

        public static void ValidateInput(ref string input, ref int number, ref bool isValid)
        {
            while (!isValid)
            {
                Console.WriteLine("########## Validate ##########");
                Console.WriteLine("Value of Number must non-negative Number");
                Console.WriteLine("Not alphabets or special character");
                Console.WriteLine("Example input: 4 or 16 or 23 or 36");
                Console.WriteLine("Please enter seat input numbers");

                input = Console.ReadLine();

                try
                {
                    number = _parseInt(input);
                }
                catch
                {
                    Console.WriteLine("Invalid input");

                    isValid = false;
                    continue;
                }

                isValid = true;
            }
        }

        public static void PlayAgain(ref bool again, ref bool isValid)
        {
            Console.WriteLine("Do you want to play again ? (Y/N)");

            string input = Console.ReadLine();

            if (input.ToUpper() == "Y")
            {
                again = true;
                isValid = false;
            }
            else
            {
                again = false;
            }
        }
    }
}
