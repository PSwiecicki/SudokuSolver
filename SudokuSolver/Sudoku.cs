using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolver
{
    class Sudoku
    {
        public Sudoku()
        {
            Rows = new Row[9];
            Columns = new Column[9];
            Squares = new Square[9];
            Initialization();
        }

        public Row[] Rows { get; set; }
        public Column[] Columns { get; set; }
        public Square[] Squares { get; set; }

        void Initialization()
        {
            for(int i =0; i< 9; i++)
            {
                Rows[i] = new Row();
                Columns[i] = new Column();
                Squares[i] = new Square();
            }
        }

        void RowsOptionalitiesRemover()
        {
            for(int selected = 0; selected < 9; selected++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Rows[selected].RowFields[i].IsValueSet == true)
                    {
                        foreach(var row in Rows)
                            if(row.RowFields[i].Options != null)
                                row.RowFields[i].Options.Remove(Rows[selected].RowFields[i].Value);
                        foreach (var field in Rows[selected].RowFields)
                            if (field != Rows[selected].RowFields[i] && field.Options != null)
                                field.Options.Remove(Rows[selected].RowFields[i].Value);
                    }
                }
            }
        }

        void ColumnsOptionalitiesRemover()
        {
            for (int selected = 0; selected < 9; selected++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Columns[selected].ColumnFields[i].IsValueSet == true)
                    {
                        foreach (var column in Columns)
                            if(column.ColumnFields[i].Options != null)
                                column.ColumnFields[i].Options.Remove(Columns[selected].ColumnFields[i].Value);
                        foreach (var field in Columns[selected].ColumnFields)
                            if (field != Columns[selected].ColumnFields[i] && field.Options != null)
                                field.Options.Remove(Columns[selected].ColumnFields[i].Value);
                    }
                }
                
            }
        }

        void SquareOptionalitiesRemover()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    if (Squares[i].SquareFields[j].IsValueSet == true)
                    {
                        foreach (var field in Squares[i].SquareFields)
                            if (field != Squares[i].SquareFields[j] && field.Options != null)
                                field.Options.Remove(Squares[i].SquareFields[j].Value);
                    }
            }
        }

        void RemoveOptionalities()
        {
            SquareOptionalitiesRemover();
            RowsOptionalitiesRemover();
            ColumnsOptionalitiesRemover();
        }

        void OneOptionInSquare()
        {
            foreach(var square in Squares)
            {
                int numCounter;
                Field optionalField = new Field(0);
                for (int i = 1; i <= 9; i++)
                {
                    numCounter = 0;
                    foreach (var field in square.SquareFields)
                    {
                        if (field.IsValueSet == false && field.Options.Contains(i))
                        {
                            numCounter++;
                            optionalField = field;
                        }
                    }
                    if (numCounter == 1)
                    {
                        optionalField.Value = i;
                        optionalField.Options = null;
                        optionalField.IsValueSet = true;
                    }
                }
            }
        }

        void OneOptionInRow()
        {
            foreach (var row in Rows)
            {
                int numCounter;
                Field optionalField = new Field(0);
                for (int i = 1; i <= 9; i++)
                {
                    numCounter = 0;
                    foreach (var field in row.RowFields)
                    {
                        if (field.IsValueSet == false && field.Options.Contains(i))
                        {
                            numCounter++;
                            optionalField = field;
                        }
                    }
                    if (numCounter == 1)
                    {
                        optionalField.Value = i;
                        optionalField.Options = null;
                        optionalField.IsValueSet = true;
                    }
                }
            }
        }

        void OneOptionInColumn()
        {
            foreach (var column in Columns)
            {
                int numCounter;
                Field optionalField = new Field(0);
                for (int i = 1; i <= 9; i++)
                {
                    numCounter = 0;
                    foreach (var field in column.ColumnFields)
                    {
                        if (field.IsValueSet == false && field.Options.Contains(i))
                        {
                            numCounter++;
                            optionalField = field;
                        }
                    }
                    if (numCounter == 1)
                    {
                        optionalField.Value = i;
                        optionalField.Options = null;
                        optionalField.IsValueSet = true;
                    }
                }
            }
        }

        void OneOptionalityInField()
        {
            foreach(var row in Rows)
            {
                foreach(var field in row.RowFields)
                {
                    if(field.Options != null && field.Options.Count == 1)
                    {
                        field.Value = field.Options[0];
                        field.Options = null;
                        field.IsValueSet = true;
                    }
                }
            }
        }

        void FewOptionalityInSquare()
        {
            for(int i = 0; i < 9; i++)
            {
                
                for(int j = 1; j <= 9; j++)
                {
                    List<int> fieldsIndex = new List<int>();
                    foreach(var field in Squares[i].SquareFields)
                    {
                        if (field.IsValueSet == false && field.Options.Contains(j))
                            fieldsIndex.Add(Squares[i].SquareFields.IndexOf(field));
                    }
                    if(fieldsIndex.Count < 4 && fieldsIndex.Count > 0)
                    {
                        int columnDetector = fieldsIndex[0] % 3;
                        int rowDetector = fieldsIndex[0] / 3;
                        bool isColumn = true;
                        bool isRow = true;
                        foreach(var index in fieldsIndex)
                        {
                            if (index % 3 != columnDetector)
                                isColumn = false;
                            if (index / 3 != rowDetector)
                                isRow = false;
                            if (!isRow && !isColumn)
                                break;
                        }
                        if(isColumn)
                        {
                            var col = Columns[(i % 3) * 3 + columnDetector];
                            for(int k =0; k <9; k++)
                            {
                                if(k/3 != i/3 && col.ColumnFields[k].IsValueSet == false)
                                {
                                    col.ColumnFields[k].Options.Remove(j);
                                }

                            }
                        }
                        if (isRow)
                        {
                            var row = Rows[(i / 3) * 3 + rowDetector];
                            for (int k = 0; k < 9; k++)
                            {
                                if (k / 3 != i % 3 && row.RowFields[k].IsValueSet == false)
                                {
                                    row.RowFields[k].Options.Remove(j);
                                }

                            }
                        }

                    }
                }
            }
        }

        void FewOptionalityInRow()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 1; j <= 9; j++)
                {
                    List<int> fieldsIndex = new List<int>();
                    foreach(var field in Rows[i].RowFields)
                    {
                        if(field.IsValueSet == false && field.Options.Contains(j))
                        {
                            fieldsIndex.Add(Rows[i].RowFields.IndexOf(field));
                        }
                    }
                    if(fieldsIndex.Count > 0 && fieldsIndex.Count < 4)
                    {
                        int detector = fieldsIndex[0] / 3;
                        bool isDetect = true;
                        foreach(var index in fieldsIndex)
                        {
                            if(detector != index/3)
                            {
                                isDetect = false;
                            }
                        }
                        if(isDetect)
                        {
                            var square = Squares[(i / 3) * 3 + detector];
                            for (int k = 0; k < 9; k++)
                            {
                                if (k / 3 != i % 3 && square.SquareFields[k].IsValueSet == false)
                                    square.SquareFields[k].Options.Remove(j);
                            }
                        }
                    }
                }
            }
        }

        void FewOptionalityInColumn()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    List<int> fieldsIndex = new List<int>();
                    foreach (var field in Columns[i].ColumnFields)
                    {
                        if (field.IsValueSet == false && field.Options.Contains(j))
                        {
                            fieldsIndex.Add(Columns[i].ColumnFields.IndexOf(field));
                        }
                    }
                    if (fieldsIndex.Count > 0 && fieldsIndex.Count < 4)
                    {
                        int detector = fieldsIndex[0] / 3;
                        bool isDetect = true;
                        foreach (var index in fieldsIndex)
                        {
                            if (detector != index / 3)
                            {
                                isDetect = false;
                            }
                        }
                        if (isDetect)
                        {
                            var square = Squares[detector * 3 + i/3];
                            for (int k = 0; k < 9; k++)
                            {
                                if (k % 3 != i % 3 && square.SquareFields[k].IsValueSet == false)
                                    square.SquareFields[k].Options.Remove(j);
                            }
                        }
                    }
                }
            }
        }

        void sameFewOptionalitiesInRow()
        {
            foreach(var row in Rows)
            {
                foreach (var checkingField in row.RowFields)
                {
                    List<int> possibleFieldsIndex = new List<int>();
                    if (checkingField.IsValueSet == false)
                    {
                        foreach (var posibleField in row.RowFields)
                        {
                            if (checkingField != posibleField && posibleField.IsValueSet == false)
                            {
                                bool haveAll = true;
                                foreach (var option in posibleField.Options)
                                {
                                    if (!checkingField.Options.Contains(option))
                                    {
                                        haveAll = false;
                                    }
                                }
                                if (haveAll)
                                {
                                    possibleFieldsIndex.Add(row.RowFields.IndexOf(posibleField));
                                }
                            }
                        }
                    }
                    if (!checkingField.IsValueSet && (checkingField.Options.Count == (possibleFieldsIndex.Count + 1)))
                    {
                        foreach (var option in checkingField.Options)
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                if (!row.RowFields[i].IsValueSet && (row.RowFields[i] != checkingField && !possibleFieldsIndex.Contains(i)))
                                    row.RowFields[i].Options.Remove(option);
                            }
                        }
                    }
                }
            }
        }

        void sameFewOptionalitiesInColumn()
        {
            foreach (var column in Columns)
            {
                foreach (var checkingField in column.ColumnFields)
                {
                    List<int> possibleFieldsIndex = new List<int>();
                    if (checkingField.IsValueSet == false)
                    {
                        foreach (var posibleField in column.ColumnFields)
                        {
                            if (checkingField != posibleField && posibleField.IsValueSet == false)
                            {
                                bool haveAll = true;
                                foreach (var option in posibleField.Options)
                                {
                                    if (!checkingField.Options.Contains(option))
                                    {
                                        haveAll = false;
                                    }
                                }
                                if (haveAll)
                                {
                                    possibleFieldsIndex.Add(column.ColumnFields.IndexOf(posibleField));
                                }
                            }
                        }
                    }
                    if (!checkingField.IsValueSet && (checkingField.Options.Count == (possibleFieldsIndex.Count + 1)))
                    {
                        foreach (var option in checkingField.Options)
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                if (!column.ColumnFields[i].IsValueSet && (column.ColumnFields[i] != checkingField && !possibleFieldsIndex.Contains(i)))
                                    column.ColumnFields[i].Options.Remove(option);
                            }
                        }
                    }
                }
            }
        }

        public void SetValues()
        {
            RemoveOptionalities();
            OneOptionInSquare();
            RemoveOptionalities();
            OneOptionInRow();
            RemoveOptionalities();
            OneOptionInColumn();
            RemoveOptionalities();
            OneOptionalityInField();
            RemoveOptionalities();
            FewOptionalityInColumn();
            FewOptionalityInRow();
            FewOptionalityInSquare();
            sameFewOptionalitiesInRow();
            sameFewOptionalitiesInColumn();
        }
    }
}
