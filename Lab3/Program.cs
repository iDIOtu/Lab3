using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Xml.XPath;

namespace LabThree
{
    internal class Program
    {
        public static (List<float>, List<char>) ParseMathExpression(string mathExpression)
        {
            mathExpression = mathExpression.Replace(" ", "").Replace(",", ".");
            List<float> numbers = new List<float>();
            List<char> operators = new List<char>();

            for (int i = 0; i < mathExpression.Length; i++)
            {
                char c = mathExpression[i];
                if (Char.IsDigit(c) || c == '.')
                {
                    string numberString = c.ToString();

                    while (i + 1 < mathExpression.Length && (Char.IsDigit(mathExpression[i + 1]) || mathExpression[i + 1] == '.'))
                    {
                        numberString += mathExpression[i + 1];
                        i++;
                    }

                    numbers.Add(float.Parse(numberString, CultureInfo.InvariantCulture.NumberFormat));
                }

                else if (c == '+' || c == '-' || c == '*' || c == '/')
                    operators.Add(c);

                else if (c == '(')
                {
                    string numberStringTwo = "";
                    int count = 1;

                    while (i + 1 < mathExpression.Length && count != 0)
                    {
                        if (mathExpression[i + 1] == '(')
                            count += 1;

                        else if (mathExpression[i + 1] == ')')
                            count -= 1;

                        numberStringTwo += mathExpression[i + 1];
                        i++;
                    }

                    (List<float> numb, List<char> op) = ParseMathExpression(numberStringTwo);
                    numbers.Add(CalculateMathExpression(numb, op));
                }
            }
            return (numbers, operators);
        }

        public static float CalculateMathExpression(List<float> numbers, List<char> operators)
        {
            Stack<float> stack = new Stack<float>();
            stack.Push(numbers[0]);

            for (int i = 0; i < operators.Count; i++)
            {
                float operation = operators[i];
                float num = numbers[i + 1];

                if (operators[i] == '*' || operators[i] == '/')
                {
                    float prevasion = stack.Pop();
                    float result = operation == '*' ? prevasion * num : prevasion / num;
                    stack.Push(result);
                }

                else if (operators[i] == '+')
                    stack.Push(num);

                else if (operators[i] == '-')
                    stack.Push(-num);
            }

            float finalResult = 0;
            while (0 < stack.Count)
            {
                finalResult += stack.Pop();
            }

            return finalResult;
        }

        public static void Main()
        {
            Console.Write("Введите математическое выражение: ");
            (List<float> numbers, List<char> operators) = ParseMathExpression(Console.ReadLine());
            Console.Write("Ваш результат: ");
            Console.WriteLine(CalculateMathExpression(numbers, operators));
        }
    }
}