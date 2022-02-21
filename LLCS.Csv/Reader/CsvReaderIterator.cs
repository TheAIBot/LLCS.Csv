using System.Collections;

namespace LLCS.Csv.Reader
{
    public struct CsvReaderIterator<U> : IEnumerator<U>
    where U : ICsvSerializer, new()
    {
        private readonly CsvReader _reader;
        public U Current { get; private set; }
        object IEnumerator.Current => Current;

        public CsvReaderIterator(CsvReader reader)
        {
            _reader = reader;
            Current = default(U)!;
        }

        public bool MoveNext()
        {
            if (!_reader.CanReadMore)
            {
                return false;
            }

            U record;
            bool parsed = _reader.TryReadRecord(out record);

            Current = record;
            return parsed;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
        }
    }
}
