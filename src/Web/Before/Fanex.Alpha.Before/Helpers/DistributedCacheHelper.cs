using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Polly;
using Polly.Simmy;
using Polly.Simmy.Latency;
using Polly.Timeout;
using Polly.Fallback;

namespace Fanex.Alpha.Before.Helpers;

public interface IDistributedCacheHelper
{
    Task<bool> Contain(string key);

    Task<T> Get<T>(string key, T defaultValue);

    Task Set<T>(string key, T value);

    Task Set<T>(string key, T value, DistributedCacheEntryOptions options);

    Task Remove(string key);
}

public class DistributedCacheHelper : IDistributedCacheHelper
{
    private readonly IDistributedCache _cache;
    private readonly Logging.ILogger _logger;
    private readonly bool _enabledResilience = true; // Hardcode control variable
    private readonly int _chaosLatencySeconds = 5; // Hardcode chaos latency duration
    private readonly ResiliencePipeline<bool> _containPipeline;
    private readonly ResiliencePipeline _voidPipeline;

    public DistributedCacheHelper(IDistributedCache cache, Logging.ILogger logger)
    {
        _cache = cache;
        _logger = logger;
        
        // Configure pipeline for Contain method (returns bool)
        var containBuilder = new ResiliencePipelineBuilder<bool>();
        
        if (_enabledResilience)
        {
            containBuilder
                .AddFallback(new FallbackStrategyOptions<bool>
                {
                    ShouldHandle = new PredicateBuilder<bool>().Handle<Exception>(),
                    FallbackAction = async args =>
                    {
                        await _logger.WarnAsync($"Fallback triggered for Contain: {args.Outcome.Exception?.Message}");
                        return Outcome.FromResult(false);
                    }
                })
                .AddTimeout(TimeSpan.FromSeconds(1))
                .AddChaosLatency(new ChaosLatencyStrategyOptions
                {
                    Latency = TimeSpan.FromSeconds(_chaosLatencySeconds),
                    InjectionRate = 1.0,
                    Enabled = true
                });
        }
        
        _containPipeline = containBuilder.Build();

        // Configure pipeline for void methods (Set/Remove)
        var voidBuilder = new ResiliencePipelineBuilder();
        
        if (_enabledResilience)
        {
            voidBuilder
                .AddTimeout(TimeSpan.FromSeconds(1))
                .AddChaosLatency(new ChaosLatencyStrategyOptions
                {
                    Latency = TimeSpan.FromSeconds(_chaosLatencySeconds),
                    InjectionRate = 1.0,
                    Enabled = true
                });
        }
        
        _voidPipeline = voidBuilder.Build();
    }

    private ResiliencePipeline<T> CreateGetPipeline<T>(T defaultValue)
    {
        var getBuilder = new ResiliencePipelineBuilder<T>();
        
        if (_enabledResilience)
        {
            getBuilder
                .AddFallback(new FallbackStrategyOptions<T>
                {
                    ShouldHandle = new PredicateBuilder<T>().Handle<Exception>(),
                    FallbackAction = async args =>
                    {
                        await _logger.WarnAsync($"Fallback triggered for Get<{typeof(T).Name}>: {args.Outcome.Exception?.Message}");
                        return Outcome.FromResult(defaultValue);
                    }
                })
                .AddTimeout(TimeSpan.FromSeconds(1))
                .AddChaosLatency(new ChaosLatencyStrategyOptions
                {
                    Latency = TimeSpan.FromSeconds(_chaosLatencySeconds),
                    InjectionRate = 1.0,
                    Enabled = true
                });
        }
        
        return getBuilder.Build();
    }

    public async Task<bool> Contain(string key)
    {
        return await _containPipeline.ExecuteAsync(async cancellationToken =>
        {
            var encodedCached = await _cache.GetAsync(key, cancellationToken);
            return encodedCached != null;
        });
    }

    public async Task<T> Get<T>(string key, T defaultValue)
    {
        var getPipeline = CreateGetPipeline(defaultValue);
        
        return await getPipeline.ExecuteAsync(async cancellationToken =>
        {
            var encodedCached = await _cache.GetAsync(key, cancellationToken);
            return encodedCached == null
                ? defaultValue
                : JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(encodedCached));
        });
    }

    public async Task Set<T>(string key, T value)
    {
        try
        {
            await _voidPipeline.ExecuteAsync(async _ =>
            {
                await Set(key, value, TimeSpan.FromDays(1));
            });
        }
        catch (Exception ex)
        {
            await _logger.WarnAsync($"Set<{typeof(T).Name}> failed: {ex.Message}");
        }
    }

    private async Task Set<T>(string key, T value, TimeSpan timeSpan)
    {
        try
        {
            await _voidPipeline.ExecuteAsync(async _ =>
            {
                await Set(key, value, new DistributedCacheEntryOptions().SetSlidingExpiration(timeSpan));
            });
        }
        catch (Exception ex)
        {
            await _logger.WarnAsync($"Set<{typeof(T).Name}> with TimeSpan failed: {ex.Message}");
        }
    }

    public async Task Set<T>(string key, T value, DistributedCacheEntryOptions options)
    {
        try
        {
            await _voidPipeline.ExecuteAsync(async cancellationToken =>
            {
                byte[] encodedValue = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(value));
                await _cache.SetAsync(key, encodedValue, options, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            await _logger.WarnAsync($"Set<{typeof(T).Name}> with options failed: {ex.Message}");
        }
    }

    public async Task Remove(string key)
    {
        try
        {
            await _voidPipeline.ExecuteAsync(async cancellationToken =>
            {
                await _cache.RemoveAsync(key, cancellationToken);
            });
        }
        catch (Exception ex)
        {
            await _logger.WarnAsync($"Remove failed: {ex.Message}");
        }
    }
}