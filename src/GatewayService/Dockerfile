# get a base image to work from and we need the .Net SDK inside Docker to help us do that
# get it from a public repo where Microsfot stores its Docker images for things like the SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
# working directory inside Docker
WORKDIR /app
EXPOSE 80

# copy all .csproj files and restore as distinct layers.  Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
# taking the solution file from our development machine's file system and copying this file into our working directory inside Docker
COPY Carauctions.sln Carauctions.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj

# Restore package dependencies
RUN dotnet restore Carauctions.sln

# copy the app folders over
COPY src/GatewayService src/GatewayService

# change the directory into this particular folder
WORKDIR /app/src/GatewayService
# publish our auction service applicationa and the published files are gonna be stored in the out directory
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# switch directort back to app
WORKDIR /app
# take all the content in build and put it into the out directory of our current working directory
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "GatewayService.dll"]