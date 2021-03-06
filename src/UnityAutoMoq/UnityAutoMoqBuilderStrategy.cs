﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using Unity.Builder;
using Unity.Strategies;

namespace UnityAutoMoq
{
    /// <summary>
    /// Builder strategy for for Unity Automoq container.
    /// </summary>
    public class UnityAutoMoqBuilderStrategy : BuilderStrategy
    {
        private readonly UnityAutoMoqContainer _autoMoqContainer;
        private readonly Dictionary<Type, object> _mocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAutoMoqBuilderStrategy"/> class.
        /// </summary>
        /// <param name="autoMoqContainer">The auto moq container.</param>
        public UnityAutoMoqBuilderStrategy(UnityAutoMoqContainer autoMoqContainer)
        {
            _autoMoqContainer = autoMoqContainer;
            _mocks = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Called during the chain of responsibility for a build operation. The
        /// PreBuildUp method is called when the chain is being executed in the
        /// forward direction.
        /// </summary>
        /// <param name="context">Context of the build operation.</param>
        public override void PreBuildUp(ref BuilderContext context)
        {
            var type = context.Type;
            if (_autoMoqContainer.Registrations.Any(r => r.RegisteredType == type))
            {
                 return;
            }

            if (type.IsInterface || type.IsAbstract)
            {
                context.Existing = GetOrCreateMock(type);
                context.BuildComplete = true;
            }
        }

        private object GetOrCreateMock(Type t)
        {
            if (_mocks.ContainsKey(t))
            {
                return _mocks[t];
            }

            Type genericType = typeof(Mock<>).MakeGenericType(t);

            object mock = Activator.CreateInstance(genericType);

            AsExpression interfaceImplementations = _autoMoqContainer.GetInterfaceImplementations(t);
            interfaceImplementations?.GetImplementations().Each(type => genericType.GetMethod("As").MakeGenericMethod(type).Invoke(mock, null));

            genericType.InvokeMember("DefaultValue", BindingFlags.SetProperty, null, mock, new object[] { _autoMoqContainer.DefaultValue });

            object mockedInstance = genericType.InvokeMember("Object", BindingFlags.GetProperty, null, mock, null);
            _mocks.Add(t, mockedInstance);

            return mockedInstance;
        }
    }
}