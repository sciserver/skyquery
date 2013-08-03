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
using Jhu.SkyQuery.Parser.NameResolver;
using Jhu.SkyQuery.Schema;
using Jhu.SkyQuery.Lib;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.CommandLineParser;

namespace Jhu.SkyQuery.CmdLineUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Type> verbs = new List<Type>() { typeof(RunParameters), typeof(ScheduleParameters) };            
            Parameters par = null;

            try
            {
                PrintHeader();
                par = (Parameters)ArgumentParser.Parse(args, verbs);
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
            Console.WriteLine(
@"JHU SkyQuery Command Line Utility
(c) 2008-2012 László Dobos dobos@pha.jhu.edu
Department of Physics and Astronomy, The Johns Hopkins University

");
        }
       
    }
}
