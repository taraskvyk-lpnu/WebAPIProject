using System.Text;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using System.Text.Json;

namespace NZWalks.UI.Controllers;

public class RegionsController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public RegionsController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    // GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get all regions from WEB API
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7178/api/regions");

            response.EnsureSuccessStatusCode();

            var regions = await response.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();

            return View(regions);
            //ViewBag.Response = stringResponseBody;
        }
        catch (Exception e)
        {
            throw;
        }
        
        //return View();
    }

    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddRegionViewModel model)
    {
        try
        {
            var client = _clientFactory.CreateClient();
            HttpRequestMessage message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7178/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            
            var response = await client.SendAsync(message);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }
        }
        catch (Exception)
        {
            Console.WriteLine();
        }
        
        return View();
    }
    
    // GET
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        // Get region from WEB API
        var client = _clientFactory.CreateClient();
        var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7178/api/regions/{id}");

        if (response != null)
        {
            return View(response);
        }

        return RedirectToAction("Index", "Regions");
    }
    
    // GET
    [HttpPost]
    public async Task<IActionResult> Edit(RegionDto request)
    {
        // Get region from WEB API
        var client = _clientFactory.CreateClient();

        HttpRequestMessage httpRequest = new HttpRequestMessage()
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"https://localhost:7178/api/regions/{request.Id}"),
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };
        
        var httpResponse = await client.SendAsync(httpRequest);
        httpResponse.EnsureSuccessStatusCode();
        
        var response = await httpResponse.Content.ReadFromJsonAsync<RegionDto>();

        if (response != null)
        {
            return RedirectToAction("Index", "Regions");
            
        }
        
        return View();
    }
    
    // DELETE   
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            // Delete region from WEB API
            var client = _clientFactory.CreateClient();
            
            var httpResponse = await client.DeleteAsync($"https://localhost:7178/api/regions/{id}");
            httpResponse.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return RedirectToAction("Index", "Regions");
    }
}