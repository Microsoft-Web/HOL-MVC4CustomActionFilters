# ASP.NET MVC 4 Custom Action Filters #

## Hands-on Lab ##

### Introduction ###

Hands-On Labs are sets of step-by-step guides that are designed to help you learn ASP.NET MVC 4.  Each Lab provides instructions to guide you through the process of developing a complete application.

In this Hands-on Lab, you will use custom filters to log your site activity. To achieve that, in the proposed exercise you will create a custom filter attribute that will work with any specified controller. That filter will catch controller's requests, and save them into a database table. Through a log view you will be able to see the list of visits that each filtered controller had.

### Repository Structure ###

In the **root** folder of this repository you will find the Hands-On Lab (HOL) document, **HOL.md**. Before beginning with the HOL exercises, make sure you have followed all the required steps indicated at the setup section of the HOL document. 

In the **Source** folder you will find the source code of each of the exercises, as well as the assets and setup scripts. Throughout the HOL you will be instructed to open and explore the different solutions from the source folder. It is typically comprised of the following subfolders:

- **Assets:** This folder contains files that are used throughout the exercises.
- **_Exercise Name_:** Each exercise that requires a programming solution has its own code folder.
  - **Begin:** The begin solution is the initial incomplete solution that you will finish by following the steps of the corresponding exercise.
  - **End:** The end solution is the final result you will achieve at the end of an exercise.
- **Setup:** This folder contains the dependency files and the setup scripts necessary to initialize specific configurations of the lab, being its execution is required in the majority of the Hands-on Labs.

### Get Started ###

In order to run the solutions of the exercises provided by this lab you will first need configure your environment and install any necessary prerequisites such as runtimes, components, or libraries. Each lab includes setup instructions for getting started.

### Contributing to the Repository ###

If you find any issues or opportunties for improving this hands-on lab, fix them!  Feel free to contribute to this project by [forking](http://help.github.com/fork-a-repo/) this repository and make changes to the content.  Once you've made your changes, share them back with the community by sending a pull request. Please see [How to send pull requests](http://help.github.com/send-pull-requests/) for more information about contributing to github projects.

### Reporting Issues ###

If you find any issues with this hands-on lab that you can't fix, feel free to report them in the [issues](https://github.com/Microsoft-Web/HOL-MVC4CustomActionFilters/issues) section of this repository.