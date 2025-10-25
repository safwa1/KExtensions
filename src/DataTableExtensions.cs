using System.Data;

namespace KExtensions;

public static class DataTableExtensions
{
    public static int BinarySearch(this DataTable dataTable, string columnName, object value)
    {
        if (dataTable == null)
            throw new ArgumentNullException(nameof(dataTable));

        if (string.IsNullOrEmpty(columnName))
            throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));

        int columnIndex = dataTable.Columns.IndexOf(columnName);
        if (columnIndex == -1)
            throw new ArgumentException($"Column '{columnName}' does not exist in the DataTable.", nameof(columnName));

        int left = 0;
        int right = dataTable.Rows.Count - 1;

        while (left <= right)
        {
            int middle = (left + right) / 2;
            int comparison = Comparer<object>.Default.Compare(dataTable.Rows[middle][columnIndex], value);

            if (comparison == 0)
            {
                return middle;
            }

            if (comparison < 0)
            {
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }

        return -1;
    }
    
    public static DataRow Clone(this DataRow source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        DataTable table = source.Table;
        if (table == null)
            throw new InvalidOperationException("DataRow does not belong to a DataTable.");

        DataRow cloneRow = table.NewRow();
        cloneRow.ItemArray = source.ItemArray.Clone() as object[] ?? throw new InvalidOperationException();
        return cloneRow;
    }
    
}