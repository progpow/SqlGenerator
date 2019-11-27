using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqlGenerator.Core;
namespace SqlGenerator.Tests
{
    [TestFixture]
    public class SelectSqlCommandTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSelectCommand1()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(columns.Select(p=>new SqlColumn(p)).ToList(), "table1", new SqlCompare(new SqlColumn(columns[0]),1, SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("SELECT {0} FROM {1} where {2}=1"
                                                        ,string.Join(",",columns)
                                                        ,tableName
                                                        ,columns[0]
                                                );
            Assert.AreEqual(comparingString.ToLower().Replace(" ", ""), selectSqlCommand.getRawCommand().ToLower().Replace(" ", ""));
        }
    }
}