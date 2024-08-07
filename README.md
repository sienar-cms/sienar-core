# Sienar
## An application development framework built on top of ASP.NET and .NET MAUI

Sienar is a framework for building desktop and web applications. When building web applications with Sienar, a small CMS is also available.

## Philosophy

Sienar is designed to allow the developer to build hookable and modular applications. The basic design philosophy was loosely inspired by WordPress' hooks and plugin systems; Sienar offers similar functionality via a variety of strongly-typed interfaces, as opposed to WordPress' system of magic strings.

### Sienar Web

Sienar web apps can be built using various features of ASP.NET, such as server-rendered MVC apps, REST APIs, and Blazor WASM (coming soon). Other types of applications can be built by hacking on top of the core Sienar functionality. If you want to use a Sienar-provided backend but provide your own client-side code, you're free to do so. No official packages for JS clients like Vue or React currently exist.

### Sienar Native (coming soon)

Sienar native applications for desktop and mobile can be built using .NET MAUI's Blazor Hybrid (code is WIP but not yet published). Other solutions can be hacked together using core Sienar functionality.

### Sienar CMS

Sienar can be used as a series of reusable components to build specific types of applications, but by including the CMS plugin, it becomes a fully functional CMS.

## Sienar core design

There are currently 3 packages that supply the core Sienar functionality.

### Sienar.Utils

The `Sienar.Utils` package supplies the foundation for Sienar. It contains all the code that is shared among different types of Sienar applications. Virtually everal Sienar package relies on `Sienar.Utils`.

[//]: # (### Sienar.Utils.Blazor)

[//]: # ()
[//]: # (The `Sienar.Utils.Blazor` package provides some Blazor WASM-specific utilities that can be used by any Blazor WASM application &#40;both web and .NET MAUI Blazor Hybrid&#41;. This package is separate from `Sienar.UI.Blazor` because `Sienar.UI.Blazor` is currently dependent on MudBlazor, which many developers don't want to use.)

[//]: # ()
[//]: # (### Sienar.UI.Blazor)

[//]: # ()
[//]: # (The `Sienar.UI.Blazor` package provides a Blazor WASM UI library. Currently, this UI library is built on top of MudBlazor, but in the future, this might be switched to a custom, fully-themeable UI library.)

### Sienar.SqliteUtils

SQLite is a robust data storage solution, and for many types of apps - including both small web apps and most mobile/desktop apps - SQLite is perfectly fine to use in production. The `Sienar.SqliteUtils` package provides two straightforward utility methods. The first simply calls `DbContextOptionsBuilder.UseSqlite()` with a correctly formatted `Data Source` string. The second backs up the existing database and migrates the database to the latest version, and rolls back to the previous version if there was an error.

### Sienar.WebPlugin

The `Sienar.WebPlugin` package provides the necessary configuration to run a Sienar app using ASP.NET Core. By using the `Ssr` plugin, you get a traditional MVC app with server-side rendered Razor code. By using the `Rest` plugin, you get MVC controllers as REST endpoints (not Minimal APIs). 

[//]: # (### Sienar.WasmPlugin)

[//]: # ()
[//]: # (The `Sienar.WasmPlugin` package does the same thing as `Sienar.WebPlugin`, but instead of configuring Sienar for use in an MVC app, it configures Sienar for use as a Blazor WASM application.)

[//]: # ()
[//]: # (### Sienar.MauiPlugin)

[//]: # ()
[//]: # (The `Sienar.MauiPlugin` does the same thing as `Sienar.WasmPlugin` and `Sienar.WebPlugin`, but for .NET MAUI Blazor Hybrid apps.)

## Other resources

The documentation can be found at [sienar.io](https://sienar.io).

### Plugins

- the [Sienar CMS plugin](https://github.com/sienar-cms/plugin-cms) provides a full CMS
- the [MailKit plugin](https://github.com/sienar-cms/plugin-mailkit) provides email capabilities via MailKit

### Architecture

Sienar uses separate packages for supplying certain architecture-specific functionality. For example, instead of tying developers to a specific technology like EntityFramework, developers can choose to implement the `IRepository` interface using whatever backend they choose. Some common implementations have already been created for you, and only need to be plugged into your app.

The [REST architecture package](https://github.com/sienar-cms/architecture-rest) supplies `IRepository` implementations that use REST APIs as the backing store. This can be useful for desktop or mobile clients that need to store information remotely on a web server.

The [EntityFramework architecture package](https://github.com/sienar-cms/architecture-ef) supplies `IRepository` implementations that use EntityFramework as the backing store. This is appropriate for most web apps that use EntityFramework, as well as many desktop and mobile clients that need to store information locally in a SQLite database.