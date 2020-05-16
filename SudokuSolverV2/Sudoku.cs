

using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class Sudoku
    {
        private readonly int _width;
        private readonly int _high;
        private readonly int _size;
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
                    Field field = new Field(_size, Rows[i], Columns[j], Rectangles[(i / b) * b + j / a]);
                    Rows[i].container.Add(field);
                    Columns[j].container.Add(field);
                    Rectangles[(i / b) * b + j / a].container.Add(field);
                }
            }
            _width = a;
            _high = b;
        }

        public void InsertData()
        {
            string[] input = new string[Rows.Length];
            for(int i = 0; i < input.Length;)
            {
                if (Rows[i].inputRow(Console.ReadLine()))
                    i++;
            }
                //{ "0 0 0 6 0 0 9 2 1",
                //  "2 0 9 0 0 0 6 0 0",
                //  "0 5 0 0 0 0 0 0 0",
                //  "8 7 0 0 4 9 0 1 0",
                //  "0 0 4 0 0 0 0 0 0",
                //  "0 0 0 0 0 8 3 0 0",
                //  "0 0 0 0 0 0 0 8 0",
                //  "6 0 0 5 1 4 0 0 7",
                //  "0 0 0 7 0 0 0 0 0"
                //    };
        }

        void OneValueOptionInConteiner(SudokuSmallContainer[] containersTable)
        {
            foreach (var item in containersTable)
            {
                if (!item.isDone)
                {
                    int numCounter;
                    Field optionalField = null;
                    for(int i = 1; i < _size; i++)
                    {
                        if (item.valuesToSet.Contains(i))
                        {
                            numCounter = 0;
                            foreach (var field in item.container)
                            {
                                if (field.IsValueSet == false && field.Optionalities.Contains(i))
                                {
                                    numCounter++;
                                    optionalField = field;
                                }
                            }
                            if (numCounter == 1)
                            {
                                optionalField.Value = i;
                            }
                        }
                    }
                }
            }
        }

        void OneValueOptionInField()
        {
            foreach (var row in Rows)
            {
                foreach (var field in row.container)
                {
                    if (!field.IsValueSet && field.Optionalities.Count == 1)
                    {
                        field.Value = field.Optionalities[0];
                    }
                }
            }
        }

        void FewOptionalityInSquare()
        {
            for (int i = 0; i < _size; i++)
            {
                if (!Rectangles[i].isDone)
                {
                    foreach( var value in Rectangles[i].valuesToSet)
                    {
                        List<int> fieldsIndex = new List<int>();
                        foreach (var field in Rectangles[i].container)
                        {
                            if (field.IsValueSet == false && field.Optionalities.Contains(value))
                                fieldsIndex.Add(Rectangles[i].container.IndexOf(field));
                        }
                        if(fieldsIndex.Count >= 2 && fieldsIndex.Count <= _high)
                        {
                            var zeroIndexField = Rectangles[i].container[fieldsIndex[0]];
                            var detectedRow = zeroIndexField.row;
                            bool isRow = true;
                            foreach(var index in fieldsIndex)
                            {
                                if(Rectangles[i].container[index].row != detectedRow)
                                {
                                    isRow = false;
                                    break;
                                }
                            }
                            if (isRow)
                            {
                                foreach (var field in detectedRow.container)
                                {
                                    if (field.IsValueSet == false && field.rectangle != zeroIndexField.rectangle)
                                    {
                                        field.Optionalities.Remove(value);
                                        field.row.wasChanged = true;
                                    }

                                }
                            }
                        }
                        if (fieldsIndex.Count >= 2 && fieldsIndex.Count <= _width)
                        {
                            var zeroIndexField = Rectangles[i].container[fieldsIndex[0]];
                            var detectedColumn = zeroIndexField.column;
                            bool isColumn = true;
                            foreach (var index in fieldsIndex)
                            {
                                if (Rectangles[i].container[index].column != detectedColumn)
                                {
                                    isColumn = false;
                                    break;
                                }
                            }
                            if (isColumn)
                            {
                                foreach (var field in detectedColumn.container)
                                {
                                    if (field.IsValueSet == false && field.rectangle != zeroIndexField.rectangle)
                                    {
                                        field.Optionalities.Remove(value);
                                        field.row.wasChanged = true;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        void FewOptionalityInRow()
        {
            for (int i = 0; i < _size; i++)
            {
                if (!Rows[i].isDone)
                {
                    foreach (var value in Rows[i].valuesToSet)
                    {
                        List<int> fieldsIndex = new List<int>();
                        foreach (var field in Rows[i].container)
                        {
                            if (field.IsValueSet == false && field.Optionalities.Contains(value))
                            {
                                fieldsIndex.Add(Rows[i].container.IndexOf(field));
                            }
                        }
                        if ( 2 <= fieldsIndex.Count && fieldsIndex.Count <= _high)
                        {
                            var zeroIndexField = Rows[i].container[fieldsIndex[0]];
                            var detectedRectangle = zeroIndexField.rectangle;
                            bool isDetect = true;
                            foreach (var index in fieldsIndex)
                            {
                                if (Rows[i].container[index].rectangle != zeroIndexField.rectangle)
                                {
                                    isDetect = false;
                                    break;
                                }
                            }
                            if (isDetect)
                            {
                                foreach (var field in detectedRectangle.container)
                                {
                                    if (field.IsValueSet == false && field.row != zeroIndexField.row)
                                    {
                                        field.Optionalities.Remove(value);
                                        field.row.wasChanged = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void FewOptionalityInColumn()
        {
            for (int i = 0; i < _size; i++)
            {
                if (!Columns[i].isDone)
                {
                    foreach (var value in Columns[i].valuesToSet)
                    {
                        List<int> fieldsIndex = new List<int>();
                        foreach (var field in Columns[i].container)
                        {
                            if (field.IsValueSet == false && field.Optionalities.Contains(value))
                            {
                                fieldsIndex.Add(Columns[i].container.IndexOf(field));
                            }
                        }
                        if (2 <= fieldsIndex.Count && fieldsIndex.Count <= _width)
                        {
                            var zeroIndexField = Columns[i].container[fieldsIndex[0]];
                            var detectedRectangle = zeroIndexField.rectangle;
                            bool isDetect = true;
                            foreach (var index in fieldsIndex)
                            {
                                if (Columns[i].container[index].rectangle != zeroIndexField.rectangle)
                                {
                                    isDetect = false;
                                    break;
                                }
                            }
                            if (isDetect)
                            {
                                foreach (var field in detectedRectangle.container)
                                {
                                    if (field.IsValueSet == false && field.column != zeroIndexField.column)
                                    {
                                        field.Optionalities.Remove(value);
                                        field.row.wasChanged = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void sameFewOptionalitiesInContainer(SudokuSmallContainer[] smallContainers)
        {
            foreach (var smallContainer in smallContainers)
            {
                if (!smallContainer.isDone)
                {
                    foreach (var checkingField in smallContainer.container)
                    {
                        List<int> possibleFieldsIndex = new List<int>();
                        if (checkingField.IsValueSet == false)
                        {
                            foreach (var posibleField in smallContainer.container)
                            {
                                if (checkingField != posibleField && posibleField.IsValueSet == false)
                                {
                                    bool haveAll = true;
                                    foreach (var option in posibleField.Optionalities)
                                    {
                                        if (!checkingField.Optionalities.Contains(option))
                                        {
                                            haveAll = false;
                                            break;
                                        }
                                    }
                                    if (haveAll)
                                    {
                                        possibleFieldsIndex.Add(smallContainer.container.IndexOf(posibleField));
                                    }
                                }
                            }
                        }
                        if (!checkingField.IsValueSet && (checkingField.Optionalities.Count == (possibleFieldsIndex.Count + 1)))
                        {
                            foreach (var option in checkingField.Optionalities)
                            {
                                for (int i = 0; i < _size; i++)
                                {
                                    if (!smallContainer.container[i].IsValueSet && (smallContainer.container[i] != checkingField && !possibleFieldsIndex.Contains(i)))
                                    {
                                        smallContainer.container[i].Optionalities.Remove(option);
                                        smallContainer.container[i].row.wasChanged = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Solve()
        {
            bool sudokuSolved = false;
            
            while (!sudokuSolved)
            {
                bool isChanged = false;
                sudokuSolved = true;
                foreach (var row in Rows)
                {
                    row.wasChanged = false;
                }
                RemoveOptionalities();
                SetValues();
                foreach (var row in Rows)
                {
                    isChanged = isChanged | row.wasChanged;
                    sudokuSolved = sudokuSolved & row.isDone;
                }
                if (!sudokuSolved && !isChanged)
                {
                    Console.WriteLine("Nie można rozwiązać tego sudoku(jeszcze nie ;)).");
                    break;
                }
            }
            

        }

        void RemoveOptionalities()
        {
            FewOptionalityInColumn();
            FewOptionalityInRow();
            FewOptionalityInSquare();
            sameFewOptionalitiesInContainer(Rows);
            sameFewOptionalitiesInContainer(Columns);
            sameFewOptionalitiesInContainer(Rectangles);
        }

        void SetValues()
        {
            OneValueOptionInConteiner(Rectangles);
            OneValueOptionInConteiner(Rows);
            OneValueOptionInConteiner(Columns);
            OneValueOptionInField();
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