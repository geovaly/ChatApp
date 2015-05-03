namespace ChatApp.Application.Commands.Base
{
    public abstract class Command<TCommand, TResponse> : ICommand<TResponse>
        where TCommand : Command<TCommand, TResponse>
        where TResponse : new()
    {
        public TResponse Send(ICommandBus bus)
        {
            return bus.Send<TCommand, TResponse>(this as TCommand);
        }
    }
}
