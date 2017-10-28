using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;
using System.Diagnostics;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Format.VOTable
{
    /// <summary>
    /// Implements functionality to read and write VOTables.
    /// </summary>
    [Serializable]
    public class VOTable : XmlDataFile, IDisposable, ICloneable
    {
        #region Constructors and initializers

        /// <summary>
        /// Initializes a VOTable object without opening any underlying stream.
        /// </summary>
        public VOTable()
            : base()
        {
            InitializeMembers(new StreamingContext());
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
            : base(uri, fileMode, encoding)
        {
            InitializeMembers(new StreamingContext());

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
            : base(stream, fileMode, encoding)
        {
            InitializeMembers(new StreamingContext());

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
            : base((Stream)null, DataFileMode.Read, encoding)
        {
            InitializeMembers(new StreamingContext());
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
            : base((Stream)null, DataFileMode.Write, encoding)
        {
            InitializeMembers(new StreamingContext());
        }

        public VOTable(XmlWriter outputWriter)
            : this(outputWriter, Encoding.UTF8)
        {
            // Overload
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            Description = new FileFormatDescription()
            {
                DisplayName = FileFormatNames.VOTable,
                MimeType = Constants.MimeTypeVOTable,
                Extension = Constants.FileExtensionVOTable,
                CanRead = true,
                CanWrite = true,
                CanDetectColumnNames = false,
                CanHoldMultipleDatasets = true,
                RequiresArchive = false,
                IsCompressed = false,
                KnowsRecordCount = false,
                RequiresRecordCount = false,
            };
        }

        private void CopyMembers(VOTable old)
        {
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
        protected override Task OnReadHeaderAsync()
        {
            // TODO: make it async
            ReadVOTableElement();
            return Task.CompletedTask;
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
        protected override Task<DataFileBlockBase> OnReadNextBlockAsync(DataFileBlockBase block)
        {
            // TODO: make it async
            return Task.FromResult(ReadResourceElement(block));
        }

        /// <summary>
        /// Reads the file footer
        /// </summary>
        protected override Task OnReadFooterAsync()
        {
            // Not important, do nothing
            return Task.CompletedTask;
        }

        /// <summary>
        /// Writes the file header but stops before the block header.
        /// </summary>
        /// <remarks>
        /// Writes from VOTABLE until the RESOURCE tag.
        /// </remarks>
        protected override async Task OnWriteHeaderAsync()
        {
            await XmlWriter.WriteStartElementAsync(null, Constants.TagVOTable, null);
            await XmlWriter.WriteAttributeStringAsync(null, Constants.AttributeVersion, null, Constants.VOTableVersion);

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
        protected override Task<DataFileBlockBase> OnCreateNextBlockAsync(DataFileBlockBase block)
        {
            return Task.FromResult(block ?? new VOTableResource(this));
        }

        /// <summary>
        /// Writes the file footer.
        /// </summary>
        /// <remarks>
        /// Writes tags after the last RESOURCE tag till the closing VOTABLE
        /// </remarks>
        protected override async Task OnWriteFooterAsync()
        {
            await XmlWriter.WriteEndElementAsync();
        }

        #region VOTable reader implementation

        private void ReadVOTableElement()
        {
            // Skip initial declarations
            XmlReader.MoveToContent();

            // Reader now should be positioned on the VOTABLE tag
            // Read attributes

            var id = XmlReader.GetAttribute(Constants.AttributeID);
            var version = XmlReader.GetAttribute(Constants.AttributeVersion);

            // Finish reading tag and move to next content
            XmlReader.ReadStartElement(Constants.TagVOTable);
            XmlReader.MoveToContent();

            // Read all tags inside VOTABLE but stop at any RESOURCE tag
            // because they are handled outside of this function
            while (XmlReader.NodeType == XmlNodeType.Element &&
                   Comparer.Compare(XmlReader.Name, Constants.TagResource) != 0)
            {
                switch (XmlReader.Name)
                {
                    case Constants.TagDescription:
                        var d = Deserialize<Description>();
                        break;
                    case Constants.TagDefinitions:
                    case Constants.TagCoosys:
                    case Constants.TagGroup:
                    case Constants.TagParam:
                    case Constants.TagInfo:
                        // TODO: implement deserializets,
                        // now just skip the tag
                        XmlReader.Skip();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                XmlReader.MoveToContent();
            }

            // TODO: implement parsers for header info including these tags:
            // * DESCRIPTION
            // * DEFINITIONS -- just ignore because it's deprecated
            // * COOSYS -- just ignore because it's deprecated
            // * GROUP
            // * PARAM
            // * INFO -- also parse TAP query status

            // Reader is positioned on the first RESOURCE tag now
            // Header is read completely, now wait for framework to call OnReadNextBlock

            // TODO: make sure XSD validation fails when no RESOURCE tag found
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        private DataFileBlockBase ReadResourceElement(DataFileBlockBase block)
        {
            // Check if the current tag is a RESOURCE. If so, create a new VOTableResource object
            // and return it. Subsequent handling of tags until the closing RESOURCE tag will
            // be done inside the VOTableResource class

            if (XmlReader.NodeType == XmlNodeType.Element &&
                VOTable.Comparer.Compare(XmlReader.Name, Constants.TagResource) == 0)
            {
                // We found a RESOURCE tag here, so return the preallocated DataFileBlock
                // or create a new one.
                // The framework will call VOTableResource.OnReadHeader to continue

                return block ?? new VOTableResource(this);
            }
            else
            {
                return null;
            }
        }

        #endregion

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
