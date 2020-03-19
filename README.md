# UnityAutoMoqContainer v1.3.0

Automocking container using Microsoft Unity and Moq.

[![Build status](https://ci.appveyor.com/api/projects/status/fldyc49w6enivpw9?svg=true)](https://ci.appveyor.com/project/AleXKarasev/unityautomoq)

## Download and install

UnityAutoMoq is available for download on [NuGet](https://www.nuget.org/packages/UnityAutoMoqContainer/).

To install it run the following command in the [NuGet Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console).

	PM> Install-Package UnityAutoMoqContainer
   
This will download all the binaries, and add necessary references to your project.

## Usage

### Creating an instance of the container

Creating the auto mock container is as simple as initializing a new instance of the UnityAutoMoqContainer class. This inherits from the UnityContainer, so all features will be available.

	var container = new UnityAutoMoqContainer();

### Resolving instances

Resolving a concrete class automatically creates mocks for the class dependencies and injects them before returning an instance of the class.

	Service service = container.Resolve<Service>();
	
Resolving an interface returns a mocked instance of that interface.

	IService mocked = container.Resolve<IService>();
	
### Getting the Mock

If you need access to the actual mock, calling GetMock<T> returns the mock on which you can do custom setup etc.

	Mock<IService> mock = container.GetMock<IService>();

### Implementing multiple interfaces

Sometimes you need to cast your mocked interface to some other type, e.g. IDisposable.  
This is done by

	container.ConfigureMock<IService>().As<IDisposable>();
	Mock<IDisposable> disposable = container.GetMock<IService>().As<IDisposable>();
	
## License

http://thedersen.mit-license.org/