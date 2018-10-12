using System;
using System.IO;

namespace ConsoleWordGuessGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        // ================= IO OPERATIONS ====================
        /// <summary>
        /// Creates new file and/or append given string to it.
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="content">String to add to file</param>
        /// <returns>return code (0 - Ok, 1 - Error)</returns>
        public static int CreateAppendTo(string path, string content)
        {
            int retCode = 1;
            StreamWriter sw = File.AppendText(path);
            try
            {
                sw.WriteLine(content);
                retCode = 0;
            }
            catch (Exception ex)
            {
                ExceptionMessageToConsole("Can't write to file", ex);
                throw ex;
            }
            finally
            {
                if (sw != null) sw.Close();
            }
            return retCode;
        }
        /// <summary>
        /// Read all strings from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Array of strings in the file</returns>
        public static string[] ReadFrom(string path)
        {
            string[] result = default(string[]);
            try
            {
                result = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                ExceptionMessageToConsole("Can't read file", ex);
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Delete file from path
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="lineNumber">Number of string to delete</param>
        /// <returns>return code (0 - Ok, 1 - Error)</returns>
        public static int DeleteFrom(string path, int lineNumber)
        {
            int result = default(int);
            try
            {
                string[] words = ReadFrom(path);
                File.Delete(path);
                for (int i = 0; i < words.Length; i++)
                {
                    if (i != lineNumber) CreateAppendTo(path, words[i]);
                }
                result = 0;
            }
            catch (Exception ex)
            {
                ExceptionMessageToConsole("Can't delete from file", ex);
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Print custom error message and internal error message to console
        /// </summary>
        /// <param name="message">Custom error message</param>
        /// <param name="ex">Original exception type</param>
        public static void ExceptionMessageToConsole(string message, Exception ex)
        {
            Console.WriteLine(message);
            Console.WriteLine(ex.Message);
        }


        // ================= APP ====================
        /// <summary>
        /// Mask with underscores the letters which are not in the word
        /// </summary>
        /// <param name="word">Word to mask</param>
        /// <param name="keepUnmasked">Words to keep unmasked</param>
        /// <returns></returns>
        public static bool LeftToMaskWithFilter(string word, string keepUnmasked, out string maskedWord)
        {
            bool withUnderscores = false;
            char[] arr = word.ToCharArray(); 
            for (int i = 0; i < arr.Length; i++)
            {
                if (!keepUnmasked.Contains(arr[i]))
                {
                    arr[i] =  '_';
                    withUnderscores = true;
                }
            }
            maskedWord = string.Join(' ', arr);
            return withUnderscores;
        }

        // ================= UI ====================
        public static void DisplayInitialScreen()
        {
            Console.Clear();
            Console.WriteLine("Word Guess Game");
            Console.WriteLine("Choose operation");
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Admin");
            Console.WriteLine("3. Exit");
            HandleInput(GetInput(), "initial");
        }

        public static void DisplayAdminScreen()
        {
            Console.Clear();
            Console.WriteLine("1. Play");
            Console.WriteLine("2. Admin");
            Console.WriteLine("3. Main Menu");
            HandleInput(GetInput(), "admin");
        }

        public static void DisplayAddWordScreen()
        {
            Console.Clear();
            Console.WriteLine("Add New Word");
            HandleInput(GetInput(), screenType);
            DisplayInitialScreen();
        }
        public static void HandleInput(string userInput, string screen) { }
        public static string GetInput() { return string.Empty; }
    }
}
