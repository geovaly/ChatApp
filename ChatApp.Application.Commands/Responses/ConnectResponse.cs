namespace ChatApp.Application.Commands.Responses
{
    public class ConnectResponse : Response
    {
        public bool FailedBecauseUserLimitReached { get; set; }

        public bool UserJoinsChatRoom { get; set; }
    }
}