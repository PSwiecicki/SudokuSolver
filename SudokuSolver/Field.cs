using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    class Field
    {

        public Field(int val)
        {
            if (val >= 1 && val <= 9)
                IsValueSet = true;
            else
                IsValueSet = false;
            if (val == 0)
                Options = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Value = val;

        }

        public bool IsValueSet { get; set; }

        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = (value >= -1 && value <= 9) ? value : -1;
                if(Value == -1)
                    Console.WriteLine("Błędna wartość.");
            }
        }

        public List<int> Options { get; set; }

    }
}
