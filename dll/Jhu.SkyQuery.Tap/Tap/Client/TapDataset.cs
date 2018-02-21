using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapDataset : Jhu.Graywulf.Sql.Schema.DatasetBase
    {


        #region Private members

        #endregion
        #region Properties

        [IgnoreDataMember]
        public override string ProviderName
        {
            get { return Constants.TapProviderName; }
        }

        /// <summary>
        /// Gets or sets the database name associated with this dataset.
        /// </summary>
        /// <remarks>
        /// The database name only refers to the schema prototype, not
        /// the actual database instances!
        /// </remarks>
        [IgnoreDataMember]
        public override string DatabaseName
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        #endregion
        #region Constructors and initializers

        /// <summary>
        /// Default constructor
        /// </summary>
        public TapDataset()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public TapDataset(DatasetBase old)
            : base(old)
        {
            InitializeMembers(new StreamingContext());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        public TapDataset(string name, string connectionString)
            : base()
        {
            InitializeMembers(new StreamingContext());

            Name = name;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="old"></param>
        public TapDataset(TapDataset old)
            : base(old)
        {
            CopyMembers(old);
        }

        /// <summary>
        /// Initializes private member variables to their default values.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            // TODO: default schema name?
            this.DefaultSchemaName = Jhu.Graywulf.Sql.Schema.SqlServer.Constants.DefaultSchemaName;
        }

        /// <summary>
        /// Copies private member variables form another instance
        /// </summary>
        /// <param name="old"></param>
        private void CopyMembers(TapDataset old)
        {
            // TODO: default schema name?
            this.DefaultSchemaName = old.DefaultSchemaName;
        }

        public override object Clone()
        {
            return new TapDataset(this);
        }

        #endregion

        public override string GetSpecializedConnectionString(string connectionString, bool integratedSecurity, string username, string password, bool enlist)
        {
            return ConnectionString;
        }

        private TapConnection OpenConnectionInternal()
        {
            var cn = new TapConnection(ConnectionString);
            cn.Open();
            return cn;
        }

        private async Task<TapConnection> OpenConnectionInternalAsync(CancellationToken cancellationToken)
        {
            var cn = new TapConnection(ConnectionString);
            await cn.OpenAsync(cancellationToken);
            return cn;
        }

        public override DbConnection OpenConnection()
        {
            return OpenConnectionInternal();
        }

        public override async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken)
        {
            return await OpenConnectionInternalAsync(cancellationToken);
        }

        #region Fully resolved names and keys

        public override string QuoteIdentifier(string identifier)
        {
            throw new NotImplementedException();
        }

        public override string GetObjectFullyResolvedName(DatabaseObject databaseObject)
        {
            // TODO: does TAP have "schema" like in SQL Server?

            throw new NotImplementedException();
        }

        public override string GetObjectUniqueKey(DatabaseObjectType objectType, string datasetName, string databaseName, string schemaName, string objectName)
        {
            // TODO: does TAP have "schema" like in SQL Server?

            return base.GetObjectUniqueKey(objectType, datasetName, databaseName, schemaName, objectName);
        }

        #endregion
        #region Schema objects

        protected override void OnLoadDatabaseObject<T>(T databaseObject)
        {
            // TODO: load table. TAP supports only tables.
            string sql;
            if (typeof(Table) == typeof(Table))
            {
                sql =
@"SELECT ""schema_name"", ""table_name"", ""description""
FROM tap_schema.tables
WHERE ""table_type"" = 'table'";

                if (!String.IsNullOrWhiteSpace(databaseObject.SchemaName))
                {
                    sql += String.Format(" AND \"schema_name\" = '{0}'", databaseObject.SchemaName);
                }

                sql += String.Format(" AND \"table_name\" = '{0}'", databaseObject.ObjectName);
            }
            else
            {
                throw new NotImplementedException();
            }

            using (var cn = OpenConnectionInternal())
            {
                using (var cmd = new TapCommand(sql, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var schemaname = dr.GetString(0);
                            var tablename = dr.GetString(1);

                            if (tablename.StartsWith(schemaname + "."))
                            {
                                tablename = tablename.Substring(1 + schemaname.Length);
                            }

                            databaseObject.Dataset = this;
                            databaseObject.DatabaseName = DatabaseName;
                            databaseObject.SchemaName = schemaname;
                            databaseObject.ObjectName = tablename;
                            databaseObject.ObjectType = DatabaseObjectType.Table;

                            databaseObject.Metadata.Summary = dr.GetString(2);
                        }
                    }
                }
            }
        }




        protected override bool OnIsObjectExisting(DatabaseObject databaseObject)
        {
            // TODO: Test if table is present.

            throw new NotImplementedException();
        }

        protected override IEnumerable<KeyValuePair<string, T>> OnLoadAllObjects<T>()
        {
            // TODO: load all tables
            string sql;

            if (typeof(T) == typeof(Table))
            {
                sql = 
@"SELECT ""schema_name"", ""table_name"", ""description""
FROM tap_schema.tables
WHERE ""table_type"" = 'table'";
            }
            else
            {
                throw new NotImplementedException();
            }

            using (var cn = OpenConnectionInternal())
            {
                using (var cmd = new TapCommand(sql, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var schemaname = dr.GetString(0);
                            var tablename = dr.GetString(1);

                            if (tablename.StartsWith(schemaname + "."))
                            {
                                tablename = tablename.Substring(1 + schemaname.Length);
                            }

                            T obj = new T()
                            {
                                Dataset = this,
                                DatabaseName = DatabaseName,
                                SchemaName = schemaname,
                                ObjectName = tablename,
                                ObjectType = DatabaseObjectType.Table,
                            };

                            // No support in TAP
                            // obj.Metadata.DateCreated = dr.GetDateTime(3).ToUniversalTime();
                            // obj.Metadata.DateModified = dr.GetDateTime(4).ToUniversalTime();

                            obj.Metadata.Summary = dr.GetString(2);

                            yield return new KeyValuePair<string, T>(GetObjectUniqueKey(obj), obj);
                        }
                    }
                }
            }
        }


        protected override IEnumerable<KeyValuePair<string, Column>> OnLoadColumns(DatabaseObject databaseObject)
        {
            // TODO: load columns of the given table
            string sql;
            if (typeof(Column) == typeof(Column))
            {
                sql =
@"SELECT  ""table_name"", ""column_name"", ""description"", ""unit"", ""ucd"", ""utype"", ""datatype"", ""size"", ""principal"", ""indexed"", ""std""
FROM tap_schema.columns
WHERE ""table_name"" = '";
                sql += databaseObject.ObjectName +"'";
            }
            else
            {
                throw new NotImplementedException();
            }

            using (var cn = OpenConnectionInternal())
            {
                using (var cmd = new TapCommand(sql, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tablename = dr.IsDBNull(0) ? null : dr.GetString(0);
                            var columnname = dr.IsDBNull(1) ? null : dr.GetString(1);
                            var description = dr.IsDBNull(2) ? null : dr.GetString(2);
                            var unit = dr.IsDBNull(3) ? null : dr.GetString(3);
                            var ucd = dr.IsDBNull(4) ? null : dr.GetString(4);
                            var utype = dr.IsDBNull(5) ? null : dr.GetString(5);
                            var datatype = dr.IsDBNull(6) ? null : dr.GetString(6);
                            var size = dr.IsDBNull(7) ? -1 : dr.GetInt32(7);
                            var principal = dr.IsDBNull(8) ? 0 : dr.GetInt32(8);
                            var indexed = dr.IsDBNull(9) ? 0 : dr.GetInt32(9);
                            var std = dr.IsDBNull(10) ? 0 : dr.GetInt32(10);
                            
                            var c = new Column()
                            {
                                ColumnName = columnname,
                            };

                            c.Metadata.Summary = description;
                            c.Metadata.Unit = Unit.Parse(unit);
                            c.Metadata.Quantity = Quantity.Parse(ucd);
                            c.Metadata.Class = utype;
                            c.DataType = DataTypes.Char;

                            // TODO: size, principal, indexed, std

                            yield return new KeyValuePair<string, Column>(c.Name, c);
                        }
                    }
                }
            }
            // TODO: how to get primary key information?
        }
  
        protected override IEnumerable<KeyValuePair<string, Index>> OnLoadIndexes(DatabaseObject databaseObject)
        {
            // TODO: TAP doesn't support querying indices. Fail or do nothing?

            throw new NotImplementedException();
        }

        protected override IEnumerable<KeyValuePair<string, IndexColumn>> OnLoadIndexColumns(Index index)
        {
            // TODO: TAP doesn't support querying indices. Fail or do nothing?

            throw new NotImplementedException();
        }

        protected override IEnumerable<KeyValuePair<string, Parameter>> OnLoadParameters(DatabaseObject databaseObject)
        {
            // TODO: TAP doesn't support anything that has parameters. Fail or do nothing?

            throw new NotImplementedException();
        }

        #endregion
        #region Metadata

        protected override DatasetMetadata OnLoadDatasetMetadata()
        {
            // TODO: does TAP support anything like this?

            throw new NotImplementedException();
        }

        protected override DatabaseObjectMetadata OnLoadDatabaseObjectMetadata(DatabaseObject databaseObject)
        {
            // TODO: How to get table metadata from TAP? Does INFORMATION_SCHEMA have it?

            return new DatabaseObjectMetadata();
        }

        protected override void OnDropDatabaseObjectMetadata(DatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveDatabaseObjectMetadata(DatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        protected override void OnLoadAllColumnMetadata(DatabaseObject databaseObject)
        {
            // TODO: How to get column metadata from TAP? Does INFORMATION_SCHEMA have it?

            throw new NotImplementedException();
        }

        protected override void OnLoadAllParameterMetadata(DatabaseObject databaseObject)
        {
            // TODO: TAP doesn't support anything that has parameters. Fail or do nothing?

            throw new NotImplementedException();
        }

        protected override void OnDropAllVariableMetadata(DatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveAllVariableMetadata(DatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Statistics

        protected override DatasetStatistics OnLoadDatasetStatistics()
        {
            // TODO: does TAP support anything like this?

            throw new NotImplementedException();
        }

        protected override TableStatistics OnLoadTableStatistics(TableOrView tableOrView)
        {
            // TODO: does TAP support anything like this?

            throw new NotImplementedException();
        }

        #endregion

        protected override void OnRenameObject(DatabaseObject obj, string schemaName, string objectName)
        {
            throw new NotImplementedException();
        }

        protected override void OnDropObject(DatabaseObject obj)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreateTable(Table table, bool createPrimaryKey, bool createIndexes)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreateIndex(Index index)
        {
            throw new NotImplementedException();
        }

        protected override void OnDropIndex(Index index)
        {
            throw new NotImplementedException();
        }

        protected override void OnTruncateTable(Table table)
        {
            throw new NotImplementedException();
        }

        #region Data type mapping functions

        protected override DataType MapDataType(string name)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
