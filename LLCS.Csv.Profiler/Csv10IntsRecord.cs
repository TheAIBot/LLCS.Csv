using Microsoft.Toolkit.HighPerformance.Enumerables;
using LLCS.Csv;
using LLCS.Csv.Reader;
using LLCS.Csv.Writer;

namespace MyBenchmarks
{
    internal struct Csv10IntsRecord : ICsvSerializer
    {
        public int Value1;
        public int Value2;
        public int Value3;
        public int Value4;
        public int Value5;
        public int Value6;
        public int Value7;
        public int Value8;
        public int Value9;
        public int Value10;

        public Csv10IntsRecord(int value)
        {
            Value1 = value + 0;
            Value2 = value + 1;
            Value3 = value + 2;
            Value4 = value + 3;
            Value5 = value + 4;
            Value6 = value + 5; 
            Value7 = value + 6;
            Value8 = value + 7;
            Value9 = value + 8;
            Value10 = value + 9;
        }

        public void Serialize(CsvWriter writer)
        {
            writer.Write(Value1);
            writer.WriteSeparator();
            writer.Write(Value2);
            writer.WriteSeparator();
            writer.Write(Value3);
            writer.WriteSeparator();
            writer.Write(Value4);
            writer.WriteSeparator();
            writer.Write(Value5);
            writer.WriteSeparator();
            writer.Write(Value6);
            writer.WriteSeparator();
            writer.Write(Value7);
            writer.WriteSeparator();
            writer.Write(Value8);
            writer.WriteSeparator();
            writer.Write(Value9);
            writer.WriteSeparator();
            writer.Write(Value10);
        }

        public bool TryDeSerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            //reader.TryRead<string>(ref tokens, out string _);
            return reader.TryRead(ref tokens, out Value1) &&
                   reader.TryRead(ref tokens, out Value2) &&
                   reader.TryRead(ref tokens, out Value3) &&
                   reader.TryRead(ref tokens, out Value4) &&
                   reader.TryRead(ref tokens, out Value5) &&
                   reader.TryRead(ref tokens, out Value6) &&
                   reader.TryRead(ref tokens, out Value7) &&
                   reader.TryRead(ref tokens, out Value8) &&
                   reader.TryRead(ref tokens, out Value9) &&
                   reader.TryRead(ref tokens, out Value10);
        }
    }
}