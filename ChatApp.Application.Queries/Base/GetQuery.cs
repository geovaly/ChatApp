namespace ChatApp.Application.Queries.Base
{
    public abstract class GetQuery<TQuery, TResult> : IGetQuery<TResult>
        where TResult : new()
        where TQuery : GetQuery<TQuery, TResult>
    {
        public TResult Get(IQueryBus bus)
        {
            return bus.Get<TQuery, TResult>(this as TQuery);
        }
    }
}
