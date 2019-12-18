// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema.Teams;
using PascueroBotSpace.Model;

namespace PascueroBotSpace
{
    public class PascueroBot<T> : ActivityHandler where T : Dialog
    {
        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;
        private readonly IStatePropertyAccessor<Usuario> _userProfileAccessor;
        public PascueroBot(ConversationState conversationState, UserState userState, T dialog)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            _userProfileAccessor = userState.CreateProperty<Usuario>("UserProfile");
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {

                    RegisterTeamsInfo(turnContext, cancellationToken);
                    await turnContext.SendActivityAsync(MessageFactory.Text(string.Format(@"Hola {0}! soy PascueroBot 🎅🎁", turnContext.Activity.From.Name)), cancellationToken);
                    await Dialog.Run(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
                }
            }
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Guarda los cambios realizados en los state
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            RegisterTeamsInfo(turnContext, cancellationToken);
            // Run the Dialog with the new message Activity.
            await Dialog.Run(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
        }
        private async void RegisterTeamsInfo(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            // 
            var userProfile = await _userProfileAccessor.GetAsync(turnContext, () => new Usuario(), cancellationToken);
            TeamsChannelData teamConversationData = turnContext.Activity.GetChannelData<TeamsChannelData>();
            userProfile.Nombre = turnContext.Activity.From.Name;
            userProfile.IdUsuario = turnContext.Activity.From.Id;
            userProfile.TenantId = teamConversationData?.Tenant?.Id;
        }
    }
}
