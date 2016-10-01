namespace Restup.HttpMessage.Models.Schemas
{
    internal struct StreamReadResult
    {
        internal byte[] Data { get; }
        internal bool ReadSuccessful { get; }

        internal StreamReadResult(byte[] data, bool successful)
        {
            Data = data;
            ReadSuccessful = successful;
        }
    }
}
