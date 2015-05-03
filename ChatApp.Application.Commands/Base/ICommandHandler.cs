namespace ChatApp.Application.Commands.Base
{
    public interface ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : new()
    {
        TResponse Handle(TCommand command);
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}
