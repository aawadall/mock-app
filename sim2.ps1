Start-Process -FilePath .\Trigger\bin\Release\net5.0\Trigger.exe -ArgumentList "localhost:29092 inbox http://localhost:3000/api/logs D:\Projects\Docker\exp30\mock_app\Trigger\payload.dat "
Write-Host "Sleeping to allow message propagation"
Start-Sleep -Seconds 1
Start-Process -FilePath .\Node\bin\Release\net5.0\Node.exe -ArgumentList "localhost:29092 inbox mid-box-1 http://localhost:3000/api/logs     "
Start-Process -FilePath .\Node\bin\Release\net5.0\Node.exe -ArgumentList "localhost:29092 mid-box-1 mid-box-2 http://localhost:3000/api/logs "
Start-Process -FilePath .\Node\bin\Release\net5.0\Node.exe -ArgumentList "localhost:29092 mid-box-2 mid-box-3 http://localhost:3000/api/logs "
Start-Process -FilePath .\Node\bin\Release\net5.0\Node.exe -ArgumentList "localhost:29092 mid-box-3 mid-box-4 http://localhost:3000/api/logs "
Start-Process -FilePath .\Node\bin\Release\net5.0\Node.exe -ArgumentList "localhost:29092 mid-box-4 mid-box-5 http://localhost:3000/api/logs "
Start-Process -FilePath .\Node\bin\Release\net5.0\Node.exe -ArgumentList "localhost:29092 mid-box-5 mid-box-3 http://localhost:3000/api/logs "
