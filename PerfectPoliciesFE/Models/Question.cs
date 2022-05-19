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

        [Required(ErrorMessage ="Question topic is required")]
        [Display(Name ="Question Topic")]
        public string QuestionTopic { get; set; }


        [Required(ErrorMessage = "Question is required")]
        [Display(Name = "Question")]
        public string QuestionText { get; set; }

        [Display(Name ="Image")]
        public string? ImagePath { get; set; }

        // navigation Property to link list of options to a question
        
        public ICollection<Option> Options { get; set; }

        // navigation property to link question to a quiz
        public Quiz Quiz { get; set; }


        // Foreign Key
        public int QuizID { get; set; }

    }
}
