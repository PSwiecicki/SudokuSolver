using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    class Column
    {
        public Column()
        {
            ColumnFields = new List<Field>();
        }

        public List<Field> ColumnFields { get; set; }
    }
}
