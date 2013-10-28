using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Jhu.SkyQuery.Format
{
    public class FileFormatFactory
    {
        public FileFormatFactory()
        {
        }

        private FileFormatDescription GetFileFormatInternal(Type type)
        {
            var f = (DataFileBase)FormatterServices.GetUninitializedObject(type);
            var fd = f.Description;

            fd.Type = type;

            return fd;
        }

        public Dictionary<string, FileFormatDescription> GetFileFormatDescriptions()
        {
            var res = new Dictionary<string, FileFormatDescription>()
            {
                { typeof(CsvFile).FullName, GetFileFormatInternal(typeof(CsvFile)) }
                // TODO: add new file formats here
            };


            //var formats = AppSettings.GetFileFormats();

            //foreach (var key in formats.Keys)
            //{
            //    res.Add((string)key, GetFileFormatInternal((string)formats[(string)key]));
            //}

            return res;
        }

        public FileFormatDescription GetFileFormatDescription(string key)
        {
            var t = Type.GetType(key);
            return GetFileFormatInternal(t);
        }

        public FileFormatDescription GetFormatFromFilename(string path, out string filename, out string extension, out CompressionMethod compression)
        {
            compression = CompressionMethod.None;

            extension = Path.GetExtension(path);
            filename = Path.GetFileNameWithoutExtension(path);

            // Check if extension refers to a compressed file
            foreach (var e in Jhu.Graywulf.Format.Constants.CompressionExtensions)
            {
                if (StringComparer.InvariantCultureIgnoreCase.Compare(extension, e.Value) == 0)
                {
                    compression = e.Key;

                    extension = Path.GetExtension(filename);
                    filename = Path.GetFileNameWithoutExtension(filename);
                    break;
                }
            }

            FileFormatDescription format = null;
            foreach (var f in GetFileFormatDescriptions())
            {
                if (StringComparer.InvariantCultureIgnoreCase.Compare(extension, f.Value.DefaultExtension) == 0)
                {
                    format = f.Value;
                    break;
                }
            }

            return format;
        }

        public DataFileBase CreateFile(FileFormatDescription format)
        {
            var c = format.Type.GetConstructor(Type.EmptyTypes);
            var f = (DataFileBase)c.Invoke(null);

            return f;
        }
    }
}
