using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }

        [Required(ErrorMessage = "Quiz Title is required")]
        [Display(Name = "Quiz Title")]
        public string QuizTitle { get; set; }

        [Required(ErrorMessage = "Quiz Topic is required")]
        [Display(Name = "Quiz Topic")]
        public string QuizTopic { get; set; }

        [Required(ErrorMessage = "Creator name is required")]
        [Display(Name = "Creator Name")]
        public string CreatorName { get; set; }

        [Display(Name ="Date Created")]
        public DateTime Created { get; set; }

        [Display(Name = "Pass Percentage")]
        public int PassPercentage { get; set; }


        // Navigation  property
        public ICollection<Question> Questions { get; set; }

    }
}
