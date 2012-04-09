<a name="HOLTitle" />
# ASP.NET MVC 4 Custom Action Filters #
---

<a name="Overview" />
## Overview ##

ASP.NET MVC provides Action Filters for executing filtering logic either before or after an action method is called. Action Filters are custom attributes that provide declarative means to add pre-action and post-action behavior to the controller's action methods.

In this Hands-on Lab you will create a custom action filter attribute into MVC Music Store solution to catch controller's requests and log the activity of a site into a database table. You will be able to add your logging filter by injection to any controller or action.  Finally, you will see the log view that shows the list of visitors.

> **Note:** This Hands-on Lab assumes you have basic knowledge of **ASP.NET MVC.** If you have not used **ASP.NET MVC** before, we recommend you to go over **ASP.NET MVC Fundamentals** Hands-on Lab.

<a name="Objectives" />
### Objectives ###

In this Hands-On Lab, you will learn how to:

- Create a custom action filter attribute to extend filtering capabilities

- Apply a custom filter attribute by injection to a specific level

<a name="SystemRequirements" /> 
### System Requirements ###

You must have the following items to complete this lab:

- Visual Studio 11 Express Beta for Web

<a name="Setup" /> 
### Setup ###

**Installing Code Snippets**

For convenience, much of the code you will be managing along this lab is available as Visual Studio code snippets. To install the code snippets run **.\Source\Assets\CodeSnippets.vsi** file.

---

<a name="Exercises" /> 
## Exercises ##

This Hands-On Lab is comprised by the following exercises:

