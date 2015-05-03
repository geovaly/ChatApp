using ChatApp.Application.Queries.Base;
using ChatApp.Application.Queries.ViewModels;

namespace ChatApp.Application.Queries
{
    public class UsersLimitReachedGetQuery : GetQuery<UsersLimitReachedGetQuery, BooleanResult>
    {
        public readonly string Username;

        public UsersLimitReachedGetQuery(string username)
        {
            Username = username;
        }
    }
}
