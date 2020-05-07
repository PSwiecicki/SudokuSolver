using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    class Row
    {
        public Row()
        {
            RowFields = new List<Field>();
        }
        public List<Field> RowFields { get; set; }

        public override string ToString()
        {
            string output = "|";
            foreach (var field in RowFields)
                output += field.Value.ToString() + "|";
            return output;
        }

    }
}
