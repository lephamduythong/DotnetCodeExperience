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
            string saltStr = "";

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
			MatchCollection mc = Regex.Matches(text, pattern);

            // Remove line that contains unreachable code
            var listLines = text.Split('\n');
            for (var i = 0; i < listLines.Length; i++)
            {   
                if (listLines[i].Contains("return") ||
                    listLines[i].Contains("break") ||
                    listLines[i].Contains("continue"))
                {
                    listLines[i] = listLines[i].Replace(" DEBUG_DEBUG", "");
                }
            }
            // Combine to text
            text = "";
            foreach (var line in listLines)
            {
                text += line;
            }

            switch (selectionCode)
            {
				// Javascript
                case 1:
                    for (int i = startIndex; i < mc.Count; i++)
                    {
                        text = regex.Replace(text, $"console.log\\(\"\\[- {i} -\\]\"\\);", 1);
                    }
                    text = text.Replace("\\", string.Empty);
                    break;
                // C#
                case 2:
                    for (int i = startIndex; i < mc.Count; i++)
                    {
                        text = regex.Replace(text, $"System.Diagnostics.Debug.WriteLine(\"-------------{i}-------------\");", 1);
                    }
                    break;
            }
			text = text.Replace("\\", string.Empty);
            File.WriteAllText("output.txt", text);
            Console.WriteLine("DONE");
        }
    }
}
