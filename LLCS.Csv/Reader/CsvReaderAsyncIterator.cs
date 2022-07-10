using System.Threading;

namespace LLCS.Csv.Reader
{
    public struct CsvReaderAsyncIterator<U> : IAsyncEnumerator<U>
    where U : ICsvSerializer, new()
    {
        private readonly CsvReader _reader;
        private readonly CancellationToken _cancellationToken;
        public U Current { get; private set; }

        public CsvReaderAsyncIterator(CsvReader reader, CancellationToken cancellationToken)
        {
            _reader = reader;
            _cancellationToken = cancellationToken;
            Current = default(U)!;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (!_reader.CanReadMore)
            {
                return false;
            }

            (Current, bool parsed) = await _reader.TryReadRecordAsync<U>(_cancellationToken);

            return parsed;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
