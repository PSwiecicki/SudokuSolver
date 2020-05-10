using System;

namespace SudokuSolverV2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Witam w aplikacji do rozwiązywania Sudoku!");
            Console.WriteLine("Jaką wysokość ma mały prostokąt?");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Jaką szerokość ma mały prostokąt?");
            int b = Convert.ToInt32(Console.ReadLine());
            Sudoku sudoku = new Sudoku(a, b);
            sudoku.InsertData();
            Console.WriteLine("");
        }
    }
}
