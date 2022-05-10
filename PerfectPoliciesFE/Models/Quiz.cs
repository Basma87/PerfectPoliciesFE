using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public string QuizTitle { get; set; }
        public string QuizTopic { get; set; }

        public string CreatorName { get; set; }

        public DateTime Created { get; set; }

        public int PassPercentage { get; set; }


        // Navigation  property
        public ICollection<Question> Questions { get; set; }

    }
}
