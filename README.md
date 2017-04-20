![Schema](./Documentation/Logo.png)

Minimalist MVVM framework for .NET.

## Install

Available on NuGet

[![NuGet](https://img.shields.io/nuget/v/Mvvmicro.svg?label=NuGet)](https://www.nuget.org/packages/Mvvmicro/)

## Usage

### ViewModelBase

A base implementation of `INotifyPropertyChanged` with helper method for raising property changes and base navigation elements.

* `Set<T>(ref T field, T newValue, [auto] string propertyName)` : sets the value of the referenced field with the given new value and raises `PropertyChanged` even if its different value that the current one. An `Assignment<T>` will be returned to indicate the change status with `HasChanged`. If the value changed, this object also allows you in to raise other linked properties that depends on the current one with `ThenRaise(params string[] dependencies)` (*use* `nameof(<Name>)` *to avoid magic strings in your code*).

* `NavigateAsync(string url)` : triggers a navigation through the injected `INavigation`.

```csharp
public class HomeViewModel : ViewModelBase
{
    public HomeViewModel(INavigation nav) : base(nav) {}
    
    private string firstname, lastname;
    
    public string Firstname
    {
    	get { return this.firstname; }
    	set { this.Set(ref this.firstname, value).ThenRaise(nameof(Fullname))); }
    }
    
    public string Lastname
    {
    	get { return lastname; }
    	set { this.Set(ref lastname, value).ThenRaise(nameof(Fullname))); }
    }
    
    public string Fullname => $"{Firstname} {Lastname}";
    
}
```

### INavigation

The navigation abstract the navigation of your app through an interpreter of URLs. It's up to you to provide an implementation to your view models by implementing its two methods `NavigateAsync(NavigationUrl url)` and `NavigateBackAsync()`.


**Example:**

```csharp
public class UwpNavigation : INavigation
{
		public UwpNavigation(Frame frame)
		{
			this.frame = frame;
		}
		
		private Frame frame;

		public Task NavigateAsync(NavigationUrl url)
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

## Roadmap / Ideas

* Create a Fody task for generating properties.

## Why ?

I decided to create this small framework because other alternatives offer often too much stuff for me and include unused or duplicated parts (IoC, ...). I also often find their navigation model to be not enough flexible. Finally, it's also a good starting point to learn MVVM to students. That's why this framework includes only the minimal bits I need for the majority of my developments.

## Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

MIT © [Aloïs Deniel](http://aloisdeniel.github.io)