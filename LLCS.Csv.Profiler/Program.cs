using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace MyBenchmarks
{

    //DOTNET_ReadyToRun=0,DOTNET_TC_QuickJitForLoops=1,DOTNET_TieredPGO=1
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly, DefaultConfig.Instance.WithOptions(ConfigOptions.JoinSummary));
        }
    }
}