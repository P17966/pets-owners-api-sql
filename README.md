# Pet & Owners 3.00 #

## UPDATE 3.0 ##
**NEW FEATURES**
UPDATED TO USE DOCKER!

How it was done:

Following Matters were needed to make this happen:

**DOCKERFILE**
In dockerfile we added few lines:

*This line tells Docker first what program needs and where to get it.*
*This time it is SDK 8.0 from Microsoft for the BUILD*
`FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build`

*After that next line will tell WORKDIR*
`WORKDIR /app`

*COPY will copy the csproj and then RUN will restore all dependency*
`COPY *.csproj ./`
`RUN dotnet restore`

*COPY will copy rest of the files and RUN will build the app*
`COPY . ./`
`RUN dotnet publish -c Release -o out`

**NOW WE START THE PARTS FOR RUNNING THE APP**

*This will tell Docker what it needs for running it*
`FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime`

*Again Work Directory*
`WORKDIR /app`

*Will COPY software from BUILD phase*
`COPY --from=build /app/out .`

*EXPOSE will open the port 5000*
`EXPOSE 5000`

*ENTRYPOINT will start the software*
`ENTRYPOINT ["dotnet","pets-owners-api-sql.dll"]`

**CONSOLE COMMANDS**

Commands needed for the console to run this are following:

*This will build the Docker and give it a tag to use.*
`docker build -t mimimalapi .`

*This will run the docker container in 5000 port*
*To specify more first 5000 is port of the client and next 5000 is port of the container*
`docker run -p  5000:5000 mimimalapi`

### IMPORTANT CHANGE TO CODE ###

To make this all work was needed to make a change to the program.cs file.
Following change was made: 
`builder.WebHost.UseUrls("http://0.0.0.0:5000");`

This line was added so that the API will use always port 5000. Otherwise it will give random ports from 5000.