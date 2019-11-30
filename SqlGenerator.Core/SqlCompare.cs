
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

        private SqlCompare(string leftOperand, string rightOperand, SqlCompareOperator sqlCompareOperator)
        {
            this.leftOperand = leftOperand;
            this.rightOperand = rightOperand;
            this.sqlCompareOperator = sqlCompareOperator;
        }

        public SqlCompare(SqlColumn leftOperand, int rightOperand, SqlCompareOperator sqlCompareOperator) 
            : this(leftOperand.getRawCommand(), rightOperand.ToString(), sqlCompareOperator)
        {
            
        }

        public SqlCompare(SqlColumn leftOperand, SqlColumn rightOperand, SqlCompareOperator sqlCompareOperator)
            : this(leftOperand.getRawCommand(), rightOperand.getRawCommand(), sqlCompareOperator)
        {

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
