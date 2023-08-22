using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oldtimer.Api.Tests
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;

        public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            _innerEnumerator = innerEnumerator;
        }

        public ValueTask DisposeAsync()
        {
            _innerEnumerator.Dispose();
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(Task.FromResult(_innerEnumerator.MoveNext()));
        }

        public T Current => _innerEnumerator.Current;
    }
}
