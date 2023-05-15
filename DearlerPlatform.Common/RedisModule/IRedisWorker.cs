using DearlerPlatform.Domain;
using StackExchange.Redis;
using System.Reflection;

namespace DearlerPlatform.Common.RedisModule
{
    public interface IRedisWorker:IocTag
    {
         void SetString(string key, string value, TimeSpan ts);

         Task SetStringAsync(string key, string value, TimeSpan ts);

         string GetString(string key);

          Task<string> GetStringAsync(string key);

        void SetHashMemory(string key, Dictionary<string, string> values);

        void SetHashMemory<T>(string key, T entity, Type type = null);

        void SetHashMemory<T>(string key, IEnumerable<T> entities, Func<T, IEnumerable<string>> func);

        T GetOneHashMemory<T>(string key) where T : new();

        List<T> GetHashMemory<T>(string keyLike) where T : new();

        void RemoveKey(string key);
    }
}