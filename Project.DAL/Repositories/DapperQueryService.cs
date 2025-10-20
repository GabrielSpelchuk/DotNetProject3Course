using System.Data;
using Dapper;

namespace Project.DAL.Repositories;

public class DapperQueryService
{
    private readonly IDbConnection _conn;
    public DapperQueryService(IDbConnection conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<TDto>> QueryAsync<TDto>(string sql, object? param, CancellationToken ct)
    {
        if (_conn.State != ConnectionState.Open)
            await ((System.Data.Common.DbConnection)_conn).OpenAsync(ct);
        return await _conn.QueryAsync<TDto>(new CommandDefinition(sql, param, cancellationToken: ct));
    }
}