using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Jhu.Graywulf.CommandLineParser;

namespace Jhu.SkyQuery.Parser.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            List<Type> verbs = new List<Type>() { typeof(Generate) };

            Generate par = null;

            try
            {
                PrintHeader();
                par = (Generate)ArgumentParser.Parse(args, verbs);
            }
            catch (ArgumentParserException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();

                ArgumentParser.PrintUsage(verbs, Console.Out);
            }

            if (par != null)
            {
                par.Run();
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("Graywulf SQL Parser Generator");
            Console.WriteLine(Copyright.InfoCopyright);
        }
    }
}
