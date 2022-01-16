# Project_xamarin_message
Cài đặt:
1. .NET Core Api

Chạy SQL Server
"ConnectionStrings": {
  "stringname":""
    Server=localDb/source;
    Database=HighSchoolDb;
    User ID=USERNAME;
    Password=PASSWORD;
    MultipleActiveResultSets=true
    "
 }

Configuration.GetConnectionString("stringname")

Cài đặt Entity Framework Core .NET Command-line Tools

dotnet tool install --global dotnet-ef

dotnet ef database update --context MessagingChatContext
Build solution
cd ../
dotnet build

sau khi build đổi port của máy tính bạn 
ipconfig 
sau đó vào
mặc định ban đầu là localhost:...

bạn đổi lại thành ip:...

sau đó build
2. Xamarin

