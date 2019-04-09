using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolCreateQuery
{
    class Program
    {
        //Function to get all file names in folder
        private static List<string> GetListCountry()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\%USERNAME%\Pictures\Flags");
            FileInfo[] files = d.GetFiles("*.png");
            List<string> lstNames = new List<string>();
            foreach (var file in files)
            {
                lstNames.Add(file.Name.Replace(".png", String.Empty)); //NL.png->NL
            }

            return lstNames;
        }

        static void Main(string[] args)
        {
        }
    }
}
