using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace SqlGenerator.Core
{
    public class SelectSqlCommand: ISqlCommand
    {

        private List<SqlColumn> columns;
        private string source;
        private SqlCompare where;

        public SelectSqlCommand(List<SqlColumn> columns, string source, SqlCompare where = null)
        {
            this.columns = columns;
            this.source = source;
            this.where = where; 
        }

        public string getRawCommand()
        {
            StringBuilder rawSql = new StringBuilder();
            rawSql.AppendFormat("SELECT {0} FROM {1}", string.Join(",", columns.Select(p=>p.getRawCommand())), source);
            if(where != null)
                rawSql.AppendFormat(" WHERE {0}", where.getRawCommand());
            return rawSql.ToString();
        }
    }
}
