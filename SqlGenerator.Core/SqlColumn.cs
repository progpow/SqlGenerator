
namespace SqlGenerator.Core
{
    public class SqlColumn 
    {
        string columnName;        
        public SqlColumn(string columnName)
        {
            this.columnName = columnName;
        }

        public string getRawCommand()
        {
            return columnName;
        }
    }
}
