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
      int selectionCode = 1;
      int startIndex = 0;

      // Init params

      string startWith = string.Empty;
      string pattern = string.Empty;
      string initReplaceText = string.Empty;
      string text = File.ReadAllText("input.txt");

      switch (selectionCode)
      {
        // After ";" Javascript
        case 1:
          // Config
          startWith = ";";
          pattern = "console.log\\(\"\\[-___-\\]\"\\);";
          initReplaceText = "; " + pattern;
          // Process
          text = text.Replace(startWith, initReplaceText);
          text = text.Replace("\\", string.Empty);
          Regex regex = new Regex(pattern);
          MatchCollection mc = Regex.Matches(text, pattern);
          for (int i = startIndex; i < mc.Count; i++)
          {
            text = regex.Replace(text, $"console.log\\(\"\\[- {i} -\\]\"\\);", 1);
          }
          text = text.Replace("\\", string.Empty);
          break;
        // After ";" C#
        case 2:
          break;
      }
      File.WriteAllText("output.txt", text);
      Console.WriteLine("DONE");
    }
  }
}
