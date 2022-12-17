$bffDir = ".\BackendForFrontend\bin\Release\net6.0"
$frontendDir = ".\Frontend"

Start-Process powershell -WorkingDirectory $bffDir -ArgumentList '-Command "& { dotnet BackendForFrontend.dll }"'
Start-Process powershell -WorkingDirectory $frontendDir -ArgumentList '-Command "& { yarn serve }"'
Start-Process "http://localhost:3000"
