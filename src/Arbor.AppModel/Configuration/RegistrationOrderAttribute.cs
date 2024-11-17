using System;

namespace Arbor.AppModel.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RegistrationOrderAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}