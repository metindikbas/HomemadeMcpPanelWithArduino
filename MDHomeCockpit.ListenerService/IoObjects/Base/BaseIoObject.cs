using CommandMessenger;

namespace MDHomeCockpit.ListenerService.IoObjects.Base
{
    public abstract class BaseIoObject
    {
        protected readonly CmdMessenger _cmdMessenger;

        protected BaseIoObject(CmdMessenger cmdMessenger)
        {
            _cmdMessenger = cmdMessenger;
        }
    }
}
