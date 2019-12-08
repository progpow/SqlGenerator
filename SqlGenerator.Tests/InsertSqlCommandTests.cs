using System.Linq;
using NUnit.Framework;
using SqlGenerator.Core;

namespace SqlGenerator.Tests
{
    [TestFixture]
    public class InsertSqlCommandTests
    {
        [Test]
        public void TestBaseInsert()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            string[] values = new string[] {"value1","value2","value3"};
            InsertSqlCommand insertSqlCommand = new InsertSqlCommand(tableName, values.Select(p=>new SqlValueColumn(p)).ToArray());
            string comparingString = string.Format("INSERT INTO {0} VALUES {1}"
                                                        ,tableName
                                                        ,string.Join(",", values.Select(p=> string.Format("'{0}'",p)))
                                                );
            Assert.AreEqual(comparingString.ToLower(), insertSqlCommand.getRawCommand().ToLower());
        }

        [Test]
        public void TestBaseWithColumnsInsert()
        {
            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            string[] values = new string[] {"value1","value2","value3"};
            InsertSqlCommand insertSqlCommand = new InsertSqlCommand(tableName, columns.Select(p=>new SqlColumn(p)).ToArray(), values.Select(p=>new SqlValueColumn(p)).ToArray());
            string comparingString = string.Format("INSERT INTO {0}({2}) VALUES {1}"
                                                        ,tableName
                                                        ,string.Join(",", values.Select(p=> string.Format("'{0}'",p)))
                                                        ,string.Join(",",columns)
                                                );
            Assert.AreEqual(comparingString.ToLower(), insertSqlCommand.getRawCommand().ToLower());
        }        

        [Test]
        public void TestSelectInsert()
        {
            const string sourceTableName = "tableSource";
            string[] columnsSource = new string[] {"columnSource1","columnSource2","columnSource3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(sourceTableName, columnsSource.Select(p=>new SqlColumn(p)).ToArray())
                                                        .where(new SqlCompare(new SqlColumn(columnsSource[0]),1, SqlCompare.SqlCompareOperator.Equals));
            string selectString = string.Format("SELECT {0} FROM {1} where {2}=1"
                                                        ,string.Join(",",columnsSource)
                                                        ,sourceTableName
                                                        ,columnsSource[0]
                                                );

            const string tableName = "table1";
            string[] columns = new string[] {"column1","column2","column3"};
            InsertSqlCommand insertSqlCommand = new InsertSqlCommand(tableName, columns.Select(p=>new SqlColumn(p)).ToArray(), selectSqlCommand);
            string comparingString = string.Format("INSERT INTO {0}({2}) {1}"
                                                        ,tableName
                                                        ,selectString
                                                        ,string.Join(",",columns)
                                                );
            Assert.AreEqual(comparingString.ToLower(), insertSqlCommand.getRawCommand().ToLower());
        }        

        [Test]
        public void TestSelectWithColumnsInsert()
        {
            const string sourceTableName = "tableSource";
            string[] columnsSource = new string[] {"columnSource1","columnSource2","columnSource3"};
            SelectSqlCommand selectSqlCommand = new SelectSqlCommand(sourceTableName, columnsSource.Select(p=>new SqlColumn(p)).ToArray())
                                                        .where(new SqlCompare(new SqlColumn(columnsSource[0]),1, SqlCompare.SqlCompareOperator.Equals));
            string selectString = string.Format("SELECT {0} FROM {1} where {2}=1"
                                                        ,string.Join(",",columnsSource)
                                                        ,sourceTableName
                                                        ,columnsSource[0]);

            const string tableName = "table1";
            
            InsertSqlCommand insertSqlCommand = new InsertSqlCommand(tableName, selectSqlCommand);
            string comparingString = string.Format("INSERT INTO {0} {1}"
                                                        ,tableName
                                                        ,selectString                                                        
                                                );
            Assert.AreEqual(comparingString.ToLower(), insertSqlCommand.getRawCommand().ToLower());
        }        
    }
}