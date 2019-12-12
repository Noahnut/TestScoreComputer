using System;
using ComputerScore;

namespace testScore
{
    class Program
    {
        static void Main(string[] args)
        {
            ScoreComputer scoreComputer = new ScoreComputer();
            string v = Console.ReadLine();
            var score = scoreComputer.SendTestPaper(v).ToString();
            Console.WriteLine("Your Score is {0}", score); 
        }
    }
}
