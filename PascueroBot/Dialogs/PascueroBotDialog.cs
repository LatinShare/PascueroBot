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
                MainStepAsync,
                ConsultaEdadStepAsync,
                ConsultaComportamientoStepAsync
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new ChoicePrompt("MainStepAsync"));
            AddDialog(new TextPrompt("ConsultaEdadStep"));
            AddDialog(new ChoicePrompt("ConsultaComportamientoStep"));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> MainStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync("MainStepAsync",
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("¿Qué deseas hacer jo jo jo? ✨"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Pedir un regalo", "Ver cards de Bot" }),
                }, cancellationToken);
        }
        private static async Task<DialogTurnResult> ConsultaEdadStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("¿Qué edad tienes? 🎁") };

            // Ask the user to enter their age.
            return await stepContext.PromptAsync("ConsultaEdadStep", promptOptions, cancellationToken);
        }

        private static async Task<DialogTurnResult> ConsultaComportamientoStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync("ConsultaComportamientoStep",
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("¿Cómo te portaste este año? 🎄"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Bien 😁", "Regular 🤐", "Mal 😣" }),
                }, cancellationToken);
        }
    }
}
