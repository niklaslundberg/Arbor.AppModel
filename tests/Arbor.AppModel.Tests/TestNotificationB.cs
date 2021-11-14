using System;

namespace Arbor.AppModel.Tests
{
    public class TestNotificationB : IIdNotification
    {
        public TestNotificationB(Guid id) => Id = id;

        public Guid Id { get; }

        public override string ToString() => base.ToString() + " " + Id;
    }
}