
namespace SqlGenerator.Core
{
    public class SqlValueColumn: SqlColumn
    {
        public SqlValueColumn(string textValue): base(SqlServerHelper.Instance.textColumn(textValue))
        {
            
        }


    }
}