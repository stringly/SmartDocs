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
    /// <summary>
    /// Abstract base class for Award Form Viewmodel types.
    /// </summary>
    public abstract class SmartAwardViewModel
    {
        /// <summary>
        /// The <see cref="SmartDocument.DocumentId"/> of the document.
        /// </summary>
        public int DocumentId { get; set; }
        /// <summary>
        /// The <see cref="SmartUser.UserId"/> of the document's author.
        /// </summary>
        public int AuthorUserId { get; set; }
        /// <summary>
        /// The name of the agency to which the award nominee is assigned.
        /// </summary>
        [Display(Name = "Nominee's Agency"), Required, StringLength(100)]
        public string AgencyName { get; set; }
        /// <summary>
        /// The nominee's name.
        /// </summary>
        [Display(Name = "Nominee's Name"), Required, StringLength(100)]
        public string NomineeName { get; set; }
        /// <summary>
        /// The nominee's class title.
        /// </summary>
        [Display(Name = "Nominee's Class Title"), Required, StringLength(100)]
        public string ClassTitle { get; set; }
        /// <summary>
        /// The nominee's assigned district/division.
        /// </summary>
        [Display(Name = "Nominee's District/Division"), Required, StringLength(100)]
        public string Division { get; set; }
        /// <summary>
        /// The integer id of the award type.
        /// </summary>
        [Required(ErrorMessage = "You must select an Award Type.")]
        [Display(Name = "Award Type")]
        public int? SelectedAward { get; set; }
        /// <summary>
        /// The kind of award.
        /// </summary>
        public string Kind { get; set; }
        /// <summary>
        /// The award's class name.
        /// </summary>
        [Required, Display(Name = "Award Class"), StringLength(100)]
        public string AwardClass { get; set; }
        /// <summary>
        /// The award's name.
        /// </summary>
        [Required, Display(Name = "Award Name"), StringLength(100)]
        public string AwardName { get; set; }
        /// <summary>
        /// The Component View Name.
        /// </summary>
        public string ComponentViewName { get; set; }
        /// <summary>
        /// The description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Boolean indicates whether the award type has an associated award ribbon.
        /// </summary>
        public bool HasRibbon { get; set; }
        /// <summary>
        /// List of <see cref="OrganizationUnit"/> used to populate a drop down.
        /// </summary>
        public List<OrganizationUnit> Units { get; set; }
        /// <summary>
        /// List of <see cref="SmartUser"/> used to populate a drop down.
        /// </summary>
        public List<SmartUser> Users { get; set; }
        /// <summary>
        /// List of <see cref="AwardSelectListOption"/> used to populate a drop down.
        /// </summary>
        public List<AwardSelectListOption> AwardList { get; set; }
        /// <summary>
        /// Returns the Component model.
        /// </summary>
        /// <returns></returns>
        public abstract AwardTypeFormViewComponentViewModel GetComponentModel();
    }
    /// <summary>
    /// Implementation of <see cref="SmartAwardViewModel"/> used for an empty award view.
    /// </summary>
    public class EmptyAwardViewModel : SmartAwardViewModel
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
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
        /// <summary>
        /// Returns the component model.
        /// </summary>
        /// <returns></returns>
        public override AwardTypeFormViewComponentViewModel GetComponentModel()
        {
            return null;
        }
    }
    /// <summary>
    /// Implementation of <see cref="SelectListItem"/> that adds a subtext property.
    /// </summary>
    public class AwardSelectListOption : SelectListItem
    {
        /// <summary>
        /// The string containing the subtext.
        /// </summary>
        public string SubText { get; set; }
    }
    /// <summary>
    /// Implementation of <see cref="SmartAwardViewModel"/> used for Good Conduct Award Types.
    /// </summary>
    public class GoodConductAwardViewModel: SmartAwardViewModel
    {
        /// <summary>
        /// The date that eligibility for the award was confirmed.
        /// </summary>
        [Display(Name = "Date Eligibility Confirmed by IAD"), Required]
        [DataType(DataType.Date)]
        public DateTime EligibilityConfirmationDate { get; set; }
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
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
        /// <summary>
        /// Returns the Component Model.
        /// </summary>
        /// <returns></returns>
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
    /// <summary>
    /// Implementation of <see cref="SmartAwardViewModel"/> used for Outstanding performance award types.
    /// </summary>
    public class OutstandingPerformanceAwardViewModel : SmartAwardViewModel
    {
        /// <summary>
        /// The start date of the eligiblity period
        /// </summary>
        [Display(Name = "Start Date of Eligibility Period"), Required]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// The end date of the eligiblity period
        /// </summary>
        [Display(Name = "End Date of Eligibility Period"), Required]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// The integer id of the selected award type.
        /// </summary>
        [Required, Display(Name = "Select the type of Outstanding Performance Award")]
        public int? SelectedAwardType { get; set; }
        /// <summary>
        /// List of award types.
        /// </summary>
        public List<SelectListItem> AwardTypes { get; set; }
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
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
        /// <summary>
        /// Returns the component model.
        /// </summary>
        /// <returns></returns>
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
    /// <summary>
    /// Custom model-binding provider used in the <see cref="SmartAwardViewModel"/> classes.
    /// </summary>
    public class AwardTypeModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Gets the model binder.
        /// </summary>
        /// <param name="context">A <see cref="ModelBinderProviderContext"/></param>
        /// <returns>An implementation of <see cref="IModelBinder"/></returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
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
    /// <summary>
    /// Custom model-binder used in <see cref="SmartAwardViewModel"/> derived classes.
    /// </summary>
    public class AwardTypeModelBinder : IModelBinder
    {
        private Dictionary<Type, (ModelMetadata, IModelBinder)> binders;
        /// <summary>
        /// Creates the binder.
        /// </summary>
        /// <param name="binders"></param>
        public AwardTypeModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }
        /// <summary>
        /// Binds the model.
        /// </summary>
        /// <param name="bindingContext">A <see cref="ModelBindingContext"/></param>
        /// <returns></returns>
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

