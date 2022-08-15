namespace Vroom.Providers.Contracts
{
    public interface IEncodingProvider
    {
        string UTF8GetString(byte[] bytes);
        byte[] UTF8GetBytes(string code);
    }
}
