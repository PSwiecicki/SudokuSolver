using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class Field
    {
        private int _Value;
        public readonly SudokuRectangle Rectangle;
        public readonly SudokuRow Row;
        public readonly SudokuColumn Column;
        public bool IsValueSet { get; set; }
        public List<int> Options { get; set; }

        public Field(int size, SudokuRow row, SudokuColumn column, SudokuRectangle rectangle)
        {
            this.Row = row;
            this.Rectangle = rectangle;
            this.Column = column;
            this.Value = 0;
            this.IsValueSet = false;
            Options = new List<int>();
            for(int i = 1; i <= size; i++)
            {
                this.Options.Add(i);
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value != _Value)
                    ClearOptions(value);
                Options = null;
                IsValueSet = true;
                _Value = value;
                Row.WasChanged = true;
            }
        }

        private void ClearOptions(int value)
        {
            Row.ClearOptions(value);
            Column.ClearOptions(value);
            Rectangle.ClearOptions(value);
        }
    }
}