
using System;

namespace SqlGenerator.Core
{
    public class InternalSqlGeneratorException: Exception 
    {
        public InternalSqlGeneratorException(string errorMessage): base(errorMessage)
        {
            
        }
    }
}