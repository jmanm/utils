using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Test
{
    [TestFixture]
    public class LoggerTest
    {
        private string testFilePath = Path.Combine(Path.GetTempPath(), "logger_test.log");

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        [Test]
        public void TestFileSize()
        {
            var logger = new Logger(testFilePath) { MaxLogFileSize = 40 };
            logger.LogMessage("Every man dies, but not every man truly lives");
            using (StreamReader reader = new StreamReader(testFilePath))
            {
                var text = reader.ReadToEnd();
                Assert.That(text.Length, Is.EqualTo(logger.MaxLogFileSize));
                Assert.That(text.Contains("lives"), Is.False);
            }
            logger.MaxLogFileSize = 50;
            logger.LogMessage("lives. SO LET'S GET WASTED!");
            using (StreamReader reader = new StreamReader(testFilePath))
            {
                var text = reader.ReadToEnd();
                Assert.That(text.Length, Is.EqualTo(logger.MaxLogFileSize));
                Assert.That(text, Is.EqualTo(" not every man truly lives. SO LET'S GET WASTED!\r\n"));
            }
        }
    }
}
