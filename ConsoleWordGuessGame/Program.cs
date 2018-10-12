using System;
using System.IO;

namespace ConsoleWordGuessGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] data = default(string[]);
            string path = "data.txt";
            while (true) { 
                if (File.Exists(path))
                {
                    data = ReadFrom(path);
                    DisplayInitialScreen();
                    switch (GetInput("numeric"))
                    {
                        case "1":
                            HandlePlay();
                            break;
                        case "2":
                            DisplayAdminScreen();
                            switch (GetInput("numeric"))
                            {
                                case "1":
                                    DisplayShowAllWordsScreen();
                                    break;
                                case "2":
                                    DisplayAddWordScreen(path);
                                    break;
                                case "3":
                                    DisplayDeleteWordScreen(path);
                                break;
                            };
                            break;
                        case "3":
                            Environment.Exit(0);
                            break;
                    };
                }
                else
                {
                    DisplayAddWordScreen(path);
                }
            }
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
        public static bool MaskWithFilter(string word, string keepUnmasked, out string maskedWord)
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
        public static string GetRandomWordFrom(string[] words)
        {
            Random random = new Random();
            return words[random.Next(0, words.Length)];
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
        }

        public static void DisplayAdminScreen()
        {
            Console.Clear();
            Console.WriteLine("1. Show All Words");
            Console.WriteLine("2. Add New Word");
            Console.WriteLine("3. Delete Word");
            Console.WriteLine("4. Main Menu");
        }

        public static void DisplayAddWordScreen(string path)
        {
            Console.Clear();
            Console.WriteLine("Add New Word (Press Enter to save, Ctrl-X to get back to Main Menu):");
            ManagedWords(GetInput("alphabetic"), "addWord", path);
        }
        public static void DisplayDeleteWordScreen(string path)
        {
            Console.Clear();
            Console.WriteLine("Choose # of Word to delete (Press Enter to save, Ctrl-X to get back to Main Menu):");
            ManagedWords(GetInput("alphabetic"), "deleteWord", path);
        }
        public static void ManagedWords(string userInput, string screen, string path)
        {
            if (userInput == "ctrlx") return;
            switch(screen)
            {
                case "addWord":
                    HandleAddWord(path, userInput);
                    break;
                case "deleteWord":
                    HandleDeleteWord(path, userInput);
                    break;
            }
        }
        public static void DisplayGamePlayScreen(string wordToShow, string alreadyUsedSymbols)
        {
        }
        public static void DisplayShowAllWordsScreen()
        {
            Console.Clear();
            Console.WriteLine("All Available Words");
            Console.ReadLine();
        }

        public static void HandleAddWord(string path, string wordToSave)
        {
            try
            {
                CreateAppendTo(path, wordToSave);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void HandleDeleteWord(string path, string wordPositionToDelete)
        {
            try
            {
                DeleteFrom(path, int.Parse(wordPositionToDelete));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetInput(string expectedInputType)
        {
            // storage for input result 
            string userInput = string.Empty;
            ConsoleKeyInfo pressed;
            // while not pressed Enter
            do
            {
                // don't show key char on the screen
                pressed = Console.ReadKey(true);
                // check whether the key pressed  is a number or not
                if (pressed.Key != ConsoleKey.Backspace && expectedInputType == "numeric")
                {
                    int value = default(int);
                    if (int.TryParse(pressed.KeyChar.ToString(), out value))
                    {
                        userInput += pressed.KeyChar;
                        Console.Write(pressed.KeyChar);
                    }
                    if ((pressed.Modifiers & ConsoleModifiers.Control) != 0 && pressed.Key == ConsoleKey.X)
                    {
                        userInput = "ctrlx";
                        break;
                    }
                }
                // check whether the key pressed is an alphabetic char or not
                else if (pressed.Key != ConsoleKey.Backspace && expectedInputType == "alphabetic")
                {
                    char value = default(char);
                    if (char.TryParse(pressed.KeyChar.ToString(), out value))
                    {
                        userInput += pressed.KeyChar;
                        Console.Write(pressed.KeyChar);
                    }
                    if ((pressed.Modifiers & ConsoleModifiers.Control) != 0 && pressed.Key == ConsoleKey.X)
                    {
                        userInput = "ctrlx";
                        break;
                    }
                }
                else
                {
                    // the user want do delete the last char
                    if (pressed.Key == ConsoleKey.Backspace && userInput.Length != 0)
                    {
                        // sync storage and screen 
                        userInput = userInput.Substring(0, userInput.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            } while (pressed.Key != ConsoleKey.Enter);
            return userInput;
        }

    }
}
