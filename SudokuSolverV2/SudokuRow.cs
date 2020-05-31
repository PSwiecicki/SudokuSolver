using System;
using System.Collections.Generic;

namespace SudokuSolverV2
{
    internal class SudokuRow : SudokuContainer
    {
        public bool WasChanged { get; set; }
        public SudokuRow(int size)
        {
            Container = new List<Field>();
            ValueToSet = new List<int>();
            for (int i = 1; i <= size; i++)
                ValueToSet.Add(i);
            isDone = false;
        }

        public bool SetValueInRow(string valueInString)
        {
            string[] valueInStringTab = valueInString.Split(" ");

            if (!(valueInStringTab.Length == Container.Count))
            {
                Console.WriteLine("Zła liczba parametrów. ");
                return false;
            }
            for(int i = 0; i < valueInStringTab.Length; i++)
            {
                if (!int.TryParse(valueInStringTab[i], out int value))
                {
                    Console.WriteLine("Podano parametr nie będący liczbą");
                    return false;
                }
                if (value < 0 && value > Container.Count)
                {
                    Console.WriteLine($"Wartość nie mieście się w przedziale od 0 do {Container.Count}");
                    return false;
                }
                if (value == 0)
                    continue;
                if (!Container[i].Options.Contains(value) )
                {
                    Console.WriteLine($"Wartość {value} nie może zostać wpisana w wyznaczone miejsce.");
                    return false;
                }
                Container[i].Value = value;
            }
            return true;
        }

        public void SetValueInRow(SudokuRow row)
        {
            for(int i = 0; i < Container.Count; i++)
            {
                if(row.Container[i].Value != 0)
                    Container[i].Value = row.Container[i].Value;
            }

        }

        public override string ToString()
        {
            string output = "|";
            foreach (var vield in Container)
                output += (vield.IsValueSet ? vield.Value.ToString() : string.Join(' ', vield.Options.ToArray())) + '|';
            return output;
        }
    }
}