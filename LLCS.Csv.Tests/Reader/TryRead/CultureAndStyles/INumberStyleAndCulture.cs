using System.Globalization;

namespace LLCS.Csv.Tests.Reader.TryRead.CultureAndStyles;

internal interface INumberStyleAndCulture
{
    NumberStyles NumberStyle { get; }
    CultureInfo? CultureInfo { get; }
}
