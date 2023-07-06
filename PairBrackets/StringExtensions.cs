using System.Text;

namespace PairBrackets
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the number of bracket pairs in the <see cref="text"/>.
        /// </summary>
        /// <param name="text">The source text.</param>
        /// <returns>The number of bracket pairs in the <see cref="text"/>.</returns>
        public static int CountBracketPairs(this string text)
        {
            Stack<char> stack = new Stack<char>();
            int count = 0;

            foreach (char c in text)
            {
                if (c == '(' || c == '[')
                {
                    stack.Push(c);
                }
                else if ((c == ')' && stack.Count > 0 && stack.Peek() == '(') || (c == ']' && stack.Count > 0 && stack.Peek() == '['))
                {
                    stack.Pop();
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Searches the <see cref="text"/> and returns the list of start and end positions of bracket pairs.
        /// </summary>
        /// <param name="text">The source text.</param>
        /// <returns>The list of start and end positions of bracket pairs.</returns>
        /// <exception cref="ArgumentNullException"><see cref="text"/> is null.</exception>
        public static IList<(int, int)> GetBracketPairPositions(this string? text)
        {
            return GetBracketPairPositions(text, BracketTypes.All);
        }

        public static IList<(int, int)> GetBracketPairPositions(string? text, BracketTypes bracketTypes)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Stack<(char, int)> stack = new Stack<(char, int)>();
            List<(int, int)> positions = new List<(int, int)>();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (IsOpeningBracket(c, bracketTypes))
                {
                    stack.Push((c, i));
                }
                else if (IsClosingBracket(c, bracketTypes) && stack.Count > 0)
                {
                    char openingBracket = stack.Peek().Item1;
                    if (BracketsMatch(openingBracket, c))
                    {
                        var start = stack.Peek().Item2;
                        var end = i;
                        stack.Pop();

                        positions.Add((start, end));
                    }
                }
            }

            return positions.OrderBy(p => p.Item1).ToList();
        }

        /// <summary>
        /// Examines the <see cref="text"/> and returns true if the pairs and the orders of brackets are balanced; otherwise returns false.
        /// </summary>
        /// <param name="text">The source text.</param>
        /// <param name="bracketTypes">The bracket type option.</param>
        /// <returns>True if the pairs and the orders of brackets are balanced; otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException"><see cref="text"/> is null.</exception>
        public static bool ValidateBrackets(this string? text, BracketTypes bracketTypes)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Stack<char> stack = new Stack<char>();

            foreach (char c in text)
            {
                if (IsOpeningBracket(c, bracketTypes))
                {
                    stack.Push(c);
                }
                else if (IsClosingBracket(c, bracketTypes))
                {
                    if (stack.Count == 0 || !BracketsMatch(stack.Peek(), c))
                    {
                    return false;
                    }

                    stack.Pop();
                }
            }

            return stack.Count == 0;
        }

        public static bool IsOpeningBracket(char c, BracketTypes bracketTypes)
        {
            return c switch
            {
                '(' => bracketTypes == BracketTypes.RoundBrackets || bracketTypes == BracketTypes.All,
                '[' => bracketTypes == BracketTypes.SquareBrackets || bracketTypes == BracketTypes.All,
                '{' => bracketTypes == BracketTypes.CurlyBrackets || bracketTypes == BracketTypes.All,
                '<' => bracketTypes == BracketTypes.AngleBrackets || bracketTypes == BracketTypes.All,
                _ => false,
            };
        }

        public static bool IsClosingBracket(char c, BracketTypes bracketTypes)
        {
            return c switch
            {
                ')' => bracketTypes == BracketTypes.RoundBrackets || bracketTypes == BracketTypes.All,
                ']' => bracketTypes == BracketTypes.SquareBrackets || bracketTypes == BracketTypes.All,
                '}' => bracketTypes == BracketTypes.CurlyBrackets || bracketTypes == BracketTypes.All,
                '>' => bracketTypes == BracketTypes.AngleBrackets || bracketTypes == BracketTypes.All,
                _ => false,
            };
        }

        public static bool BracketsMatch(char openingBracket, char closingBracket)
        {
            return (openingBracket == '(' && closingBracket == ')') ||
                   (openingBracket == '[' && closingBracket == ']') ||
                   (openingBracket == '{' && closingBracket == '}') ||
                   (openingBracket == '<' && closingBracket == '>');
        }
    }

    [Flags]
    public enum BracketTypes
    {
        All = 0,
        RoundBrackets = 1,
        SquareBrackets = 2,
        CurlyBrackets = 3,
        AngleBrackets = 4,
    }
}
