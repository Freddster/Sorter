using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Sorter.DataAccessLogic;
using Sorter.Models;
using Sorter.Modules.Announcement;
using Sorter.Modules.User;

namespace Sorter
{
    public class Program
    {
        private DiscordSocketClient _client;
        // Keep the CommandService and IServiceCollection around for use with commands.
        // These two types require you install the Discord.Net.Commands package.
        private CommandService _commands = new CommandService();
        private IServiceProvider _services;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        //TODO: Move bot program to bot file
        private Program()
        {
            Version operatingSystemVersion = Environment.OSVersion.Version;
            int major = operatingSystemVersion.Major;
            var minor = operatingSystemVersion.Minor;
            if (major >= 6 && (minor == 2 || minor == 3 || minor == 0))
            {
                _client = new DiscordSocketClient(new DiscordSocketConfig
                {
                    // How much logging do you want to see?
                    LogLevel = LogSeverity.Info,

                    // If you or another service needs to do anything with messages
                    // (eg. checking Reactions, checking the content of edited/deleted messages),
                    // you must set the MessageCacheSize. You may adjust the number as needed.
                    //MessageCacheSize = 50,

                    // If your platform doesn't have native websockets,
                    // add Discord.Net.Providers.WS4Net from NuGet,
                    // add the `using` at the top, and uncomment this line:
                    //WebSocketProvider = WS4NetProvider.Instance
                });
            }
            else
            {
                _client = new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Info,
                    WebSocketProvider = WS4NetProvider.Instance
                });
            }

            CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");
            // Subscribe the logging handler to both the client and the CommandService.
            _client.Log += Logger;
            _commands.Log += Logger;
        }

        public async Task MainAsync()
        {
            await InitCommands();
            await RegisterCommandsAsync();

            bool loggedIn = false;
            while (!loggedIn)
            {
                try
                {
                    UnitOfWork unitOfWork = new UnitOfWork();

                    var result = unitOfWork.KeyValueRepository.GetToken("token");
                    await _client.LoginAsync(TokenType.Bot, result.Value);
                    unitOfWork.KeyValueRepository.Remove(result);
                    unitOfWork.Save();
                    loggedIn = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Thread.Sleep(5000);
                }
            }

            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task InitCommands()
        {
            // Repeat this for all the service classes
            // and other dependencies that your commands might need.
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
        }

        public async Task RegisterCommandsAsync()
        {
            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += AnnouncementModule.UserJoinedAnnoucement;
            _client.UserLeft += AnnouncementModule.UserLeftAnnoucement;
            _client.UserVoiceStateUpdated += UserModule.VoiceChannelUpdated;
            // Either search the program and add all Module classes that can be found.
            // Module classes MUST be marked 'public' or they will be ignored.
            //await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            // Or add Modules manually if you prefer to be a little more explicit:
            //await _commands.AddModuleAsync<SomeModule>();
            // Note that the first one is 'Modules' (plural) and the second is 'Module' (singular).
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private static Task Logger(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            var msg = message as SocketUserMessage;

            if (msg is null || msg.Author.IsBot)
                return;

            int argPos = 0;
            if (msg.HasStringPrefix("!", ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, msg);

                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
