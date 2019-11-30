
namespace SqlGenerator.Core
{
    public class SqlCompare
    {
        public enum SqlCompareOperator
        {
            Equals
        }
        
        string leftOperand;
        string rightOperand;
        SqlCompareOperator sqlCompareOperator;

        public SqlCompare(SqlColumn leftOperand, int rightOperand, SqlCompareOperator sqlCompareOperator)
        {
            this.leftOperand = leftOperand.getRawCommand();
            this.rightOperand = rightOperand.ToString();
            this.sqlCompareOperator = sqlCompareOperator;
        }
        public string getRawCommand()
        {
            switch(sqlCompareOperator) {
                case SqlCompareOperator.Equals:
                    return string.Format("{0}={1}", leftOperand, rightOperand);
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}
