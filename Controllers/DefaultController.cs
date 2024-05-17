using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class DefaultController(HttpClient httpClient) : Controller
{
    private readonly HttpClient _httpClient = httpClient;

    public IActionResult Home()
    {
        
        return View();
    }


    public async Task<IActionResult> Subscribe(SubscribeViewModel model)
    {
        if (ModelState.IsValid)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,"application/json");
            var respons = await _httpClient.PostAsync("https://localhost:7229/api/Subscribe", content);
            if (respons.IsSuccessStatusCode)
            {
                TempData["StatusMessage"] = "you are now subscribed";
            }
            else if (respons.StatusCode == System.Net.HttpStatusCode.Conflict) 
            {
                TempData["StatusMessage"] = "you are already subscribed";
            }
           
        }
        else
        {
            TempData["StatusMessage"] = "Invalid email address";
        }
        return RedirectToAction("Home", "Default", "subscribe");
    }
}
