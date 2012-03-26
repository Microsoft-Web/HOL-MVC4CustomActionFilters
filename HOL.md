<a name="ASP.NET MVC Custon Action Filters">

﻿# ASP.NET MVC Custom Action Filter #
---

## Overview ##

>**Note:** This Hands-on Lab assumes you have basic knowledge of **ASP.NET MVC.** If you have not used **ASP.NET MVC** before, we recommend you to go over **ASP.NET MVC Fundamentals** Hand-on Lab.

ASP.NET MVC provides Action Filters for performing logic either before an action method is called or after its run. Action Filters are custom attributes that provide a declarative means to add pre-action and post-action behavior to controller action methods.

In this Hands-on Lab you will create a custom action filter attribute into MVC Music Store solution to catch controller's requests and log site actvity into a database table. You will be able to add your logging filter by injection to any controller or action.  Finally, you will see the log view that shows a visit list.

### Objectives ###

In this Hands-On Lab, you will learn how to:

- Create a custom action filter attribute to extend filtering capabilities

- Apply a custom filter attribute by injection to a specific level

 
### System Requirements ###

You must have the following items to complete this lab:

- Visual Studio 11 Express Beta for Web

	>**Note:** You can install the previous system requirements by using the Web Platform Installer 4.0:  http://www.microsoft.com/web/gallery/install.aspx?appid=VWD11_BETA&prerelease=true.

 
### Setup ###

_**Installing Code Snippets**_

For convenience, much of the code you will be managing along this lab is available as Visual Studio code snippets. To install the code snippets run **.\Source\Assets\CodeSnippets.vsi** file.

 
## Exercises ##

This Hands-On Lab is comprised by the following exercises:

1. Exercise 1: Logging actions

 
Estimated time to complete this lab: 20 minutes.

>**Note:** Each exercise is accompanied by an **End** folder containing the resulting solution you should obtain after completing the exercises. You can use this solution as a guide if you need additional help working through the exercises.
 
### Next Step ###

Click here to enter text.
### Exercise 1: Logging Actions ###

In this exercise, you will learn how to create a custom action log filter by using MVC3 Filter Providers.  For that purpose you will apply a logging filter to the MusicStore site that will record all the activities in the selected controllers.

The filter will extend **ActionFilterAttributeClass** and override **OnActionExecuting** method to catch each request and then perform the logging actions. The context information about HTTP requests, executing methods, results and parameters will be provided by MVC **ActionExecutingContext** class**.**

### About MVC Music Store Application logging feature ###

This Music Store solution has a new data model table for site logging, **ActionLog**, with the following fields: Name of the controller that received a request, Called action, Client IP and Time stamp.

 ![Data model. ActionLog table.](./images/Data-model.-ActionLog-table..png?raw=true "Data model. ActionLog table.")
 
_Data model. ActionLog table._

The solution provides an MVC View for the Action log that can be found at **MvcMusicStores/Views/ActionLog**:

 ![Action Log view](./images/Action-Log-view.png?raw=true "Action Log view")
 
_Action Log view_

With this given structure, all the work will be focused on interrupting controller's request and performing the logging by using custom filtering.

#### Task 1 - Creating a Custom Filter to Catch a Controller's Request ####

In this task you will create a custom filter attribute class that will contain the logging logic. For that purpose you will extend MVC **ActionFilterAttribute** Class and implement the interface **IActionFilter**.

> **Note: ActionFilterAttribute** is the base class for all the attribute filters. It provides the following methods to execute a specific logic after and before controller action's execution:**- OnActionExecuting(ActionExecutedContext** filterContext**)**

> Just before the action method is called**- OnActionExecuted(ActionExecutingContext** filterContext**):** 

> After the action method is called and before the result is executed (before view render).

> **- OnResultExecuting(ResultExecutingContext** filterContext**):** 

> Just before the result is executed (before view render).

> **- OnResultExecuted(ResultExecutedContext** filterContext**):** 

> After the result is executed (after the view is rendered).

> By overriding any of these methods into a derived class, you can execute your own filtering code.

1. Open the begin solution **MvcMusicStore.sln** at **Source\Ex01-Logging Actions\Begin**

1. Create a new folder **Filters** at project root, which will include all the custom filters. 

1. Add a new C# class into the **Filters** folder and rename it to **ActionLogFilterAttribute.cs**

