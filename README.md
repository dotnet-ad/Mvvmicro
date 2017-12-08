![Schema](./Documentation/Logo.png)

[![NuGet](https://img.shields.io/nuget/v/Mvvmicro.svg?label=NuGet)](https://www.nuget.org/packages/Mvvmicro/) [![Donate](https://img.shields.io/badge/donate-paypal-yellow.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ZJZKXPPGBKKAY&lc=US&item_name=GitHub&item_number=0000001&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted)

Minimalist MVVM framework for .NET.

## Install

Available on [NuGet](https://www.nuget.org/packages/Mvvmicro/).

## Usage

### Observable

A base implementation of `INotifyPropertyChanged` with helper method for raising property changes.

* `Set<T>(ref T field, T newValue, [auto] string propertyName)` : sets the value of the referenced field with the given new value and raises `PropertyChanged` even if its different value that the current one. An `Assignment<T>` will be returned to indicate the change status with `HasChanged`. If the value changed, this object also allows you in to raise other linked properties or commands that depends on the current one with `ThenRaise`.

```csharp
public class HomeViewModel : Observable
{
    private string firstname, lastname;
    
    public string Firstname
    {
    	get => this.firstname;
    	set => this.Set(ref this.firstname, value).ThenRaise(() => this.Fullname);
    }
    
    public string Lastname
    {
    	get => this.lastname;
    	set => this.Set(ref this.lastname, value).ThenRaise(() => this.Fullname);
    }
    
    public string Fullname => $"{Firstname} {Lastname}";
    
}
```

### ViewModelBase

An `Observable` object that adds navigation elements. This should be the base class for the view models associated to your application screens. A `NavigationRequested` event is available for your view to subscribe and react to.

```csharp
public class HomeViewModel : ViewModelBase
{
	//...
	
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

### Commands

The `(Async)RelayCommand` classes are helpers for creating `ICommand` synchronous and asynchronous implementations on the fly from methods. An alternate version `(Async)RelayCommand<T>` with a typed parameterer is also provided. 

The advantages of using `IAsyncRelayCommand` is :

* **Executing state** : `IsExecuting` property, that can be bound for updating your UI (activity indicators and so on).
* **Managing lifecycle** : `LastSuccededExecution` property, for managing lifecycle of display last successful execution time.
* **Failure management** : `ExecutionFailed` event, thrown whenever a
* **Cancellation** : `Cancel()` call will cancel the tokens given as a parameter of the execution method. 
* **Single execution** : when already running, a new execution can't be triggered.

**Example:**

```csharp
public class HomeViewModel : ViewModelBase
{
    public HomeViewModel()
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

### Dependency container

A deliberately basic container is provided for managing your dependencies. Declarations may seem too verbose compared to other popular IoC frameworks (*Autofac, Ninject, ...*), but **it doesn't rely on any reflection (which can has a cost in a mobile environment)** for injecting instances into newly created instances.

```csharp
Container.Default.Register<IApi>((c) => new WebApi(), isInstance: true); // A unique instance for entire lifecycle
Container.Default.Register((c) => new HomeViewModel(c.Get<IApi>())); // a new instance is created each time the type is requested

// ...

var homeViewModel = Container.Default.Get<HomeViewModel>();
```

### Observers

Sometimes you wish to listen to observable property changes : if you subscribe to `PropertyChanged` event, memory leaks could append since your observable will keep a reference to your observer in order to notify it. 

This is often solved by subscribing to this event when your observer becomes active (*i.e. when your page become visible, like `OnResume` in an Android activity*) and unsubcribing to it when the observable becomes inactive (*i.e. when your page disappears, like `OnPause` in an Android activity*). But what happens if your observable triggers property changes while your observer is inactive : **the pending modifications are never applied to it**! This can be deactivated through `ShouldTriggerPendingChanges` property.

The observer also manages initial values by triggering a change on all registered properties the first time. This can be deactivated through `ShouldTriggerInitialValues` property.

The `NotifyPropertyObserver` solves those requirements :

```csharp
public partial class HomeViewController : UIViewController
{
    public HomeViewController(IntPtr ptr) : base(ptr)
    {
       this.ViewModel = new HomeViewModel();
       this.observer = this.ViewModel.CreateObserver(this);
    }

    public HomeViewModel ViewModel { get; }

    private NotifyPropertyObserver<HomeViewController, HomeViewModel> observer;

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        this.observer
            .Observe(vm => vm.Fullname, (vm, value) => this.Title = value)
            .Start();
    }

    public override void ViewDidDisappear(bool animated)
    {
        base.ViewDidDisappear(animated);
        
        this.observer.Stop();
    }
}
```

## Complementary tools

Bellow, you will find a list of tools that can be used in combination with **Mvvmicro** to build great mobile or desktop application projects.

* [AutoFindViews](https://github.com/aloisdeniel/AutoFindViews) : Auto extract Xamarin.Android layout elements from declared identifiers.
* [StaticBind](https://github.com/aloisdeniel/StaticBind) : View data bindings for Xamarin.
* [Refit](https://github.com/paulcbetts/refit) : Rest client implementation generator.
* [LiteDB](http://www.litedb.org/) : Small embedded NoSQL database.
* [Xamarin Plugins](https://github.com/xamarin/XamarinComponents) : Various abstractions for platform specific behaviours

## Roadmap / Ideas

* Create a Fody task for generating properties.

## Why ?

I decided to create this small framework because other alternatives offer often too much stuff for me and include unused or duplicated parts. I also often find their navigation model to be not enough flexible. Finally, it's also a good starting point to learn MVVM to students. That's why this framework includes only the minimal bits I need for the majority of my developments.

## Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

![MIT © Aloïs](https://img.shields.io/badge/licence-MIT-blue.svg) 

© [Aloïs Deniel](http://aloisdeniel.github.io)
