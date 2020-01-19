
namespace SqlGenerator.Core
{
    public class SqlServerHelper
    {
        private static readonly object lockCreate = new object();
        private static SqlServerHelper instance;
        private SqlServerHelper()
        {

        }

        public static SqlServerHelper Instance {
            get
            {
                if (instance == null)
                {
                    lock (lockCreate)
                    {
                        if (instance == null)
                        {
                            instance = new SqlServerHelper();
                        }
                    }
                }
                return instance;
            }
        }

        public string textColumn(string columnValue)
        {
            return string.Format("{0}{1}{0}", SqlKeywords.QUOTES, columnValue);
        }
    }   
}
