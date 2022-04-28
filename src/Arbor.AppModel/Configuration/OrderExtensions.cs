using System.Reflection;

namespace Arbor.AppModel.Configuration
{
    public static class OrderExtensions
    {
        public static int GetRegistrationOrder(this object? instance, int defaultOrder)
        {
            if (instance is null)
            {
                return defaultOrder;
            }

            var attribute = instance.GetType().GetCustomAttribute<RegistrationOrderAttribute>();

            if (attribute is null)
            {
                return defaultOrder;
            }

            return attribute.Order;
        }
    }
}