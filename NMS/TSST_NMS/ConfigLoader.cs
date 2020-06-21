using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST_NMS
{
    class ConfigLoader
    {
        string fileName;
        string path;

        public ConfigLoader()
        {
            fileName = "config.txt";
            path = ""; // @"B:\STUDIA\Programy\byVisual\TSST_nms\TSST_nms\";
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();
            if (path != "-")
                messages = System.IO.File.ReadAllLines(path + fileName).ToList();

            return messages;
        }

        public string GetAll()
        {
            string all = System.IO.File.ReadAllText(path + fileName);
            return all;
        }
    }
}
