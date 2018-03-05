using System;
using System.Web;
using Moq;
using NUnit.Framework;
using Unity;

namespace UnityAutoMoq.Tests
{
    [TestFixture]
    public class UnityAutoMoqContainerFixture
    {
        private UnityAutoMoqContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = new UnityAutoMoqContainer();
        }

        [Test]
        public void Can_get_instance_without_registering_it_first()
        {
            var mocked = _container.Resolve<IService>();

            mocked.ShouldNotBeNull();
        }

        [Test]
        public void Can_get_mock()
        {
            Mock<IService> mock = _container.GetMock<IService>();

            mock.ShouldNotBeNull();
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same()
        {
            Mock<IService> mock = _container.GetMock<IService>();
            var mocked = _container.Resolve<IService>();

            mock.Object.ShouldBeSameAs(mocked);
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same_order_independent()
        {
            var mocked = _container.Resolve<IService>();
            Mock<IService> mock = _container.GetMock<IService>();

            mock.Object.ShouldBeSameAs(mocked);
        }

        [Test]
        public void Should_apply_default_default_value_when_none_specified()
        {
            _container = new UnityAutoMoqContainer();
            var mocked = _container.GetMock<IService>();

            mocked.DefaultValue.ShouldEqual(DefaultValue.Mock);
        }

        [Test]
        public void Should_apply_specified_default_value_when_specified()
        {
            _container = new UnityAutoMoqContainer(DefaultValue.Empty);
            var mocked = _container.GetMock<IService>();

            mocked.DefaultValue.ShouldEqual(DefaultValue.Empty);
        }

        [Test]
        public void Should_apply_specified_default_value_when_specified_2()
        {
            _container = new UnityAutoMoqContainer{DefaultValue = DefaultValue.Empty};
            var mocked = _container.GetMock<IService>();

            mocked.DefaultValue.ShouldEqual(DefaultValue.Empty);
        }

        [Test]
        public void Can_resolve_concrete_type_with_dependency()
        {
            var concrete = _container.Resolve<Service>();

            concrete.ShouldNotBeNull();
            concrete.AnotherService.ShouldNotBeNull();
        }

        [Test]
        public void Getting_mock_after_resolving_concrete_type_should_return_the_same_mock_as_passed_as_argument_to_the_concrete()
        {
            var concrete = _container.Resolve<Service>();
            Mock<IAnotherService> mock = _container.GetMock<IAnotherService>();

            concrete.AnotherService.ShouldBeSameAs(mock.Object);
        }

        [Test]
        public void Can_configure_mock_as_several_interfaces()
        {
            _container.ConfigureMock<IService>().As<IDisposable>();

            _container.GetMock<IService>().As<IDisposable>();
        }

        [Test]
        public void Can_configure_mock_as_several_interfaces_2()
        {
            _container.ConfigureMock<IService>().As<IDisposable>().As<IAnotherService>();

            _container.GetMock<IService>().As<IDisposable>();
            _container.GetMock<IService>().As<IAnotherService>();
        }

        [Test]
        public void Can_lazy_load_dependencies()
        {
            var service = _container.Resolve<LazyService>();

            Assert.That(service.ServiceFunc(), Is.InstanceOf<IService>());
        }

        [Test]
        public void Can_mock_abstract_classes()
        {
            var mock = _container.GetMock<HttpContextBase>();

            mock.ShouldBeOfType<Mock<HttpContextBase>>();
        }

        [Test]
        public void Can_inject_mocked_abstract_class()
        {
            var concrete = _container.Resolve<ServiceWithAbstractDependency>();
            var mock = _container.GetMock<HttpContextBase>();

            concrete.HttpContextBase.ShouldBeSameAs(mock.Object);
        }

        [Test]
        public void Can_get_registered_implementation()
        {
            _container.RegisterType<IAnotherService, AnotherService>();
            var real = _container.Resolve<IAnotherService>();

            real.ShouldBeOfType<AnotherService>();
        }

        [Test]
        public void GetStubMethod_ShouldReturn_TheSameMockedInstance()
        {
            _container = new UnityAutoMoqContainer(DefaultValue.Empty);
            var mocked = _container.GetMock<IService>();

            var stub = _container.GetStub<IService>();

            mocked.Object.ShouldBeSameAs(stub.Object);
        }
    }
}