using f1_api.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace f1_api.Services;

public interface IF1ApiService
{
    Task<List<Driver>> GetAllDriversAsync(int limit = 1000, int offset = 0);
    Task<List<Driver>> GetDriversByYearAsync(int year);
    Task<Driver?> GetDriverByIdAsync(string driverId);
}

public class F1ApiService : IF1ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<F1ApiService> _logger;
    private readonly IMemoryCache _cache;
    private const string BaseUrl = "https://api.jolpi.ca/ergast/f1";
    
    // Cache durations
    private static readonly TimeSpan HistoricalDataCacheDuration = TimeSpan.FromHours(24); // Historical data rarely changes
    private static readonly TimeSpan CurrentSeasonCacheDuration = TimeSpan.FromHours(1); // Current season data changes more frequently
    private static readonly TimeSpan SingleDriverCacheDuration = TimeSpan.FromHours(12); // Single driver data

    public F1ApiService(HttpClient httpClient, ILogger<F1ApiService> logger, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<Driver>> GetAllDriversAsync(int limit = 1000, int offset = 0)
    {
        var cacheKey = $"drivers_all_{limit}_{offset}";
        
        if (_cache.TryGetValue(cacheKey, out List<Driver>? cachedDrivers))
        {
            _logger.LogDebug("Returning cached drivers (all)");
            return cachedDrivers ?? new List<Driver>();
        }

        try
        {
            var url = $"{BaseUrl}/drivers.json?limit={limit}&offset={offset}";
            var response = await _httpClient.GetFromJsonAsync<DriversResponse>(url);
            
            var drivers = response?.MRData?.DriverTable?.Drivers ?? new List<Driver>();
            
            // Cache historical data for 24 hours (all drivers is historical data)
            _cache.Set(cacheKey, drivers, HistoricalDataCacheDuration);
            _logger.LogDebug("Cached drivers (all) for {Duration} hours", HistoricalDataCacheDuration.TotalHours);
            
            return drivers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all drivers");
            throw;
        }
    }

    public async Task<List<Driver>> GetDriversByYearAsync(int year)
    {
        var cacheKey = $"drivers_year_{year}";
        
        if (_cache.TryGetValue(cacheKey, out List<Driver>? cachedDrivers))
        {
            _logger.LogDebug("Returning cached drivers for year {Year}", year);
            return cachedDrivers ?? new List<Driver>();
        }

        try
        {
            var url = $"{BaseUrl}/{year}/drivers.json";
            var response = await _httpClient.GetFromJsonAsync<DriversResponse>(url);
            
            var drivers = response?.MRData?.DriverTable?.Drivers ?? new List<Driver>();
            
            // Use shorter cache for current year, longer for historical years
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            
            _cache.Set(cacheKey, drivers, cacheDuration);
            _logger.LogDebug("Cached drivers for year {Year} for {Duration} hours", year, cacheDuration.TotalHours);
            
            return drivers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching drivers for year {Year}", year);
            throw;
        }
    }

    public async Task<Driver?> GetDriverByIdAsync(string driverId)
    {
        var cacheKey = $"driver_{driverId.ToLowerInvariant()}";
        
        if (_cache.TryGetValue(cacheKey, out Driver? cachedDriver))
        {
            _logger.LogDebug("Returning cached driver {DriverId}", driverId);
            return cachedDriver;
        }

        try
        {
            var url = $"{BaseUrl}/drivers/{driverId}.json";
            var response = await _httpClient.GetFromJsonAsync<DriversResponse>(url);
            
            var driver = response?.MRData?.DriverTable?.Drivers?.FirstOrDefault();
            
            if (driver != null)
            {
                // Cache single driver data for 12 hours
                _cache.Set(cacheKey, driver, SingleDriverCacheDuration);
                _logger.LogDebug("Cached driver {DriverId} for {Duration} hours", driverId, SingleDriverCacheDuration.TotalHours);
            }
            
            return driver;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching driver {DriverId}", driverId);
            throw;
        }
    }
}
