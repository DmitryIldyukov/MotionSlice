namespace CommonLib.CQRS.Query
{
    public interface IQueryHandler<in TQuery, TResponse>
        where TQuery : class
        where TResponse : class
    {
        Task<TResponse> Handle( TQuery query );
    }
}