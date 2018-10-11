using System;
using System.IO;

namespace ConsoleWordGuessGame
{
    public class Program
    {
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
                Console.WriteLine("Can't write to file");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sw != null) sw.Close();
            }
            return retCode;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
