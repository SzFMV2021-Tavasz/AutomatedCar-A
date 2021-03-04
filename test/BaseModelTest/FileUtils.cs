using System;
using System.IO;

namespace BaseModelTest
{
    public class FileUtils
    {
        public Boolean TextFilesEqual(string path_1, string path_2)
        {
            string[] lines1 = File.ReadAllLines(path_1);
            string[] lines2 = File.ReadAllLines(path_2);

            if (lines1.Length != lines2.Length) return false;

            for (int i = 0; i < lines1.Length; i++)
            {
                if (lines1[i] != lines2[i]) return false;
            }

            return true;
        }
    }
}