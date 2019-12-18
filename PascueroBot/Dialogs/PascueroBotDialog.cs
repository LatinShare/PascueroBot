using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using PascueroBot.Controllers;
using PascueroBotSpace.Model;
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
        private static Regalo[] regaloNinos;
        private static Regalo[] regaloJovenes;
        private static Regalo[] regaloAbuelos;
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
                RegaloStepAsync,
                ConfirmStepAsync,
                FinalStepAsync
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new ChoicePrompt("MainStepAsync"));
            AddDialog(new NumberPrompt<int>("ConsultaEdadStep"));
            AddDialog(new ChoicePrompt("ConsultaComportamientoStep"));
            AddDialog(new ChoicePrompt("ConsultaComportamientoStep"));
            AddDialog(new ConfirmPrompt("ConfirmStep"));

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

        private async Task<DialogTurnResult> RegaloStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["comportamiento"] = ((FoundChoice)stepContext.Result).Value;

            await MentionActivityAsync(stepContext, cancellationToken);

            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is the end.
            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }
        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is the end.
            return await stepContext.PromptAsync("ConfirmStep", new PromptOptions { Prompt = MessageFactory.Text("¿Te gustó el regalo jo jo jo?") }, cancellationToken);
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["confirmacion"] = (bool)stepContext.Result;

            if ((bool)stepContext.Values["confirmacion"])
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Disfrutalo jo jo jo 🎅"), cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Portate mejor el próximo año 🎅"), cancellationToken);
            }
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is the end.
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        private async Task MentionActivityAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Usuario usuario = await GetUserInfo(stepContext.Context, cancellationToken);
            usuario.Comportamiento = stepContext.Values["comportamiento"].ToString().Contains("Bien") ? ComportamientoEnum.Comportamiento.Bien : (stepContext.Values["comportamiento"].ToString().Contains("Mal") ? ComportamientoEnum.Comportamiento.Mal : ComportamientoEnum.Comportamiento.Regular);
            usuario.Edad = (int)stepContext.Values["edad"];


            usuario.RegaloSolicitado = true;
            Regalo regalo = ObtenerRegalo(usuario);




            var mention = new Mention
            {
                Mentioned = stepContext.Context.Activity.From,
                Text = $"<at>{XmlConvert.EncodeName(stepContext.Context.Activity.From.Name)}</at>",
            };

            var replyActivity = MessageFactory.Text(string.Format(regalo.MensajeRegalo, mention.Text));
            replyActivity.Entities = new List<Entity> { mention };



            var heroCard = new HeroCard
            {
                Title = $"{regalo.Nombre}",
                //Subtitle = "Microsoft Bot Framework",
                Text = regalo.Descripcion,
                Images = new List<CardImage> { new CardImage(regalo.UrlImagen) },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Ver Regalo", value: regalo.UrlImagen) },

            };

            replyActivity.Attachments.Add(heroCard.ToAttachment());

            await stepContext.Context.SendActivityAsync(replyActivity, cancellationToken);
        }

        public async static Task<Usuario> GetUserInfo(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            IStatePropertyAccessor<Usuario> _userProfileAccessor = _userState.CreateProperty<Usuario>("UserProfile");
            return await _userProfileAccessor.GetAsync(turnContext, () => new Usuario(), cancellationToken);

        }

        public Regalo ObtenerRegalo(Usuario usuario)
        {
            Regalo regaloSeleccionado = null;
            Regalo[] regalos = new Regalos().regalos;

            foreach (Regalo regalo in regalos)
            {
                if (regalo.Comportamiento == usuario.Comportamiento && (regalo.EdadMinima <= usuario.Edad && regalo.EdadMaxima >= usuario.Edad))
                {
                    regaloSeleccionado = regalo;
                    break;
                }
            }

            string mensajeRegalo = "";

            if (usuario.Comportamiento == ComportamientoEnum.Comportamiento.Bien)
            {
                regaloSeleccionado.MensajeRegalo = "Ya que te portaste bien {0} este año tu regalo es :";
            }else if (usuario.Comportamiento == ComportamientoEnum.Comportamiento.Regular)
            {
                regaloSeleccionado.MensajeRegalo = "Ya que no te portaste muy bien {0} este año tu regalo es :";
            }
            else{
                regaloSeleccionado.MensajeRegalo = "Este año te portaste muy mal, pero igual tienes regalo:";
            }

            return regaloSeleccionado;
        }
    }
}
