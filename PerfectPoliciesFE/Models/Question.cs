using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Models
{
    public class Question
    {
        public int QuestionID { get; set; }

        public string QuestionTopic { get; set; }

        public string QuestionText { get; set; }

        [Display(Name ="Image")]
        public string? ImagePath { get; set; }

        // navigation Property
        public ICollection<Option> Options { get; set; }
        public Quiz Quiz { get; set; }


        // Foreign Key
        public int QuizID { get; set; }

    }
}
