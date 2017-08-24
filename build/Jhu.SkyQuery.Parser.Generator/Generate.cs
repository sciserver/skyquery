using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using Jhu.Graywulf.CommandLineParser;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser.Generator
{
    [Verb(Name = "Generate", Description = "Generates a parser from a BNF and token file.")]
    class Generate
    {
        private string output;

        [Parameter(Name = "Output", Description = "Output file", Required = true)]
        public string Output
        {
            get { return output; }
            set { output = value; }
        }

        public Generate()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.output = null;
        }

        public void Run()
        {
            // Start parsing and record time
            DateTime start = DateTime.Now;

            using (var outfile = new StreamWriter(output))
            {
#if !DEBUG
                try
                {
#endif

                    var g = new Jhu.Graywulf.ParserLib.ParserGenerator();
                    g.Execute(outfile, typeof(SkyQueryGrammar));

#if !DEBUG
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("ERROR: {0}", ex.Message);
                }
#endif
            }

            Console.WriteLine("Parser classes generated in {0} sec.", (DateTime.Now - start).TotalSeconds);
        }
    }
}
