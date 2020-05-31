using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolverV2
{
    class SudokuContainer
    {
        public List<Field> Container { get; set; }
        public List<int> ValueToSet { get; set; }

        public bool isDone;

        public void ClearOptions(int value)
        {
            if (!isDone)
            {
                foreach (var field in Container)
                {
                    if (!field.IsValueSet)
                        field.Options.Remove(value);
                }
                ValueToSet.Remove(value);
                if (ValueToSet.Count == 0)
                    isDone = true;
            }
        }

    }
}
