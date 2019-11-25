﻿using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.ViewComponents
{
    public class AwardTypeFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AwardType award)
        {
            return View(award.ComponentViewName, award);
        }
    }
}