using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Moq;

namespace UnityAutoMoq
{
    /// <summary>
    /// Builder strategy for for Unity Automq container.
    /// </summary>
    public class UnityAutoMoqBuilderStrategy : BuilderStrategy
    {
        private readonly UnityAutoMoqContainer autoMoqContainer;
        private readonly Dictionary<Type, object> mocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAutoMoqBuilderStrategy"/> class.
        /// </summary>
        /// <param name="autoMoqContainer">The auto moq container.</param>
        public UnityAutoMoqBuilderStrategy(UnityAutoMoqContainer autoMoqContainer)
        {
            this.autoMoqContainer = autoMoqContainer;
            mocks = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PreBuildUp(IBuilderContext context)
        {
            var type = context.OriginalBuildKey.Type;

            if (autoMoqContainer.Registrations.Any(r => r.RegisteredType == type))
                return;
            
            if (type.IsInterface || type.IsAbstract)
            {
                context.Existing = GetOrCreateMock(type);
                context.BuildComplete = true;
            }
        }
       
        private object GetOrCreateMock(Type t)
        {
            if (mocks.ContainsKey(t))
                return mocks[t];

            Type genericType = typeof(Mock<>).MakeGenericType(new[] { t });

            object mock = Activator.CreateInstance(genericType);

            AsExpression interfaceImplementations = autoMoqContainer.GetInterfaceImplementations(t);
            if(interfaceImplementations != null)
                interfaceImplementations.GetImplementations().Each(type => genericType.GetMethod("As").MakeGenericMethod(type).Invoke(mock, null));

            genericType.InvokeMember("DefaultValue", BindingFlags.SetProperty, null, mock, new object[] { autoMoqContainer.DefaultValue });

            object mockedInstance = genericType.InvokeMember("Object", BindingFlags.GetProperty, null, mock, null);
            mocks.Add(t, mockedInstance);

            return mockedInstance;
        }
    }
}