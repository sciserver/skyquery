using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace Jhu.SkyQuery.Format.Fits
{
    public class Card
    {
        private static readonly Regex StringRegex = new Regex(@"'(?:[^']|'{2})*'");

        private string keyword;
        private string rawValue;
        private string comments;

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public bool IsComment
        {
            get { return StringComparer.InvariantCultureIgnoreCase.Compare(keyword, "COMMENT") == 0; }
        }

        public bool IsEnd
        {
            get { return StringComparer.InvariantCultureIgnoreCase.Compare(keyword, "END") == 0; }
        }

        #region Constructors and initializers

        public Card()
        {
            InitializeMembers();
        }

        public Card(string keyword, string value)
        {
            InitializeMembers();

            this.keyword = keyword;
            SetValue(value);
        }

        public Card(Card old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.keyword = null;
            this.rawValue = null;
            this.comments = null;
        }

        private void CopyMembers(Card old)
        {
            this.keyword = old.keyword;
            this.rawValue = old.rawValue;
            this.comments = old.comments;
        }

        #endregion

        #region Value accessors

        public String GetString()
        {
            return rawValue.Trim('\'');
        }

        public void SetValue(String value)
        {
            // Quote and escape
            rawValue = "'" + value.Replace("'", "''") + "'";
        }

        public Boolean GetBoolean()
        {
            if (StringComparer.InvariantCultureIgnoreCase.Compare(rawValue.Trim(), "T") == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetValue(Boolean value)
        {
            rawValue = value ? "T" : "F";
        }

        public Int32 GetInt32()
        {
            return Int32.Parse(rawValue, CultureInfo.InvariantCulture);
        }

        public void SetValue(Int32 value)
        {
            rawValue = value.ToString(CultureInfo.InvariantCulture);
        }

        public Double GetDouble()
        {
            return Double.Parse(rawValue, CultureInfo.InvariantCulture);
        }

        public void SetValue(Double value)
        {
            rawValue = value.ToString(CultureInfo.InvariantCulture);
        }

        // *** TODO: implement other types of getters and setters

        #endregion

        /// <summary>
        /// Reads a single card image from the stream at the current position
        /// </summary>
        /// <param name="stream"></param>
        /// <remarks>
        /// A card image consits of a 80 char long line, first eight characters
        /// containing the keyword. Strings are delimited with 's, commend is
        /// separated by a /.
        /// If no '= ' sequence found at bytes 8 and 9, the entire line is treated
        /// as a comment.
        /// </remarks>
        internal void Read(Stream stream)
        {
            var buffer = new byte[80];
            var res = stream.Read(buffer, 0, buffer.Length);

            if (res == 0)
            {
                throw new EndOfStreamException();
            }
            else if (res < buffer.Length)
            {
                throw new FitsException("Unexpected end of stream.");  // *** TODO
            }

            string line = Encoding.ASCII.GetString(buffer);

            // bytes 0-7: keyword name
            this.keyword = line.Substring(0, 8).Trim();

            // bytes 8-9: "= " sequence if there's value
            if (line[8] == '=' && line[9] == ' ')
            {
                ReadValue(line.Substring(10));
            }
            else
            {
                rawValue = null;
                comments = line.Substring(8);
            }
        }

        private void ReadValue(string line)
        {
            // Try to match a string

            Match m = StringRegex.Match(line);

            int ci;
            if (m.Success)
            {
                rawValue = m.Value.Replace("''", "'");     // Handle escapes
                ci = line.IndexOf('/', m.Length);
            }
            else
            {
                ci = line.IndexOf('/', m.Length);
                if (ci > 0)
                {
                    rawValue = line.Substring(0, ci - 1);
                }
                else
                {
                    rawValue = line.Substring(0);
                }
            }

            if (ci >= 0)
            {
                comments = line.Substring(ci + 1);
            }
            else
            {
                comments = null;
            }
        }

        public override string ToString()
        {
            return String.Format("{0:8}= {1}", keyword, rawValue);
        }
    }
}
