using System.Linq;
using System.Text;

namespace SqlGenerator.Core
{
    public class InsertSqlCommand: ISqlCommand
    {
        protected string target {get;private set;}
        protected SqlValueColumn[] values { get; private set; }
        protected SqlColumn[] targetColumns { get; private set; }
        SelectSqlCommand selectSqlCommand;

        public InsertSqlCommand(string target, SqlValueColumn[] values)
        {
            this.target = target;
            this.values = values;
        }

        public InsertSqlCommand(string target, SqlColumn[] targetColumns, SqlValueColumn[] values)
        {
            this.target = target;
            this.values = values;
            this.targetColumns = targetColumns;
        }

        public InsertSqlCommand(string target, SelectSqlCommand selectSqlCommand)
        {
            this.target = target;
            this.selectSqlCommand = selectSqlCommand;
        }

        public InsertSqlCommand(string target, SqlColumn[] targetColumns, SelectSqlCommand selectSqlCommand)
        {
            this.target = target;
            this.selectSqlCommand = selectSqlCommand;
            this.targetColumns = targetColumns;
        }

        public virtual string getRawCommand()
        {
            StringBuilder rawSql = new StringBuilder();
            rawSql.Append("INSERT INTO");
            rawSql.AppendFormat(" {0}", target);
            if(targetColumns != null && targetColumns.Length > 0)
                rawSql.AppendFormat("({0})", string.Join(",", targetColumns.Select(p=>p.getRawCommand())));
            if(values != null && values.Length > 0)
                rawSql.AppendFormat(" VALUES {0}", string.Join(",", values.Select(p=>p.getRawCommand())));
            if(selectSqlCommand != null)
                rawSql.AppendFormat(" {0}", selectSqlCommand.getRawCommand());
            return rawSql.ToString();
        }
    }
}