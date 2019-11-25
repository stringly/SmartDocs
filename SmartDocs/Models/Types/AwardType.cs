using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.Types
{
    public abstract class AwardType
    {
        public string Kind { get; set; }
        [Required, Display(Name = "Award Class")]
        public string AwardClass { get; set; }
        [Required, Display(Name = "Award Name")]
        public string Name { get; set; }
        public string ComponentViewName { get; set;}
        public string Description { get;set; }
        public bool HasRibbon { get; set; }

        public AwardType()
        {

        }
    }

    public class GoodConductAward: AwardType
    {
        [Display(Name ="Date Eligibility Confirmed by IAD"), Required]
        public DateTime EligibilityConfirmationDate { get; set; }

        public GoodConductAward()
        {
            Kind = "GoodConductAward";
            EligibilityConfirmationDate = DateTime.Now;
            ComponentViewName = "GoodConduct";
            AwardClass = "Special Achievement Award";
            Name = "Good Conduct Award";
            HasRibbon = true;
            Description = "To be eligible for the Good Conduct Award, the employee must have received an overall rating of at least 'Exceeds Satisfactory' on the last two appraisals, and has not received any sustained disciplinary action within the last 24 months. Documentation confirming the employee's eligibility must be obtained from IAD and included with the Nomination Form.";
        }
    }
    public class OutstandingPerformanceAward: AwardType
    {
        
        [Display(Name = "Start Date of Eligibility Period"), Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date of Eligibility Period"), Required]
        public DateTime EndDate { get; set; }
        [Required, Display(Name = "Select the type of Outstanding Performance Award")]
        public int SelectedAwardType { get;set;}
        public List<SelectListItem> AwardTypes { get;set;}

        public OutstandingPerformanceAward()
        {
            Kind = "OutstandingPerformanceAward";
            EndDate = DateTime.Now;
            StartDate = EndDate.AddYears(-1);
            ComponentViewName = "Exemplary";
            AwardClass = "Exemplary Performance Award";
            Name = "Outstanding Performance Award";
            HasRibbon = false;
            Description = "To be eligible for the Outstanding Performance Award, the employee must have received at minimum a rating of 'Exceeds Satisfactory' on the last appraisal. See the options below for more detail.";
            AwardTypes = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Employee has received an overall rating of 'Exceeds Satisfactory' on the annual appraisal (Awards 8 Hours Annual Leave)",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Employee has received an overall rating of 'Outstanding' on the annual appraisal (Awards 16 Hours Annual Leave)",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "Employee has received an overall rating of 'Outstanding' on the past two consecutive appraisals (Awards 24 Hours Annual Leave)",
                    Value = "3"
                }
            };
        }
    }

    public class AwardTypeModelBinderProvider : Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider
    {
        public Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder GetBinder(Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(AwardType))
            {
                return null;
            }
            var subclasses = new[] { typeof(GoodConductAward), typeof(OutstandingPerformanceAward), };
            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }
            return new AwardTypeModelBinder(binders);
        }
    }
    public class AwardTypeModelBinder : IModelBinder
    {
        private Dictionary<Type, (ModelMetadata, IModelBinder)> binders;
        public AwardTypeModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelKindName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(AwardType.Kind));
            var modelTypeValue = bindingContext.ValueProvider.GetValue(modelKindName).FirstValue;

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;
            if (modelTypeValue == "GoodConductAward")
            {
                (modelMetadata, modelBinder) = binders[typeof(GoodConductAward)];
            }
            else if (modelTypeValue == "OutstandingPerformanceAward")
            {
                (modelMetadata, modelBinder) = binders[typeof(OutstandingPerformanceAward)];
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                bindingContext.ModelName);
            await modelBinder.BindModelAsync(newBindingContext);
            bindingContext.Result = newBindingContext.Result;

            if (newBindingContext.Result.IsModelSet)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[newBindingContext.Result] = new Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidationStateEntry
                {
                    Metadata = modelMetadata,
                };
            }
        }
    }
}
