using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace SqlGenerator.Core
{
    public class SelectSqlCommand: ISqlCommand
    {

        private SqlColumn[] columns;
        private string source;
        private SqlCompare whereCondition;
        private List<string> joinsList;

        private SqlColumn[] groupByColumns;
        private SqlCompare havingCompare;

        private SqlColumn[] orderByColumns;
        private bool orderByAsc;

        private int topValue;
        private bool usingDistinct;

        public SelectSqlCommand(string source, params SqlColumn[] columns)
        {
            this.columns = columns;
            this.source = source;
            this.whereCondition = null;
            this.joinsList = new List<string>();
            this.groupByColumns = null;
            this.havingCompare = null;
            this.orderByColumns = null;
            this.topValue = 0;
            this.usingDistinct = false;
        }

        public SelectSqlCommand where(SqlCompare whereCondition)
        {
            this.whereCondition = whereCondition;
            return this;
        }

        public SelectSqlCommand leftJoin(string joinedSource, SqlCompare joinCompare)
        {
            joinsList.Add(baseJoin(SqlKeywords.LEFT_JOIN, joinedSource, joinCompare));
            return this;
        }

        public SelectSqlCommand rightJoin(string joinedSource, SqlCompare joinCompare)
        {
            joinsList.Add(baseJoin(SqlKeywords.RIGHT_JOIN, joinedSource, joinCompare));
            return this;
        }

        public SelectSqlCommand innerJoin(string joinedSource, SqlCompare joinCompare)
        {
            joinsList.Add(baseJoin(SqlKeywords.INNER_JOIN, joinedSource, joinCompare));
            return this;
        }

        public SelectSqlCommand groupBy(params SqlColumn[] columns)
        {
            groupByColumns = columns;
            return this;
        }

        public SelectSqlCommand having(SqlCompare havingCompare)
        {
            this.havingCompare = havingCompare;
            return this;
        }

        public SelectSqlCommand orderBy(params SqlColumn[] orderByColumns)
        {
            orderByAsc = true;
            this.orderByColumns = orderByColumns;
            return this;
        }

        public SelectSqlCommand orderByDesc(params SqlColumn[] orderByColumns)
        {
            orderByAsc = false;
            this.orderByColumns = orderByColumns;
            return this;
        }

        public SelectSqlCommand top(int topValue)
        {
            this.topValue = topValue;
            return this;
        }

        public SelectSqlCommand distinct()
        {
            this.usingDistinct = true;
            return this;
        }

        public string getRawCommand()
        {
            StringBuilder rawSql = new StringBuilder();
            string columnString =  string.Join(",", columns.Select(p=>p.getRawCommand()));
            if(usingDistinct)
                columnString = string.Format("DISTINCT {0}",columnString);
            if(topValue > 0) 
                columnString = string.Format("TOP({0}) {1}",topValue, columnString);
            rawSql.AppendFormat("SELECT {0} FROM {1}", columnString, source);
            if(whereCondition != null)
                rawSql.AppendFormat(" WHERE {0}", whereCondition.getRawCommand());
            if(joinsList != null) 
            foreach(var joinStr in joinsList) 
                rawSql.AppendFormat(" {0}", joinStr);
            if(groupByColumns != null && groupByColumns.Length > 0)
                rawSql.AppendFormat(" GROUP BY {0}", string.Join(",", groupByColumns.Select(p=>p.getRawCommand())));
            if(havingCompare != null) 
                rawSql.AppendFormat(" HAVING {0}", havingCompare.getRawCommand());
            if(orderByColumns != null)
                rawSql.AppendFormat(" ORDER BY {0} {1}",string.Join(",", orderByColumns.Select(p=>p.getRawCommand())), orderByAsc ? SqlKeywords.ORDER_ASC: SqlKeywords.ORDER_DESC);
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