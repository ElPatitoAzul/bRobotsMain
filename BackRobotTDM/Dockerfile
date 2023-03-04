FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
COPY publish /app
WORKDIR /app
EXPOSE 5004

ENTRYPOINT ["dotnet", "BackRobotTDM.dll"]