using System;
using Arbor.App.Extensions.ExtensionMethods;
using Xunit;

namespace Arbor.App.Extensions.Tests.TypeExtensionsTests
{
    public class TypeExtensionsTests
    {
        [InlineData(typeof(TestTypeSub))]
        [Theory]
        public void FindPublicConcreteTypesImplementing_ShouldFindPublicInterfaceType(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().FindPublicConcreteTypesImplementing<ITestInterface>();
            Assert.Contains(type, types);
        }

        [InlineData(typeof(TestTypeSub))]
        [Theory]
        public void FindPublicConcreteTypesImplementing_ShouldFindPublicConcreteType(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().FindPublicConcreteTypesImplementing<TestTypeSub>();
            Assert.Contains(type, types);
        }

        [InlineData(typeof(TestTypeSub))]
        [Theory]
        public void FindPublicConcreteTypesImplementing_ShouldFindPublicType(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().FindPublicConcreteTypesImplementing<TestType>();
            Assert.Contains(type, types);
        }

        [InlineData(typeof(TestType))]
        [InlineData(typeof(InternalTestTypeSub))]
        [InlineData(typeof(string))]
        [Theory]
        public void FindPublicConcreteTypesImplementing_ShouldNotFindInvalidTypes(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().FindPublicConcreteTypesImplementing<TestType>();

            Assert.DoesNotContain(type, types);
        }

        [Fact]
        public void FindPublicConcreteTypesImplementing_ShouldReturnEmptyCollectionWhenNoImplementation()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                 .FindPublicConcreteTypesImplementing<INoImplementation>();

            Assert.Empty(types);
        }

        [Fact]
        public void IsConcrete_WhenInputIsAbstract_ShouldReturnFalse() => Assert.False(typeof(TestType).IsConcrete());

        [Fact]
        public void IsConcrete_WhenInputIsConcrete_ShouldReturnTrue() => Assert.True(typeof(TestTypeSub).IsConcrete());

        [Fact]
        public void IsConcrete_WhenInputIsInterface_ShouldReturnFalse() =>
            Assert.False(typeof(ITestInterface).IsConcrete());

        [Fact]
        public void IsConcrete_WhenInputIsNull_ShouldReturnFalse() => Assert.False(((Type)null!).IsConcrete());

        [Fact]
        public void IsConcreteTypeImplementing_ShouldThrowWhenInputIsNull() =>
            Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsConcreteTypeImplementing<ITestInterface>());

        [Fact]
        public void IsPublicConcreteTypeImplementing_ShouldThrowWhenInputIsNull() =>
            Assert.Throws<ArgumentNullException>(() => ((Type)null!).IsPublicConcreteTypeImplementing<ITestInterface>());
    }
}