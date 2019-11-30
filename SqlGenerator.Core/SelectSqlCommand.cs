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
        private List<string> joinsList;

        public SelectSqlCommand(List<SqlColumn> columns, string source, SqlCompare where = null)
        {
            this.columns = columns;
            this.source = source;
            this.where = where; 
            this.joinsList = new List<string>();
        }

        public SelectSqlCommand leftJoin(string joinedSource, SqlCompare joinCompare)
        {
            joinsList.Add(baseJoin(SqlKeywords.LEFT_JOIN, joinedSource, joinCompare));
            return this;
        }

        public string getRawCommand()
        {
            StringBuilder rawSql = new StringBuilder();
            rawSql.AppendFormat("SELECT {0} FROM {1}", string.Join(",", columns.Select(p=>p.getRawCommand())), source);
            if(where != null)
                rawSql.AppendFormat(" WHERE {0}", where.getRawCommand());
            if(joinsList != null) 
            foreach(var joinStr in joinsList) 
                rawSql.AppendFormat(" {0}", joinStr);
            return rawSql.ToString();
        }

        private string baseJoin(string joinCommand, string joinedSource, SqlCompare joinCompare)
        {
            if(joinCompare == null)
                return string.Format("{0} {1}", joinCommand, joinedSource);
            return string.Format("{0} {1} ON {2}", joinCommand, joinedSource, joinCompare.getRawCommand());
        }
    }
}
