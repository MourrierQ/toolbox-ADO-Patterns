using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADOToolbox
{
    public sealed class Connection : IConnection
    {

        public string ConnectionString { get; private set; }
        

        public DbProviderFactory DbFactory { get; private set; }

        public Connection(string connectionString, string Provider)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string can't be null or empty");
            this.ConnectionString = connectionString;

            if (string.IsNullOrWhiteSpace(Provider))
                throw new ArgumentException("Provider string can't be null or empty");

            this.DbFactory = DbProviderFactories.GetFactory(Provider);

            using (DbConnection c = CreateConnection())
            {
                c.Open();
            }
        }

        private DbConnection CreateConnection()
        {
            DbConnection c = DbFactory.CreateConnection();
            c.ConnectionString = ConnectionString;
            return c;
        }

        private DbCommand CreateCommand(Command command, DbConnection c)
        {
            DbCommand cmd = c.CreateCommand();
            cmd.CommandText = command.Query;
            cmd.CommandType = (command.IsStoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
            foreach (KeyValuePair<string, object> item in command.Parameters)
            {
                DbParameter param = DbFactory.CreateParameter();
                param.ParameterName = item.Key;
                param.Value = item.Value;
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        public object ExecuteScalar(Command Command)
        {
            using (DbConnection context = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(Command,context))
                {
                    context.Open();
                    object res = cmd.ExecuteScalar();
                    context.Close();
                    return (res is DBNull) ? null : res;
                }
                
            }
            
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> Selector)
        {
            using (DbConnection context = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(command, context))
                {
                    context.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return Selector(reader);
                        }
                    }
                    context.Close();
                }
            }
        }

        public int ExecuteNonQuery(Command command)
        {
            using (DbConnection context = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(command,context))
                {
                    context.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetDataTable(Command command)
        {
            using (DbConnection context = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(command, context))
                {
                    using (DbDataAdapter adapter = DbFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        return table;
                    }
                    
                    
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(Command command)
            where T : class,new()
        {
            using (DbConnection context = CreateConnection())
            {
                using (DbCommand cmd = CreateCommand(command,context))
                {
                    context.Open();
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        PropertyInfo propertyInfo;
                        while (reader.Read())
                        {
                            T temp = new T();
                            
                            int size = reader.FieldCount;
                            for (int i = 0; i < size; i++)
                            {
                                string fieldName = reader.GetName(i);
                                propertyInfo = typeof(T).GetProperty(fieldName);
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(temp, reader[i]);
                                }
                            }

                            yield return temp;
                        }
                    }
                    
                }
            }

           
        }

    }
}
