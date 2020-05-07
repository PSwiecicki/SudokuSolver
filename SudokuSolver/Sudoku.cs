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
                            if (field != Squares[i].SquareFields[j] && Squares[i].SquareFields[j].Options != null)
                                field.Options.Remove(Squares[i].SquareFields[j].Value);
                    }
            }
        }

        public void RemoveOptionalities()
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
                        optionalField.Value = optionalField.Options[0];
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
                        optionalField.Value = optionalField.Options[0];
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
                        optionalField.Value = optionalField.Options[0];
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

        public void SetValues()
        {
            OneOptionInSquare();
            OneOptionInRow();
            OneOptionInColumn();
            OneOptionalityInField();
        }
    }
}
