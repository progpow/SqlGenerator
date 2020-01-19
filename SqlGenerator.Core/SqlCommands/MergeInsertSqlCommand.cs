using System.Linq;
using System.Text;

namespace SqlGenerator.Core
{
    public class MergeInsertSqlCommand : InsertSqlCommand
    {
        public MergeInsertSqlCommand(string target, SqlColumn[] sqlColumns, SqlValueColumn[] values): base(target, sqlColumns, values)
        {
            
        }

        public override string getRawCommand()
        {
            StringBuilder rawSql = new StringBuilder();
            rawSql.Append("INSERT");
            if(targetColumns != null && targetColumns.Length > 0)
            rawSql.AppendFormat("({0})", string.Join(",", targetColumns.Select(p=>p.getRawCommand())));
            if(values != null && values.Length > 0)
                rawSql.AppendFormat(" VALUES {0}", string.Join(",", values.Select(p=>p.getRawCommand())));
            return rawSql.ToString();
        }
    }
}
