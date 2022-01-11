﻿using Discord;
using Discord.Commands;
using Discord.Net;
using Marina.Utils;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marina.Commands.Animals.Dog
{
    public class DogUpload : ModuleBase<SocketCommandContext>
    {
        [Command("Dog")]
        [Summary("Gets a random dog picture.")]
        public async Task UploadDog()
        {
            DogData jsonData;
            using HttpClient client = new();
            try
            {
                jsonData = JsonConvert.DeserializeObject<DogData>(
                    await client.GetStringAsync("https://dog.ceo/api/breeds/image/random"));
            }
            catch (WebException e)
            {
                await Error.SendDiscordError(Context, value: $"{e.Message}", e: e);
                return;
            }

            if (jsonData.Status != "success")
                await Error.SendDiscordError(Context, value: "API Failed!", e: new Exception("Dog API Failed!"));
            EmbedBuilder builder = new()
            {
                Color = Color.Teal,
                ImageUrl = jsonData.ImageUrl.OriginalString
            };
            builder.WithCurrentTimestamp();
            builder.WithFooter("Taken from https://dog.ceo/dog-api/");

            await ReplyAsync(embed: builder.Build());

        }
    }
}