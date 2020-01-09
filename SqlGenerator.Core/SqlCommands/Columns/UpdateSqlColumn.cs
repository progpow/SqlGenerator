using System.Text;

namespace SqlGenerator.Core
{
    public class UpdateSqlColumn 
    {
        SqlColumn updatedColumn;
        SqlColumn updatedValue;
        public UpdateSqlColumn(SqlColumn updatedColumn, SqlColumn updatedValue)
        {
            this.updatedColumn = updatedColumn;
            this.updatedValue = updatedValue;
        }

        public string getRawCommand() 
        {
            StringBuilder rawSql = new StringBuilder();
            rawSql.AppendFormat("{0}={1}", updatedColumn.getRawCommand(), updatedValue.getRawCommand());
            return rawSql.ToString();
        }
    }
}
