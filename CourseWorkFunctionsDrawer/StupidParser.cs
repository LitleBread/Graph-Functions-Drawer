using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CourseWorkFunctionsDrawer
{
    static class StupidParser
    {
        public static string Parse(string parseString)
        {
            parseString = parseString.ToLower().Trim();

            
            Regex brakets = new Regex(@"\([^\(][^\)]*\)");
            MatchCollection collection = brakets.Matches(parseString);
            string[] done = new string[collection.Count];
            int i = 0;
            foreach (Match item in collection)
            {
                string m = item.Value;
                m = ParseBracket(m);
                done[i] = m;
                i++;
            }
            i = 0;
            foreach (Match item in collection)
            {
                parseString = parseString.Replace(item.Value, done[i]);
                i++;
            }
            parseString = parseString.Replace("sin", "Math.Sin");
            parseString = parseString.Replace("cos", "Math.Cos");
            parseString = parseString.Replace("tan", "Math.Tan");
            parseString = parseString.Replace("arcsin", "Math.Asin");
            parseString = parseString.Replace("arccos", "Math.Acos");
            parseString = parseString.Replace("arctan", "Math.Atan");

            parseString = parseString.Replace("sqrt", "Math.Sqrt");
            parseString = parseString.Replace("pi", "Math.PI");
            parseString = parseString.Replace("e", "Math.E");
            parseString = parseString.Replace("exp", "Math.Exp");
            parseString = parseString.Replace("log", "Math.Log");
            parseString = parseString.Replace("lg", "Math.Log10");
            return parseString;
        }

        private static string ParseBracket(string parseString)
        {
            Regex power = new Regex(@"\((.*)\^(.*)\)");
            for (int i = 0; i < 2; i++)
            {

                try
                {
                    parseString = parseString.Replace(power.Match(parseString).Value, "Math.Pow(" + power.Match(parseString).Groups[1].Value + ", " + power.Match(parseString).Groups[2].Value + ")");
                    
                }
                catch { }
                parseString = parseString.Replace("sin", "Math.Sin");
                parseString = parseString.Replace("cos", "Math.Cos");
                parseString = parseString.Replace("tan", "Math.Tan");
                parseString = parseString.Replace("arcsin", "Math.Asin");
                parseString = parseString.Replace("arccos", "Math.Acos");
                parseString = parseString.Replace("arctan", "Math.Atan");

                parseString = parseString.Replace("sqrt", "Math.Sqrt");
                parseString = parseString.Replace("pi", "Math.PI");
                parseString = parseString.Replace("e", "Math.E");
                parseString = parseString.Replace("exp", "Math.Exp");
                parseString = parseString.Replace("log", "Math.Log");
                parseString = parseString.Replace("lg", "Math.Log10");
            }

            if (parseString[parseString.Length - 1] == '*')
                parseString = parseString.Substring(0, parseString.Length - 2);
            return parseString;
        }
    }
}
