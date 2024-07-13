using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraffiLearn.Domain.Exceptions
{
    public class QuestionAlreadyRemovedFromTopicException : Exception
    {
        public QuestionAlreadyRemovedFromTopicException() : base("The provided topic does not contain the provided question.")
        { }

        public QuestionAlreadyRemovedFromTopicException(Guid topicId, Guid questionId) : base($"The topic with '{topicId}' id does not contain the question with '{questionId}' id.")
        { }
    }
}
