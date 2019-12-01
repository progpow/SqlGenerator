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
        public void TestSelectCommandWithWhere()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, columns.Select(p=>new SqlColumn(p)).ToArray())
                                                        .where(new SqlCompare(new SqlColumn(columns[0]),1, SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("SELECT {0} FROM {1} where {2}=1"
                                                        ,string.Join(",",columns)
                                                        ,tableName
                                                        ,columns[0]
                                                );
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandWithoutWhere()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, columns.Select(p=>new SqlColumn(p)).ToArray());
            string comparingString = string.Format("SELECT {0} FROM {1}"
                                                        ,string.Join(",",columns)
                                                        ,tableName
                                                        ,columns[0]
                                                );
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandWithAllColumns()
        {
            const string tableName = "table1";
            SelectSqlCommand selectSqlCommand =  new SelectSqlCommand(tableName,new SqlColumnAll());
            string comparingString = string.Format("SELECT * FROM {0}", tableName);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandWithAliasColumn()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            const string columnAlias = "alias1";
            SelectSqlCommand selectSqlCommand =  new SelectSqlCommand(tableName, new SqlColumn(columns[0]).setAlias(columnAlias));
            string comparingString = string.Format("SELECT ({0}) as {1} FROM {2}", columns[0], columnAlias, tableName);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandWithLeftJoin()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            const string tableName2 = "table2";
            string[] columns2 = new string[] {"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, new SqlColumn(columns[0], tableName))
                                                .leftJoin(tableName2,new SqlCompare(new SqlColumn(columns[0], tableName), new SqlColumn(columns2[0],tableName2), SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("SELECT {1}.{0} FROM {1} LEFT JOIN {2} ON {1}.{3}={2}.{4}", columns[0], tableName, tableName2, columns[0], columns2[0]);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandWithRightJoin()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            const string tableName2 = "table2";
            string[] columns2 = new string[] {"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, new SqlColumn(columns[0], tableName))
                                                        .rightJoin(tableName2,new SqlCompare(new SqlColumn(columns[0], tableName), new SqlColumn(columns2[0],tableName2), SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("SELECT {1}.{0} FROM {1} RIGHT JOIN {2} ON {1}.{3}={2}.{4}", columns[0], tableName, tableName2, columns[0], columns2[0]);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandWithInnerJoin()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            const string tableName2 = "table2";
            string[] columns2 = new string[] {"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, new SqlColumn(columns[0], tableName))
                                                    .innerJoin(
                                                        tableName2,
                                                        new SqlCompare(new SqlColumn(columns[0], tableName), new SqlColumn(columns2[0],tableName2), SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("SELECT {1}.{0} FROM {1} INNER JOIN {2} ON {1}.{3}={2}.{4}", columns[0], tableName, tableName2, columns[0], columns2[0]);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandGroupBy()
        {
            const string tableName = "table1";
            string[] columns = new string []{"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, new SqlColumn(columns[0], tableName))
                                                    .groupBy(new SqlColumn(columns[0]));
            string comparingString = string.Format("SELECT {1}.{0} FROM {1} GROUP BY {0}", columns[0], tableName);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandGroupByHaving()
        {
            const string tableName = "table1";
            string[] columns = new string []{"column1","column2","column3"};
            int comparedValue =  10;
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, new SqlColumn(columns[0], tableName))
                                                    .groupBy(new SqlColumn(columns[0]))
                                                    .having(new SqlCompare(new  SqlColumn(columns[1]), comparedValue, SqlCompare.SqlCompareOperator.Equals));
            string comparingString = string.Format("SELECT {1}.{0} FROM {1} GROUP BY {0} HAVING {2}={3}", columns[0], tableName, columns[1], comparedValue);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestSelectCommandGroupByFuncMax()
        {
            const string tableName = "table1";
            string[] columns = new string []{"column1","column2","column3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(tableName, new SqlColumn(columns[0], tableName).setMaxFunc())
                                                    .groupBy(new SqlColumn(columns[0]));
            string comparingString = string.Format("SELECT MAX({1}.{0}) FROM {1} GROUP BY {0}", columns[0], tableName);
            Assert.AreEqual(comparingString.ToLower(), selectSqlCommand.getRawCommand().ToLower());
        }
    }
}