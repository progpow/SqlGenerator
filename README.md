# SqlGenerator
Generate raw sql(Sql Server) from object model.

Examples of using:
  SQL Script:
  
    SELECT name, SUM(points) FROM Customer LEFT JOIN Order WHERE Order.Category=3 GROUP BY name
    
  C# Code:
  
    const string tableName = "Customer";
    string[] columns = new string[] {"id","name"};
    const string tableName2 = "Order";
    string[] columns2 = new string[] {"id", "customerId", "category", "points"};
    
    SelectSqlCommand selectSqlCommand = new SelectSqlCommand(
                                                tableName
                                                ,new SqlColumn(columns[1])
                                                ,new SqlColumn(columns2[3]).setSumFunc()
                                            )
                                            .leftJoin(
                                                     tableName2
                                                     ,new SqlCompare(
                                                             new SqlColumn(columns[0], tableName)
                                                             ,new SqlColumn(columns2[1], tableName2)
                                                             ,SqlCompare.SqlCompareOperator.Equals)
                                            )
                                            .where(
                                                new SqlCompare(
                                                   new SqlColumn(columns2[2], tableName2)
                                                   ,3
                                                   ,SqlCompare.SqlCompareOperator.Equals 
                                                )
                                            )
                                            .groupBy(
                                                new SqlColumn(columns[1], tableName)
                                            );      

TODO:
MERGE Command
Insert
Update
Delete

ORM generator as separate project
