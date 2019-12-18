// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.6.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PascueroBotSpace.Dialogs;
using System;

namespace PascueroBotSpace
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            /* COSMOSDB STORAGE - Uncomment the code in this section to use CosmosDB storage */

            //var cosmosDbStorageOptions = new CosmosDbStorageOptions
            //{
            //    AuthKey = "9ZGyNgGi2NrfqilBBd5V4FVJfgyZPJWAMwEGHoQcypAAWaIA2SgiK8cwuFAZWNh7IyumlsfYw0q6eU6PCbr2pA==",
            //    CosmosDBEndpoint = new System.Uri("https://presentacioncosmos.documents.azure.com:443/"),
            //    DatabaseId = "PascueroBot",
            //    PartitionKey = "PascueroBot",
            //    CollectionId = "PascueroBotContainer"
            //};

            //var storage = new CosmosDbStorage(cosmosDbStorageOptions);
            //services.AddSingleton<IStorage>(new AzureBlobStorage("DefaultEndpointsProtocol=https;AccountName=storebotdoctor365;AccountKey=76uU7kZq1sn5NVknRZ6WtaB6Ww+7DVF5IOt3ILvljBHt/lj6Fc/rhV9x4DniGsaOeUNCkaPNpRMvKpAf+wDoVg==;EndpointSuffix=core.windows.net", "blob01"));
            //services.AddSingleton<IStorage>(storage);
           /* var userState = new UserState(storage);
            services.AddSingleton(userState);

            // Create the Conversation state passing in the storage layer.
            var conversationState = new ConversationState(storage);
            services.AddSingleton(conversationState);*/

            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            // The Dialog that will be run by the bot.
            services.AddSingleton<PascueroBotDialog>();
            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, PascueroBot<PascueroBotDialog>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseMvc();
        }
    }
}
