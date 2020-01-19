using System.Linq;
using NUnit.Framework;
using SqlGenerator.Core;

namespace SqlGenerator.Tests
{
    [TestFixture]
    public class MergeSqlCommandTests {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMergeSqlCommand()
        {
            const string tableName = "table1";
            const string tableName2 = "table2";
            string[] columns = {"column1","column2","column3"}; 
            string[] columns2 = {"column21","column22","column23"}; 
            MergeSqlCommand mergeSqlCommand = new MergeSqlCommand(
                tableName
                , columns.Select(p=> new SqlColumn(p)).ToArray()
                , new SelectSqlCommand(tableName2, columns2.Select(p=>new  SqlColumn(p)).ToArray())
                , new SqlCompare(new SqlColumn(columns[0]), new SqlColumn(columns2[0]), SqlCompare.SqlCompareOperator.Equals)                
            );
            mergeSqlCommand.whenMatched(
                new UpdateSqlCommand(
                    string.Empty
                    , new UpdateSqlColumn(new SqlColumn(columns[0]), new SqlColumn(columns2[0])))
                , new SqlCompare(new SqlColumn(columns[1]), new SqlColumn(columns2[1]), SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("MERGE {0} AS TARGET USING (select {1} from {2}) as source ({9}) on ({3}={4}) WHEN MATCHED AND {5}={6} THEN UPDATE SET {7}={8}",tableName, string.Join(",",columns2), tableName2,columns[0], columns2[0], columns[1], columns2[1], columns[0], columns2[0], string.Join(",",columns));
            Assert.AreEqual(comparingString.ToLower(), mergeSqlCommand.getRawCommand().ToLower());
        }
    }
}