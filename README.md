# Dynamics 365 Customer Engagement - Online Management API Client  

[![Build status](https://ci.appveyor.com/api/projects/status/5a8726c67ybu720p?svg=true)](https://ci.appveyor.com/project/shanec-/onlinemanagementapiclient)

A tool that can be used to interact with the Dynamics 365 Customer Engagement API.

## Usage

Execute the client using the following command line parameters:

​	`omac.exe <OperationName> --<Parameter1> "<ParameterValue1>" --<Parameter2> "<ParameterValue2>"`

Example:

​	`omac.exe GetInstances --username "admin@sndbx16.onmicrosoft.com" --password "Pass@word1"`

### Available Operations

Use the following command in order to get a list of available operations:

​	`omac.exe --help`

and the following command to get more information about a specific operation.

​	`omac.exe <operationName> --help`

| Operation            | Description                              |
| -------------------- | ---------------------------------------- |
| `GetInstances`       | Retrieves a Customer Engagement instance in your Office 365 tenant. |
| `CreateInstance`     | Provisions (creates) a Customer Engagement instance in your Office 365 tenant. |
| `DeleteInstance`     | Deletes a Customer Engagement instance in your Office 365 tenant. |
| `GetOperation`       | Retrieves status of an operation in your Customer Engagement instance. |
| `GetServiceVersions` | Retrieves information about all the supported releases for Customer Engagement. |
| `GetBackups`         | Retrieves all backups of a Customer Engagement instance. |
| `CreateBackup`       | Backs up a Customer Engagement instance. |
| `RestoreBackup`      | Restores a Customer Engagement instance in your Office 365 tenant. |