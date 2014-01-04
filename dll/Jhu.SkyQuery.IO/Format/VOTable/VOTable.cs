using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Collections;
using System.Data;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    /// <summary>
    /// Implements functionality to read and write VOTables.
    /// </summary>
    [Serializable]
    public class VOTable : FormattedDataFileBase, IDisposable, ICloneable
    {
        /// <summary>
        /// Used to compare VOTABLE tags and attribute names. Must be case-sensitive.
        /// </summary>
        public static readonly StringComparer Comparer = StringComparer.InvariantCulture;

        /// <summary>
        /// XmlReader that wraps the stream and used to retreive xml elements.
        /// </summary>
        [NonSerialized]
        private XmlReader inputReader;

        /// <summary>
        /// If true, the input reader is opened by the class and will need to be closed
        /// and disposed when the VOTable object is disposed.
        /// </summary>
        [NonSerialized]
        private bool ownsInputReader;

        /// <summary>
        /// XmlWriter that wraps the output stream and used to write xml elements.
        /// </summary>
        [NonSerialized]
        private XmlWriter outputWriter;

        /// <summary>
        /// If true, the output reader is opened by the class and will need to be closed
        /// on disposed when the VOTable object is disposed.
        /// </summary>
        [NonSerialized]
        private bool ownsOutputWriter;

        /// <summary>
        /// Returns the description of the file format
        /// </summary>
        public override FileFormatDescription Description
        {
            get
            {
                return new FileFormatDescription()
                {
                    DisplayName = FileFormatNames.VOTable,
                    DefaultExtension = Constants.FileExtensionVOTable,
                    CanRead = true,
                    CanWrite = true,
                    CanDetectColumnNames = false,
                    CanHoldMultipleDatasets = true,
                    RequiresArchive = false,
                    IsCompressed = false
                };
            }
        }

        /// <summary>
        /// Gets the xml readed opened for this VOTable.
        /// </summary>
        protected internal XmlReader XmlReader
        {
            get { return inputReader; }
        }

        /// <summary>
        /// Gets the xml writer opened for this VOTable.
        /// </summary>
        protected internal XmlWriter XmlWriter
        {
            get { return outputWriter; }
        }

        #region Constructors and initializers

        /// <summary>
        /// Initializes a VOTable object without opening any underlying stream.
        /// </summary>
        public VOTable()
            : base()
        {
            InitializeMembers();
        }

        public VOTable(VOTable old)
            : base(old)
        {
            CopyMembers(old);
        }

        /// <summary>
        /// Initializes a VOTable object by automatically opening an underlying stream
        /// identified by an URI.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fileMode"></param>
        /// <param name="compression"></param>
        /// <param name="encoding"></param>
        public VOTable(Uri uri, DataFileMode fileMode, Encoding encoding)
            : base(uri, fileMode, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            Open();
        }

        public VOTable(Uri uri, DataFileMode fileMode)
            : this(uri, fileMode, Encoding.UTF8)
        {
            // Overload
        }

        /// <summary>
        /// Initializes a VOTable object by automatically wrapping and already open binary stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileMode"></param>
        /// <param name="compression"></param>
        /// <param name="encoding"></param>
        public VOTable(Stream stream, DataFileMode fileMode, Encoding encoding)
            : base(stream, fileMode, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            Open();
        }

        public VOTable(Stream stream, DataFileMode fileMode)
            : this(stream, fileMode, Encoding.UTF8)
        {
            // Overload
        }

        /// <summary>
        /// Initializes a VOTable object by re-using an already open xml reader.
        /// </summary>
        /// <param name="inputReader"></param>
        /// <param name="encoding"></param>
        public VOTable(XmlReader inputReader, Encoding encoding)
            : base((Stream)null, DataFileMode.Read, encoding, CultureInfo.InvariantCulture)
        {
            InitializeMembers();

            this.inputReader = inputReader;
        }

        public VOTable(XmlReader inputReader)
            : this(inputReader, Encoding.UTF8)
        {
            // Overload
        }

        /// <summary>
        /// Initializes a VOTable object by re-using an already open xml writer.
        /// </summary>
        /// <param name="outputWriter"></param>
        /// <param name="encoding"></param>
        public VOTable(XmlWriter outputWriter, Encoding encoding)
            : base((Stream)null, DataFileMode.Write, encoding, CultureInfo.InvariantCulture)
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

        private void CopyMembers(VOTable old)
        {
            InitializeMembers();
        }

        public override void Dispose()
        {
            Close();
            base.Dispose();
        }

        public override object Clone()
        {
            return new VOTable(this);
        }

        #endregion
        #region Stream open and close

        /// <summary>
        /// Starts reading a VOTable from an already open xml reader.
        /// </summary>
        /// <param name="inputReader"></param>
        public virtual void Open(XmlReader inputReader)
        {
            base.Open((Stream)null, DataFileMode.Read);

            this.inputReader = inputReader;

            Open();
        }

        /// <summary>
        /// Starts writing a VOTable into an already open xml writer.
        /// </summary>
        /// <param name="outputWriter"></param>
        public virtual void Open(XmlWriter outputWriter)
        {
            base.Open((Stream)null, DataFileMode.Write);

            this.outputWriter = outputWriter;

            Open();
        }

        /// <summary>
        /// Makes sure that no stream is opened yet.
        /// </summary>
        protected override void EnsureNotOpen()
        {
            if (ownsInputReader && inputReader != null ||
                ownsOutputWriter && outputWriter != null)
            {
                throw new InvalidOperationException();
            }

            // TODO: check if base method has to be called or not
        }

        //

        /// <summary>
        /// If necessary, opens an XmlReader and wraps the underlying stream in it.
        /// </summary>
        /// <remarks>
        /// This function is called by the infrastructure when starting to read
        /// the file by the FileDataReader
        /// </remarks>
        protected override void OpenForRead()
        {
            if (inputReader == null)
            {
                base.OpenForRead();

                var settings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true,
                };

                inputReader = XmlReader.Create(new DetachedStream(BaseStream), settings);
                ownsInputReader = true;
            }
        }

        /// <summary>
        /// If necessary, opens an XmlWriter and wraps the underlying stream in it.
        /// </summary>
        protected override void OpenForWrite()
        {
            if (outputWriter == null)
            {
                base.OpenForWrite();

                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    NamespaceHandling = NamespaceHandling.OmitDuplicates,
                };

                if (Encoding != null)
                {
                    settings.Encoding = Encoding;
                }

                outputWriter = XmlWriter.Create(new DetachedStream(BaseStream), settings);
                ownsOutputWriter = true;
            }
        }

        /// <summary>
        /// Closes the streams, if they were opened by the object itself.
        /// </summary>
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
                outputWriter.Flush();
                outputWriter.Close();
                outputWriter = null;
                ownsOutputWriter = false;
            }

            base.Close();
        }

        /// <summary>
        /// Gets the state of the underlying data stream.
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <remarks>
        /// Called by the infrastructure after a new block is appended to the file.
        /// </remarks>
        protected override void OnBlockAppended(DataFileBlockBase block)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the file header but stops at the block (resource) header.
        /// </summary>
        protected override void OnReadHeader()
        {
            XmlReader.ReadStartElement(Constants.VOTableKeywordVOTable);

            // consume info tags in the header before the first resource
            if (XmlReader.NodeType != XmlNodeType.Element ||
                Comparer.Compare(XmlReader.Name, Constants.VOTableKeywordResource) != 0)
            {
                XmlReader.Read();
            }

            // Reader is positioned on the first RESOURCE tag now
        }

        /// <summary>
        /// Starts reading the next block from the file.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        /// <remarks>
        /// Called by the infrastructure when the stream is advanced to the next file block.
        /// If the block parameter is null a new VOTableResource should be created. If it isn't null
        /// the supplied block object has to be reused. This usually happens when the user passes a
        /// predefined block that has some column mapping defined.
        /// </remarks>
        protected override DataFileBlockBase OnReadNextBlock(DataFileBlockBase block)
        {
            // Reader must now be positioned on a RESOUCE tag
            if (XmlReader.NodeType == XmlNodeType.Element &&
                VOTable.Comparer.Compare(XmlReader.Name, Constants.VOTableKeywordResource) == 0)
            {
                // Read next tag
                if (XmlReader.Read())
                {
                    if (XmlReader.NodeType == XmlNodeType.Element &&
                        Comparer.Compare(XmlReader.Name, Constants.VOTableKeywordTable) == 0)
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

        /// <summary>
        /// Reads the file footer
        /// </summary>
        protected override void OnReadFooter()
        {
            // Not important, do nothing
        }

        /// <summary>
        /// Writes the file header but stops before the block header.
        /// </summary>
        /// <remarks>
        /// Writes from VOTABLE until the RESOURCE tag.
        /// </remarks>
        protected override void OnWriteHeader()
        {
            XmlWriter.WriteStartElement(Constants.VOTableKeywordVOTable);
            XmlWriter.WriteAttributeString(Constants.VOTableKeywordVersion, Constants.VOTableVersion);

            // *** TODO: how to add these?
            //XmlWriter.WriteAttributeString("xmlns", Constants.VOTableXsi);
            //XmlWriter.WriteAttributeString("xmlns:xsi", Constants.VOTableXsi);
            //XmlWriter.WriteAttributeString("xmlns:stc", Constants.StcNs);
        }

        /// <summary>
        /// Initializes writing of the next block.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        protected override DataFileBlockBase OnWriteNextBlock(DataFileBlockBase block, IDataReader dr)
        {
            return block ?? new VOTableResource(this);
        }

        /// <summary>
        /// Writes the file footer.
        /// </summary>
        /// <remarks>
        /// Writes tags after the last RESOURCE tag till the closing VOTABLE
        /// </remarks>
        protected override void OnWriteFooter()
        {
            XmlWriter.WriteEndElement();
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
