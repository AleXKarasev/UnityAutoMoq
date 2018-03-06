namespace UnityAutoMoq.Tests.TestClasses
{
    public interface IService
    {
        IAnotherService AnotherService { get; set; }
        string PropertyWithoutSetter { get; }
        string Property { get; set; }
    }
}