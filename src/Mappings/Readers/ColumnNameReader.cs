﻿using System;
using ExcelDataReader;

namespace ExcelMapper.Mappings.Readers
{
    /// <summary>
    /// Reads the value of a single cell given the name of it's column.
    /// </summary>
    public sealed class ColumnNameValueReader : ICellValueReader
    {
        /// <summary>
        /// The name of the column to read.
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Constructs a reader that reads the value of a single cell given the name of it's column.
        /// </summary>
        /// <param name="columnName">The name of the column to read.</param>
        public ColumnNameValueReader(string columnName)
        {
            if (columnName == null)
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            if (columnName.Length == 0)
            {
                throw new ArgumentException("Column name cannot be empty.", nameof(columnName));
            }

            ColumnName = columnName;
        }

        public ReadCellValueResult GetValue(ExcelSheet sheet, int rowIndex, IExcelDataReader reader)
        {
            try
            {
                var index = sheet?.Heading?.GetColumnIndex(ColumnName);
                var value = reader[index ?? rowIndex]?.ToString();
                return new ReadCellValueResult(index ?? rowIndex, value);
            }
            catch (Exception e)
            {
                return new ReadCellValueResult(rowIndex, null);
            }
        }
    }
}
