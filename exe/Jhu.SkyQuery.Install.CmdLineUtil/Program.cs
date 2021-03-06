﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.CommandLineParser;
using Jhu.Graywulf.Install.CmdLineUtil;

namespace Jhu.SkyQuery.Install.CmdLineUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            // Initialize logger
            Jhu.Graywulf.Logging.LoggingContext.Current.StartLogger(Jhu.Graywulf.Logging.EventSource.CommandLineTool, true);

            List<Type> verbs = new List<Type>() { typeof(Install) };

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

            Jhu.Graywulf.Logging.LoggingContext.Current.StopLogger();
        }

        private static void PrintHeader()
        {
            Console.WriteLine("SkyQuery Command Line Utility");
        }
    }
}
