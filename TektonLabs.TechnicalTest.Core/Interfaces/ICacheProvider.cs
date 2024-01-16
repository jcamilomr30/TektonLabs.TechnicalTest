namespace TektonLabs.TechnicalTest.Core.Interfaces
{
    public interface ICacheProvider
    {
        bool TryGetValue<TItem>(string key, out TItem value);
        TItem Set<TItem>(string key, TItem value, int expiration);
    }
}
