using System;

namespace Arbor.AppModel.Validation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public sealed class NoValidationAttribute : Attribute
    {
    }
}