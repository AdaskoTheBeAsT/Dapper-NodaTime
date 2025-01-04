using System.Threading.Tasks;
using LocalDb;
using Xunit;

namespace AdaskoTheBeAsT.Dapper.NodaTime.Test
{
    public sealed class DatabaseFixture
        : IAsyncLifetime
    {
#pragma warning disable IDISP006 // Implement IDisposable
        private SqlInstance? _sqlInstance;
        private SqlDatabase? _database;
#pragma warning restore IDISP006 // Implement IDisposable

        public string ConnectionString => _database?.ConnectionString ?? string.Empty;

        public async Task InitializeAsync()
        {
            _sqlInstance = new SqlInstance(name: GetDatabaseName(), buildTemplate: _ => Task.CompletedTask);

#if NET6_0_OR_GREATER
            if (_database is not null)
            {
                await _database.DisposeAsync().ConfigureAwait(false);
            }
#else
            _database?.Dispose();
#endif
            _database = await _sqlInstance.Build("DapperNodaTimeTests").ConfigureAwait(false);
        }

#if NET6_0_OR_GREATER
        public async Task DisposeAsync()
#else
        public Task DisposeAsync()
#endif
        {
#if NET6_0_OR_GREATER
            if (_database is not null)
            {
                await _database.DisposeAsync().ConfigureAwait(false);
            }
#else
            _database?.Dispose();
#endif
            _sqlInstance?.Cleanup();
#if NET6_0_OR_GREATER

#else
            return Task.CompletedTask;
#endif
        }

        private static string GetDatabaseName()
        {
#if NET462
            return "DapperNodaTimeTests_NET462";
#elif NET47
            return "DapperNodaTimeTests_NET47";
#elif NET471
            return "DapperNodaTimeTests_NET471";
#elif NET472
            return "DapperNodaTimeTests_NET472";
#elif NET48
            return "DapperNodaTimeTests_NET48";
#elif NET481
            return "DapperNodaTimeTests_NET481";
#elif NET6_0
            return "DapperNodaTimeTests_NET6";
#elif NET7_0
            return "DapperNodaTimeTests_NET7";
#elif NET8_0
            return "DapperNodaTimeTests_NET8";
#elif NET9_0
            return "DapperNodaTimeTests_NET9";
#else
            // Fallback or unknown TFM
            return "DapperNodaTimeTests_Unknown";
#endif
        }
    }
}
