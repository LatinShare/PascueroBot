using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PascueroBotSpace.Dialogs
{
    public class PascueroBotDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<Usuario> _userProfileAccessor;
        public PascueroBotDialog(UserState userState)
            : base(nameof(PascueroBotDialog))
        {
            _userProfileAccessor = userState.CreateProperty<Usuario>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                ConsultaEdadStepAsync
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt("ConsultaEdadStep"));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }


        private static async Task<DialogTurnResult> ConsultaEdadStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("¿Qué edad tienes?") };

            // Ask the user to enter their age.
            return await stepContext.PromptAsync("ConsultaEdadStep", promptOptions, cancellationToken);
        }
    }
}
