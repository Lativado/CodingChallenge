﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CodingChallenge.Models;

namespace CodingChallenge.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var notificationViewModel = new NotificationViewModel();
        ViewBag.Supervisors = notificationViewModel.Supervisors;
        return View(notificationViewModel);
    }

    [HttpPost]
    public ActionResult NotificationFormSubmit(NotificationViewModel notificationViewModel)
    {
        if (ModelState.IsValid)
        {
            //Call API etc

            return View();
        }
        return View("Index", notificationViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}