1. Open **ActionLogFilterAttribute.cs** and add a reference to **System.Web.Mvc** and the **MvcMusicStore.Models** namespace:

	````C#
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using MvcMusicStore.Models;
	````

1. Inherit the **ActionLogFilterAttribute** class from **ActionFilterAttribute** and then make **ActionLogFilterAttribute** class implement **IActionFilter** interface.

	````C#
	...
	namespace MvcMusicStore.Filters
	{
	    public class ActionLogFilterAttribute : **ActionFilterAttribute**, **IActionFilter**
	    {
	        ...
	````

1. Make **ActionLogFilterAttribute** class override the method **OnActionExecuting,** where you will write the logging code. After that, your class should look like the following:

	_(Code Snippet - ASP.NET MVC 4 Custom Action Filters - Ex1 Logging Actions - CSharp)_

	````C#
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using MvcMusicStore.Models;
	
	namespace MvcMusicStore.Filters
	{
	    public class ActionLogFilterAttribute : ActionFilterAttribute, IActionFilter
	    {
	        public override void OnActionExecuting(ActionExecutingContext filterContext)
	        {
	            MusicStoreEntities storeDB = new MusicStoreEntities();
	
	            ActionLog log = new ActionLog()
	            {
	                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
	                Action = filterContext.ActionDescriptor.ActionName,
	                IP = filterContext.HttpContext.Request.UserHostAddress,
	                DateTime = filterContext.HttpContext.Timestamp
	            };
	
	            storeDB.AddToActionLogs(log);
	            storeDB.SaveChanges();
	
	            base.OnActionExecuting(filterContext);
	        }
	    }
	}
	````

	> **Note:**  **OnActionExecuting** method is using **Entity Framework** to add a new ActionLog register.  It creates and fills a new entity instance with the context information from **filterContext**. 

	> You could read more about **ControllerContext** class at [msdn](http://msdn.microsoft.com/en-us/library/system.web.mvc.controllercontext.aspx).

 
#### Task 2 - Injecting a Code Interceptor into the Store Controller Class ####

In this task you will add the custom filter by injecting it to all controller classes and controller actions that will be logged. For the purpose of this exercise, the Store Controller class will have a log.

The method **OnActionExecuting** from **ActionLogFilterAttribute** custom filter runs when an injected element is called.

It is also possible to intercept a specific controller method.

1. Open the **StoreController** at **MvcMusicStore\Controllers** and add a reference to the **Filters** namespace:

	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;
	
	namespace MvcMusicStore.Controllers
	{
	...
	````

1. Inject the custom filter **ActionLogFilter** into **StoreController** class. 

	````C#
	...
	using MvcMusicStore.ViewModels;
	using MvcMusicStore.Models;
	using MvcMusicStore.Filters;
	
	namespace MvcMusicStore.Controllers
	{
	    [ActionLogFilter]
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

	> **Note:** When a filter is injected into a controller class, all its actions are also injected. If you would like to apply the filter only for a set of actions, you would have to inject **[ActionLogFilter]** to each one of them:**[ActionLogFilter]**

	>     public ActionResult Index()

	>     {

	>        ...

	>     }

	>     ...

	> **[ActionLogFilter]**

	>     public ActionResult Browse(string genre)

	>     {

	>     ...

	>     }

 
#### Task 3 - Running the Application ####

In this task, you will test that the logging filter is working. You will start the application and visit the store, and then you will check logged activities.

1. Press **F5** to run the application.

1. Browse to **/ActionLog** to see log view initial state:

 	![Log tracker status before page activity](./images/Log-tracker-status-before-page-activity.png?raw=true "Log tracker status before page activity")
 
	_Log tracker status before page activity_

1. Browse to **/Store** and perform some actions there, like browsing an album detail.

1. Browse to **/ActionLog** and if the log is empty press **F5** to refresh the page. Check that your visits were tracked:

 	![Action log with activity logged](./images/Action-log-with-activity-logged.png?raw=true "Action log with activity logged")
 
	_Action log with activity logged_

 
### Next Step ###

Summary
 
## Summary ##

By completing this Hands-On Lab you have learned how to extend an action filter to execute custom actions. You have also learned how to inject any filter to your page controllers. The following concepts were used:

- How to create Custom Action filters with MVC ActionFilterAttribute class

- How to inject filters into MVC controllers
