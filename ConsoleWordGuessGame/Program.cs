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
                            HandlePlay(data);
                            break;
                        case "2":
                            DisplayAdminScreen();
                            switch (GetInput("numeric"))
                            {
                                case "1":
                                    DisplayShowAllWordsScreen(data);
                                    break;
                                case "2":
                                    DisplayAddWordScreen(path);
                                    break;
                                case "3":
                                    DisplayDeleteWordScreen(data, path);
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
        /// Handle addition/deletion of word in a file
        /// </summary>
        /// <param name="dataToHandle">Data we want be added/deleted from file</param>
        /// <param name="action">Action we want to undertake</param>
        /// <param name="path">Path to file</param>
        public static void ManageFileContent(string dataToHandle, string action, string path)
        {
            if (dataToHandle== "ctrlx") return;
            switch (action)
            {
                case "addWord":
                    CreateAppendTo(path, dataToHandle);
                    break;
                case "deleteWord":
                    // empty input destroys it
                    DeleteFrom(path, int.Parse(dataToHandle));
                    break;
            }
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
                    arr[i] = '_';
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
        public static void HandlePlay(string[] words)
        {
            string word = GetRandomWordFrom(words);
            string maskedWord = string.Empty;
            string userInput = string.Empty;
            if(MaskWithFilter(word, userInput.ToUpper(), out maskedWord))
            { 
                DisplayGamePlayScreen(word, userInput);
            }
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
            ManageFileContent(GetInput("alphabetic").ToUpper(), "addWord", path);
            Console.WriteLine("Word was added");
            Console.ReadLine();
        }
        public static void DisplayDeleteWordScreen(string[] words, string path)
        {
            Console.Clear();
            Console.WriteLine("Choose # of Word to delete (Press Enter to save, Ctrl-X to get back to Main Menu):");
            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {words[i]}");
            }
            ManageFileContent(GetInput("alphabetic"), "deleteWord", path);
            Console.WriteLine("Word was deleted");
            Console.ReadLine();

        }
        public static void DisplayShowAllWordsScreen(string[] words)
        {
            Console.Clear();
            Console.WriteLine("All Available Words");
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
            Console.ReadLine();
        }
        public static string DisplayGamePlayScreen(string wordToShow, string alreadyUsedSymbols)
        {
            Console.Clear();
            Console.WriteLine("Choose a letter");
            Console.WriteLine();
            Console.WriteLine(wordToShow);
            Console.WriteLine(alreadyUsedSymbols);
            return GetInput("alphabetic");
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
