using System;
using System.Collections.Generic;
using Moq;
using Unity;

namespace UnityAutoMoq
{
    /// <summary>
    /// Define members for auto mocking functionality with Moq and Unity container.
    /// </summary>
    public class UnityAutoMoqContainer : UnityContainer
    {
        private readonly Dictionary<Type, AsExpression> _asExpressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAutoMoqContainer"/> class.
        /// </summary>
        public UnityAutoMoqContainer()
            : this(DefaultValue.Mock)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAutoMoqContainer"/> class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        public UnityAutoMoqContainer(DefaultValue defaultValue)
        {
            DefaultValue = defaultValue;
            AddExtension(new UnityAutoMoqExtension(this));

            _asExpressions = new Dictionary<Type, AsExpression>();
        }

        /// <summary>
        /// Default Value to be used for automocking
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public DefaultValue DefaultValue { get; set; }

        /// <summary>
        /// Provide an instance of the requested type with the given name from the container.
        /// Any associated virtual types have proxy instances generated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The retrieved instance</returns>
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        /// <summary>
        /// Provide a mock object of the given instance.
        /// Any associated virtual types have proxy instances generated.
        /// </summary>
        /// <typeparam name="T">The type paramater of the instance mock</typeparam>
        /// <returns>The mocked instance</returns>
        public Mock<T> GetMock<T>() where T : class
        {
            return Mock.Get(Resolve<T>());
        }

        /// <summary>
        /// Provide a stub object of the given instance.
        /// Any associated virtual types have proxy instances generated.
        /// </summary>
        /// <typeparam name="T">The type parameter of the instance to stub</typeparam>
        /// <returns>The mocked instance</returns>
        public Mock<T> GetStub<T>() where T : class
        {
            return Mock.Get(Resolve<T>());
        }

        /// <summary>
        /// Configures the mock.
        /// </summary>
        /// <typeparam name="T">The type parameter of the instance to configure the mock</typeparam>
        /// <returns></returns>
        public AsExpression ConfigureMock<T>()
        {
            var asExpression = new AsExpression(typeof(T));
            _asExpressions.Add(typeof(T),  asExpression);
            return asExpression;
        }

        internal AsExpression GetInterfaceImplementations(Type t)
        {
            return _asExpressions.ContainsKey(t) ? _asExpressions[t] : null;
        }
    }
}