using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebAPI.Models
{
    public partial class MessengeChatTestContext : DbContext
    {
        public MessengeChatTestContext()
        {
        }

        public MessengeChatTestContext(DbContextOptions<MessengeChatTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetailUserGroup> DetailUserGroups { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<MessageRecipient> MessageRecipients { get; set; }
        public virtual DbSet<Messaging> Messagings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=TANH\\SQLEXPRESS;Database=MessengeChatTest;Integrated Security=True");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DetailUserGroup>(entity =>
            {
                entity.HasKey(e => e.DetailId)
                    .HasName("PK__detailUs__83077839F8D99E7B");

                entity.ToTable("detailUserGroup");

                entity.Property(e => e.DetailId).HasColumnName("detailID");

                entity.Property(e => e.AddUserId).HasColumnName("addUserID");

                entity.Property(e => e.NickNameGuest)
                    .HasMaxLength(60)
                    .HasColumnName("nickNameGuest");

                entity.Property(e => e.FriendKey)
                    .HasMaxLength(250)
                    .HasColumnName("FriendKey");

                entity.Property(e => e.UserGroupId).HasColumnName("userGroupID");

                entity.HasOne(d => d.AddUser)
                    .WithMany(p => p.DetailUserGroups)
                    .HasForeignKey(d => d.AddUserId)
                    .HasConstraintName("fk_useraddid");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.DetailUserGroups)
                    .HasForeignKey(d => d.UserGroupId)
                    .HasConstraintName("fk_usergroupid");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("friend");

                entity.Property(e => e.FriendId).HasColumnName("friendID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.UserIdfriend).HasColumnName("userIDFriend");

                entity.Property(e => e.status).HasColumnName("status");
         
                  entity.Property(e => e.FriendKey)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FriendKey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FriendUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_usersourceid");

                entity.HasOne(d => d.UserIdfriendNavigation)
                    .WithMany(p => p.FriendUserIdfriendNavigations)
                    .HasForeignKey(d => d.UserIdfriend)
                    .HasConstraintName("fk_usertargetid");
            });

            modelBuilder.Entity<MessageRecipient>(entity =>
            {
                entity.HasKey(e => e.RecipientId)
                    .HasName("PK__messageR__A9B8B542BC874A6C");

                entity.ToTable("messageRecipients");

                entity.Property(e => e.RecipientId).HasColumnName("recipientID");

                entity.Property(e => e.MessageId).HasColumnName("messageID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageRecipients)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("fk_messageid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MessageRecipients)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_messagefrom");
            });

            modelBuilder.Entity<Messaging>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK__messagin__4808B873679FD2F0");

                entity.ToTable("messaging");

                entity.Property(e => e.MessageId).HasColumnName("messageID");

                entity.Property(e => e.AttachedFiles)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("attachedFiles");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.DateRead)
                    .HasColumnType("datetime")
                    .HasColumnName("dateRead");

                entity.Property(e => e.DateSent)
                    .HasColumnType("datetime")
                    .HasColumnName("dateSent");

                entity.Property(e => e.FromUserId).HasColumnName("fromUserID");



                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.Messagings)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("fk_messageto");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.ActivatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("activatedDate");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("address1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("address2");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birthDate");

                entity.Property(e => e.City)
                    .HasMaxLength(60)
                    .HasColumnName("city");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(60)
                    .HasColumnName("companyName");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(60)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(60)
                    .HasColumnName("lastName");

                entity.Property(e => e.ImgURL)
                    .HasMaxLength(250)
                    .HasColumnName("ImgURL");
                
                entity.Property(e => e.Passwordd)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("passwordd");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.UserIdtype).HasColumnName("userIDtype");

                entity.HasOne(d => d.UserIdtypeNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserIdtype)
                    .HasConstraintName("fk_usertype");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("userGroup");

                entity.Property(e => e.UserGroupId).HasColumnName("userGroupID");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.NickNameowner)
                    .HasMaxLength(60)
                    .HasColumnName("nickNameowner");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_userid");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("userType");

                entity.Property(e => e.UserTypeId).HasColumnName("userTypeID");

                entity.Property(e => e.Typeuser)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("typeuser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
