using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using Syn.Bot.Oscova.Messages;

namespace Slithin.Modules.Bot.Dialogs;

public class GreetingsDialog : Dialog
{
    [Event("start")]
    public static void Start(Context context, Result result)
    {
        if (result.Bot.Settings.Contains("Name"))
        {
            var response = new Response();
            response.Messages.Add(new TextMessage(){ Texts = new()
            {
                "Welcome back, $bot.Name",
                "Howdy, $bot.Name",
                "How can I help you, $bot.Name?"
            }});
            
            result.SendResponse(response);
        }
        else
        {
            context.Add("init.name");
            
            result.SendResponse("Hello, my name is Tardis. I am your helper for Slithin.");
            result.SendResponse("What is your name?");
        }
    }
    
    [Expression("{text}")]
    [Context("init.name")]
    [Entity(Sys.Text)]
    public static void InitName(Context context, Result result)
    {
        result.Bot.Settings.Add("Name", result.Entities.OfType(Sys.Text).Value);

        context.Clear();
        result.SendResponse("Hello $bot.Name.");
    }

    [Expression("Call me @sys.text")]
    [Expression("Change my name to @sys.text")]
    public static void ChangeName(Context context, Result result)
    {
        result.Bot.Settings.Remove("Name");
        result.Bot.Settings.Add("Name", result.Entities.OfType(Sys.Text).Value);
        
        result.SendResponse("Ok, Now I will call you $bot.Name");
    }
    
    [Fallback]
    public static void Fallback(Context context, Result result)
    {
        var response = new Response();
        response.Text = "I have no awsner for your question.";
        //response.Messages.Add(new ImageMessage(){ Url = "avares://ChatbotTest/Assets/unknown.png"});
        
        result.SendResponse(response);
    }
}
