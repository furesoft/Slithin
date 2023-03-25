using Syn.Bot.Oscova;

namespace Slithin.Modules.Bot;

public class UnknownRequestMessage : Syn.Bot.Oscova.Messages.TextMessage
{
    public Request ResultRequest { get; }

    public UnknownRequestMessage(Request resultRequest)
    {
        ResultRequest = resultRequest;
    }
}
