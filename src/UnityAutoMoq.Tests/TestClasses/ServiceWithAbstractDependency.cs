using System.Web;

namespace UnityAutoMoq.Tests.TestClasses
{
    public class ServiceWithAbstractDependency
    {
        public HttpContextBase HttpContextBase { get; set; }

        public ServiceWithAbstractDependency(HttpContextBase httpContextBase)
        {
            HttpContextBase = httpContextBase;
        }
    }
}