1. [Exercise 1: Logging actions](#Exercise1)
1. [Exercise 2: Managing Multiple Action Filters](#Exercise2)

 
Estimated time to complete this lab: **30 minutes**.

>**Note:** Each exercise is accompanied by an **End** folder containing the resulting solution you should obtain after completing the exercises. You can use this solution as a guide if you need additional help working through the exercises.

<a name="Exercise1" /> 
### Exercise 1: Logging Actions ###

In this exercise, you will learn how to create a custom action log filter by using MVC 4 Filter Providers.  For that purpose you will apply a logging filter to the MusicStore site that will record all the activities in the selected controllers.

The filter will extend **ActionFilterAttributeClass** and override **OnActionExecuting** method to catch each request and then perform the logging actions. The context information about HTTP requests, executing methods, results and parameters will be provided by MVC **ActionExecutingContext** class**.**

> **Note:** MVC 4 also has default filters providers you can use without creating a custom filter. MVC 4 provides the following types of filters:
> 
> - **Authorization** filter, which makes security decisions about whether to execute an action method, such as performing authentication or validating properties of the request. 
> - **Action** filter, which wraps the action method execution. This filter can perform additional processing, such as providing extra data to the action method, inspecting the return value, or canceling execution of the action method
> - **Result** filter, which wraps execution of the ActionResult object. This filter can perform additional processing of the result, such as modifying the HTTP response. 
> - **Exception** filter, which executes if there is an unhandled exception thrown somewhere in action method, starting with the authorization filters and ending with the execution of the result. Exception filters can be used for tasks such as logging or displaying an error page.
>
>For more information about Filters Providers please visit this MSDN link: (http://msdn.microsoft.com/en-us/library/dd410209.aspx) .

<a name="AboutLoggingFeature" /> 
#### About MVC Music Store Application logging feature ####

This Music Store solution has a new data model table for site logging, **ActionLog**, with the following fields: Name of the controller that received a request, Called action, Client IP and Time stamp.

 ![Data model. ActionLog table.](./images/Data-model.-ActionLog-table..png?raw=true "Data model. ActionLog table.")
 
_Data model - ActionLog table_

The solution provides an MVC View for the Action log that can be found at **MvcMusicStores/Views/ActionLog**:

 ![Action Log view](./images/Action-Log-view.png?raw=true "Action Log view")
 
_Action Log view_

With this given structure, all the work will be focused on interrupting controller's request and performing the logging by using custom filtering.

<a name="Ex1Task1" />
#### Task 1 - Creating a Custom Filter to Catch a Controller's Request ####

In this task you will create a custom filter attribute class that will contain the logging logic. For that purpose you will extend MVC **ActionFilterAttribute** Class and implement the interface **IActionFilter**.

> **Note:** The **ActionFilterAttribute** is the base class for all the attribute filters. It provides the following methods to execute a specific logic after and before controller action's execution:
>
>- **OnActionExecuting**(ActionExecutedContext filterContext): Just before the action method is called.
>- **OnActionExecuted**(ActionExecutingContext filterContext): After the action method is called and before the result is executed (before view render).
>- **OnResultExecuting**(ResultExecutingContext filterContext): Just before the result is executed (before view render).
>- **OnResultExecuted**(ResultExecutedContext filterContext): After the result is executed (after the view is rendered).
>
> By overriding any of these methods into a derived class, you can execute your own filtering code.

1. Open the begin solution **MvcMusicStore.sln** at **Source\Ex01-LoggingActions\Begin**

1.	Follow these steps to install the **NuGet** package dependencies.

	1.	Open the **NuGet** **Package Manager Console**. To do this, select **Tools | Library Package Manager | Package Manager Console**.

	1.	In the **Package Manager Console,** type **Install-Package NuGetPowerTools**.

	1.	After installing the package, type **Enable-PackageRestore**.

	1.	Build the solution. The **NuGet** dependencies will be downloaded and installed automatically.

	>**Note:** One of the advantages of using NuGet is that you don't have to ship all the libraries in your project, reducing the project size. With NuGet Power Tools, by specifying the package versions in the Packages.config file, you will be able to download all the required libraries the first time you run the project. This is why you will have to run these steps after you open an existing solution from this lab.
	
	>For more information, see this article: <http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages>.

1. Create a new folder **Filters** at project root, which will include all the custom filters. 

1. Add a new C# class into the **Filters** folder and rename it to **CustomActionFilter.cs**

1. Open **CustomActionFilter.cs** and add a reference to **System.Web.Mvc** and the **MvcMusicStore.Models** namespace:

	(Code Snippet - _MVC 4 Custom Action Filters - Ex1-CustomActionFilterNamespaces_)

	<!-- mark:5-6 -->
	````C#
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using MvcMusicStore.Models;
	````

1. Inherit the **CustomActionFilter** class from **ActionFilterAttribute** and then make **CustomActionFilter** class implement **IActionFilter** interface.
	
	<!-- mark:4 -->
	````C#
	...
	namespace MvcMusicStore.Filters
	{
	    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
	    {
	        ...
	````

1. Make **CustomActionFilter** class override the method **OnActionExecuting** and add the necessary logic to log the filter's execution. To do this, add the following highlighted code within **CustomActionFilter** class.

	(Code Snippet - _MVC 4 Custom Action Filters - Ex1-LoggingActions_)

	<!-- mark:6-26 -->
	````C#	
	...
	namespace MvcMusicStore.Filters
	{
		 public class CustomActionFilter : ActionFilterAttribute, IActionFilter
		 {

			  void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
			  {
					// TODO: Add your acction filter's tasks here

					// Log Action Filter Call
					MusicStoreEntities storeDB = new MusicStoreEntities();

					ActionLog log = new ActionLog()
					{
						 Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
						 Action = filterContext.ActionDescriptor.ActionName + " (Logged By: Custom Action Filter)",
						 IP = filterContext.HttpContext.Request.UserHostAddress,
						 DateTime = filterContext.HttpContext.Timestamp
					};

					storeDB.AddToActionLog(log);
					storeDB.SaveChanges();

					base.OnActionExecuting(filterContext);
			  }
		 }
	}
	````

	> **Note:**  **OnActionExecuting** method is using **Entity Framework** to add a new ActionLog register.  It creates and fills a new entity instance with the context information from **filterContext**. 

	> You can read more about **ControllerContext** class at [msdn](http://msdn.microsoft.com/en-us/library/system.web.mvc.controllercontext.aspx).

<a name="Ex1Task2" />
#### Task 2 - Injecting a Code Interceptor into the Store Controller Class ####

In this task you will add the custom filter by injecting it to all controller classes and controller actions that will be logged. For the purpose of this exercise, the Store Controller class will have a log.

The method **OnActionExecuting** from **ActionLogFilterAttribute** custom filter runs when an injected element is called.

It is also possible to intercept a specific controller method.

1. Open the **StoreController** at **MvcMusicStore\Controllers** and add a reference to the **Filters** namespace:
	
	<!-- mark:4 -->
	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;
	
	namespace MvcMusicStore.Controllers
	{
	...
	````

1. Inject the custom filter **CustomActionFilter** into **StoreController** class. 

	<!-- mark:8 -->
	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;
	
	namespace MvcMusicStore.Controllers
	{
	    [CustomActionFilter]
	    public class StoreController : Controller
	    {
	        MusicStoreEntities storeDB = new MusicStoreEntities();
	        //
	        // GET: /Store/
	
	        public ActionResult Index()
	        {
	            // Create list of genres
	...
	````

	> **Note:** When a filter is injected into a controller class, all its actions are also injected. If you would like to apply the filter only for a set of actions, you would have to inject **[CustomActionFilter]** to each one of them:
	>
	>````C#
	> [CustomActionFilter]
	> public ActionResult Index()
	> {
	> 	...
	> }
	> 
	> [CustomActionFilter]
	> public ActionResult Browse(string genre)
	> {
	> 	...
	> }
	> ````


<a name="Ex1Task3" />
#### Task 3 - Running the Application ####

In this task, you will test that the logging filter is working. You will start the application and visit the store, and then you will check logged activities.

1. Press **F5** to run the application.

1. Browse to **/ActionLog** to see log view initial state:

 	![Log tracker status before page activity](./images/Log-tracker-status-before-page-activity.png?raw=true "Log tracker status before page activity")
 
	_Log tracker status before page activity_

	> **Note:** For simplicity purposes we're truncating the **ActionLog** table each time the application runs so it will only show the logs of each particular task's verification.
	>
	> You might need to remove the following code from the **Application_Start** method (in the **Global.asax** class), in order to save an historical log for all the actions executed within the Store Controller.
	>
	> ````C#
	> // Clean up Logs Table
	> MusicStoreEntities storeDB = new MusicStoreEntities();
	> storeDB.ExecuteStoreCommand("TRUNCATE TABLE [ActionLog]");
	> ````

1. Browse to **/Store** and perform some actions there, like browsing a Genre's list of available albums.

1. Browse to **/ActionLog** and if the log is empty press **F5** to refresh the page. Check that your visits were tracked:

 	![Action log with activity logged](./images/Action-log-with-activity-logged.png?raw=true "Action log with activity logged")
 
	_Action log with activity logged_


<a name="Exercise2" />
### Exercise 2: Managing Multiple Action Filters ###

In this exercise you will add a second Custom Action Filter to the StoreController class and define the specific order in which both filters will be executed.

There are different options to take into account when defining the Filters' execution order. For example, the Order property and the Filters' scope:

You can define a **Scope** for each of the Filters, for example, you could scope all the Action Filters to run within the **Controller Scope**, and all Authorization Filters to run in **Global scope**. The scopes have a defined execution order.

Additionally, each action filter has an Order property which is used to determine the execution order in the scope of the filter.

For more information about Custom Action Filters execution order, please visit this MSDN article: (http://msdn.microsoft.com/en-us/library/dd381609.aspx).

<a name="Ex2Task1" />
#### Task 1: Creating a new Custom Action Filter ####

In this task, you will create a new Custom Action Filter to inject into the StoreController class, learning how to manage the execution order of the filters.

1. Open the begin solution **MvcMusicStore.sln** at **Source\Ex02-ManagingMultipleAF\Begin**. You can also continue using the End solution you obtained by completing Exercise 1.

1.	If you opened the **Begin** solution from **Source\Ex02-ManagingMultipleAF\**, follow these steps to install the **NuGet** package dependencies.

	1.	Open the **NuGet** **Package Manager Console**. To do this, select **Tools | Library Package Manager | Package Manager Console**.

	1.	In the **Package Manager Console,** type **Install-Package NuGetPowerTools**.

	1.	After installing the package, type **Enable-PackageRestore**.

	1.	Build the solution. The **NuGet** dependencies will be downloaded and installed automatically.

		>**Note:** One of the advantages of using NuGet is that you don't have to ship all the libraries in your project, reducing the project size. With NuGet Power Tools, by specifying the package versions in the Packages.config file, you will be able to download all the required libraries the first time you run the project. This is why you will have to run these steps after you open an existing solution from this lab.
	
		>For more information, see this article: <http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages>.

1. Add a new C# class into the **Filters** folder and rename it to **MyNewCustomActionFilter.cs**

1. Open **MyNewCustomActionFilter.cs** and add a reference to **System.Web.Mvc** and the **MvcMusicStore.Models** namespace:

	(Code Snippet - _MVC 4 Custom Action Filters - Ex2-MyNewCustomActionFilterNamespaces_)

	<!-- mark:5-6 -->
	````C#
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using MvcMusicStore.Models;
	````

1. Replace the default class declaration with the following code.

	(Code Snippet - _MVC 4 Custom Action Filters - Ex2-MyNewCustomActionFilterClass_)

	<!-- mark:1-23 -->
	````C#
	public class MyNewCustomActionFilter : ActionFilterAttribute, IActionFilter
	{
	  void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
	  {
			// TODO: Add your acction filter's tasks here

			// Log Action Filter Call
			MusicStoreEntities storeDB = new MusicStoreEntities();

			ActionLog log = new ActionLog()
			{
				 Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
				 Action = filterContext.ActionDescriptor.ActionName + " (Logged By: MyNewCustomActionFilter)",
				 IP = filterContext.HttpContext.Request.UserHostAddress,
				 DateTime = filterContext.HttpContext.Timestamp
			};

			storeDB.AddToActionLog(log);
			storeDB.SaveChanges();

			base.OnActionExecuting(filterContext);
	  }
	}
	````

	> **Note:** This Custom Action Filter is almost the same than the one you created in the previous exercise. The main difference is that it has the _"Logged By"_ attribute updated with this new class' name to identify wich filter registered the log.

<a name="Ex2Task2" />
#### Task 2: Injecting a new Code Interceptor into the StoreController Class ####

In this task, you will add a new custom filter into the StoreController Class and run the solution to verify how both filters work together.

1. Open the **StoreController** class located at **MvcMusicStore\Controllers** and inject the new custom filter **MyNewCustomActionFilter** into **StoreController** class like is shown in the following code.

	<!-- mark:8 -->
	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;

	namespace MvcMusicStore.Controllers
	{
		 [MyNewCustomActionFilter]
		 [CustomActionFilter]
		 public class StoreController : Controller
		 {
			  MusicStoreEntities storeDB = new MusicStoreEntities();

			  //
			  // GET: /Store/

			  public ActionResult Index()
			  {
	...
	````

1. Now, run the application in order to see how these two Custom Action Filters work. To do this, press **F5** and wait until the application starts.

1. Browse to **/ActionLog** to see log view initial state.

 	![Log tracker status before page activity](./images/Log-tracker-status-before-page-activity.png?raw=true "Log tracker status before page activity")
 
	_Log tracker status before page activity_

1. Browse to **/Store** and perform some actions there, like browsing a Genre's list of available albums.

1. Check that this time; your visits were tracked twice: once for each of the Custom Action Filters you added in the **StorageController** class.

 	![Action log with activity logged](./images/Action-log-with-activity-logged2.png?raw=true "Action log with activity logged")
 
	_Action log with activity logged_


<a name="Ex2Task3" />
#### Task 2: Managing Filter Ordering ####

In this task, you will learn how to manage the filters' execution order by using the Order propery.

1. Open the **StoreController** class located at **MvcMusicStore\Controllers** and specify the **Order** property in both filters like shown below.

	<!-- mark:8-9 -->
	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;

	namespace MvcMusicStore.Controllers
	{
		 [MyNewCustomActionFilter (Order = 2)]
		 [CustomActionFilter (Order = 1)]
		 public class StoreController : Controller
		 {
			  MusicStoreEntities storeDB = new MusicStoreEntities();

			  //
			  // GET: /Store/

			  public ActionResult Index()
			  {
	...
	````

1. Now, verify how the filters are executed depending on its Order property's value. You will find that the filter with the smallest Order value (**CustomActionFilter**) is the first one that is executed. Press **F5** and wait until the application starts.

1. Browse to **/ActionLog** to see log view initial state.

 	![Log tracker status before page activity](./images/Log-tracker-status-before-page-activity.png?raw=true "Log tracker status before page activity")
 
	_Log tracker status before page activity_

1. Browse to **/Store** and perform some actions there, like browsing a Genre's list of available albums.

1. Check that this time, your visits were tracked ordered by the filters' Order value: **CustomActionFilter** logs' first.

 	![Action log with activity logged](./images/Action-log-with-activity-logged3.png?raw=true "Action log with activity logged")
 
	_Action log with activity logged_

1. Now, you will update the Filters' order value and verify how the logging order changes. In the **StoreController** class, update the Filters' Order value like shown below.

	<!-- mark:8-9 -->
	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;

	namespace MvcMusicStore.Controllers
	{
		 [MyNewCustomActionFilter (Order = 1)]
		 [CustomActionFilter (Order = 2)]
		 public class StoreController : Controller
		 {
			  MusicStoreEntities storeDB = new MusicStoreEntities();

			  //
			  // GET: /Store/

			  public ActionResult Index()
			  {
	...
	````

1. Run the application again by pressing **F5**.

1. Browse to **/Store** and perform some actions there, like browsing a Genre's list of available albums.

1. Check that this time, the logs created by **MyNewCustomActionFilter** filter appear first.

 	![Action log with activity logged](./images/Action-log-with-activity-logged4.png?raw=true "Action log with activity logged")
 
	_Action log with activity logged_

---

<a name="Summary" />
## Summary ##

By completing this Hands-On Lab you have learned how to extend an action filter to execute custom actions. You have also learned how to inject any filter to your page controllers. The following concepts were used:

- How to create Custom Action filters with the MVC ActionFilterAttribute class

- How to inject filters into MVC controllers

- How to manage filter ordering using the Order property
