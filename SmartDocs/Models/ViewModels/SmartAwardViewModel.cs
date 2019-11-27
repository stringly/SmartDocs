using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    public abstract class SmartAwardViewModel
    {
        public int DocumentId { get; set; }
        public int AuthorUserId { get; set; }
        [Display(Name = "Nominee's Agency"), Required]
        public string AgencyName { get; set; }
        [Display(Name = "Nominee's Name"), Required]
        public string NomineeName { get; set; }
        [Display(Name = "Nominee's Class Title"), Required]
        public string ClassTitle { get; set; }
        [Display(Name = "Nominee's District/Division"), Required]
        public string Division { get; set; }
        [Required(ErrorMessage = "You must select an Award Type.")]
        [Display(Name = "Award Type")]
        public int? SelectedAward { get; set; }
        public string Kind { get; set; }
        [Required, Display(Name = "Award Class")]
        public string AwardClass { get; set; }
        [Required, Display(Name = "Award Name")]
        public string AwardName { get; set; }
        public string ComponentViewName { get; set; }
        public string Description { get; set; }
        public bool HasRibbon { get; set; }
        public List<OrganizationComponent> Components { get; set; }
        public List<SmartUser> Users { get; set; }
        public List<AwardSelectListOption> AwardList { get; set; }

        public abstract AwardTypeFormViewComponentViewModel GetComponentModel();
    }

    public class EmptyAwardViewModel : SmartAwardViewModel
    {
        public EmptyAwardViewModel()
        {
            AwardList = new List<AwardSelectListOption>
            {
                new AwardSelectListOption
                {
                    Text = "Good Conduct Award",
                    Value = "1",
                    SubText = "(No Sustained Discipline in past 24 months.)"

                },
                new AwardSelectListOption
                {
                    Text = "Exemplary Performance",
                    Value = "2",
                    SubText = "(Exceeds Satisfactory or above on appraisal)"
                },
            };
        }
        public override AwardTypeFormViewComponentViewModel GetComponentModel()
        {
            return null;
        }
    }

    public class AwardSelectListOption : SelectListItem
    {
        public string SubText { get; set; }
    }

    public class GoodConductAwardViewModel: SmartAwardViewModel
    {
        [Display(Name = "Date Eligibility Confirmed by IAD"), Required]
        [DataType(DataType.Date)]
        public DateTime EligibilityConfirmationDate { get; set; }

        public GoodConductAwardViewModel()
        {           
            AwardList = new List<AwardSelectListOption>
            {
                new AwardSelectListOption
                {
                    Text = "Good Conduct Award",
                    Value = "1",
                    SubText = "(No Sustained Discipline in past 24 months.)"

                },
                new AwardSelectListOption
                {
                    Text = "Exemplary Performance",
                    Value = "2",
                    SubText = "(Exceeds Satisfactory or above on appraisal)"
                },
            };
        }
        public override AwardTypeFormViewComponentViewModel GetComponentModel()
        {
            return new GoodConductAwardFormViewComponentViewModel
            {
                Kind = "GoodConductAwardViewModel",
                ComponentViewName = "GoodConduct",
                AwardClass = "Special Achievement Award",
                AwardName = "Good Conduct Award",
                HasRibbon = true,
                Description = "To be eligible for the Good Conduct Award, the employee must have received an overall rating of at least 'Exceeds Satisfactory' on the last two appraisals, and has not received any sustained disciplinary action within the last 24 months. Documentation confirming the employee's eligibility must be obtained from IAD and included with the Nomination Form.",
                EligibilityConfirmationDate = this.EligibilityConfirmationDate
            };
        }
    }
    public class OutstandingPerformanceAwardViewModel : SmartAwardViewModel
    {

        [Display(Name = "Start Date of Eligibility Period"), Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date of Eligibility Period"), Required]
        public DateTime EndDate { get; set; }
        [Required, Display(Name = "Select the type of Outstanding Performance Award")]
        public int? SelectedAwardType { get; set; }
        public List<SelectListItem> AwardTypes { get; set; }

        public OutstandingPerformanceAwardViewModel()
        {
           
            AwardList = new List<AwardSelectListOption>
            {
                new AwardSelectListOption
                {
                    Text = "Good Conduct Award",
                    Value = "1",
                    SubText = "(No Sustained Discipline in past 24 months.)"

                },
                new AwardSelectListOption
                {
                    Text = "Exemplary Performance",
                    Value = "2",
                    SubText = "(Exceeds Satisfactory or above on appraisal)"
                },
            };
        }
        public override AwardTypeFormViewComponentViewModel GetComponentModel()
        {
            return new OutstandingPerformanceAwardFormViewComponentViewModel
            {
                Kind = "OutstandingPerformanceAwardViewModel",
                ComponentViewName = "Exemplary",
                AwardClass = "Exemplary Performance Award",
                AwardName = "Outstanding Performance Award",
                HasRibbon = false,
                Description = "To be eligible for the Outstanding Performance Award, the employee must have received at minimum a rating of 'Exceeds Satisfactory' on the last appraisal. See the options below for more detail.",
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                SelectedAwardType = this.SelectedAwardType,
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
                }
            };
        }
    }

    public class AwardTypeModelBinderProvider : Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider
    {
        public Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder GetBinder(Microsoft.AspNetCore.Mvc.ModelBinding.ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(SmartAwardViewModel))
            {
                return null;
            }
            var subclasses = new[] { typeof(GoodConductAwardViewModel), typeof(OutstandingPerformanceAwardViewModel), typeof(EmptyAwardViewModel) };
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
            var modelKindName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(SmartAwardViewModel.Kind));
            var modelTypeValue = bindingContext.ValueProvider.GetValue(modelKindName).FirstValue;

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;
            if (modelTypeValue == "GoodConductAwardViewModel")
            {
                (modelMetadata, modelBinder) = binders[typeof(GoodConductAwardViewModel)];
            }
            else if (modelTypeValue == "OutstandingPerformanceAwardViewModel")
            {
                (modelMetadata, modelBinder) = binders[typeof(OutstandingPerformanceAwardViewModel)];
            }
            else if (modelTypeValue == "EmptyAwardViewModel")
            {
                (modelMetadata, modelBinder) = binders[typeof(EmptyAwardViewModel)];
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

