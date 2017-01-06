using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Reflection;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using Jhu.SkyQuery.Parser;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.CommandLineParser;

namespace Jhu.SkyQuery.CmdLineUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            List<Type> verbs = new List<Type>() { typeof(Query) };
            Verb v = null;

            try
            {
                PrintHeader();
                v = (Verb)ArgumentParser.Parse(args, verbs);
            }
            catch (ArgumentParserException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();

                ArgumentParser.PrintUsage(verbs, Console.Out);
            }

            if (v != null)
            {
                v.Run();
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("SkyQuery Command Line Utility");
            Console.WriteLine(Jhu.Graywulf.Util.AssemblyReflector.GetCopyright());
        }
       
    }
}
