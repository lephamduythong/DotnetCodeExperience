using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Docnet.Core;
using Docnet.Core.Models;
using iTextSharp.text.pdf;

namespace PDFParserTest
{
    class TCVNEncodingConverter
    {
        private static char[] tcvnchars = {
            'µ', '¸', '¶', '·', '¹',
            '¨', '»', '¾', '¼', '½', 'Æ',
            '©', 'Ç', 'Ê', 'È', 'É', 'Ë',
            '®', 'Ì', 'Ð', 'Î', 'Ï', 'Ñ',
            'ª', 'Ò', 'Õ', 'Ó', 'Ô', 'Ö',
            '×', 'Ý', 'Ø', 'Ü', 'Þ',
            'ß', 'ã', 'á', 'â', 'ä',
            '«', 'å', 'è', 'æ', 'ç', 'é',
            '¬', 'ê', 'í', 'ë', 'ì', 'î',
            'ï', 'ó', 'ñ', 'ò', 'ô',
            '­', 'õ', 'ø', 'ö', '÷', 'ù',
            'ú', 'ý', 'û', 'ü', 'þ',
            '¡', '¢', '§', '£', '¤', '¥', '¦'};
        private static char[] unichars = {
            'à', 'á', 'ả', 'ã', 'ạ',
            'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
            'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
            'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
            'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
            'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
            'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'};
        private static char[] convertTable;
        public TCVNEncodingConverter()
        {
            convertTable = new char[256];
            for (int i = 0; i < 256; i++)
                convertTable[i] = (char)i;
            for (int i = 0; i < tcvnchars.Length; i++)
                convertTable[tcvnchars[i]] = unichars[i];
        }
        public string TCVN3ToUnicode(string value)
        {
            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                if (chars[i] < (char)256)
                    chars[i] = convertTable[chars[i]];
            return new string(chars);
        }
    }

    class Program
    {
        private static char[] vietnameseChars = {
            'à', 'á', 'ả', 'ã', 'ạ',
            'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
            'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
            'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
            'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
            'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ'
        };

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string outputPath = "data_extracted.csv";
            string inputPath = "data.pdf";
            var converter = new TCVNEncodingConverter();
            File.Delete(outputPath);
            var fileStream = File.AppendText(outputPath);
            var docReader = DocLib.Instance.GetDocReader(inputPath, new PageDimensions());
            var vietnameseCharsList = new List<char>();
            var str = new StringBuilder();
            
            for (int i = 0; i < vietnameseChars.Length; i++)
            {
                vietnameseCharsList.Add(vietnameseChars[i]);
            }

            for (int i = 0; i < docReader.GetPageCount(); i++)
            {
                using (var pageReader = docReader.GetPageReader(100))
                {
                    var text = pageReader.GetText();
                    var convertedText = converter.TCVN3ToUnicode(text); 
                    for (var j = 0; j < convertedText.Length; j++) {
                        if (vietnameseCharsList.Contains(convertedText[j])) 
                        {
                            str.Append(convertedText[j]);
                        } 
                        else 
                        {
                            var charToProcess = convertedText[j];
                            str.Append(charToProcess.ToString().ToLower());
                        }
                    }
                }
            }
               
            string studentPattern = @"\d+ (\d+) ([a-z\sàáảãạăằắẳẵặâầấẩẫậđèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵ]+) (\d+) (\d+/\d+/\d+) ([a-z\sàáảãạăằắẳẵặâầấẩẫậđèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵ\.-]*)\d+ (\d+.\d+) (trung bình khá|khá||giỏi) (nam|nữ)";
            var regex = new Regex(studentPattern);
            var matches = regex.Matches(str.ToString());
            foreach (Match match in matches)
            {
                // System.Console.WriteLine(match.Groups[1].Value); // id
                // System.Console.WriteLine(match.Groups[2].Value); // fullname
                // System.Console.WriteLine(match.Groups[3].Value); // class
                // System.Console.WriteLine(match.Groups[4].Value); // date of birth
                // System.Console.WriteLine(match.Groups[5].Value); // location
                // System.Console.WriteLine(match.Groups[6].Value); // GPA
                // System.Console.WriteLine(match.Groups[7].Value); // type
                // System.Console.WriteLine(match.Groups[8].Value); // gender
                // System.Console.WriteLine();

                var studentStr = new StringBuilder();
                studentStr.Append(match.Groups[1].Value.Trim() + ",");
                studentStr.Append(match.Groups[2].Value.Trim() + ",");
                studentStr.Append(match.Groups[3].Value.Trim() + ",");
                studentStr.Append(match.Groups[4].Value.Trim() + ",");
                studentStr.Append(match.Groups[5].Value.Trim() + ",");
                studentStr.Append(match.Groups[6].Value.Trim() + ",");
                studentStr.Append(match.Groups[7].Value.Trim() + ",");
                studentStr.Append(match.Groups[8].Value.Trim() + ",");
                fileStream.WriteLine(studentStr.ToString());
            }

            fileStream.Close();

            // Insert 3 bytes header of ouput file to fix Excel reading UTF-8 error 
            var outputOldBytes = File.ReadAllBytes(outputPath);
            var outputNewBytes = new byte[outputOldBytes.Length + 3];
            outputNewBytes[0] = 0xef;
            outputNewBytes[1] = 0xbb;
            outputNewBytes[2] = 0xbf;
            for (var i = 0; i < outputOldBytes.Length; i++) {
                outputNewBytes[i + 3] = outputOldBytes[i];
            }
            var stream = File.OpenWrite(outputPath);
            stream.Write(outputNewBytes);
            stream.Close();
        }
    }
}
