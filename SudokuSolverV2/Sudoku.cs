

using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class Sudoku
    {
        private readonly int _width;
        private readonly int _high;
        private readonly int _size;
        private bool Error { get; set; }
        public SudokuRow[] Rows { get; set; }
        public SudokuColumn[] Columns { get; set; } 
        public SudokuRectangle[] Rectangles { get; set; } 
        public Sudoku(int a, int b)
        {
            _size = a * b;
            Rows = new SudokuRow[_size];
            Columns = new SudokuColumn[_size];
            Rectangles = new SudokuRectangle[_size];
            for (int i = 0; i < _size; i++)
            {
                Rows[i] = new SudokuRow(_size);
                Columns[i] = new SudokuColumn(_size);
                Rectangles[i] = new SudokuRectangle(_size);
            }
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Field pole = new Field(_size, Rows[i], Columns[j], Rectangles[(i / b) * b + j / a]);
                    Rows[i].Container.Add(pole);
                    Columns[j].Container.Add(pole);
                    Rectangles[(i / b) * b + j / a].Container.Add(pole);
                }
            }
            _width = a;
            _high = b;
        }

        public Sudoku(Sudoku sudokuCopy, Field fieldToChange, int Value)
        {
            _size = sudokuCopy._size;
            _high = sudokuCopy._high;
            _width = sudokuCopy._width;
            Rows = new SudokuRow[_size];
            Columns = new SudokuColumn[_size];
            Rectangles = new SudokuRectangle[_size];
            for (int i = 0; i < _size; i++)
            {
                Rows[i] = new SudokuRow(_size);
                Columns[i] = new SudokuColumn(_size);
                Rectangles[i] = new SudokuRectangle(_size);
            }
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Field field = new Field(_size, Rows[i], Columns[j], Rectangles[(i / _high) * _high + j / _width]);
                    Rows[i].Container.Add(field);
                    Columns[j].Container.Add(field);
                    Rectangles[(i / _high) * _high + j / _width].Container.Add(field);
                }
            }
            InsertData(sudokuCopy.Rows);
            for(int i = 0; i < sudokuCopy.Rows.Length; i++)
            {
                for(int j = 0;  j < sudokuCopy.Rows[i].Container.Count; j++)
                {
                    if(fieldToChange == sudokuCopy.Rows[i].Container[j])
                    {
                        Rows[i].Container[j].Value = Value;
                    }
                }
            }
        }
        public void InsertData()
        {
            for(int i = 0; i < Rows.Length;)
            {
                if (Rows[i].SetValueInRow(Console.ReadLine()))
                    i++;
            }
        }

        private void InsertData(SudokuRow[] rows)
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                Rows[i].SetValueInRow(rows[i]);
            }
        }

        void OneValueInContainer(SudokuContainer[] containerTable)
        {
            foreach (var container in containerTable)
            {
                if (!container.isDone)
                {
                    int counter;
                    Field fieldWithPosspibleValue = null;
                    for(int i = 1; i < _size; i++)
                    {
                        if (container.ValueToSet.Contains(i))
                        {
                            counter = 0;
                            foreach (var field in container.Container)
                            {
                                if (field.IsValueSet == false && field.Options.Contains(i))
                                {
                                    counter++;
                                    fieldWithPosspibleValue = field;
                                }
                            }
                            if (counter == 1)
                            {
                                fieldWithPosspibleValue.Value = i;
                            }
                        }
                    }
                }
            }
        }

        void OneOptionInField()
        {
            foreach (var row in Rows)
            {
                foreach (var field in row.Container)
                {
                    if (!field.IsValueSet && field.Options.Count == 1)
                    {
                        field.Value = field.Options[0];
                    }
                    if(!field.IsValueSet && field.Options.Count == 0)
                    {
                        this.Error = true;
                    }
                }
            }
        }

        void FewSameOptionalitiesInRectangle()
        {
            for (int i = 0; i < _size; i++)
            {
                if (!Rectangles[i].isDone)
                {
                    foreach( var value in Rectangles[i].ValueToSet)
                    {
                        List<int> fieldIndexes = new List<int>();
                        foreach (var field in Rectangles[i].Container)
                        {
                            if (field.IsValueSet == false && field.Options.Contains(value))
                                fieldIndexes.Add(Rectangles[i].Container.IndexOf(field));
                        }
                        if(fieldIndexes.Count >= 2 && fieldIndexes.Count <= _high)
                        {
                            var firstIndexField = Rectangles[i].Container[fieldIndexes[0]];
                            var detectedRow = firstIndexField.Row;
                            bool isItRow = true;
                            foreach(var index in fieldIndexes)
                            {
                                if(Rectangles[i].Container[index].Row != detectedRow)
                                {
                                    isItRow = false;
                                    break;
                                }
                            }
                            if (isItRow)
                            {
                                foreach (var field in detectedRow.Container)
                                {
                                    if (field.IsValueSet == false && field.Rectangle != firstIndexField.Rectangle && field.Options.Contains(value))
                                    {
                                        field.Options.Remove(value);
                                        field.Row.WasChanged = true;
                                    }

                                }
                            }
                        }
                        if (fieldIndexes.Count >= 2 && fieldIndexes.Count <= _width)
                        {
                            var firstIndexField = Rectangles[i].Container[fieldIndexes[0]];
                            var detectedColumn = firstIndexField.Column;
                            bool isItColumn = true;
                            foreach (var index in fieldIndexes)
                            {
                                if (Rectangles[i].Container[index].Column != detectedColumn)
                                {
                                    isItColumn = false;
                                    break;
                                }
                            }
                            if (isItColumn)
                            {
                                foreach (var field in detectedColumn.Container)
                                {
                                    if (field.IsValueSet == false && field.Rectangle != firstIndexField.Rectangle && field.Options.Contains(value))
                                    {
                                        field.Options.Remove(value);
                                        field.Row.WasChanged = true;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        void SameOptionalitiesInRow()
        {
            for (int i = 0; i < _size; i++)
            {
                if (!Rows[i].isDone)
                {
                    foreach (var value in Rows[i].ValueToSet)
                    {
                        List<int> fieldIndexes = new List<int>();
                        foreach (var field in Rows[i].Container)
                        {
                            if (field.IsValueSet == false && field.Options.Contains(value))
                            {
                                fieldIndexes.Add(Rows[i].Container.IndexOf(field));
                            }
                        }
                        if ( 2 <= fieldIndexes.Count && fieldIndexes.Count <= _high)
                        {
                            var firstIndexField = Rows[i].Container[fieldIndexes[0]];
                            var detectedRectangle = firstIndexField.Rectangle;
                            bool isItRectangle = true;
                            foreach (var index in fieldIndexes)
                            {
                                if (Rows[i].Container[index].Rectangle != firstIndexField.Rectangle)
                                {
                                    isItRectangle = false;
                                    break;
                                }
                            }
                            if (isItRectangle)
                            {
                                foreach (var field in detectedRectangle.Container)
                                {
                                    if (field.IsValueSet == false && field.Row != firstIndexField.Row && field.Options.Contains(value))
                                    {
                                        field.Options.Remove(value);
                                        field.Row.WasChanged = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void SameOptionaliteiesInColumn()
        {
            for (int i = 0; i < _size; i++)
            {
                if (!Columns[i].isDone)
                {
                    foreach (var value in Columns[i].ValueToSet)
                    {
                        List<int> fieldIndexes = new List<int>();
                        foreach (var field in Columns[i].Container)
                        {
                            if (field.IsValueSet == false && field.Options.Contains(value))
                            {
                                fieldIndexes.Add(Columns[i].Container.IndexOf(field));
                            }
                        }
                        if (2 <= fieldIndexes.Count && fieldIndexes.Count <= _width)
                        {
                            var firstIndexField = Columns[i].Container[fieldIndexes[0]];
                            var detectedRectangle = firstIndexField.Rectangle;
                            bool isItRectangle = true;
                            foreach (var index in fieldIndexes)
                            {
                                if (Columns[i].Container[index].Rectangle != firstIndexField.Rectangle)
                                {
                                    isItRectangle = false;
                                    break;
                                }
                            }
                            if (isItRectangle)
                            {
                                foreach (var field in detectedRectangle.Container)
                                {
                                    if (field.IsValueSet == false && field.Column != firstIndexField.Column && field.Options.Contains(value))
                                    {
                                        field.Options.Remove(value);
                                        field.Row.WasChanged = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void SameOptionalitiesInContainer(SudokuContainer[] containerTablr)
        {
            foreach (var container in containerTablr)
            {
                if (!container.isDone)
                {
                    foreach (var checkingField in container.Container)
                    {
                        List<int> possibleFieldIndexes = new List<int>();
                        if (checkingField.IsValueSet == false)
                        {
                            foreach (var possibleField in container.Container)
                            {
                                if (checkingField != possibleField && possibleField.IsValueSet == false)
                                {
                                    bool gotAllOptionalities = true;
                                    foreach (var option in possibleField.Options)
                                    {
                                        if (!checkingField.Options.Contains(option))
                                        {
                                            gotAllOptionalities = false;
                                            break;
                                        }
                                    }
                                    if (gotAllOptionalities)
                                    {
                                        possibleFieldIndexes.Add(container.Container.IndexOf(possibleField));
                                    }
                                }
                            }
                        }
                        if (!checkingField.IsValueSet && (checkingField.Options.Count == (possibleFieldIndexes.Count + 1)))
                        {
                            foreach (var option in checkingField.Options)
                            {
                                for (int i = 0; i < _size; i++)
                                {
                                    if (!container.Container[i].IsValueSet && container.Container[i].Options.Contains(option) && (container.Container[i] != checkingField && !possibleFieldIndexes.Contains(i)))
                                    {
                                        container.Container[i].Options.Remove(option);
                                        container.Container[i].Row.WasChanged = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void SameOptionalitiesFromTwoFieldsInContainer(SudokuContainer[] containerTable)
        {
            foreach (var container in containerTable)
            {
                if (!container.isDone)
                {
                    foreach (var checkingField1 in container.Container)
                    {
                        foreach (var checkingField2 in container.Container)
                        {
                            List<int> possibleFieldIndexes = new List<int>();
                            HashSet<int> possibleValues = new HashSet<int>();
                            if (checkingField1 != checkingField2 && checkingField1.IsValueSet == false && checkingField2.IsValueSet == false)
                            {
                                foreach (var Value in checkingField1.Options)
                                    possibleValues.Add(Value);
                                foreach (var Value in checkingField2.Options)
                                    possibleValues.Add(Value);
                                foreach (var possibleField in container.Container)
                                {
                                    if (checkingField2 != possibleField && checkingField1 != possibleField && possibleField.IsValueSet == false)
                                    {
                                        bool gotAllOptionalities = true;
                                        foreach (var option in possibleField.Options)
                                        {
                                            if (!possibleValues.Contains(option))
                                            {
                                                gotAllOptionalities = false;
                                                break;
                                            }
                                        }
                                        if (gotAllOptionalities)
                                        {
                                            possibleFieldIndexes.Add(container.Container.IndexOf(possibleField));
                                        }
                                    }
                                }
                            }

                            if (!checkingField2.IsValueSet && !checkingField1.IsValueSet && (possibleValues.Count == (possibleFieldIndexes.Count + 2)))
                            {
                                foreach (var option in possibleValues)
                                {
                                    for (int i = 0; i < _size; i++)
                                    {
                                        if (!container.Container[i].IsValueSet && container.Container[i].Options.Contains(option) && (container.Container[i] != checkingField2 && container.Container[i] != checkingField1 && !possibleFieldIndexes.Contains(i)))
                                        {
                                            container.Container[i].Options.Remove(option);
                                            container.Container[i].Row.WasChanged = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public bool Solve()
        {
            bool isSudokuSolved = false;
            
            while (!isSudokuSolved)
            {
                bool wasChanged = false;
                isSudokuSolved = true;
                foreach (var row in Rows)
                {
                    row.WasChanged = false;
                }
                RemoveOptionalities();
                if (this.Error)
                    return false;
                SetValues();
                foreach (var row in Rows)
                {
                    wasChanged |= row.WasChanged;
                    isSudokuSolved &= row.isDone;
                }
                if (!isSudokuSolved && !wasChanged)
                {
                    Stack<Sudoku> sudokuPossibleSolves = new Stack<Sudoku>();
                    bool isFieldChosen = false;
                    foreach (var row in Rows)
                    {
                        if (!row.isDone)
                        {
                            foreach (var field in row.Container)
                            {
                                if (!field.IsValueSet)
                                {
                                    foreach(var value in field.Options)
                                    {
                                        sudokuPossibleSolves.Push(new Sudoku(this, field, value));
                                    }
                                    isFieldChosen = true;
                                    break;
                                }
                            }
                        }
                        if (isFieldChosen)
                            break;
                    }
                    Sudoku sudoku = sudokuPossibleSolves.Pop();
                    bool correctSudoku;
                    do
                    {
                        correctSudoku = sudoku.Solve();
                        if(sudoku.Error)
                        {
                            sudoku = sudokuPossibleSolves.Pop();
                        }
                    }
                    while (!correctSudoku);
                   for(int i = 0; i < Rows.Length; i++)
                    {
                        for(int j = 0; j < Rows[i].Container.Count; j++)
                        {
                            if(!Rows[i].Container[j].IsValueSet)
                            {
                                Rows[i].Container[j].Value = sudoku.Rows[i].Container[j].Value;
                            }
                        }
                    }
                }
            }
            return true;
            

        }

        void RemoveOptionalities()
        {
            SameOptionaliteiesInColumn();
            SameOptionalitiesInRow();
            FewSameOptionalitiesInRectangle();
            SameOptionalitiesInContainer(Rows);
            SameOptionalitiesInContainer(Columns);
            SameOptionalitiesInContainer(Rectangles);
            SameOptionalitiesFromTwoFieldsInContainer(Rows);
            SameOptionalitiesFromTwoFieldsInContainer(Columns);
            SameOptionalitiesFromTwoFieldsInContainer(Rectangles);
        }

        void SetValues()
        {
            OneValueInContainer(Rectangles);
            OneValueInContainer(Rows);
            OneValueInContainer(Columns);
            OneOptionInField();
        }

        public override string ToString()
        {
            string s = "";
            foreach(var row in Rows)
            {
                s += row.ToString() + "\n";
            }
            return s;
        }

    }
}