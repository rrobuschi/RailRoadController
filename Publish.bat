cd C:\Users\rrobu\CloudStation\Sviluppo\RailRoadController\RailRoadController
dotnet clean .
dotnet restore .
dotnet build .
dotnet publish . -r linux-arm
cd ..
pause
