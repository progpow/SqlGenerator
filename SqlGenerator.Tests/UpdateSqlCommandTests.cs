using NUnit.Framework;
using SqlGenerator.Core;

namespace SqlGenerator.Tests
{
    [TestFixture]
    public class UpdateSqlCommandTests
    {
        [SetUp]
        public void Setup()
        {
            
        }
        
        [Test]
        public void TestUpdateAll()
        {
            string tableName = "table1";
            string[] columns =  new string []{"column1", "column2", "columns3"};
            string[] values =  new string[] {"test" , "column2 + 100", "column4"};
            UpdateSqlCommand updateSqlCommand = new UpdateSqlCommand(tableName, 
                                new UpdateSqlColumn(new SqlColumn(columns[0]), new SqlValueColumn(values[0])),
                                new UpdateSqlColumn(new SqlColumn(columns[1]),new SqlColumn(columns[1]).useExpression(SqlOperand.Plus, new SqlValueColumn(100))),
                                new UpdateSqlColumn(new SqlColumn(columns[2]),new SqlColumn(values[2]))
                                );

            string comparingString = string.Format("UPDATE {0} SET {1}='{2}',{3}={4},{5}={6}", tableName, columns[0], values[0], columns[1], values[1], columns[2], values[2]);
            Assert.AreEqual(comparingString.ToLower(), updateSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestUpdateWhere()
        {
            string tableName = "table1";
            string[] columns =  new string []{"column1", "column2"};
            object[] values = new object[] {"test", 5};
            UpdateSqlCommand updateSqlCommand = new UpdateSqlCommand(tableName, new SqlCompare(new SqlColumn(columns[1]), new SqlValueColumn((int)values[1]), SqlCompare.SqlCompareOperator.Equals), new  UpdateSqlColumn(new SqlColumn(columns[0]), new  SqlValueColumn(values[0].ToString())));
            string comparingString = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3}={4}", tableName, columns[0], values[0], columns[1], values[1]);
            Assert.AreEqual(comparingString.ToLower(), updateSqlCommand.getRawCommand().ToLower());
        }
    }
}