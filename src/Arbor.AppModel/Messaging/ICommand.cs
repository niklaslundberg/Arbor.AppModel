using MediatR;

namespace Arbor.AppModel.Messaging
{
    public interface ICommand<out T> : IRequest<T> where T : ICommandResult
    {
    }
}