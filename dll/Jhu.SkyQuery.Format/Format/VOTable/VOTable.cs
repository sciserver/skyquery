using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Collections;
using System.Data;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    [Serializable]
    public class VOTable : FormattedDataFile, IDisposable
    {
        public static readonly StringComparer Comparer = StringComparer.InvariantCulture;

        [NonSerialized]
        private XmlReader inputReader;

        [NonSerialized]
        private bool ownsInputReader;

        [NonSerialized]
        private XmlWriter outputWriter;

        [NonSerialized]
        private bool ownsOutputWriter;

        public override FileFormatDescription Description
        {
            get
            {
                return new FileFormatDescription()
                {
                    DisplayName = FileFormatNames.VOTable,
                    DefaultExtension = ".votable",
                    CanCompress = true,
                };
            }
        }

        protected internal XmlReader XmlReader
        {
            get { return inputReader; }
        }

        protected internal XmlWriter XmlWriter
        {
            get { return outputWriter; }
        }

        #region Constructors and initializers

        public VOTable()
            : base()
        {
            InitializeMembers();
        }

        public VOTable(Uri uri, DataFileMode fileMode, CompressionMethod compression, Encoding encoding)
            : base(uri, fileMode, compression, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            Open();
        }

        public VOTable(Uri uri, DataFileMode fileMode)
            : this(uri, fileMode, CompressionMethod.None, Encoding.UTF8)
        {
            // Overload
        }

        public VOTable(Stream stream, DataFileMode fileMode, CompressionMethod compression, Encoding encoding)
            : base(stream, fileMode, compression, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            Open();
        }

        public VOTable(Stream stream, DataFileMode fileMode)
            : this(stream, fileMode, CompressionMethod.None, Encoding.UTF8)
        {
            // Overload
        }

        public VOTable(XmlReader inputReader, Encoding encoding)
            : base((Stream)null, DataFileMode.Read, CompressionMethod.None, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            this.inputReader = inputReader;
        }

        public VOTable(XmlReader inputReader)
            : this(inputReader, Encoding.UTF8)
        {
            // Overload
        }

        public VOTable(XmlWriter outputWriter, Encoding encoding)
            : base((Stream)null, DataFileMode.Write, CompressionMethod.None, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            this.outputWriter = outputWriter;
        }

        public VOTable(XmlWriter outputWriter)
            : this(outputWriter, Encoding.UTF8)
        {
            // Overload
        }

        private void InitializeMembers()
        {
            this.inputReader = null;
            this.ownsInputReader = false;

            this.outputWriter = null;
            this.ownsOutputWriter = false;
        }

        public override void Dispose()
        {
            Close();
            base.Dispose();
        }

        #endregion
        #region Stream open and close

        public override void Open(Stream stream, DataFileMode fileMode)
        {
            base.Open(stream, fileMode);

            Open();
        }

        public virtual void Open(XmlReader inputReader)
        {
            base.Open(null, DataFileMode.Read);

            this.inputReader = inputReader;

            Open();
        }

        public virtual void Open(XmlWriter outputWriter)
        {
            base.Open(null, DataFileMode.Write);

            this.outputWriter = outputWriter;

            Open();
        }

        protected override void EnsureNotOpen()
        {
            if (ownsInputReader && inputReader != null ||
                ownsOutputWriter && outputWriter != null)
            {
                throw new InvalidOperationException();
            }
        }

        //

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This function is called by the infrastructure when starting to read
        /// the file by the FileDataReader
        /// </remarks>
        protected override void OpenForRead()
        {
            base.OpenForRead();

            if (inputReader == null)
            {
                var settings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true,
                };

                inputReader = System.Xml.XmlReader.Create(base.Stream, settings);

                ownsInputReader = true;
            }
        }

        protected override void OpenForWrite()
        {
            base.OpenForWrite();

            if (outputWriter == null)
            {
                outputWriter = new XmlTextWriter(base.Stream, Encoding);
                ownsOutputWriter = true;
            }
        }

        public override void Close()
        {
            if (ownsInputReader && inputReader != null)
            {
                inputReader.Close();
                inputReader = null;
                ownsInputReader = false;
            }

            if (ownsOutputWriter && outputWriter != null)
            {
                outputWriter.Close();
                outputWriter = null;
                ownsOutputWriter = false;
            }

            base.Close();
        }

        public override bool IsClosed
        {
            get
            {
                switch (FileMode)
                {
                    case DataFileMode.Read:
                        return inputReader == null;
                    case DataFileMode.Write:
                        return outputWriter == null;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        protected override void OnBlockAppended(DataFileBlockBase block)
        {
            throw new NotImplementedException();
        }

        protected override void OnReadHeader()
        {
            XmlReader.ReadStartElement(VOTableKeywords.VoTable);

            // consume info tags in the header before the first resource
            if (XmlReader.NodeType != XmlNodeType.Element ||
                Comparer.Compare(XmlReader.Name, VOTableKeywords.Resource) != 0)
            {
                XmlReader.Read();
            }

            // Reader is positioned on the first RESOURCE tag now

            // Read VOTable until the first RESOURCE tag
            //inputReader.ReadToNextSibling(VOTableKeywords.Resource);
        }

        protected override DataFileBlockBase OnReadNextBlock(DataFileBlockBase block)
        {
            // Reader must now be positioned on a RESOUCE tag
            if (XmlReader.NodeType == XmlNodeType.Element &&
                VOTable.Comparer.Compare(XmlReader.Name, VOTableKeywords.Resource) == 0)
            {
                // Read next tag
                if (XmlReader.Read())
                {
                    if (XmlReader.NodeType == XmlNodeType.Element &&
                        Comparer.Compare(XmlReader.Name, VOTableKeywords.Table) == 0)
                    {

                        return block ?? new VOTableResource(this);
                    }
                    else
                    {
                        throw new FileFormatException();    //  *** TODO
                    }
                }
            }

            return null;
        }

        protected override void OnReadFooter()
        {
            // Tags to consume: /DATA /TABLE /RESOURCE
        }

        protected override void OnWriteHeader()
        {
            throw new NotImplementedException();
        }

        protected override DataFileBlockBase OnWriteNextBlock(DataFileBlockBase block, IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override void OnWriteFooter()
        {
            throw new NotImplementedException();
        }

#if false
        

        protected override void OnWriteHeader()
        {
            StartVoTable();

            for (int i = 0; i < Columns.Count; i++)
            {
                outputWriter.WriteStartElement(VOTableKeywords.Field);
                outputWriter.WriteAttributeString(VOTableKeywords.Name, Columns[i].Name.Replace(separator, '_'));
                outputWriter.WriteAttributeString(VOTableKeywords.DataType, Columns[i].DataType.Name.ToString());
                outputWriter.WriteEndElement();               
            }

            StartVOTableData();
        }

        private void StartVoTable() {
            
            outputWriter.WriteStartElement(VOTableKeywords.VoTable);
            outputWriter.WriteStartElement(VOTableKeywords.Resource);
            outputWriter.WriteStartElement(VOTableKeywords.Table);                    
        }

        private void StartVOTableData()
        {
            outputWriter.WriteStartElement(VOTableKeywords.Data);
            outputWriter.WriteStartElement(VOTableKeywords.TableData);
        }

        protected override void OnWrite(object[] values)
        {           
            outputWriter.WriteStartElement(VOTableKeywords.TR);
            for (int i = 0; i < Columns.Count; i++)
            {
                outputWriter.WriteElementString(VOTableKeywords.TD, ColumnFormatters[i](values[i], Columns[i].Format));   
            }
            outputWriter.WriteEndElement();
            
        }
        protected override void OnWriteFooter()
        {
            outputWriter.WriteEndElement();
            outputWriter.WriteEndElement();
            outputWriter.Flush();
            outputWriter.Close();            
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
                inputReader.ReadToFollowing(VOTableKeywords.TD);
                do
                {
                    parts[cnt] = inputReader.ReadString();
                    cnt++;
                } while (inputReader.ReadToNextSibling(VOTableKeywords.TD));

                inputReader.ReadToNextSibling(VOTableKeywords.TR);
                return true;

            }catch(Exception exp){
                return false;
            }
        }

        /*
        public void Open(XmlTextWriter output, Encoding en, CultureInfo culture) {
            EnsureNotOpen();
            this.FileMode = DataFileMode.Write;
            this.outputWriter = output;            
        }
        
        public void Open(XmlReader input)
        {
            EnsureNotOpen();
            this.FileMode = DataFileMode.Read;
            this.inputReader = input;            
        }

        public void EnsureNotOpen() {
            if (this.inputReader != null) {
                throw new InvalidOperationException();
            }
        }
         * */
#endif
    }
}
