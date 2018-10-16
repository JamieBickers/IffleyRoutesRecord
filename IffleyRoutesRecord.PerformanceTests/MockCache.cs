using Microsoft.Extensions.Caching.Memory;

namespace IffleyRoutesRecord.PerformanceTests
{
    class MockCache : IMemoryCache
    {
        public ICacheEntry CreateEntry(object key)
        {
            return null;
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            
        }

        public void Remove(object key)
        {
            
        }

        public bool TryGetValue(object key, out object value)
        {
            value = null;
            return false;
        }
    }
}
