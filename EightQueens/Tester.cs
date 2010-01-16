using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EightQueens
{
    class Tester
    {
        static void Main(string[] args)
        {
            EightQueens queen = new EightQueens();

            Console.WriteLine("Calculating...");

            queen.BoardSize = 8;
            queen.CalculateSolutions();
            queen.WriteSolutionsToFile();

            Console.WriteLine("Total execution time: " + queen.TotalTime.Hours + " hours, " + queen.TotalTime.Minutes + " minutes, " + queen.TotalTime.Seconds + " seconds, " + queen.TotalTime.Milliseconds + " milliseconds");
            Console.WriteLine("Number of cycles: " + queen.cycles);
        }
    }
}
