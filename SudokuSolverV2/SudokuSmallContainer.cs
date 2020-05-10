using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolverV2
{
    class SudokuSmallContainer
    {
        public List<Field> container { get; set; }
        public List<int> valuesToSet { get; set; }

        public bool isDone;

        public void clearOptionalities(int value)
        {
            if (!isDone)
            {
                foreach (var field in container)
                {
                    if (!field.IsValueSet)
                        field.Optionalities.Remove(value);
                }
                valuesToSet.Remove(value);
                if (valuesToSet.Count == 0)
                    isDone = true;
            }
        }

    }
}
