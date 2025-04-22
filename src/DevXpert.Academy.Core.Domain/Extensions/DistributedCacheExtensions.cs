using JsonNet.ContractResolvers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace DevXpert.Academy.Core.Domain.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static TItem GetOrCreate<TItem>(this IDistributedCache cache, Func<TItem> factory, string key = "", int minutesExpiration = 0)
        {
            if (string.IsNullOrEmpty(key))
                key = GenerateKey(factory);

            var circuitBreaker = CacheCircuitBreakerPolicy<string>.CachePolicy;

            var policy = Policy<string>
                .Handle<Exception>()
                .Fallback(string.Empty)
                .Wrap(circuitBreaker);

            var result = policy.Execute(() =>
            {
                return cache.GetString(key);
            });

            if (!string.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<TItem>(result, JsonSettingsPrivateSetters);

            var obj = factory();
            if (obj == null) return default;

            if (circuitBreaker.CircuitState != CircuitState.Closed)
                return obj;

            policy.Execute(() =>
            {
                var value = JsonConvert.SerializeObject(obj);

                var opcoesCache = new DistributedCacheEntryOptions();

                if (minutesExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutesExpiration));

                cache.SetString(key, value, opcoesCache);

                return value;
            });

            return obj;
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(this IDistributedCache cache, Func<Task<TItem>> factory, string key = "", int minutesExpiration = 0)
        {
            if (string.IsNullOrEmpty(key))
                key = GenerateKey(factory);

            var circuitBreaker = CacheCircuitBreakerPolicy<string>.CachePolicyAsync;

            var policy = Policy<string>
                .Handle<Exception>()
                .FallbackAsync(string.Empty)
                .WrapAsync(circuitBreaker);

            var result = await policy.ExecuteAsync(() =>
            {
                return cache.GetStringAsync(key);
            });

            if (!string.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<TItem>(result, JsonSettingsPrivateSetters);

            var obj = await factory();
            if (obj == null) return default;

            if (circuitBreaker.CircuitState != CircuitState.Closed)
                return obj;

            await policy.ExecuteAsync(async () =>
            {
                var value = JsonConvert.SerializeObject(obj);

                var opcoesCache = new DistributedCacheEntryOptions();

                if (minutesExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutesExpiration));

                await cache.SetStringAsync(key, value, opcoesCache);

                return value;
            });

            return obj;
        }

        public static async Task<TItem> GetOrCreateCompressedAsync<TItem>(this IDistributedCache cache, Func<Task<TItem>> factory, string key = "", int minutesExpiration = 0)
        {
            if (string.IsNullOrEmpty(key))
                key = GenerateKey(factory);

            var circuitBreaker = CacheCircuitBreakerPolicy<byte[]>.CachePolicyAsync;

            var policy = Policy<byte[]>
                .Handle<Exception>()
                .FallbackAsync([])
                .WrapAsync(circuitBreaker);

            var result = await policy.ExecuteAsync(() =>
            {
                return cache.GetAsync(key);
            });

            if (result?.LongLength > 0)
            {
                try
                {
                    return JsonConvert.DeserializeObject<TItem>(DecompressJson(result), JsonSettingsPrivateSetters);
                }
                catch // O catch tenta extrair os dados do result considerando que ele ja é um json descomprimido por conta de caches gerados anteriormente
                {
                    return JsonConvert.DeserializeObject<TItem>(Encoding.UTF8.GetString(result), JsonSettingsPrivateSetters);
                }
            }

            var obj = await factory();
            if (obj == null) return default;

            if (circuitBreaker.CircuitState != CircuitState.Closed)
                return obj;

            await policy.ExecuteAsync(async () =>
            {
                var value = JsonConvert.SerializeObject(obj);

                var opcoesCache = new DistributedCacheEntryOptions();

                if (minutesExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutesExpiration));

                byte[] compressedValue = CompressJson(value);

                await cache.SetAsync(key, compressedValue, opcoesCache);

                return compressedValue;
            });

            return obj;
        }

        public static bool CreateOrUpdate<TItem>(this IDistributedCache cache, string key, TItem obj, int minutesExpiration = 0, int secondsExpiration = 0)
        {
            var policy = CacheCircuitBreakerPolicy<string>.CachePolicy;

            var result = policy.ExecuteAndCapture(() =>
            {
                cache.Remove(key);

                var value = JsonConvert.SerializeObject(obj);
                var opcoesCache = new DistributedCacheEntryOptions();

                if (minutesExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutesExpiration));
                else if (secondsExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromSeconds(secondsExpiration));

                cache.SetString(key, value, opcoesCache);

                return value;
            });

            return OutcomeType.Successful.Equals(result.Outcome);
        }

        public static async Task<bool> CreateOrUpdateAsync<TItem>(this IDistributedCache cache, string key, TItem obj, int minutesExpiration = 0, int secondsExpiration = 0)
        {
            var policy = CacheCircuitBreakerPolicy<string>.CachePolicyAsync;

            var result = await policy.ExecuteAndCaptureAsync(async () =>
            {
                await cache.RemoveAsync(key);

                var value = JsonConvert.SerializeObject(obj);
                var opcoesCache = new DistributedCacheEntryOptions();

                if (minutesExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromMinutes(minutesExpiration));
                else if (secondsExpiration > 0)
                    opcoesCache.SetAbsoluteExpiration(TimeSpan.FromSeconds(secondsExpiration));

                await cache.SetStringAsync(key, value, opcoesCache);

                return value;
            });

            return OutcomeType.Successful.Equals(result.Outcome);
        }

        public static TItem Get<TItem>(this IDistributedCache cache, string key, Func<TItem> factory)
        {
            var circuitBreaker = CacheCircuitBreakerPolicy<string>.CachePolicy;

            var policy = Policy<string>
                .Handle<Exception>()
                .Fallback(string.Empty)
                .Wrap(circuitBreaker);

            var result = policy.Execute(() =>
            {
                return cache.GetString(key);
            });

            return !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<TItem>(result, JsonSettingsPrivateSetters) : factory();
        }

        public static async Task<TItem> GetAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> factory)
        {
            var circuitBreaker = CacheCircuitBreakerPolicy<string>.CachePolicyAsync;

            var policy = Policy<string>
                .Handle<Exception>()
                .FallbackAsync(string.Empty)
                .WrapAsync(circuitBreaker);

            var result = await policy.ExecuteAsync(async () =>
            {
                return await cache.GetStringAsync(key);
            });

            return !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<TItem>(result, JsonSettingsPrivateSetters) : await factory();
        }

        private static readonly JsonSerializerSettings JsonSettingsPrivateSetters = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };

        private static string GenerateKey<TItem>(Func<TItem> factory) => $"{typeof(TItem).FullName}.{factory.Method.Name}";

        private static byte[] CompressJson(string json)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            using var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                gzipStream.Write(jsonBytes, 0, jsonBytes.Length);
            }
            return outputStream.ToArray();
        }
        private static string DecompressJson(byte[] compressedJson)
        {
            using var inputStream = new MemoryStream(compressedJson);
            using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
            using var outputStream = new MemoryStream();
            gzipStream.CopyTo(outputStream);
            return Encoding.UTF8.GetString(outputStream.ToArray());
        }
    }

    public static class CacheCircuitBreakerPolicy<T>
    {
        public static CircuitBreakerPolicy<T> CachePolicy =
            Policy<T>
                .Handle<Exception>((e) =>
                {
                    if (e?.Message?.Contains("'max_allowed_packet'") ?? false)
                        return false;
                    return true;
                })
                .CircuitBreaker(1, TimeSpan.FromMinutes(5));

        public static AsyncCircuitBreakerPolicy<T> CachePolicyAsync =
            Policy<T>
                .Handle<Exception>((e) =>
                {
                    if (e?.Message?.Contains("'max_allowed_packet'") ?? false)
                        return false;
                    return true;
                })
                .CircuitBreakerAsync(1, TimeSpan.FromMinutes(5));
    }
}
