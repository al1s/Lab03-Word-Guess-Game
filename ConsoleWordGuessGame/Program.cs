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
        public static void ExceptionMessageToConsole(string message, Exception ex)
        {
            Console.WriteLine(message);
            Console.WriteLine(ex.Message);
        }
    }
}
