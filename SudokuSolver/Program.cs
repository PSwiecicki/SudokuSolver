using System;
using System.Collections.Generic;

namespace SudokuSolver
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Proszę podawać wartości z zakresu od 0 do 9 oddzielone spacjami.");
            Console.WriteLine("Podanie 0 jako wartości oznacza, że jest ona nieznana.");
            Sudoku sudoku = new Sudoku();
            string[] input = new string[9]
                { "0 0 0 6 5 0 3 0 0",
                  "0 0 5 0 0 8 0 7 0",
                  "0 0 1 0 7 2 6 0 5",
                  "0 5 0 0 0 0 8 0 7",
                  "0 0 0 0 4 0 0 0 0",
                  "1 0 7 0 0 0 0 6 0",
                  "9 0 3 2 6 0 7 0 0",
                  "0 1 0 5 0 0 4 0 0",
                  "0 0 4 0 8 1 0 0 0"
                    };
            for (int i = 0; i < 9; i++)
            {
                
                string[] input_val = input[i].Split(" ");
                for(int j = 0; j < 9; j++)
                {
                    Field field = new Field(Convert.ToInt32(input_val[j]));
                    sudoku.Rows[i].RowFields.Add(field);
                    sudoku.Columns[j].ColumnFields.Add(field);
                    sudoku.Squares[3*(i/3)+(j/3)].SquareFields.Add(field);
                }
            }
            for (int i = 0; i < 20; i++)
            {
                sudoku.RemoveOptionalities();
                sudoku.SetValues();
            }
            for(int i =0; i < 9; i++)
            {
                Console.WriteLine(sudoku.Rows[i].ToString());

                Console.WriteLine("-------------------");
            }
        }
    }
}
