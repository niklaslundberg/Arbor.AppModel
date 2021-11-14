using System;
using MediatR;

namespace Arbor.AppModel.Tests
{
    public interface IIdNotification : INotification
    {
        Guid Id { get; }
    }
}