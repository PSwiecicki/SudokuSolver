using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class Field
    {
        private int _value;
        public readonly SudokuRectangle rectangle;
        public readonly SudokuRow row;
        public readonly SudokuColumn column;

        public Field(int sudokuSize, SudokuRow row, SudokuColumn col, SudokuRectangle rec)
        {
            this.row = row;
            this.rectangle = rec;
            this.column = col;
            this.Value = 0;
            this.IsValueSet = false;
            Optionalities = new List<int>();
            for(int i = 1; i <= sudokuSize; i++)
            {
                this.Optionalities.Add(i);
            }
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
                row.wasChanged = true;
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