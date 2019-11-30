
namespace SqlGenerator.Core
{
    public class SqlColumn 
    {
        string columnName;      
        string tableName;  
        string alias;
        public SqlColumn(string columnName): this(columnName, string.Empty)
        {
        }

        public SqlColumn(string columnName, string tableName)
        {
            this.columnName = columnName;
            this.tableName = tableName;
        }

        public SqlColumn setAlias(string alias)
        {
            this.alias = alias;
            return this;
        }

        public string getRawCommand()
        {
            string ret = string.Empty; 
            if(!string.IsNullOrEmpty(tableName))
                ret += string.Format("{0}.",tableName);
            ret+=columnName;
            if(!string.IsNullOrEmpty(alias)) 
                ret = string.Format("({0}) as {1}",ret,alias);
            return ret;
        }
    }
}
