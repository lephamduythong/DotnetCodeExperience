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
            // int selectionCode = 2;
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
            MatchCollection mc = Regex.Matches(text, pattern);

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
                    // Remove lines that contain unreachable code
                    var listLines = text.Split('\n');
                    for (var i = 0; i < listLines.Length; i++)
                    {
                        string type = null;
                        bool isSingle = false;

                        if (listLines[i].Contains("return"))
                        {
                            type = "return";
                        }
                        else if (listLines[i].Contains("break"))
                        {
                            type = "break";
                        }
                        else if (listLines[i].Contains("continue"))
                        {
                            type = "continue";
                        }

                        if (i >= 1)
                        {
                            var test = listLines[i - 1].Trim();
                            if (test == "{")
                            {
                                isSingle = true;
                            }
                        }

                        if (type != null && isSingle == true)
                        {
                            listLines[i] = listLines[i].Replace(type, "DEBUG_DEBUG_FIX " + type);
                            listLines[i] = listLines[i].Replace(" DEBUG_DEBUG", "");
                            listLines[i] = listLines[i].Replace("DEBUG_DEBUG_FIX", "DEBUG_DEBUG");
                        }
                    }
                    text = "";
                    foreach (var line in listLines)
                    {
                        text += line;
                    }

                    // Replace to debug code
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
