# Project xamarin message

## Hình Ảnh Demo về dự án
![Image](https://res.cloudinary.com/uit-information/image/upload/v1642806339/tutuanle/image/upload/MicrosoftTeams-image_1_qjupxn.png)
## Link Youtobe
[Nhấn vào đây để xem ](https://www.youtube.com/watch?v=NLgLuTEqc-s&t=7s)
## Link File Apk
[Nhấn vào đây để tải ](https://drive.google.com/file/d/1PgRPa7_xqOYd1evFrL5DD-5SLPFix6lX/view?usp=sharing)

### Cài đặt:
NET Core Api

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
Nếu đã build sql lên server vd như là somee.com thì config sẵn luôn trong `connectionString`
```
"ConnectionStrings": {
    "DbConnection": "
      workstation id=messagingchat.mssql.somee.com;
      packet size=4096;
      user id=tutuanle_SQLLogin_1;
      pwd=t8cdox39ct;
      data source=messagingchat.mssql.somee.com;
      persist security info=False;
      initial catalog=messagingchat"
  }
```

```C#
Configuration.GetConnectionString("connectionStrings")
```
Cài đặt `Entity Framework Core` .NET Command-line Tools

```
dotnet tool install --global dotnet-ef

```
hoặc là có thể vào `Manage Nuget Packages for Solution` để cài đặt
```
Microsoft.AspNetCore.SignalR.Core
```
```
Microsoft.AspNetCore.SignalR.Core
```
```
Microsoft.EntityFrameworkCore.Relational
```
```
Microsoft.EntityFrameworkCore.SqlServer
```
```
Microsoft.EntityFrameworkCore.Tools
```
```
Twilio
```
```
CloudinaryDotNet
```

![](https://res.cloudinary.com/uit-information/image/upload/v1642831593/tutuanle/image/upload/Screenshot_2022-01-22_130615_hl6yco.png)

Tiếp theo là cài sql server bằng file **MessengeChatTest.bak** hoặc **SQKMessaging.sql**


rồi vào lại visual studio chạy dòng lệnh
```
Scaffold-DbContext "Server=******;Database=******;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

Build solution
cd ../
dotnet build

sau khi build đổi port của máy tính bạn 
```
ipconfig 
```
sau đó vào
mặc định ban đầu là localhost:5000

bạn đổi lại thành ip:5000 vd Ip máy của mình là 192.168.1.8 
Nhấn vào **Project** -> **Properies** -> **Debug**
![Ảnh Minh Hoạ](https://res.cloudinary.com/uit-information/image/upload/v1642832122/tutuanle/image/upload/Screenshot_2022-01-22_131505_yuxizz.png)

sau đó build backend 

###2. Buid frontend
  vào **helpers/config** rồi chỉnh lại cái địa chỉ backend build
  ```c#
   public static string UrlWebsite = "************";
  ```
  
 
### Deploy lên backend lên somee.com

1. Đăng kí tài khoản
2. Sau đó vào MS SQL sau đó chọn **restore**
3. ![Ảnh Minh Hoạ](https://res.cloudinary.com/uit-information/image/upload/v1642808759/tutuanle/image/upload/Screenshot_2022-01-22_064539_c1o3oa.png)
4. Chọn upload and restore rồi chọn file ``MessengeChatTest.bak``
5. copy chuỗi **connection string** từ somee.com và dán vào backend ở **appsettings.json** 
6. ![](https://res.cloudinary.com/uit-information/image/upload/v1642809188/tutuanle/image/upload/Screenshot_2022-01-22_065254_q2ay5m.png)
7. Tạo mới một website 
8. ![](https://res.cloudinary.com/uit-information/image/upload/v1642809365/tutuanle/image/upload/Screenshot_2022-01-22_065545_hgknu7.png)
9. Vào backend sau đó **nhấn chuột phải vào project** -> **public** -> **Folder** 
10. Sau khi public thành công 
11. ![](https://res.cloudinary.com/uit-information/image/upload/v1642809673/tutuanle/image/upload/Screenshot_2022-01-22_070021_eknkml.png)
12. Vào thư mục nãy deploy rồi nén tất cả lại thành file `zip`
13. ![](https://res.cloudinary.com/uit-information/image/upload/v1642809801/tutuanle/image/upload/Screenshot_2022-01-22_070305_eexhmo.png)
14. Vào **somee.com** -> **websites**-> **website của bạn** ->**File manager** -> **Choose File** (``Chọn file nãy nén thành file zip``) -> **Upload and Unzip Archives** 
15. ![](https://user-images.githubusercontent.com/74090975/150615360-b608772a-3f30-48dc-bd65-5838ffff5668.png)
16. Để chạy fontend thì config lại cái **UrlWebsite** mà mình vừa tạo ở **helpers/config**

