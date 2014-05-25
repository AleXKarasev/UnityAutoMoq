using System;
using System.Collections.Generic;

namespace UnityAutoMoq
{
    /// <summary>
    /// Provide AsExpression spefied by the type paramter T
    /// </summary>
    public class AsExpression
    {
        private readonly List<Type> implements = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AsExpression"/> class.
        /// </summary>
        /// <param name="implements">The implements.</param>
        public AsExpression(Type implements)
        {
            this.implements.Add(implements);
        }

        /// <summary>
        /// Ases this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>AsExpression</returns>
        public AsExpression As<T>() where T : class
        {
            implements.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Gets the implementations.
        /// </summary>
        /// <returns>enumerable types</returns>
        internal IEnumerable<Type> GetImplementations()
        {
            return implements;
        }
    }
}