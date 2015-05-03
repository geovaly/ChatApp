namespace ChatApp.Application.Commands.Base
{
    public interface ICommand { }

    public interface ICommand<TResponse>
        where TResponse : new()
    {
    }
}