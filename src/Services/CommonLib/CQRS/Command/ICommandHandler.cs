namespace CommonLib.CQRS.Command
{
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : class
    {
        Task<TResponse> Handle( TCommand command );
    }
}