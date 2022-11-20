using System.Linq;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using NodaTime;
using Xunit;

namespace AdaskoTheBeAsT.Dapper.NodaTime.Test
{
    [Collection("DBTests")]
    public sealed class OffsetDateTimeTests
    {
        private readonly string _connectionString;

        public OffsetDateTimeTests(DatabaseFixture databaseFixture)
        {
            _connectionString = databaseFixture.ConnectionString;
            SqlMapper.AddTypeHandler(OffsetDateTimeHandler.Default);
        }

        [Theory]
        [ClassData(typeof(DbVendorLibraryTestData))]
        public void Can_Write_And_Read_OffsetDateTime_Stored_As_DateTimeOffset(DbVendorLibrary library)
        {
            using var connection = DbVendorLibraryConnectionProvider.Provide(library, _connectionString);
            var o = new TestObject
            {
                Value = new OffsetDateTime(
                    new LocalDateTime(1234, 1, 2, 3, 4, 5, 6),
                    Offset.FromHoursAndMinutes(-5, -30)),
            };

            const string sql = "CREATE TABLE #T ([Value] datetimeoffset); INSERT INTO #T VALUES (@Value); SELECT * FROM #T";
            var t = connection.Query<TestObject>(sql, o).First();

            t.Value.Should().Be(o.Value);
        }

        [Theory]
        [ClassData(typeof(DbVendorLibraryTestData))]
        public void Can_Write_And_Read_OffsetDateTime_With_Null_Value(DbVendorLibrary library)
        {
            using var connection = DbVendorLibraryConnectionProvider.Provide(library, _connectionString);
            var o = new TestObject();

            const string sql = "CREATE TABLE #T ([Value] datetimeoffset NULL); INSERT INTO #T VALUES (@Value); SELECT * FROM #T";
            var t = connection.Query<TestObject>(sql, o).First();

            using (new AssertionScope())
            {
                t.Value.Should().BeNull();
                t.Value.Should().Be(o.Value);
            }
        }

        public class TestObject
        {
            public OffsetDateTime? Value { get; set; }
        }
    }
}
