using ChatApp.Application.Commands.Base;
using ChatApp.Utility;
using RestSharp;

namespace ChatApp.Application.RestSharp
{
    public class RestSharpCommandBus : RestSharpBase, ICommandBus
    {
        public RestSharpCommandBus(string serviceUrl)
            : base(serviceUrl)
        {
        }

        public TResponse Send<TCommand, TResponse>(TCommand command)
            where TCommand : ICommand<TResponse>
            where TResponse : new()
        {
            return Execute<TCommand, TResponse>(command, Method.POST);
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            Execute(command, Method.POST);
        }

        protected override void SetObject<TObject>(RestRequest request, TObject obj)
        {
            request.AddBody(obj);
        }

        protected override string GetResource<TObject>()
        {
            return "api/ChatRoom/" + typeof(TObject).Name.RemoveSubstring("Command");
        }
    }
}