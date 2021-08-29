using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Models
{
    public class Question
    {
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; set; }

    }
    public class Answer
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
