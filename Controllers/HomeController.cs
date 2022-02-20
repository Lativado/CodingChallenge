using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CodingChallenge.Models;
using System.Reflection;

namespace CodingChallenge.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly String baseUrl = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" ? "http://codingchallengeapi:5196/" : "localhost:5196/";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var notificationViewModel = new NotificationViewModel();
        NotificationViewModel.Supervisors = NotificationViewModel.Supervisors ?? PopulateSupervisors();

        return View(notificationViewModel);
    }

    public ActionResult NotificationFormSubmit(NotificationViewModel? notificationViewModel = null)
    {
        notificationViewModel = notificationViewModel ?? new NotificationViewModel();
        if (!ModelState.IsValid)
        {
            return View("Index", notificationViewModel);
        }

        PostForm(notificationViewModel);

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private List<SelectListItem> PopulateSupervisors()
    {
        try
        {
            return RequestSupervisors().Result;
        }
        catch (Exception)
        {
            //Just move on with an empty list of supervisors.
            return new List<SelectListItem>();
        }
    }

    private async Task<List<SelectListItem>> RequestSupervisors()
    {
        var returnItems = new List<SelectListItem>();

        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(baseUrl + "api/Supervisors");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            responseBody = responseBody.Trim('[').Trim(']');
            int countId = 1;

            foreach (var item in responseBody.Split("\",\""))
            {
                returnItems.Add(new SelectListItem(item.Trim('"'), countId.ToString()));
                countId++;
            }
        }

        return returnItems;
    }

    private async void PostForm(NotificationViewModel notificationViewModel)
    {
        using (var client = new HttpClient())
        {

            var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>(nameof(NotificationViewModel.FirstName), notificationViewModel.FirstName ?? String.Empty),
                new KeyValuePair<string, string>(nameof(NotificationViewModel.LastName), notificationViewModel.LastName ?? String.Empty),
                new KeyValuePair<string, string>(nameof(NotificationViewModel.IsEmailSelected), notificationViewModel.IsEmailSelected ? "True" : "False"),
                new KeyValuePair<string, string>(nameof(NotificationViewModel.IsPhoneSelected), notificationViewModel.IsPhoneSelected ? "True" : "False"),
                new KeyValuePair<string, string>(nameof(NotificationViewModel.Email), notificationViewModel.Email ?? String.Empty),
                new KeyValuePair<string, string>(nameof(NotificationViewModel.PhoneNumber), notificationViewModel.PhoneNumber ?? String.Empty),
                //Can't use the reflected name here, and my selection of the supervisor feels dirty. There has to be a better way.
                new KeyValuePair<string, string>("Supervisor", NotificationViewModel.Supervisors != null ? NotificationViewModel.Supervisors.Where(s => s.Value == notificationViewModel.SelectedSupervisorId.ToString()).FirstOrDefault().Text : String.Empty)
            });
            var response = await client.PostAsync(baseUrl + "api/Submit", content);

            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            Debug.WriteLine(result);
        }
    }
}
