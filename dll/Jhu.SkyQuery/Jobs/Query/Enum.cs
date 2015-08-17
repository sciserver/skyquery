using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Jobs.Query
{
    /// <summary>
    /// Column list nullable type.
    /// </summary>
    public enum ColumnListNullType
    {
        /// <summary>
        /// No 'NULL' or 'NOT NULL' added after columns.
        /// </summary>
        /// <remarks>
        /// For use with select lists, view definitions.
        /// </remarks>
        Nothing,

        /// <summary>
        /// 'NULL' added after each column.
        /// </summary>
        /// <remarks>
        /// Not used.
        /// </remarks>
        Null,

        /// <summary>
        /// 'NOT NULL' added after each column
        /// </summary>
        /// <remarks>
        /// From use with create table.
        /// </remarks>
        NotNull
    }

    /// <summary>
    /// Column list type.
    /// </summary>
    public enum ColumnListType
    {
        /// <summary>
        /// To use with 'CREATE TABLE' column list.
        /// </summary>
        /// <remarks>
        /// Escaped name used, column type is added.
        /// </remarks>
        ForCreateTable,

        /// <summary>
        /// To use with 'CREATE VIEW' column list.
        /// </summary>
        /// <remarks>
        /// Escaped name used, column type is not added.
        /// </remarks>
        ForCreateView,

        /// <summary>
        /// To use with 'INSERT' column list.
        /// </summary>
        /// <remarks>
        /// Escaped name used, column type is not added.
        /// </remarks>
        ForInsert,

        /// <summary>
        /// To use with 'SELECT'.
        /// </summary>
        /// <remarks>
        /// Original name used. To use with zone table create
        /// from source tables.
        /// </remarks>
        ForSelectWithOriginalName,

        /// <summary>
        /// To use with 'SELECT'.
        /// </summary>
        /// <remarks>
        /// Escaped name used. To use with anything but zone table
        /// create from source tables.
        /// </remarks>
        ForSelectWithEscapedName,

        /// <summary>
        /// To use with 'SELECT'.
        /// </summary>
        /// <remarks>
        /// Escaped name used, no column alias added. To use
        /// with 'INSERT' or 'CREATE INDEX'
        /// </remarks>
        ForSelectNoAlias
    }

    [Flags]
    public enum ColumnListInclude
    {
        None = 0,
        PrimaryKey = 1,
        Referenced = 2,
        All = PrimaryKey | Referenced
    }
}
