using System;

namespace Arbor.AppModel.Tests;

public class TestNotificationA(Guid id) : IIdNotification
{
    public Guid Id { get; } = id;

    public override string ToString() => base.ToString() + " " + Id;
}