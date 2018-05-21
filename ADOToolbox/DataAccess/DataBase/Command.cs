using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOToolbox
{
    public sealed class Command
    {
        internal string Query { get; private set; }
        internal bool IsStoredProcedure { get; private set; }
        internal Dictionary<string, object> Parameters { get; private set; }

     
        public Command(string query, bool flag)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query can't be empty or null");
            Parameters = new Dictionary<string, object>();
            this.Query = query;
            this.IsStoredProcedure = flag;
        }

        public Command(string query):this(query, false)
        {
            
        }



        

        public void AddParameter(string parameterName, Object value)
        {
            Parameters.Add(parameterName, value);
        }
    }
}
