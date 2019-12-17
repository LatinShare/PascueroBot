using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PascueroBot.Controllers
{
    public class Cards
    {
        
        private static readonly string[] _cards =
        {
            Path.Combine(".", "AdaptiveCards", "FlightItineraryCard.json"),
            Path.Combine(".", "AdaptiveCards", "ImageGalleryCard.json"),
            Path.Combine(".", "AdaptiveCards", "LargeWeatherCard.json"),
            Path.Combine(".", "AdaptiveCards", "RestaurantCard.json"),
            Path.Combine(".", "AdaptiveCards", "SolitaireCard.json"),
            Path.Combine(".", "AdaptiveCards", "Form.json")
        };
        private static Microsoft.Bot.Schema.Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = System.IO.File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Microsoft.Bot.Schema.Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        public static async Task SendAdaptiveCard(ITurnContext _context, CancellationToken cancellationToken)
        {
            foreach (string path in _cards)
            {
                var cardAttachment = CreateAdaptiveCardAttachment(path);
                var reply = _context.Activity.CreateReply();
                reply.Attachments = new List<Microsoft.Bot.Schema.Attachment>() { cardAttachment };
                await _context.SendActivityAsync(reply, cancellationToken);
            }
        }
    }
}
