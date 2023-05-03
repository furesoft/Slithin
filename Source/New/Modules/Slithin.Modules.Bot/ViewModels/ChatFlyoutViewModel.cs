using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Slithin.Core.MVVM;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Bot.Dialogs;
using Slithin.Modules.Bot.MessageModels;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Events;
using Syn.Bot.Oscova.Messages;

namespace Slithin.Modules.Bot.ViewModels;

public class ChatFlyoutViewModel : BaseViewModel
{
    private readonly OscovaBot _bot;
    private readonly IProfanityFilter _profanityFilter;

    public string Message
    {
        get => _message;
        set => this.SetValue(ref _message, value);
    }

    public ICommand SendCommand { get; set; }

    public ObservableCollection<ChatMessage> Messages { get; set; } = new();

    private string _message;

    public ChatFlyoutViewModel(OscovaBot bot, IProfanityFilter profanityFilter)
    {
        _bot = bot;
        _profanityFilter = profanityFilter;

        InitBot(bot);
    }

    private void InitBot(OscovaBot bot)
    {
        Dispatcher.UIThread.Post(() =>
        {
            SendCommand = new DelegateCommand(Send);

            bot.CreateRecognizer("orientation", new[] {"portrait", "landscape"});
            bot.CreateRecognizer("asset", new[] {"screen", "template", "notebook"});

            bot.Dialogs.Add(new NotebooksDialog());
            bot.Dialogs.Add(new TemplatesDialog());
            bot.Dialogs.Add(new GreetingsDialog());

            bot.Trainer.StartTraining();

            bot.MainUser.ResponseReceived += BotUserOnResponseReceived;

            bot.Raise("start");
        });
    }

    private void BotUserOnResponseReceived(object? sender, ResponseReceivedEventArgs e)
    {
        Dispatcher.UIThread.Post(async () =>
        {
            if (!string.IsNullOrEmpty(e.Response.Text))
            {
                await CreateBotMessage(e.Response.Text, e.Response.Hint);
            }

            foreach (var msg in e.Response.Messages)
            {
                if (msg is UnknownRequestMessage ukmsg)
                {
                    await CreateUnknownMessage(ukmsg);
                }
                else if (msg is TextMessage tmsg)
                {
                    await CreateBotMessage(tmsg.GetRandomText(), e.Response.Hint);
                }
                else if (msg is BitmapMessage imgMsg)
                {
                    await CreateImageBotMessage(imgMsg);
                }
            }
        });
    }

    private async Task CreateUnknownMessage(UnknownRequestMessage ukmsg)
    {
        await CreateBotMessage(ukmsg, null);
    }

    private async Task CreateImageBotMessage(BitmapMessage imgMsg)
    {
        await CreateBotMessage(imgMsg.Bitmap, null);
    }

    private async Task CreateBotMessage(object content, string hint)
    {
        Dispatcher.UIThread.Post(async () =>
        {
            var msg = new ChatMessage() {SentByMe = false, Username = "Tardis", IsWriting = true, Hint = hint};

            object msgModel = null;
            if (!string.IsNullOrEmpty(hint))
            {
                var hintModel = new HintMessageModel
                {
                    Message = content.ToString(), HintsVisible = true, Hints = hint.Split('|'),
                };
                hintModel.Command = new DelegateCommand(_ =>
                {
                    CreateUserMessage(_.ToString());
                    hintModel.HintsVisible = false;
                });

                msgModel = hintModel;
            }
            else
            {
                if (content is string s)
                {
                    msgModel = new SimpleMessageModel() {Message = s};
                }
                else if(content is Bitmap bmp)
                {
                    msgModel = new ImageMessageModel(bmp);
                }
                else if (content is UnknownRequestMessage ukmsg)
                {
                    msgModel = new UnknownMessageModel(ukmsg);
                }
            }

            msg.Content = msgModel;

            Messages.Add(msg);

            await Task.Delay(1500);

            msg.IsWriting = false;
        });
    }

    private void Send(object parameter)
    {
        if (_profanityFilter.HasProfanities(_message))
        {
            CreateBotMessage("Your message has profanities. Please be more polite.", null);
            return;
        }
        
        CreateUserMessage(Message);

        Message = null;
    }

    private void CreateUserMessage(string message)
    {
        Messages.Add(new() {SentByMe = true, Content = new SimpleMessageModel() {Message = message}, Username = "You"});

        _bot.Evaluate(message).Invoke();
    }
}
