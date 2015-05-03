namespace ChatApp.Application.Queries.Base
{
    public interface IGetQueryHandler<TQuery, TResult>
    {
        TResult Handle(TQuery query);
    }
}
