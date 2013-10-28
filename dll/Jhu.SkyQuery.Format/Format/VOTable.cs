using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Collections;
using System.Data;

namespace Jhu.Graywulf.Format
{
    [Serializable]
    public class VOTable : FormattedData, IDisposable 
    {
        private bool generateIdentity;
        private long rowCounter;
        private XmlReader reader;
        private XmlTextWriter writer;

        private char comment;
        private char quote;
        private char separator;

        public override FileFormatDescription Description
        {
            get
            {
                return new FileFormatDescription()
                {
                    DisplayName = FileFormatNames.Jhu_Graywulf_Format_CsvFile,
                    DefaultExtension = ".xml",
                    CanCompress = true,
                };
            }
        }                

        public VOTable(XmlReader input, CultureInfo culture) 
            : base(input,culture )   
        {           
            Open(input);
        }

        public VOTable(String filepath, CultureInfo culture) 
        {
            reader = XmlReader.Create(filepath);
            Open(reader);
        }

        public VOTable(TextReader txtReader, CultureInfo culture) 
        {
            reader = XmlReader.Create(txtReader);
            Open(reader);
        }

        public VOTable(Stream stream, CultureInfo culture) 
        {
            reader = XmlReader.Create(stream);
            Open(reader);
        }
        
        public VOTable(string path, DataFileMode fileMode)
            : base(path, fileMode,null,null)
        {
            InitializeMembers();
            this.FileMode = fileMode;
        }

        public VOTable(string path, CultureInfo culture, DataFileMode fileMode) 
            :base(path,culture)
        {
            InitializeMembers();            
        }



        public VOTable(XmlWriter writer, DataFileMode fileMode)
            : base(writer, fileMode)
        {
           //base.Encoding = Encoding.UTF8;
            InitializeMembers();
        }

        public VOTable(XmlTextWriter writer, CultureInfo culture)
            : base(writer, null, culture)
        {
            InitializeMembers();
            Open(writer, null, culture);
        
           // this.writer = writer;
        }

        public VOTable(TextWriter writer, CultureInfo culture)
            : base(writer,null, culture) 
        {
            InitializeMembers();
        }

        public VOTable(DataSet ds, DataFileMode fileMode)
            //: base(ds, fileMode)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {

             //this.nextResultsCalled = false;
             this.comment = '#';
             //this.quote = '"';
             //this.separator = ',';
        }

        public bool GenerateIdentity
        {
            get { return generateIdentity; }
            set { generateIdentity = value; }
        }               

        protected DataType GetSqlDataType(String votableType) {

            switch (votableType.ToLower()) { 

                case "boolean"       : return DataType.Bit ;
                case "bit"           : return DataType.Bit;
                case "unsignedbyte"  : return DataType.Binary;
                case "short"         : return DataType.SmallInt ;
                case "int"           : return DataType.Int;
                case "long"          : return DataType.BigInt;
                case "char"          : return DataType.Char;
                case "unicodechar"   : return DataType.Char;
                case "float"         : return DataType.Float;
                case "double"        : return DataType.Real;
                case "floatcomplex"  : return DataType.Unknown;
                case "doublecomplex" : return DataType.Unknown;
                default              : return DataType.Text; 
            }           
        }        

        public void DetectColumns() {

            ArrayList columnNames = new ArrayList();
            ArrayList columnTypes = new ArrayList();
            int count = 0;
            reader.ReadToDescendant(VOTableKeywords.VOTable);
            while (reader.Read() && reader.NodeType == XmlNodeType.Element && reader.Name == VOTableKeywords.VOField)
            {
                columnNames.Add(reader.GetAttribute(VOTableKeywords.name));
                columnTypes.Add(GetSqlDataType(reader.GetAttribute(VOTableKeywords.datatype)));
                count++;
            }
            UpdateColumns(columnNames, columnTypes, count);
        }

        
        protected void UpdateColumns(ArrayList names, ArrayList types, int count)
        {

            DataFileColumn[] columns = new DataFileColumn[count];

            for (int i = 0; i < count; i++)
            {
                columns[i] = new DataFileColumn();
                columns[i].Name = names[i].ToString();
                columns[i].DataType = (DataType)types[i];
            }

            this.Columns.Clear();
            this.Columns.AddRange(columns);
        }

        
        protected override bool OnNextResult()
        {
            return true;
        }

