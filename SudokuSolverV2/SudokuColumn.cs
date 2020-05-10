using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class SudokuColumn : SudokuSmallContainer
    {
        public SudokuColumn(int size)
        {
            container = new List<Field>();
            valuesToSet = new List<int>();
            for (int i = 1; i <= size; i++)
                valuesToSet.Add(i);
            isDone = false;
        }
    }
}