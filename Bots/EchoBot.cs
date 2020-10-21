// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace EchoBot2.Bots
{
    public class EchoBot : ActivityHandler
    {
        

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //var replyText = $"Echo: {turnContext.Activity.Text}";
            //await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            await GetAnswerFromQnAMaker(turnContext, cancellationToken);
        }

        private async Task GetAnswerFromQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if(results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Bot says:" + results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry,couldn't find an answer"), cancellationToken);
            }
        }
        public Microsoft.Bot.Builder.AI.QnA.QnAMaker EchoBotQnA { get; private set; }
        public EchoBot(Microsoft.Bot.Builder.AI.QnA.QnAMakerEndpoint endpoint)
        {
            EchoBotQnA = new Microsoft.Bot.Builder.AI.QnA.QnAMaker(endpoint);
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        
       
        }
        
    }
}
