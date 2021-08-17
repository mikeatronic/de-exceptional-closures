﻿using de_exceptional_closures.Extensions;
using de_exceptional_closures.Models;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Home;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Data;
using de_exceptional_closures_infraStructure.Features.ReasonType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public HomeController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel model = new IndexViewModel();

            if (HttpContext.Session.Get<IndexViewModel>("CreateClosureObj") == null)
            {
                HttpContext.Session.Set("CreateClosureObj", model);
            }
            else
            {
                model = HttpContext.Session.Get<IndexViewModel>("CreateClosureObj");
            }

            model.TitleTagName = "Create closure";
            model.InstitutionName = GetInstitutionName();

            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened Is the closure for a single day GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            model.TitleTagName = "Create closure";

            if (!ModelState.IsValid)
            {
                model.ReasonTypeList = await GetReasonTypes();

                return View(model);
            }


            if (model.IsSingleDay.Value)
            {
                DateTime datefrom;

                // Check for valid dates
                if (DateTime.TryParse(CreateDate(model.DateFromYear.ToString(), model.DateFromMonth.ToString(), model.DateFromDay.ToString()), out datefrom))
                {
                    model.DateFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
                }
                else
                {
                    LogAudit("Encountered an error: Please enter in a valid date. DateFrom POST view.");
                    ModelState.AddModelError("DateFromDay", "Please enter in a valid date");

                    model.ReasonTypeList = await GetReasonTypes();

                    return View(model);
                }
            }
         
            if (!model.IsSingleDay.Value)
            {
                DateTime datefrom;

                // Check for valid dates
                if (DateTime.TryParse(CreateDate(model.DateMultipleFromYear.ToString(), model.DateMultipleFromMonth.ToString(), model.DateMultipleFromDay.ToString()), out datefrom))
                {
                    model.DateMultipleFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
                }
                else
                {
                    LogAudit("Encountered an error: Please enter in a valid date. DateFrom POST view.");
                    ModelState.AddModelError("DateFromDay", "Please enter in a valid date");

                    model.ReasonTypeList = await GetReasonTypes();

                    return View(model);
                }

                DateTime dateTo;

                // Check for valid dates
                if (DateTime.TryParse(CreateDate(model.DateToYear.ToString(), model.DateToMonth.ToString(), model.DateToDay.ToString()), out dateTo))
                {
                    model.DateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);
                }
                else
                {
                    LogAudit("Encountered an error: Please enter in a valid date. DateTo POST view.");
                    ModelState.AddModelError("DateTo", "Please enter in a valid date");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }

                //Then Check if Date To is less than Date From
                if (model.DateTo < model.DateMultipleFrom)
                {
                    ModelState.AddModelError("DateToDay", "Date To cannot be less than Date From");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }
            }

            // Set Approval type
            model.ApprovalTypeId = await GetApprovalType(model.ReasonTypeId);

            HttpContext.Session.Set("CreateClosureObj", model);

            LogAudit("opened Is the closure for a single day POST view and selected model.IsSingleDay = ");

            return RedirectToAction("CheckAnswers", "Closure");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Privacy()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Privacy";

            LogAudit("opened Privacy GET view");

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Accessibility()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Accessibility";

            LogAudit("opened Accessibility GET view");

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Cookies()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Cookies";

            LogAudit("opened Cookies GET view");

            return View(model);
        }

        internal string GetInstitutionName()
        {
            var getUser = _userManager.GetUserAsync(User);

            return getUser.Result.InstitutionName;
        }

        internal async Task<List<ReasonTypeDto>> GetReasonTypes()
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            return getReasons.Value;
        }

        internal async Task<int> GetApprovalType(int id)
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            if (getReasons.Value.SingleOrDefault(i => i.Id == id).ApprovalRequired.HasValue && getReasons.Value.SingleOrDefault(i => i.Id == id).ApprovalRequired.Value)
            {
                return (int)ApprovalType.ApprovalRequired;
            }

            return (int)ApprovalType.PreApproved;
        }

        internal string CreateDate(string dateOfBirthYear, string dateOfBirthMonth, string dateOfBirthDay)
        {
            string dateToCheck = dateOfBirthYear + "/" + dateOfBirthMonth + "/" + dateOfBirthDay;

            return dateToCheck;
        }

        internal void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(User.Identity.Name + " " + message + ". " + ip);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}