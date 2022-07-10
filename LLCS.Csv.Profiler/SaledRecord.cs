using Microsoft.Toolkit.HighPerformance.Enumerables;
using LLCS.Csv;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;

namespace MyBenchmarks
{
    internal class SaledRecord : ICsvSerializer
    {
        public string Region;
        public string Country;
        public string ItemType;
        public string SalesChannel;
        public string OrderPriority;
        public string OrderDate;
        public string OrderID;
        public string ShipDate;
        public string UnitsSold;
        public string UnitPrice;
        public string UnitCost;
        public string TotalRevenue;
        public string TotalCost;
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
}