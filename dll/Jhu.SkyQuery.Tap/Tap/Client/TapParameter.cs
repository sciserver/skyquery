using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapParameter : DbParameter
    {
        #region Private member variables

        private DbType dbType;
        private ParameterDirection direction;
        private bool isNullable;
        private string parameterName;
        private int size;
        private string sourceColumn;
        private bool sourceColumnNullMapping;
        private object value;

        #endregion
        #region Properties

        public override DbType DbType
        {
            get { return dbType; }
            set
            {
                // TODO: validate supported types here
                // TODO: figure out how to map DbType to VOTable types

                dbType = value;
            }
        }

        public override ParameterDirection Direction
        {
            get { return direction; }
            set
            {
                // TODO: validate and only allow input parameters

                direction = value;
            }
        }

        public override bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        public override string ParameterName
        {
            get { return parameterName; }
            set
            {
                // TODO: validate, do not allow null or empty string
                parameterName = value;
            }
        }

        public override int Size
        {
            get { return size; }
            set { size = value; }
        }

        public override string SourceColumn
        {
            get { return sourceColumn; }
            set { sourceColumn = value; }
        }

        public override bool SourceColumnNullMapping
        {
            get { return sourceColumnNullMapping; }
            set { sourceColumnNullMapping = value; }
        }

        public override object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        #endregion
        #region Constructors and initializers

        public TapParameter()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.dbType = DbType.UInt32;
            this.direction = ParameterDirection.Input;
            this.isNullable = true;
            this.parameterName = null;
            this.size = 4;
            this.sourceColumn = null;
            this.sourceColumnNullMapping = false;
            this.value = null;
        }

        #endregion

        public override void ResetDbType()
        {
            this.dbType = DbType.UInt32;
        }
    }
}
