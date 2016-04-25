using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace Restup.HttpMessage.UnitTests.TestHelpers
{
    public class TestStream : IInputStream
    {
        private int indexCounter = 0;
        private IEnumerable<byte[]> _byteStreamParts;

        public TestStream(IEnumerable<byte[]> byteStreamParts)
        {
            _byteStreamParts = byteStreamParts;
        }

        public void Dispose() { }

        public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            return AsyncInfo.Run<IBuffer, uint>((token, progress) =>
            {
                IBuffer buff = new byte[0].AsBuffer();
                if (indexCounter < _byteStreamParts.Count())
                {
                    buff = _byteStreamParts.ElementAt(indexCounter).AsBuffer();
                }

                indexCounter++;
                return Task.FromResult(buff);
            });
        }
    }
}
