using System;
using Slithin.Core.Messaging;

namespace Slithin.Core.Services;

public interface IMailboxService
{
    void Init();

    void InitMessageRouter();

    void Post(AsynchronousMessage msg);

    void PostAction(Action p);
}