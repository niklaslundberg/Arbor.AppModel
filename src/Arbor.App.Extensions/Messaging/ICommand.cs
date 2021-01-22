using MediatR;

namespace Arbor.App.Extensions.Messaging
{
    public interface ICommand<out T> : IRequest<T> where T : ICommandResult
    {
    }
}