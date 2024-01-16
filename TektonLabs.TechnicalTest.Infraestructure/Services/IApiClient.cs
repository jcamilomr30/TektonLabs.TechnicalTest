using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Infraestructure
{
    public interface IApiClient
    {
        void SetAuthentication(string token);
        void SetHeader(string key, string value);
        Task<T> GetAsync<T>(string subPath);
        Task<T> PostAsync<T>(string subPath, object requestDto);
        Task PostAsync(string subPath, object requestDto);
        Task<T> PutAsync<T>(string subPath, object requestDto);
        Task PutAsync(string subPath, object requestDto);
        Task<T> DeleteAsync<T>(string subPath);
        Task DeleteAsync(string subPath);
    }
}