using LLCS.Csv.Tests.GenericContainers;
using LLCS.Csv.Writer;
using System.IO;
using System.Text;
using Xunit;

namespace LLCS.Csv.Tests.Writer
{
    public abstract class WriteNumber<T>
    {
        [Fact]
        public void TestWriteOneNumber()
        {
            using MemoryStream stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            using var writer = new CsvWriter<OneValueStruct<T>>(streamWriter, "da-DK");

            writer.WriteRecord(new OneValueStruct<T>() { Value = CastHelper<T>.CastTo(1) });

            writer.Flush();
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            string actualCsv = Encoding.UTF8.GetString(stream.ToArray());
            Assert.Equal("1", actualCsv);
        }
    }
}
