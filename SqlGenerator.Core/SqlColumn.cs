using System.Collections.Generic;
using System.Text;

namespace SqlGenerator.Core
{
    public class SqlColumn 
    {
        string columnName;      
        string tableName;  
        string alias;
        string function;
        List<KeyValuePair<SqlOperand, SqlColumn>> expressions = new List<KeyValuePair<SqlOperand, SqlColumn>>();
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

        public SqlColumn useExpression(SqlOperand sqlOperand, SqlColumn sqlColumn)
        {
            expressions.Add(new KeyValuePair<SqlOperand, SqlColumn>(sqlOperand, sqlColumn));
            return this;
        }

        public string getRawCommand()
        {
            StringBuilder ret = new StringBuilder(); 
            if(!string.IsNullOrEmpty(tableName))
                ret.AppendFormat("{0}.",tableName);
            ret.AppendFormat(columnName);
            if(!string.IsNullOrEmpty(function))
                ret= new StringBuilder(string.Format("{0}({1})", function, ret.ToString()));
            if(expressions.Count > 0)
                foreach(var expression in expressions)
                    ret = new StringBuilder(string.Format("{0} {1} {2}", ret.ToString(),SqlKeywords.sqlOperandParse(expression.Key), expression.Value.getRawCommand()));
            if(!string.IsNullOrEmpty(alias)) 
                ret = new StringBuilder(string.Format("({0}) as {1}",ret.ToString(),alias));
            return ret.ToString();
        }

        public SqlColumn setMaxFunc()
        {
            this.function = SqlKeywords.MAX_FUNC;
            return this;
        }

        public SqlColumn setSumFunc()
        {
            this.function = SqlKeywords.SUM_FUNC;
            return this;
        }        
    }
}
