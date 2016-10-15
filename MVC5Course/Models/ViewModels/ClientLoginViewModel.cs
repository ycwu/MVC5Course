using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ViewModels
{
    public class ClientLoginViewModel
    {
        [Required]
        [DisplayName("名")]
        [StringLength(10, ErrorMessage = "{0} 最大不得超過{1}字")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("中間名")]
        [StringLength(10, ErrorMessage = "{0} 最大不得超過{1}字")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} 最大不得超過{1}字")]
        [DisplayName("姓")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("[MF]", ErrorMessage = "{0} 欄位只能輸入 M 或 F")]
        [DisplayName("性別")]
        public string Gender { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DisplayName("生日")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
    }
}