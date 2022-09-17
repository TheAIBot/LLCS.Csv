using CommunityToolkit.HighPerformance.Enumerables;
using LLCS.Csv;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;
using System.Diagnostics.CodeAnalysis;

namespace MyBenchmarks
{
    internal class SaledRecord : ICsvSerializer
    {
        [AllowNull]
        public string Region;
        [AllowNull]
        public string Country;
        [AllowNull]
        public string ItemType;
        [AllowNull]
        public string SalesChannel;
        [AllowNull]
        public string OrderPriority;
        [AllowNull]
        public string OrderDate;
        [AllowNull]
        public string OrderID;
        [AllowNull]
        public string ShipDate;
        [AllowNull]
        public string UnitsSold;
        [AllowNull]
        public string UnitPrice;
        [AllowNull]
        public string UnitCost;
        [AllowNull]
        public string TotalRevenue;
        [AllowNull]
        public string TotalCost;
        [AllowNull]
        public string TotalProfit;

        public void Serialize(CsvWriter writer)
        {
        }

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            //reader.TryRead<string>(ref tokens, out string _);
            return reader.TryReadString(ref tokens, out Region) &&
                   reader.TryReadString(ref tokens, out Country) &&
                   reader.TryReadString(ref tokens, out ItemType) &&
                   reader.TryReadString(ref tokens, out SalesChannel) &&
                   reader.TryReadString(ref tokens, out OrderPriority) &&
                   reader.TryReadString(ref tokens, out OrderDate) &&
                   reader.TryReadString(ref tokens, out OrderID) &&
                   reader.TryReadString(ref tokens, out ShipDate) &&
                   reader.TryReadString(ref tokens, out UnitsSold) &&
                   reader.TryReadString(ref tokens, out UnitPrice) &&
                   reader.TryReadString(ref tokens, out UnitCost) &&
                   reader.TryReadString(ref tokens, out TotalRevenue) &&
                   reader.TryReadString(ref tokens, out TotalCost) &&
                   reader.TryReadString(ref tokens, out TotalProfit);
        }
    }

    internal class SaledRecord2
    {
        [AllowNull]
        public string Region { get; set; }
        [AllowNull]
        public string Country { get; set; }
        [AllowNull]
        public string ItemType { get; set; }
        [AllowNull]
        public string SalesChannel { get; set; }
        [AllowNull]
        public string OrderPriority { get; set; }
        [AllowNull]
        public string OrderDate { get; set; }
        [AllowNull]
        public string OrderID { get; set; }
        [AllowNull]
        public string ShipDate { get; set; }
        [AllowNull]
        public string UnitsSold { get; set; }
        [AllowNull]
        public string UnitPrice { get; set; }
        [AllowNull]
        public string UnitCost { get; set; }
        [AllowNull]
        public string TotalRevenue { get; set; }
        [AllowNull]
        public string TotalCost { get; set; }
        [AllowNull]
        public string TotalProfit { get; set; }
    }
}