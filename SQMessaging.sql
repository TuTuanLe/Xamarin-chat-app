create database MessengeChatTest
use MessengeChatTest


create table users(
userID int identity(1,1) primary key,
userIDtype int,
active tinyint,
activatedDate datetime,
companyName nvarchar(60),
firstName nvarchar(60),
lastName nvarchar(60),
passwordd varchar(60),
birthDate datetime,
address1 varchar(100),
address2 varchar(100),
city nvarchar(60),
phone varchar(12)
)

go
create table userGroup(
userGroupID int identity(1,1) primary key,
userID int,
isAdmin tinyint
)
alter table userGroup add nickNameowner nvarchar(60)
go
create table detailUserGroup(
detailID int identity(1,1) primary key,
userGroupID int,
addUserID int,
nickNameGuest nvarchar(60)
)
go
create table userType(
userTypeID int identity(1,1) primary key,
typeuser varchar(15)
)
go
create table friend(
friendID int identity(1,1) primary key,
userID int,
userIDFriend int
)
go
create table messageRecipients(
recipientID int identity(1,1) primary key,
messageID int,
userID int
)
go
create table messaging(
messageID int identity(1,1) primary key,
fromUserID int,
dateSent datetime,
dateRead datetime,
content ntext,
attachedFiles varchar(255)
)
--Tạo khóa ngoại
alter table users add constraint fk_usertype foreign key (userIDtype) references userType (userTypeID)
alter table userGroup add constraint fk_userid foreign key (userID) references users (userID)
alter table detailUserGroup add constraint fk_usergroupid foreign key (userGroupID) references userGroup (userGroupID)
alter table detailUserGroup add constraint fk_useraddid foreign key (addUserID) references users (userID)
alter table friend add constraint fk_usersourceid foreign key (userID) references users (userID)
alter table friend add constraint fk_usertargetid foreign key (userIDFriend) references users (userID)
alter table messageRecipients add constraint fk_messageid foreign key (messageID) references messaging (messageID)
alter table messageRecipients add constraint fk_messagefrom foreign key (userID) references users (userID)
alter table messaging add constraint fk_messageto foreign key (fromUserID) references users (userID)


select * from users

insert into friend values (5, 1, '1551', 1);


select * from friend

insert into messageRecipients values (1, 2);

alter table  messaging add friendID int 

select * from  messaging 

select m.messageID from   messaging m , friend f
where  f.friendID = m.friendID and f.userID = 1

alter table friend add friendKey nvarchar(250)	   

alter table friend add status int 

update friend set status = 1 where friendID = 1

alter table messaging alter column friendID nvarchar(250)	


select * from users

select * from friend f ,users u 
where f.userID = u.userID and u.userID=1

where friendID ='1221'
order by dateSent desc

alter table users add ImgURL nvarchar(250) 

select m.content from messaging m 
where m.friendID = '1221' 
order by m.dateSent desc


select * from messaging
update messaging set content ='recording 0:2' where messageID = 272

select * from users 

select * from friend
delete from messaging where messageID = 313
alter table detailUserGroup add friendKey nvarchar(250)	  

select * from detailUserGroup 

select * from friend

select * from Users u 
  join  friend f 
on u.userID = f.userID
group by u.userID
select * from users u
where u.userID = ( select top 1  f.userIDFriend 
					from friend f
					where f.userID = 1
				)



select * from users
where phone = '0396694769'

select * from messaging