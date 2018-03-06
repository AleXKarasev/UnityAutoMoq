using System;
using System.Collections.Generic;

namespace UnityAutoMoq
{
    /// <summary>
    /// Provide AsExpression specified by the type parameter T
    /// </summary>
    public class AsExpression
    {
        private readonly List<Type> _implements = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AsExpression"/> class.
        /// </summary>
        /// <param name="implements">The implements.</param>
        public AsExpression(Type implements)
        {
            _implements.Add(implements);
        }

        /// <summary>
        /// Ases this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>AsExpression</returns>
        public AsExpression As<T>() where T : class
        {
            _implements.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Gets the implementations.
        /// </summary>
        /// <returns>enumerable types</returns>
        internal IEnumerable<Type> GetImplementations()
        {
            return _implements;
        }
    }
}