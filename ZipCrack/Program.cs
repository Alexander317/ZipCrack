using System;
using Ionic.Zip;

namespace ZipCrack
{
    class Program
    {
        static string fileName = @"";
        static bool found = false;
        static void Main(string[] args)
        {
            Console.WriteLine("Введи формат пароля (например nllnnl, где n - цифра, l - заглавная буква): ");
            string pattern = Console.ReadLine().ToLower();
            Console.WriteLine("Введи путь к файлу (например C:\\Users\\User\\Desktop\\archive.zip): ");
            fileName = $@"{Console.ReadLine()}";

            if (fileName[0] == '"')
                fileName = fileName.Substring(1, fileName.Length - 2);
            if (fileName[fileName.Length - 1] == '"')
                fileName = fileName.Substring(0, fileName.Length - 2);

            GenerateCombinations(pattern, 0, "");

            if (!found)
            {
                Console.WriteLine("Нема пароля");
            }
        }
        static void GenerateCombinations(string pattern, int index, string current)
        {
            if (found)
                return;

            if (index == pattern.Length)
            {
                if (ExtractArсhive(current))
                {
                    Console.WriteLine("Пароль: " + current);
                    found = true;
                }
                return;
            }

            char symbol = pattern[index];
            string possibleChars = GetPossibleChars(symbol);

            foreach (char c in possibleChars)
            {
                GenerateCombinations(pattern, index + 1, current + c);
                if (found)
                    break;
            }
        }
        static string GetPossibleChars(char symbol)
        {
            switch (symbol)
            {
                case 'n':
                    return "0123456789";
                case 'l':
                    return "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                default:
                    return symbol.ToString();
            }
        }
        static bool ExtractArсhive(string password)
        {
            string outputFolder = "output";
            try
            {
                using (ZipFile arhive = new ZipFile(fileName))
                {
                    arhive.Password = password;
                    arhive.Encryption = EncryptionAlgorithm.PkzipWeak;
                    arhive.StatusMessageTextWriter = null;

                    arhive.ExtractAll(outputFolder, ExtractExistingFileAction.OverwriteSilently);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
