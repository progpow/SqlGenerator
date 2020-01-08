
using System;
using System.Text;

namespace SqlGenerator.Core
{

    public class UpdateSqlCommand : ISqlCommand
    {
        string tableName;
        UpdateSqlColumn[] updatedColumns;
        SqlCompare whereCondition;
        public UpdateSqlCommand(string tableName, params UpdateSqlColumn[] updatedColumns): this(tableName, null, updatedColumns)
        {
        }

        public UpdateSqlCommand(string tableName, SqlCompare where, params UpdateSqlColumn[] updatedColumns)
        {
            this.tableName = tableName;
            this.updatedColumns =  updatedColumns;
            this.whereCondition = where;
        }

        public string getRawCommand()
        {
            StringBuilder rawSql = new StringBuilder();
            rawSql.AppendFormat("UPDATE {0} SET ", tableName);
            for (int i = 0; i < updatedColumns.Length; i++)
            {
                rawSql.AppendFormat("{0}{1}",i == 0 ? string.Empty : SqlKeywords.COLUMNS_SEPARATOR, updatedColumns[i].getRawCommand());
            }
            if(whereCondition != null)
                rawSql.AppendFormat(" WHERE {0}", whereCondition.getRawCommand());            
            return rawSql.ToString();    
        }
    }
}