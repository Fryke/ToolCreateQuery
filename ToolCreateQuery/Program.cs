﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolCreateQuery
{
    class Program
    {
        //For Android
        public static string ConvertToUnsign(string str)
        {
            //Convert unicode to ascii character
            //Because android drawable recourse cant contain unicode
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty)
                .Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        //Function to get all file names in folder
        private static List<string> GetListCountry()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\%USERNAME%\Pictures\Flags\");
            FileInfo[] files = d.GetFiles("*.png");
            List<string> lstNames = new List<string>();
            foreach (var file in files)
            {
                //Add this line to rename all upper file name to lower file name
                //Android drawable resource can't contain ' and - characters
                //So it gets removed
                File.Move(file.FullName, ConvertToUnsign(file.FullName.ToLower().Replace("'", String.Empty)
                    .Replace("-", String.Empty)));
                lstNames.Add(file.Name.Replace(".png", String.Empty)); //NL.png->NL
            }

            return lstNames;
        }

        //Function get random name of country from list without duplicate
        private static List<string> GetNameRandom(string name, List<string> lstNames)
        {
            HashSet<string> myHashSet = new HashSet<string>();
            myHashSet.Add(name); //first add name
            while (myHashSet.Count < 4) //4 names for 4 answers
            {
                //Add random name to HashSet
                myHashSet.Add(lstNames.OrderBy(s => Guid.NewGuid()).First());
            }

            return myHashSet.OrderBy(s => Guid.NewGuid()).ToList();
        }

        //Function to generate query insert into for create Database Question
        private static async Task GenerateQuery()
        {
            List<string> lstQuery = new List<string>();
            List<string> lstCountryName = GetListCountry();
            string query = string.Empty;
            foreach (var name in lstCountryName)
            {
                //With one value in lstCountryname, we create one question with 4 aswers and 1 correct answer
                List<string> answerList = GetNameRandom(name, lstCountryName);

                query = "INSERT INTO Question(Image,AnswerA,AnswerB,AnswerC,AnswerD,CorrectAnswer)"
                        + $"VALUES(\"{name}\",\"{answerList[0]}\",\"{answerList[1]}\",\"{answerList[2]}\",\"{answerList[3]}\",\"{name}\");";
                //Ex
                //INSERT INTO Question(Image,AnswerA,AnswerB,AnswerC,AnswerD,CorrectAnswer)
                //VALUES('NL','DE','BE','UK,'FR','NL');

                lstQuery.Add(query); //add query we just create to list
            }
            //Write all to file
            System.IO.File.WriteAllLines(@".//QueryGenerate.txt", lstQuery);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("TOOL GENERATE QUERY 1.0");
            Console.WriteLine("Please wait...");
            GenerateQuery();
            Console.WriteLine("Success...");
            Console.ReadKey();
        }
    }
}
