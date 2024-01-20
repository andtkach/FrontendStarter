using System.Text.Json;
using BloomBFF.Extensions;
using BFF.Data;
using BloomBFF.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;

namespace BFF.Services;

public class BaseService
{
    protected readonly HttpClient Client;
    protected readonly ICurrentUserService CurrentUserService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BaseService> _logger;
    
    protected BaseService(HttpClient client, ICurrentUserService currentUserService, IConfiguration configuration, ILogger<BaseService> logger)
    {
        Client = client;
        CurrentUserService = currentUserService;
        _configuration = configuration;
        _logger = logger;
    }

    protected void AddRequestHeaders()
    {
        //Client.DefaultRequestHeaders.Add(Constants.HttpClientId, _configuration.GetValue<string>(Constants.ConfigClientId));
    }

    protected void AddRequestAuth(string token)
    {
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    protected async Task VerifyResponse(HttpResponseMessage resultMessage)
    {
        try
        {
            resultMessage.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            var response = await resultMessage.Content.ReadAsStringAsync();
            _logger.LogError("Error in VerifyResponse {E}. Response: {Response}", e, response);
            
            if (response.IsJson())
            {
                var err = JsonSerializer.Deserialize<ErrorDto>(response);
                throw new Exception($"Message: {err.Message}. Error: {e.Message}");
            }

            throw new Exception($"Error: {e.Message}. Http result: {response}");
        }
    }
    
    protected void VerifyResult(object result, string message)
    {
        if (result == null)
        {
            _logger.LogError("Error in VerifyResult for {message}", message);
            throw new Exception(message);
        }
    }
}