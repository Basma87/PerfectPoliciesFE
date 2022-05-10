using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Models
{
    public class Option
    {
        public int OptionID { get; set; }

        public string OptionText { get; set; }
        public int OptionNumber { get; set; }

        public bool CorrectAnswer { get; set; }

        //navigation property
        public Question Question { get; set; }

        //define foreign Key
        public int QuestionID { get; set; }
    }
}
