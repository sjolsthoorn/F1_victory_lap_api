using f1_api.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace f1_api.Services;

public interface IF1ApiService
{
    Task<List<Driver>> GetAllDriversAsync(int limit = 1000, int offset = 0);
    Task<List<Driver>> GetDriversByYearAsync(int year);
    Task<Driver?> GetDriverByIdAsync(string driverId);
    
    // Seasons
    Task<List<Season>> GetAllSeasonsAsync(int limit = 1000, int offset = 0);
    
    // Races
    Task<List<Race>> GetRacesByYearAsync(int year, int limit = 1000, int offset = 0);
    Task<Race?> GetRaceByYearAndRoundAsync(int year, int round);
    
    // Results
    Task<List<Result>> GetResultsByYearAsync(int year, int limit = 1000, int offset = 0);
    Task<List<Result>> GetResultsByYearAndRoundAsync(int year, int round);
    
    // Driver Standings
    Task<List<DriverStanding>> GetDriverStandingsByYearAsync(int year, int? round = null);
    
    // Constructor Standings
    Task<List<ConstructorStanding>> GetConstructorStandingsByYearAsync(int year, int? round = null);
    
    // Status
    Task<List<Status>> GetAllStatusAsync(int limit = 1000, int offset = 0);
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
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = HistoricalDataCacheDuration;
            _logger.LogDebug("Fetching drivers (all) from API - not in cache");
            
