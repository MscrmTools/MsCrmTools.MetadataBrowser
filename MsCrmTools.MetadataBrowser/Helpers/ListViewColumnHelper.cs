using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MsCrmTools.MetadataBrowser.Helpers
{
    internal class ListViewColumnHelper
    {
        public static ColumnHeader AddColumnHeaderByProperty(ListView listView, Type type, string propertyName)
        {
            return new ColumnHeader
            {
                Text = type.GetProperties().First(p => p.Name == propertyName).Name,
                Width = 7 * type.GetProperties().First(p => p.Name == propertyName).Name.Length
            };
        }

        public static void AddColumnsHeader(ListView listView, Type type, string[] firstColumns, string[] selectedColumns,
            string[] columnsToIgnore)
        {
            var columns = new List<ColumnHeader>();

            foreach (var firstColumn in firstColumns)
            {
                columns.Add(AddColumnHeaderByProperty(listView, type, firstColumn));
            }

            if (selectedColumns != null)
            {
                var properties = type.GetProperties();

                foreach (var attr in selectedColumns)
                {
                    if (firstColumns.Contains(attr))
                        continue;

                    var prop = properties.First(p => p.Name == attr);
                    columns.Add(AddColumnHeaderByProperty(listView, type, prop.Name));
                }
            }
            else
            {
                foreach (var prop in type.GetProperties().OrderBy(p => p.Name))
                {
                    if (firstColumns.Contains(prop.Name) || columnsToIgnore.Contains(prop.Name))
                        continue;

                    columns.Add(AddColumnHeaderByProperty(listView, type, prop.Name));
                }
            }

            listView.Columns.AddRange(columns.ToArray());
        }
    }
}