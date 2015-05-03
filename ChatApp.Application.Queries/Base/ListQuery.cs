using System.Collections.Generic;

namespace ChatApp.Application.Queries.Base
{
    public abstract class ListQuery<TQuery, TResult> : IListQuery<TResult>
        where TResult : new()
        where TQuery : ListQuery<TQuery, TResult>
    {
        public List<TResult> List(IQueryBus bus)
        {
            return bus.List<TQuery, TResult>(this as TQuery);
        }
    }
}
