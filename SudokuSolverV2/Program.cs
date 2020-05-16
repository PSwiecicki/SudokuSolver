using System;

namespace SudokuSolverV2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Witam w aplikacji do rozwiązywania Sudoku!");
            Console.WriteLine("Jaką szerokość ma mały prostokąt?");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Jaką wysokość ma mały prostokąt?");
            int b = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Podaj wszystkie {a * b} rzędów. Cyfry oddziel spacją, a w miejsce pustych pól wpisz 0.");
            Sudoku sudoku = new Sudoku(a, b);
            sudoku.InsertData();
            sudoku.Solve();
            Console.WriteLine("Stan Sudoku:");
            Console.WriteLine(sudoku.ToString());
        }
    }
}
