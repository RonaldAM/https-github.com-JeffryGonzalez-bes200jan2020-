# Some Notes

- Add the RabbitMQUtils physically in the project, and replace the dll reference with a project reference.
- In the **root of the solution** add a .dockerignore file with the following entries:
```
*/bin
*/obj
.dockerignore
.env
.git
.gitignore
.vs
.vscode
**/.toolstarget
.idea
LibraryApiIntegrationTests
ApiClient
```
From the root of the solution, build the API project with `docker build -f LibraryApi/Dockerfile -t jeffgonzalez/libraryapi .`

## Sql Server

Create a folder in the solution called SqlServer
Create a Dockerfile

**Start With**
```
FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu

ENV SA_PASSWORD Tokyo!_Joe138
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express
```

You could use that to run a clean SQL Server (Express). 

`docker build -t db-demo .`

And then run it with:

`docker run -p 1433:1433 -d db-demo`

### Have it initialize the database.

First, get an initialization script. 

Install the ef tools: 

`dotnet tool install --global dotnet-ef`

In the LibraryApi folder run:

`dotnet ef migrations script > ../Sql/library.sql`

Open that file and delete the first couple of lines. 

Add the lines:

```
create database library
go

use library
go
```

Make an `entrypoint.sh` file (with just LF) with:

```
# Run Microsoft SQl Server and initialization script (at the same time)
/usr/src/app/run-initialization.sh & /opt/mssql/bin/sqlservr
```

Make an `run-initialization.sh` (with just LF) with:

```
# Wait to be sure that SQL Server came up
sleep 30s

# Run the setup script to create the DB and the schema in the DB
# Note: make sure that your password matches what is in the Dockerfile
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Tokyo!_Joe138 -d master -i create-database.sql
```
Build the docker image with:
`docker build  -t jeffgonzalez/librarysql .`




