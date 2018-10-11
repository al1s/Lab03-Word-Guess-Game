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
    }
}
