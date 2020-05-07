using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    class Square
    {
        public Square()
        {
            SquareFields = new List<Field>();
        }
        public List<Field> SquareFields { get; set; }
    }
}
