using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _innerEnumerator;

    public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
    {
        _innerEnumerator = innerEnumerator;
    }

    public T Current => _innerEnumerator.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(Task.FromResult(_innerEnumerator.MoveNext()));
    }

    public ValueTask DisposeAsync()
    {
        _innerEnumerator.Dispose();
        return default;
    }
}
