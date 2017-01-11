using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomServer
{
    class TextLogger : ILogger
    {
        public void Write(string dataToWrite)
        {
            string path = "ChatLogFile.txt";
            string fullPath = System.IO.Path.GetFullPath(path);
            try
            {
                System.IO.File.AppendAllText(fullPath, dataToWrite);
            }
            catch
            {
                Console.WriteLine("Error while saving.");

            }
        }
    }
}
