using Microsoft.AspNetCore.Builder;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SmartPPA.Models
{
    public static class SeedData
    {

        public static void EnsurePopulated(SmartDocContext context)
        {
            
            //if (!context.Users.Any())
            //{
            //    context.Users.AddRange(
            //        new SmartUser
            //        {
            //            BlueDeckId = 1,
            //            DisplayName = "Lt. J. Smith #3134",
            //            LogonName = "jcsmith1"
            //        },
            //        new SmartUser
            //        {
            //            BlueDeckId = 1,
            //            DisplayName = "Lt. J. Smith #3134",
            //            LogonName = "jcs3082@hotmail.com"
            //        }, 
            //        new SmartUser
            //        {
            //            BlueDeckId = 2,
            //            DisplayName = "Sgt. P. McClam #3374",
            //            LogonName = "pemmclam"
            //        }
            //    );
            //}
            
            //if (!context.Templates.Any())
            //{
            //    context.Templates.Add(
            //        new SmartTemplate
            //        {
            //            DocumentName = "Performance Appraisal",
            //            DataStream = File.ReadAllBytes("TemplateNoJobDescriptionCell.docx")
            //        }
            //    );                
            //}

            //context.SaveChanges(); // I have to save before I add Documents so that I can refer to the users and the Templates
            //if (!context.Documents.Any())
            //{
            //    // TODO: I need to Convert the FormData from the View to XML for the DB Column
            //    context.Documents.AddRange(
            //        new SmartRecord
            //        {
            //            Template = context.Templates.Where(x => x.TemplateId == 1).FirstOrDefault(),
            //            User = context.Users.Where(x => x.UserId == 1).FirstOrDefault(),
            //            Created = DateTime.Now,
            //            Modified = DateTime.Now
            //        },
            //        new SmartRecord
            //        {
            //            Template = context.Templates.Where(x => x.TemplateId == 1).FirstOrDefault(),
            //            User = context.Users.Where(x => x.UserId == 2).FirstOrDefault(),
            //            Created = DateTime.Now,
            //            Modified = DateTime.Now
            //        },
            //        new SmartRecord
            //        {
            //            Template = context.Templates.Where(x => x.TemplateId == 1).FirstOrDefault(),
            //            User = context.Users.Where(x => x.UserId == 1).FirstOrDefault(),
            //            Created = DateTime.Now,
            //            Modified = DateTime.Now
            //        }
            //    );
            //}
            //context.SaveChanges();
        }
    }
}
