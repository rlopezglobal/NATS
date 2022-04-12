pushd

Set-Location -Path "C:\Users\roberto.lopez\OneDrive - Material Handling Systems\Documents\MyData\Playground\Nats\nats-demo\src\NATS"
cd producer
dotnet build
Start-Process -FilePath "C:\Users\roberto.lopez\OneDrive - Material Handling Systems\Documents\MyData\Playground\Nats\nats-demo\src\NATS\producer\bin\Debug\net6.0\producer.exe"


cd ..\consumer
dotnet build
Start-Process -FilePath "C:\Users\roberto.lopez\OneDrive - Material Handling Systems\Documents\MyData\Playground\Nats\nats-demo\src\NATS\consumer\bin\Debug\net6.0\consumer.exe"

Start-Process -FilePath "C:\Users\roberto.lopez\OneDrive - Material Handling Systems\Documents\MyData\Playground\Nats\nats-demo\src\NATS\consumer\bin\Debug\net6.0\consumer.exe"

popd
