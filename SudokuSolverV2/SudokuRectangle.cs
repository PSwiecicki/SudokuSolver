using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class SudokuRectangle : SudokuContainer
    {
        public SudokuRectangle(int size)
        {
            Container = new List<Field>();
            ValueToSet = new List<int>();
            for (int i = 1; i <= size; i++)
                ValueToSet.Add(i);
            isDone = false;
        }
    }
}