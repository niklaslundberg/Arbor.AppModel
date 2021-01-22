using MediatR;

namespace Arbor.App.Extensions.Messaging
{
    public interface IQuery<out T> : IRequest<T> where T : IQueryResult
    {
    }
}