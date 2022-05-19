using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Models
{
    public class Option
    {
        public int OptionID { get; set; }

        [Required (ErrorMessage =" option text is required")]
        [Display(Name =" Option ")]
        public string OptionText { get; set; }

        [Display(Name ="Option Number")]
        public int OptionNumber { get; set; }

        [Display(Name ="Correct Answer")]
        public bool CorrectAnswer { get; set; }

        //navigation property to link options to a question
        public Question Question { get; set; }

        //define foreign Key for database table
        public int QuestionID { get; set; }
    }
}
