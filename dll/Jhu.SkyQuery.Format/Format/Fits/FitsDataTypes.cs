using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    public static class FitsDataTypes
    {
        public static FitsDataType Logical
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameLogical,
                    Type = typeof(Boolean),
                    ByteSize = sizeof(Byte),
                };
            }
        }

        public static FitsDataType Bit
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameBit,
                    Type = typeof(Byte),
                    ByteSize = sizeof(Byte),
                };
            }
        }

        public static FitsDataType Byte
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameByte,
                    Type = typeof(Byte),
                    ByteSize = sizeof(Byte),
                };
            }
        }

        public static FitsDataType Int16
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameInt16,
                    Type = typeof(Int16),
                    ByteSize = sizeof(Int16),
                };
            }
        }

        public static FitsDataType Int32
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameInt32,
                    Type = typeof(Int32),
                    ByteSize = sizeof(Int32),
                };
            }
        }

        public static FitsDataType Char
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameChar,
                    Type = typeof(Char),
                    ByteSize = sizeof(Byte),
                };
            }
        }

        public static FitsDataType Single
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameSingle,
                    Type = typeof(Single),
                    ByteSize = sizeof(Single),
                };
            }
        }

        public static FitsDataType Double
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameDouble,
                    Type = typeof(Double),
                    ByteSize = sizeof(Double),
                };
            }
        }

        public static FitsDataType SingleComplex
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameSingleComplex,
                    Type = typeof(SingleComplex),
                    ByteSize = 2 * sizeof(Single),
                };
            }
        }

        public static FitsDataType DoubleComplex
        {
            get
            {
                return new FitsDataType()
                {
                    Name = Constants.FitsTypeNameDoubleComplex,
                    Type = typeof(DoubleComplex),
                    ByteSize = 2 * sizeof(Double),
                };
            }
        }
    }
}
