﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExcelMapper
{
    /// <summary>
    /// An object that represents the heading read from a sheet.
    /// </summary>
    public class ExcelHeading
    {
        private readonly string[] _columnNames;

        internal ExcelHeading(IDataRecord reader)
        {
            var nameMapping = new Dictionary<string, int>(reader.FieldCount);
            var columnNames = new string[reader.FieldCount];

            for (var columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                var columnName = reader.GetValue(columnIndex)?.ToString();
                if (columnName == null)
                    columnNames[columnIndex] = string.Empty;
                else
                {
                    if (nameMapping.ContainsKey(columnName))
                        columnName += "_" + Guid.NewGuid();

                    nameMapping.Add(columnName, columnIndex);
                    columnNames[columnIndex] = columnName;
                }
            }

            NameMapping = nameMapping;
            _columnNames = columnNames;
        }

        private Dictionary<string, int> NameMapping { get; }

        /// <summary>
        /// Gets the name of the column at the given zero-based index.
        /// </summary>
        /// <param name="index">The zero-based index to get the name of.</param>
        /// <returns>The name of the column at the given zero-based index.</returns>
        public string GetColumnName(int index) => _columnNames[index];

        /// <summary>
        /// Gets the zero-based index of the column with the given name. This method throws an ExcelMappingException
        /// if the column does not exist.
        /// </summary>
        /// <param name="columnName">The name of the column to get the zero-based index of.</param>
        /// <returns>The zero-based index of the column with the given name.</returns>
        public int GetColumnIndex(string columnName)
        {
            if (!NameMapping.TryGetValue(columnName, out var index))
            {
                var foundColumns = string.Join(", ", NameMapping.Keys.Select(c => $"\"{c}\""));
                throw new ExcelMappingException($"Column \"{columnName}\" does not exist in [{foundColumns}]");
            }

            return index;
        }

        /// <summary>
        /// Gets the list of all column names in the heading.
        /// </summary>
        public IEnumerable<string> ColumnNames => _columnNames;
    }
}
