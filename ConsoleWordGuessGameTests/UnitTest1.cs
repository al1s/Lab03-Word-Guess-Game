using System;
using Xunit;
using ConsoleWordGuessGame;
using System.IO;

namespace ConsoleWordGuessGameTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanCreateFile()
        {
            Program.CreateAppendTo("data.txt", "");
            Assert.True(File.Exists("data.txt"));
        }
        [Fact]
        public void CanReadFromFile()
        {
            string path = "data.txt";
            string testString = "check we can read a string from file";
            Program.CreateAppendTo(path, testString);
            string[] dataContent = Program.ReadFrom(path);
            Assert.Contains(testString, dataContent);
        }
        [Fact]
        public void CanDeleteFromFile()
        {
            
        }
    }
}
