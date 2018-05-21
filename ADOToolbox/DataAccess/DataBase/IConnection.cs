using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ADOToolbox
{
    public interface IConnection
    {
        string ConnectionString { get; }
        DbProviderFactory DbFactory { get; }

        int ExecuteNonQuery(Command command);
        IEnumerable<T> ExecuteReader<T>(Command command) where T : class, new();
        IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> Selector);
        object ExecuteScalar(Command Command);
        DataTable GetDataTable(Command command);
    }
}