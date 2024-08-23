using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Questions
{
    public sealed class AuthorizedQuestionRequestSender
    {
        private readonly RequestSender _requestSender;

        public AuthorizedQuestionRequestSender(RequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task CreateValidQuestionAsync()
        {

        }
    }
}
