# Setup
[//]: # (This page provides information about how to run, deploy, and build this component)

***

## Prerequisites
[//]: # (List any secure/git accesses, credentials, libraries, and software that this component relies on)

Java 17
Apache Maven 3.5.3
IntelliJ IDE
Lombok plugin for IntelliJ

***

## Configuring the Component
[//]: # (List steps for getting the component configured locally)

### Configure IntelliJ
There is some initial setup required in IntelliJ:

Install the Lombok plugin for IntelliJ
Under Preferences > Build, Execution, Deployment > Compiler > Annotation Processors, check the box for Enable annotation processing
Under Preferences > Build, Execution, Deployment > Build Tools > Maven > Runner
Check the box for Delegate IDE build/run action to Maven
Check the box for Skip tests (otherwise every compile will run the test suite)
Without this configuration, the code won't build properly in IntelliJ.

### Configure the Application
You will run the application using the local profile. The checked-in application.yml has defaults for this profile. However, if you prefer, you can also create a file application-local.yml to make your own changes, for instance to increase or decrease the logging level:

logging:
level:
com.optum.ecp: DEBUG
Do not check this file into Git.

***

## Building the Component
[//]: # (List steps for building the component)

Set the current working directory of the terminal to source code location.
Compile the code by executing
mvn clean package

***

## Running the Component
[//]: # (List steps for running this component locally)

You can run the application from the command line using Maven, or from IntelliJ. To run the command line from Maven, use the following command:

$ mvn spring-boot:run -Dspring-boot.run.profiles=local

Use CTRL-C to interrupt the application.


To run the application from IntelliJ requires a few extra steps:

Open IntelliJ IDE
Navigate to the package com.optum.ecp.sample
Right click on SampleApplication.java and choose Run SampleApplication
Immediately stop the application when it starts booting
Go to Run > Edit Configurations and set active profile local
Re-run the application, which will now start with the right profile and configuration

***

## Deploying the Component
[//]: # (List steps for deploying the component)

[Github Actions](https://github.com/optum-ecp/pims-ui-webapp/actions)

***

## Integrating the Component
[//]: # (List steps for using this component in another service or instance)

None
