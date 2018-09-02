![Schema](./Documentation/Logo.png)

Minimalist MVVM framework for .NET.

## Install

Available on NuGet

[![NuGet](https://img.shields.io/nuget/v/Mvvmicro.svg?label=NuGet)](https://www.nuget.org/packages/Mvvmicro/)

## Usage

### Observable

A base implementation of `INotifyPropertyChanged` with helper method for raising property changes.

* `Set<T>(ref T field, T newValue, [auto] string propertyName)` : sets the value of the referenced field with the given new value and raises `PropertyChanged` even if its different value that the current one. An `Assignment<T>` will be returned to indicate the change status with `HasChanged`. If the value changed, this object also allows you in to raise other linked properties or commands that depends on the current one with `ThenRaise` (*use* `nameof(<Name>)` *to avoid magic strings in your code*).

```csharp
public class HomeViewModel : Observable
{
    private string firstname, lastname;
    
    public string Firstname
    {
    	get { return this.firstname; }
    	set { this.Set(ref this.firstname, value).ThenRaise(nameof(Fullname))); }
    }
    
    public string Lastname
    {
    	get { return lastname; }
    	set { this.Set(ref this.lastname, value).ThenRaise(nameof(Fullname))); }
    }
    
    public string Fullname => $"{Firstname} {Lastname}";
    
}
```

### ViewModelBase

An `Observable` object that adds navigation elements. This should be the base class for the view models associated to your application screens.

* `NavigateBackCommand` : a command that triggers `NavigateBackAsync` on the view model `INavigation`.

```csharp
public class HomeViewModel : ViewModelBase
{
    public HomeViewModel(INavigation nav) : base(nav) {}
    
    private async Task NavigateDetailAsync(int id)
    {
    	// ...
    	await this.Navigation.NavigateAsync($"/Details?id={id}");
    }
    
    private Task NavigateBackAsync()
    {
    	// ...
    	this.NavigateBackCommand().TryExecute(null);
    }
}
```

### Navigation router

#### INavigationRouter

The router abstracts the navigation of your app through an interpreter of `NavigationUrl`. It's up to you to provide an implementation to your view models by implementing its two methods `NavigateToAsync(NavigationUrl url)` and `NavigateBackAsync()`.

**Example:**

```csharp
public class UwpNavigation : INavigation
{
		public UwpNavigation(Frame frame)
		{
			this.frame = frame;
		}
		
		private Frame frame;

		public bool CanNavigateBack => this.frame.CanGoBack;

		public Task NavigateToAsync(NavigationUrl url)
		{
			var segment = url.Segments.First();
			var pageType = Type.GetType($"MyApp.Pages.{segment.Value}Page, MyApp.Pages"); 
			this.frame.Navigate(pageType, segment.Query);
			return Task.FromResult(true);
		}

		public Task NavigateBackAsync()
		{
			this.frame.GoBack();
			return Task.FromResult(true);
		}
}
```

Then, to trigger a navigation from your ViewModel layer, use the navigate method from the router.

```csharp
this.Navigation.NavigateToAsync($"/Product?id={5}");
```

```csharp
var url = new NavigationUrl("/Product").AddArg("id",5)
this.Navigation.NavigateToAsync(url);
```

#### Navigation url matching

To create more complex interpreter, several matching methods are also available from `NavigationUrl`.

```csharp
var url = new NavigationUrl("/Products/Details?v1=4&v2=6");
if(url.Match("/Products/Details")) // True
{
	// ...
}
if(url.StartsWith("/Products")) // True
{
	// ...
}
```

#### NavigationRouter

A more complete implementation of `INavigationRouter` is also provided for helping with url matching mapped to method execution. 

Each route method should be registered through a `RouteAttribute` that decorates you method.

```csharp
public class SampleRouter : NavigationRouter
{
	[DefaultRoute]
	public void NavigateToDefault() 
	{
		// When no other router matches, you can provide a default route.
		// It's a right place to show a popup to the user
	}

	[Route("/")]
	public void NavigateToRoot() 
	{ 
		// Reset view to the root view of your app and reset your navigation state.
	}

	[Route("/tab1/**")]
	public void NavigateToSubroute([SubRoute]NavigationUrl suburl) 
	{ 
		// With a '**' ending segment, all urls starting like the registered url will match
		// The suburl parameter will be the remaining url ("/tab1/detail" => "/detail")
		// You can have a sub navigation router by tab for example
		// Note the 'SubRoute' attribute that indicates which parameters will be the subroute
	}

	[Route("/detailWithParameters")]
	public void NavigateToDetailWithParameters(int id, string description) 
	{ 
		// parameters will be extracted from query string 
		// example: "/detailWithParameters?id=5&description=sample"
		//          => { id = 5, description = "sample" }
	}

	[Route("/detailWithQuery")]
	public void NavigateToDetailWithQuery(NavigationUrlQuery query) 
	{ ing description) 
	{ 
		// the query string of the last segment will be given
		// you can then extract arguments like 'query.Get<int>("id")'
	}

	[Route("/detailWithUrl")]
	public void NavigateToDetailWithUrl(NavigationUrl url) 
	{ 
		// the entire matched url will be given as parameter
	}

	[Route("/detailWithPath/{id}/{description}")]
	public void NavigateToDetailWithPath(int id, string description) 
	{ 
		// parameters will be extracted from query string 
		// example: "/detailWithParameters/5/sample"
		//          => { id = 5, description = "sample" }
	}
	
	public override bool CanNavigateBack
	{
	   get { /* Indicates whether a back navigation is available */ }
	}
	
	public override Task NavigateBackAsync() { /* Cancel previous navigation */ }
}

```

### Relay commands

The `(Async)RelayCommand` classes are helpers for creating `ICommand` synchronous and asynchronous implementations on the fly from methods. An alternate version `(Async)RelayCommand<T>` with a typed parameterer is also provided. 

**Example:**

```csharp
public class HomeViewModel : ViewModelBase
{
    public HomeViewModel(INavigation nav) : base(nav)
    {
    	this.UpdateCommand = new AsyncRelayCommand(ExecuteUpdateCommandAsync);
    }
    
    public AsyncRelayCommand UpdateCommand { get; }
    
    private Task ExecuteUpdateCommandAsync(CancellationToken token)
    {
        // ...
    }
}
```

## Complementary tools

Bellow, you will find a list of tools that can be used in combination with **Mvvmicro** to build great mobile or desktop application projects.

* [Autofac](https://autofac.org/) : Inversion of control.
* [Wires](https://github.com/aloisdeniel/Wires) : View bindings for Xamarin.
* [Refit](https://github.com/paulcbetts/refit) : Rest client implementation generator.
* [LiteDB](http://www.litedb.org/) : Small embedded NoSQL database.
* [Xamarin Plugins](https://github.com/xamarin/XamarinComponents) : Various abstractions for platform specific behaviours

## Roadmap / Ideas

* Create a Fody task for generating properties.

## Why ?

I decided to create this small framework because other alternatives offer often too much stuff for me and include unused or duplicated parts (IoC, ...). I also often find their navigation model to be not enough flexible. Finally, it's also a good starting point to learn MVVM to students. That's why this framework includes only the minimal bits I need for the majority of my developments.

## Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

![MIT © Aloïs](https://img.shields.io/badge/licence-MIT-blue.svg) 

© [Aloïs Deniel](http://aloisdeniel.github.io)