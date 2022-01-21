# Project xamarin message

## Hình Ảnh Demo về dự án
![Image](https://res.cloudinary.com/uit-information/image/upload/v1642806339/tutuanle/image/upload/MicrosoftTeams-image_1_qjupxn.png)
## Link Youtobe
[Nhấn vào đây để xem ](https://www.youtube.com/watch?v=NLgLuTEqc-s&t=7s)

### Cài đặt:
1. .NET Core Api

Chạy SQL Server
```
"ConnectionStrings": {
  "stringname":""
    Server=localDb/source;
    Database=HighSchoolDb;
    User ID=USERNAME;
    Password=PASSWORD;
    MultipleActiveResultSets=true
    "
 }
```
Configuration.GetConnectionString("stringname")

Cài đặt Entity Framework Core .NET Command-line Tools

```
dotnet tool install --global dotnet-ef
```

```
dotnet ef database update --context MessagingChatContext
```

Build solution
cd ../
dotnet build

sau khi build đổi port của máy tính bạn 
```
ipconfig 
```
sau đó vào
mặc định ban đầu là localhost:...

bạn đổi lại thành ip:...

sau đó build
2. Xamarin

