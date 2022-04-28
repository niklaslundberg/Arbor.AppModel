using System;

namespace Arbor.AppModel.Tests
{
    public class TestNotificationA : IIdNotification
    {
        public TestNotificationA(Guid id) => Id = id;

        public Guid Id { get; }

        public override string ToString() => base.ToString() + " " + Id;
    }
}