using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DebugFaster
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change these settings
            int selectionCode = int.Parse(args[0]);
            int startIndex = 0;

            string startWithPattern = string.Empty;
            string pattern = "DEBUG_DEBUG";
            string exceptPattern = string.Empty;
            string initReplaceText = string.Empty;
            string text = File.ReadAllText("input.txt");
            Regex regex = new Regex(pattern);
			startWithPattern = ";";
            initReplaceText = "; " + pattern;
            text = text.Replace(startWithPattern, initReplaceText);
            text = text.Replace("\\", string.Empty);
			System.Console.WriteLine(text);
			MatchCollection mc = Regex.Matches(text, pattern);

            switch (selectionCode)
            {
				// Javascript
                case 1:
                    for (int i = startIndex; i < mc.Count; i++)
                    {
						System.Console.WriteLine(i);
                        text = regex.Replace(text, $"console.log\\(\"\\[- {i} -\\]\"\\);", 1);
                    }
                    text = text.Replace("\\", string.Empty);
                    break;
                // C#
                case 2:
                    for (int i = startIndex; i < mc.Count; i++)
                    {
                        text = regex.Replace(text, $"Debug.WriteLine\\(\"\\[- {i} -\\]\"\\);", 1);
                    }
                    break;
            }
			text = text.Replace("\\", string.Empty);
            File.WriteAllText("output.txt", text);
            Console.WriteLine("DONE");
        }
    }
}
