using BenchmarkDotNet.Attributes;
using Microsoft.Toolkit.HighPerformance.Enumerables;
using LLCS.Csv;

namespace MyBenchmarks
{
    public class CsvIntProfiler
    {
        private readonly string FilePath;
        private CsvReader<Csv10IntsRecord> _reader;

        public CsvIntProfiler()
        {
            FilePath = "./csv_ints_profile_file.csv";
            if (!File.Exists(FilePath))
            {
                File.WriteAllLines(FilePath, Enumerable.Range(0, 1_000_000_000).Select(x => string.Join(";", Enumerable.Range(x, 10))));
            }
        }

        [IterationSetup]
        public void CreateCsvReader()
        {
            _reader = CsvReader<Csv10IntsRecord>.FromFile(FilePath);
        }

        [Benchmark]
        public int ReadRecords()
        {
            int count = 0;
            foreach (var record in _reader)
            {
                count++;
            }
            Console.WriteLine(count);

            return count;
        }

        [IterationCleanup]
        public void DisposeCsvReader()
        {
            _reader.Dispose();
        }
    }

    //DOTNET_ReadyToRun=0,DOTNET_TC_QuickJitForLoops=1,DOTNET_TieredPGO=1
    public class Program
    {
        public static void Main(string[] args)
        {
            var FilePath = @"Q:\Github\LLCS.Csv\LLCS.Csv.Profiler\bin\Release\net6.0\bf0b42a8-fe2f-4b8c-b313-0d258a9fb237\bin\Release\net6.0\csv_ints_profile_file.csv";
            using var _reader = CsvReader<Csv10IntsRecord>.FromFile(FilePath);


            int count = 0;
            using CancellationTokenSource cancelSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                int previousCount = 0;
                using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
                while (!cancelSource.IsCancellationRequested && await timer.WaitForNextTickAsync(cancelSource.Token))
                {
                    int change = count - previousCount;
                    Console.WriteLine(change.ToString("N0"));

                    previousCount = count;
                }
            });




            foreach (var record in _reader)
            {
                count += 10;
            }
            cancelSource.Cancel();
            Console.WriteLine(count);

            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

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

        public bool TrySerialize(CsvReader reader, ref ReadOnlySpanTokenizer<char> tokens)
        {
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value1 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value2 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value3 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value4 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value5 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value6 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value7 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value8 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value9 = value;
            }
            {
                if (!reader.TryRead(ref tokens, out int value))
                {
                    return false;
                }
                Value10 = value;
            }

            return true;
        }
    }
}