            try
            {
                var url = $"{BaseUrl}/drivers.json?limit={limit}&offset={offset}";
                var response = await _httpClient.GetFromJsonAsync<DriversResponse>(url);
                
                var drivers = response?.MRData?.DriverTable?.Drivers ?? new List<Driver>();
                _logger.LogDebug("Cached drivers (all) for {Duration} hours", HistoricalDataCacheDuration.TotalHours);
                
                return drivers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all drivers");
                throw;
            }
        }) ?? new List<Driver>();
    }

    public async Task<List<Driver>> GetDriversByYearAsync(int year)
    {
        var cacheKey = $"drivers_year_{year}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching drivers for year {Year} from API - not in cache", year);
            
            try
            {
                var url = $"{BaseUrl}/{year}/drivers.json";
                var response = await _httpClient.GetFromJsonAsync<DriversResponse>(url);
                
                var drivers = response?.MRData?.DriverTable?.Drivers ?? new List<Driver>();
                _logger.LogDebug("Cached drivers for year {Year} for {Duration} hours", year, cacheDuration.TotalHours);
                
                return drivers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching drivers for year {Year}", year);
                throw;
            }
        }) ?? new List<Driver>();
    }

    public async Task<Driver?> GetDriverByIdAsync(string driverId)
    {
        var cacheKey = $"driver_{driverId.ToLowerInvariant()}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = SingleDriverCacheDuration;
            _logger.LogDebug("Fetching driver {DriverId} from API - not in cache", driverId);
            
            try
            {
                var url = $"{BaseUrl}/drivers/{driverId}.json";
                var response = await _httpClient.GetFromJsonAsync<DriversResponse>(url);
                
                var driver = response?.MRData?.DriverTable?.Drivers?.FirstOrDefault();
                _logger.LogDebug("Cached driver {DriverId} for {Duration} hours", driverId, SingleDriverCacheDuration.TotalHours);
                
                return driver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching driver {DriverId}", driverId);
                throw;
            }
        });
    }

    public async Task<List<Season>> GetAllSeasonsAsync(int limit = 1000, int offset = 0)
    {
        var cacheKey = $"seasons_all_{limit}_{offset}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = HistoricalDataCacheDuration;
            _logger.LogDebug("Fetching seasons (all) from API - not in cache");
            
            try
            {
                var url = $"{BaseUrl}/seasons.json?limit={limit}&offset={offset}";
                var response = await _httpClient.GetFromJsonAsync<SeasonsResponse>(url);
                
                var seasons = response?.MRData?.SeasonTable?.Seasons ?? new List<Season>();
                _logger.LogDebug("Cached seasons (all) for {Duration} hours", HistoricalDataCacheDuration.TotalHours);
                
                return seasons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all seasons");
                throw;
            }
        }) ?? new List<Season>();
    }

    public async Task<List<Race>> GetRacesByYearAsync(int year, int limit = 1000, int offset = 0)
    {
        var cacheKey = $"races_year_{year}_{limit}_{offset}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching races for year {Year} from API - not in cache", year);
            
            try
            {
                var url = $"{BaseUrl}/{year}/races.json?limit={limit}&offset={offset}";
                var response = await _httpClient.GetFromJsonAsync<RacesResponse>(url);
                
                var races = response?.MRData?.RaceTable?.Races ?? new List<Race>();
                _logger.LogDebug("Cached races for year {Year} for {Duration} hours", year, cacheDuration.TotalHours);
                
                return races;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching races for year {Year}", year);
                throw;
            }
        }) ?? new List<Race>();
    }

    public async Task<Race?> GetRaceByYearAndRoundAsync(int year, int round)
    {
        var cacheKey = $"race_{year}_{round}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching race {Year}/{Round} from API - not in cache", year, round);
            
            try
            {
                var url = $"{BaseUrl}/{year}/{round}/races.json";
                var response = await _httpClient.GetFromJsonAsync<RacesResponse>(url);
                
                var race = response?.MRData?.RaceTable?.Races?.FirstOrDefault();
                _logger.LogDebug("Cached race {Year}/{Round} for {Duration} hours", year, round, cacheDuration.TotalHours);
                
                return race;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching race {Year}/{Round}", year, round);
                throw;
            }
        });
    }

    public async Task<List<Result>> GetResultsByYearAsync(int year, int limit = 1000, int offset = 0)
    {
        var cacheKey = $"results_year_{year}_{limit}_{offset}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching results for year {Year} from API - not in cache", year);
            
            try
            {
                var url = $"{BaseUrl}/{year}/results.json?limit={limit}&offset={offset}";
                var response = await _httpClient.GetFromJsonAsync<ResultsResponse>(url);
                
                // Results are nested within races in the API response
                var allResults = response?.MRData?.RaceTable?.Races
                    ?.SelectMany(r => r.Results ?? new List<Result>())
                    .ToList() ?? new List<Result>();
                
                _logger.LogDebug("Cached results for year {Year} for {Duration} hours", year, cacheDuration.TotalHours);
                
                return allResults;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching results for year {Year}", year);
                throw;
            }
        }) ?? new List<Result>();
    }

    public async Task<List<Result>> GetResultsByYearAndRoundAsync(int year, int round)
    {
        var cacheKey = $"results_{year}_{round}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching results for {Year}/{Round} from API - not in cache", year, round);
            
            try
            {
                var url = $"{BaseUrl}/{year}/{round}/results.json";
                var response = await _httpClient.GetFromJsonAsync<ResultsResponse>(url);
                
                var results = response?.MRData?.RaceTable?.Races?.FirstOrDefault()?.Results ?? new List<Result>();
                _logger.LogDebug("Cached results for {Year}/{Round} for {Duration} hours", year, round, cacheDuration.TotalHours);
                
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching results for {Year}/{Round}", year, round);
                throw;
            }
        }) ?? new List<Result>();
    }

    public async Task<List<DriverStanding>> GetDriverStandingsByYearAsync(int year, int? round = null)
    {
        var cacheKey = $"driver_standings_{year}_{round ?? 0}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching driver standings for {Year}/{Round} from API - not in cache", year, round);
            
            try
            {
                var url = round.HasValue 
                    ? $"{BaseUrl}/{year}/{round}/driverStandings.json"
                    : $"{BaseUrl}/{year}/driverStandings.json";
                
                var response = await _httpClient.GetFromJsonAsync<DriverStandingsResponse>(url);
                
                var standings = response?.MRData?.StandingsTable?.StandingsLists?.FirstOrDefault()?.DriverStandings 
                    ?? new List<DriverStanding>();
                
                _logger.LogDebug("Cached driver standings for {Year}/{Round} for {Duration} hours", year, round, cacheDuration.TotalHours);
                
                return standings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching driver standings for {Year}/{Round}", year, round);
                throw;
            }
        }) ?? new List<DriverStanding>();
    }

    public async Task<List<ConstructorStanding>> GetConstructorStandingsByYearAsync(int year, int? round = null)
    {
        var cacheKey = $"constructor_standings_{year}_{round ?? 0}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            var currentYear = DateTime.Now.Year;
            var cacheDuration = year >= currentYear ? CurrentSeasonCacheDuration : HistoricalDataCacheDuration;
            entry.AbsoluteExpirationRelativeToNow = cacheDuration;
            _logger.LogDebug("Fetching constructor standings for {Year}/{Round} from API - not in cache", year, round);
            
            try
            {
                var url = round.HasValue 
                    ? $"{BaseUrl}/{year}/{round}/constructorStandings.json"
                    : $"{BaseUrl}/{year}/constructorStandings.json";
                
                var response = await _httpClient.GetFromJsonAsync<ConstructorStandingsResponse>(url);
                
                var standings = response?.MRData?.StandingsTable?.StandingsLists?.FirstOrDefault()?.ConstructorStandings 
                    ?? new List<ConstructorStanding>();
                
                _logger.LogDebug("Cached constructor standings for {Year}/{Round} for {Duration} hours", year, round, cacheDuration.TotalHours);
                
                return standings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching constructor standings for {Year}/{Round}", year, round);
                throw;
            }
        }) ?? new List<ConstructorStanding>();
    }

    public async Task<List<Status>> GetAllStatusAsync(int limit = 1000, int offset = 0)
    {
        var cacheKey = $"status_all_{limit}_{offset}";
        
        return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = HistoricalDataCacheDuration;
            _logger.LogDebug("Fetching status (all) from API - not in cache");
            
            try
            {
                var url = $"{BaseUrl}/status.json?limit={limit}&offset={offset}";
                var response = await _httpClient.GetFromJsonAsync<StatusResponse>(url);
                
                var statuses = response?.MRData?.StatusTable?.Status ?? new List<Status>();
                _logger.LogDebug("Cached status (all) for {Duration} hours", HistoricalDataCacheDuration.TotalHours);
                
                return statuses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all status");
                throw;
            }
        }) ?? new List<Status>();
    }
}
