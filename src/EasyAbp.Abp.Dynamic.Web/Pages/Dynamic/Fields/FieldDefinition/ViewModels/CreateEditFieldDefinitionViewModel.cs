using System;

using System.ComponentModel.DataAnnotations;

namespace EasyAbp.Abp.Dynamic.Web.Pages.Dynamic.Fields.FieldDefinition.ViewModels
{
    public class CreateEditFieldDefinitionViewModel
    {
        [Display(Name = "FieldDefinitionName")]
        public string Name { get; set; }

        [Display(Name = "FieldDefinitionType")]
        public string Type { get; set; }

        [Display(Name = "FieldDefinitionOrder")]
        public int Order { get; set; }
    }
}