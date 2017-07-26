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

An `Observable` object that adds navigation elements. This should be the base class for the view models associated to your application screens. A `NavigationRequested` event is available for your view to subscribe and react to.

```csharp
public class HomeViewModel : ViewModelBase
{
    private void NavigateDetail(int id)
    {
    	// "/DayViewModel?id=467674"
    	this.Navigate<DayViewModel>((s) =>
		{
			s.Query.Set(day.Identifier, "id");
		});
    }
}
```

```csharp
public class HomeViewController : UIViewController
{
	// ...

	public HomeViewModel ViewModel { get; } = new HomeViewModel();

	public override void ViewDidAppear(bool animated)
	{
		base.ViewDidAppear(animated);
		this.ViewModel.NavigationRequested += OnNavigation;
	}
	
	public override void ViewWillDisappear(bool animated)
	{
		base.ViewDidAppear(animated);
		this.ViewModel.NavigationRequested -= OnNavigation;
	}
	
	private void OnNavigation(object sender, NavigationArgs nav)
	{
		this.PerformSegue(nav.Segment.Value, this);
	}
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
* [AutoFindViews](https://github.com/aloisdeniel/AutoFindViews) : Auto extract Xamarin.Android layout elements from declared identifiers.
* [StaticBind](https://github.com/aloisdeniel/StaticBind) : View data bindings for Xamarin.
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