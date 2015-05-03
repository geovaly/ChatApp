using System.Collections.Generic;

namespace ChatApp.Application.Queries.Base
{
    public interface IListQueryHandler<TQuery, TResult>
    {
        List<TResult> Handle(TQuery query);
    }
}