        protected override bool OnRead(object[] values )
        {
            string[] parts = null;
            var res = GetVOTableData(out parts);

            if (!res)
            {
                return false;
            }

            // Now parse the parts
            int pi = 0;
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].IsIdentity && GenerateIdentity)
                {
                    // IDs are 1 based (like in SQL server)
                    values[i] = rowCounter + 1;     // TODO: might need to convert to a smaller type?
                }
                else
                {
                    if (!ColumnParsers[i](parts[pi], out values[i]))
                    {
                        throw new FormatException();    // TODO: add logic to skip exceptions
                    }
                    pi++;
                }
            }

            // TODO: add logic to handle nulls
            rowCounter++;

            return true;
        }


        protected override bool OnRead(object[] values, XmlReader reader)
        {
            string[] parts ;

            var res = GetVOTableData(out parts);

            if (!res)
            {
                return false;
            }

            // Now parse the parts
            int pi = 0;
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].IsIdentity && GenerateIdentity)
                {
                    // IDs are 1 based (like in SQL server)
                    values[i] = rowCounter + 1;     // TODO: might need to convert to a smaller type?
                }
                else
                {
                    if (!ColumnParsers[i](parts[pi], out values[i]))
                    {
                        throw new FormatException();    // TODO: add logic to skip exceptions
                    }

                    pi++;
                }
            }

            // TODO: add logic to handle nulls
            rowCounter++;

            return true;
        }

        protected override void OpenForWrite()
        {
            if (writer == null)
            {
                // No open TextWriter yet
                base.OpenForWrite();
                
                // Open TextWriter
                if (base.Encoding == null)
                {
                    writer = new XmlTextWriter(base.Stream,Encoding.UTF8);//new StreamWriter(base.Stream);
                }
                else
                {
                    writer = new XmlTextWriter(base.Stream, base.Encoding);//writer = new StreamWriter(base.Stream, base.Encoding);
                }
                //ownsOutputWriter = true;
            }
        }

        protected override void OnWriteHeader()
        {
            StartVoTable();

            for (int i = 0; i < Columns.Count; i++)
            {
                writer.WriteStartElement(VOTableKeywords.VOField);
                writer.WriteAttributeString(VOTableKeywords.name, Columns[i].Name.Replace(separator, '_'));
                writer.WriteAttributeString(VOTableKeywords.datatype, Columns[i].DataType.Name.ToString());
                writer.WriteEndElement();               
            }

            StartVOTableData();
        }

        private void StartVoTable() {
            
            writer.WriteStartElement(VOTableKeywords.VOTableStart);
            writer.WriteStartElement(VOTableKeywords.VOResource);
            writer.WriteStartElement(VOTableKeywords.VOTable);                    
        }

        private void StartVOTableData()
        {
            writer.WriteStartElement(VOTableKeywords.VOdata);
            writer.WriteStartElement(VOTableKeywords.VOTabledata);
        }

        protected override void OnWrite(object[] values)
        {           
            writer.WriteStartElement(VOTableKeywords.TR);
            for (int i = 0; i < Columns.Count; i++)
            {
                writer.WriteElementString(VOTableKeywords.TD, ColumnFormatters[i](values[i], Columns[i].Format));   
            }
            writer.WriteEndElement();
            
        }
        protected override void OnWriteFooter()
        {
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();            
        }
       
        public override bool IsClosed {
            get { return false; }
        }

        protected FormatterDelegate[] ColumnFormatters
        {
            get { return columnFormatters; }
        }

        public ParserDelegate[] ColumnParsers
        {
            get { return columnParsers; }
        }        

        private bool GetVOTableData(out string[] parts)
        {
            int cnt = 0;
            parts = new string[Columns.Count];
            try
            {
                reader.ReadToFollowing(VOTableKeywords.TD);
                do
                {
                    parts[cnt] = reader.ReadString();
                    cnt++;
                } while (reader.ReadToNextSibling(VOTableKeywords.TD));

                reader.ReadToNextSibling(VOTableKeywords.TR);
                return true;

            }catch(Exception exp){
                return false;
            }
        }

        public void Open(XmlTextWriter output, Encoding en, CultureInfo culture) {
            EnsureNotOpen();
            this.FileMode = DataFileMode.Write;
            this.writer = output;            
        }
        
        public void Open(XmlReader input)
        {
            EnsureNotOpen();
            this.FileMode = DataFileMode.Read;
            this.reader = input;            
        }

        public void EnsureNotOpen() {
            if (this.reader != null) {
                throw new InvalidOperationException();
            }
        }
    }
}
