using System.Collections.Generic;

namespace ChatApp.Application.Queries.Base
{
    public interface IQueryBus
    {
        List<TResult> List<TQuery, TResult>(TQuery query)
            where TQuery : IListQuery<TResult>
            where TResult : new();

        List<TResult> List<TQuery, TResult>()
            where TQuery : IListQuery<TResult>, new()
            where TResult : new();

        TResult Get<TQuery, TResult>(TQuery query)
            where TQuery : IGetQuery<TResult>
            where TResult : new();

    }
}