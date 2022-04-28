using System;

namespace Arbor.AppModel.Configuration
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RegistrationOrderAttribute : Attribute
    {
        public RegistrationOrderAttribute(int order) => Order = order;

        public int Order { get; }
    }
}