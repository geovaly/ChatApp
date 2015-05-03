namespace ChatApp.Application.Commands.Base
{
    public interface ICommandBus
    {
        TResponse Send<TCommand, TResponse>(TCommand command)
            where TCommand : ICommand<TResponse>
            where TResponse : new();

        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}