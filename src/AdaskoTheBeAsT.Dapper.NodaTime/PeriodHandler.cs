using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Dapper;
using NodaTime;

namespace AdaskoTheBeAsT.Dapper.NodaTime
{
    public sealed class PeriodHandler
        : SqlMapper.TypeHandler<Period>
    {
        public const string Space = " ";
        public static readonly PeriodHandler Default = new();
#pragma warning disable MA0009 // Add regex evaluation timeout
        private static readonly Regex PeriodRegex =
            new Regex("(Y[:](?<years>[-]?\\d+))?\\s*(M[:](?<months>[-]?\\d+))?\\s*(W[:](?<weeks>[-]?\\d+))?\\s*(D[:](?<days>[-]?\\d+))?\\s*(h[:](?<hours>[-]?\\d+))?\\s*(m[:](?<minutes>[-]?\\d+))?\\s*(s[:](?<seconds>[-]?\\d+))?\\s*(ms[:](?<miliseconds>[-]?\\d+))?\\s*(t[:](?<ticks>[-]?\\d+))?\\s*(ns[:](?<nanoseconds>[-]?\\d+))?");
#pragma warning restore MA0009 // Add regex evaluation timeout

        private PeriodHandler()
        {
        }

        public override void SetValue(IDbDataParameter parameter, Period value)
        {
            var list = new List<string>();
            ProcessSetDateComponent(list, value);
            ProcessSetTimeComponent(list, value);
            ProcessSetSubSecondTimeComponent(list, value);

            parameter.Value = string.Join(Space, list);
            parameter.SetSqlDbType(SqlDbType.VarChar);
        }

        public override Period Parse(object value)
        {
            if (value is Period period)
            {
                return period;
            }

            if (value is string str)
            {
                var match = PeriodRegex.Match(str);
                if (!match.Success)
                {
                    return Period.Zero;
                }

                var builder = new PeriodBuilder();
                SetDateComponentInBuilder(builder, match);
                SetTimeComponentInBuilder(builder, match);
                SetSubSecondTimeComponentInBuilder(builder, match);
                return builder.Build();
            }

            throw new DataException($"Cannot convert {value.GetType()} to NodaTime.Period");
        }

        private static void ProcessSetDateComponent(List<string> list, Period value)
        {
            if (value.Years != 0)
            {
                list.Add($"Y:{value.Years.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Months != 0)
            {
                list.Add($"M:{value.Months.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Weeks != 0)
            {
                list.Add($"W:{value.Weeks.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Days != 0)
            {
                list.Add($"D:{value.Days.ToString(CultureInfo.InvariantCulture)}");
            }
        }

        private static void ProcessSetTimeComponent(List<string> list, Period value)
        {
            if (value.Hours != 0)
            {
                list.Add($"h:{value.Hours.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Minutes != 0)
            {
                list.Add($"m:{value.Minutes.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Seconds != 0)
            {
                list.Add($"s:{value.Seconds.ToString(CultureInfo.InvariantCulture)}");
            }
        }

        private static void ProcessSetSubSecondTimeComponent(List<string> list, Period value)
        {
            if (value.Milliseconds != 0)
            {
                list.Add($"ms:{value.Milliseconds.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Ticks != 0)
            {
                list.Add($"t:{value.Ticks.ToString(CultureInfo.InvariantCulture)}");
            }

            if (value.Nanoseconds != 0)
            {
                list.Add($"ns:{value.Nanoseconds.ToString(CultureInfo.InvariantCulture)}");
            }
        }

        private static void SetDateComponentInBuilder(PeriodBuilder builder, Match match)
        {
            if (match.Groups["years"].Success)
            {
                builder.Years = int.Parse(match.Groups["years"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["months"].Success)
            {
                builder.Months = int.Parse(match.Groups["months"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["weeks"].Success)
            {
                builder.Weeks = int.Parse(match.Groups["weeks"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["days"].Success)
            {
                builder.Days = int.Parse(match.Groups["days"].Value, CultureInfo.InvariantCulture);
            }
        }

        private static void SetTimeComponentInBuilder(PeriodBuilder builder, Match match)
        {
            if (match.Groups["hours"].Success)
            {
                builder.Hours = long.Parse(match.Groups["hours"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["minutes"].Success)
            {
                builder.Minutes = long.Parse(match.Groups["minutes"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["seconds"].Success)
            {
                builder.Seconds = long.Parse(match.Groups["seconds"].Value, CultureInfo.InvariantCulture);
            }
        }

        private static void SetSubSecondTimeComponentInBuilder(PeriodBuilder builder, Match match)
        {
            if (match.Groups["miliseconds"].Success)
            {
                builder.Milliseconds = long.Parse(match.Groups["miliseconds"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["ticks"].Success)
            {
                builder.Ticks = long.Parse(match.Groups["ticks"].Value, CultureInfo.InvariantCulture);
            }

            if (match.Groups["nanoseconds"].Success)
            {
                builder.Nanoseconds = long.Parse(match.Groups["nanoseconds"].Value, CultureInfo.InvariantCulture);
            }
        }
    }
}
