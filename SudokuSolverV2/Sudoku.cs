

using System;

namespace SudokuSolverV2
{
    internal class Sudoku
    {
        public SudokuRow[] Rows { get; set; }
        public SudokuColumn[] Columns { get; set; } 
        public SudokuRectangle[] Rectangles { get; set; } 
        public Sudoku(int a, int b)
        {
            int size = a * b;
            Rows = new SudokuRow[size];
            Columns = new SudokuColumn[size];
            Rectangles = new SudokuRectangle[size];
            for (int i = 0; i < size; i++)
            {
                Rows[i] = new SudokuRow(size);
                Columns[i] = new SudokuColumn(size);
                Rectangles[i] = new SudokuRectangle(size);
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Field field = new Field(size, Rows[i], Columns[j], Rectangles[(i / b) * b + j / a]);
                    Rows[i].container.Add(field);
                    Columns[j].container.Add(field);
                    Rectangles[(i / b) * b + j / a].container.Add(field);
                }
            }

        }

        public void InsertData()
        {
            string[] input = new string[Rows.Length];
            for(int i = 0; i < input.Length;)
            {
                if (Rows[i].inputRow(Console.ReadLine()))
                    i++;
            }
                //{ "0 0 0 6 0 0 9 2 1",
                //  "2 0 9 0 0 0 6 0 0",
                //  "0 5 0 0 0 0 0 0 0",
                //  "8 7 0 0 4 9 0 1 0",
                //  "0 0 4 0 0 0 0 0 0",
                //  "0 0 0 0 0 8 3 0 0",
                //  "0 0 0 0 0 0 0 8 0",
                //  "6 0 0 5 1 4 0 0 7",
                //  "0 0 0 7 0 0 0 0 0"
                //    };
        }
    }
}