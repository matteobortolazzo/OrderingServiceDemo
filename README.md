# Ordering Demo service

Over-engineered .NET Core ordering API demo.

APIs:
* Create order;
* Confirm order;
* Get user's orders.

It follows the CQRS pattern, use EF Core, MediatR, FluentValidation and Dapper.

Not in the project: 
* Authentication;
* Authorization;
* Idempotency commands;
* Proper tables (Product/User).

## Build

Run `script/build.ps1`.
It buils *APIs* and *DB* projects.

Notes:
* SSDT must be installed;
* Add "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin" in the PATH environment variable.

## Test

Run `script/test.ps1`.

## Deploy

### DB

Run `Import-Module sql-migrations-module.psm1`.

Run `DeployDB '<Conn_String>' '<Path>\Ordering.DB.dacpac' '<DB_Name>`.

The path for the .dacpac is *\<AbsolutePath\>/Ordering.DB/bin/Release/Ordering.DB.dacpac*

### APIs

Copy the folder *bin/Release/netcoreapp2.2/publish* and deploy is any server that support .NET Core 2.2.