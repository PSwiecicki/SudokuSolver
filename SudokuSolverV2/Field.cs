using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class Field
    {
        private int _value;
        private readonly SudokuRectangle rectangle;
        private readonly SudokuRow row;
        private readonly SudokuColumn column;

        public Field(int sudokuSize, SudokuRow row, SudokuColumn col, SudokuRectangle rec)
        {
            this.Value = 0;
            this.IsValueSet = false;
            Optionalities = new List<int>();
            for(int i = 1; i <= sudokuSize; i++)
            {
                this.Optionalities.Add(i);
            }
            this.row = row;
            this.rectangle = rec;
            this.column = col;
        }
        public bool IsValueSet { get; set; }
        public List<int> Optionalities { get; set; }
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                    ClearOptionalities(value);
                Optionalities = null;
                IsValueSet = true;
                _value = value;
            }
        }

        private void ClearOptionalities(int value)
        {
            row.clearOptionalities(value);
            column.clearOptionalities(value);
            rectangle.clearOptionalities(value);
        }
    }
}