using MediatR;

namespace Arbor.AppModel.Messaging
{
    public interface IQuery<out T> : IRequest<T> where T : IQueryResult
    {
    }
}