using Slithin.Controls;
using Slithin.Messages;

namespace Slithin.Core.MessageHandlers;

public class AttentionRequiredMessageHandler : IMessageHandler<AttentionRequiredMessage>
{
    public async void HandleMessage(AttentionRequiredMessage message)
    {
        var result = await DialogService.ShowDialog(message.Text);

        if (result)
        {
            message.Action(message);
        }
    }
}