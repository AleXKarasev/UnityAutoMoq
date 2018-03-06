using System;

namespace UnityAutoMoq.Tests.TestClasses
{
    public class LazyService
    {
        public Func<IService> ServiceFunc;

        public LazyService(Func<IService> serviceFunc)
        {
            ServiceFunc = serviceFunc;
        }
    }
}