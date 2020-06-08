# Wingtip Toys - .NET Core Modernization Project

## **Overview**
This project is based on Microsoft's "Wingtip Toys Store" demo web portal written as a 3-Tier application leveraging .NET 4.5 with Webforms and SQL Server database for both data and user management.  
> A copy of that application can be found  here: [https://github.com/corn-pivotal/WingtipToys](https://github.com/corn-pivotal/WingtipToys).

## Approach
The goal is to leverage 12-factor application and Cloud Native principles to "decompose" the monolith. There are many ways to solve the same problem and these are just a few ideas to consider. Each project will implement different aspects leveraging .NET Core, Steeltoe, SQL Server, Spring Cloud Services, RabbitMQ, Redis, and other products. The aim is to allow all of the projects to be deployed to containers running in Cloud Foundry, Docker, or Kubernetes with pipelines initiated from AzureDevOps or Concourse.
