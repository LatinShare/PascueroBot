using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using PascueroBot.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PascueroBotSpace.Dialogs
{
    public class PascueroBotDialog : ComponentDialog
    {
        //private readonly IStatePropertyAccessor<Usuario> _userProfileAccessor;
        private static UserState _userState;
        public PascueroBotDialog(UserState userState)
            : base(nameof(PascueroBotDialog))
        {
            //_userProfileAccessor = userState.CreateProperty<Usuario>("UserProfile");
            _userState = userState;
            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                MainStepAsync,
                ConsultaEdadStepAsync,
                ConsultaComportamientoStepAsync,
                FinalStepAsync
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new ChoicePrompt("MainStepAsync"));
            AddDialog(new NumberPrompt<int>("ConsultaEdadStep"));
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
            stepContext.Values["accion_realizar"] = ((FoundChoice)stepContext.Result).Value;
            
            if (stepContext.Values["accion_realizar"].ToString() == "Ver cards de Bot")
            {
                await Cards.SendAdaptiveCard(stepContext.Context, cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

            }
            else
            {
                
                Usuario usuario = await PascueroBotDialog.GetUserInfo(stepContext.Context, cancellationToken);
                if (usuario.RegaloSolicitado)
                {
                    await stepContext.Context.SendActivityAsync("(Haré cuenta que no habías pedido nada 🎅)", cancellationToken: cancellationToken);
                }

            }


            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("¿Qué edad tienes? 🎁") };

            // Ask the user to enter their age.
            return await stepContext.PromptAsync("ConsultaEdadStep", promptOptions, cancellationToken);
        }

        private static async Task<DialogTurnResult> ConsultaComportamientoStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["edad"] = (int)stepContext.Result;

            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync("ConsultaComportamientoStep",
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("¿Cómo te portaste este año? 🎄"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Bien 😁", "Regular 🤐", "Mal 😣" }),
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["comportamiento"] = ((FoundChoice)stepContext.Result).Value;

            await MentionActivityAsync((ITurnContext<IMessageActivity>)stepContext.Context, cancellationToken);

            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is the end.
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task MentionActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard
            {
                Title = "BotFramework Hero Card",
                Subtitle = "Microsoft Bot Framework",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are," +
                       " from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.microsoft.com/bot-framework") },
            };


            Usuario usuario = await GetUserInfo(turnContext, cancellationToken);
            usuario.RegaloSolicitado = true;
            var mention = new Mention
            {
                Mentioned = turnContext.Activity.From,
                Text = $"<at>{XmlConvert.EncodeName(turnContext.Activity.From.Name)}</at>",
            };

            var replyActivity = MessageFactory.Text($"Hello {mention.Text}.");
            replyActivity.Entities = new List<Entity> { mention };


            replyActivity.Attachments.Add(heroCard.ToAttachment());

            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }

        public async static Task<Usuario> GetUserInfo(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            IStatePropertyAccessor<Usuario> _userProfileAccessor = _userState.CreateProperty<Usuario>("UserProfile");
            return await _userProfileAccessor.GetAsync(turnContext, () => new Usuario(), cancellationToken);

        }
    }
}
