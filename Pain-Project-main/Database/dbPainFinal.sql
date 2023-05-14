USE [master]
GO


IF EXISTS (SELECT*FROM SYS.DATABASES WHERE NAME = 'dbPain')
begin
alter database dbPain set single_user with rollback immediate
DROP DATABASE dbPain
end
GO

/****** Object:  Database [dbPain]    Script Date: 07.02.2022 20:02:31 ******/
CREATE DATABASE [dbPain]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbf', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\dbPain.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dbf_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\dbPain_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [dbPain] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbPain].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dbPain] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dbPain] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dbPain] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dbPain] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dbPain] SET ARITHABORT OFF 
GO
ALTER DATABASE [dbPain] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dbPain] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dbPain] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dbPain] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dbPain] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dbPain] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dbPain] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dbPain] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dbPain] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dbPain] SET  DISABLE_BROKER 
GO
ALTER DATABASE [dbPain] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dbPain] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dbPain] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dbPain] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dbPain] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dbPain] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dbPain] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dbPain] SET RECOVERY FULL 
GO
ALTER DATABASE [dbPain] SET  MULTI_USER 
GO
ALTER DATABASE [dbPain] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dbPain] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dbPain] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dbPain] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [dbPain] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [dbPain] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'dbPain', N'ON'
GO
ALTER DATABASE [dbPain] SET QUERY_STORE = OFF
GO
USE [dbPain]
GO
/****** Object:  Table [dbo].[tb_Administrator]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Administrator](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[AccountCreation] [datetime] NOT NULL,
	[Admin] [bit] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Surname] [nvarchar](50) NULL,
	[Email] [nvarchar](max) NULL,
 CONSTRAINT [PK_tb_Administrator] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Client]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Client](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](20) NOT NULL,
	[MacAddress] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tb_Client] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Config]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Config](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdClientConfig] [int] NOT NULL,
	[IdAdministrator] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Cron] [nvarchar](50) NOT NULL,
	[BackUpType] [nvarchar](10) NOT NULL,
	[BackUpFormat] [nvarchar](10) NOT NULL,
	[RetentionPackages] [int] NOT NULL,
	[RetentionPackagesSize] [int] NULL,
 CONSTRAINT [PK_tb_Config] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_ConfigClient]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_ConfigClient](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IdClient] [int] NOT NULL,
	[IdConfig] [int] NOT NULL,
	[Downloaded] [bit] NOT NULL,
 CONSTRAINT [PK_tb_ConfigClient] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_LoginLog]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_LoginLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdAdministrator] [int] NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[IpAddress] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tb_LoginLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_SourceToDest]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_SourceToDest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdConfig] [int] NOT NULL,
	[Source] [nvarchar](max) NOT NULL,
	[Destination] [nvarchar](max) NOT NULL,
	[DestType] [nvarchar](3) NOT NULL,
 CONSTRAINT [PK_tb_SourceToDest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_Task_List]    Script Date: 07.02.2022 20:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_Task_List](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdConfigClient] [int] NOT NULL,
	[State] [nchar](2) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Size(MB)] [int] NOT NULL,
 CONSTRAINT [PK_tb_TaskList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_tb_ConfigClient]    Script Date: 07.02.2022 20:02:31 ******/
CREATE NONCLUSTERED INDEX [IX_tb_ConfigClient] ON [dbo].[tb_ConfigClient]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tb_Config] ADD  CONSTRAINT [DF_tb_Config_RetentionPackagesSize]  DEFAULT ((0)) FOR [RetentionPackagesSize]
GO
ALTER TABLE [dbo].[tb_Task_List] ADD  CONSTRAINT [DF_tb_Task_List_State]  DEFAULT ('NR') FOR [State]
GO
ALTER TABLE [dbo].[tb_Config]  WITH CHECK ADD  CONSTRAINT [FK_tb_Config_tb_Administrator] FOREIGN KEY([IdAdministrator])
REFERENCES [dbo].[tb_Administrator] ([Id])
GO
ALTER TABLE [dbo].[tb_Config] CHECK CONSTRAINT [FK_tb_Config_tb_Administrator]
GO
ALTER TABLE [dbo].[tb_ConfigClient]  WITH CHECK ADD  CONSTRAINT [FK_tb_ConfigClient_tb_Client] FOREIGN KEY([IdClient])
REFERENCES [dbo].[tb_Client] ([Id])
GO
ALTER TABLE [dbo].[tb_ConfigClient] CHECK CONSTRAINT [FK_tb_ConfigClient_tb_Client]
GO
ALTER TABLE [dbo].[tb_ConfigClient]  WITH CHECK ADD  CONSTRAINT [FK_tb_ConfigClient_tb_Config] FOREIGN KEY([IdConfig])
REFERENCES [dbo].[tb_Config] ([Id])
GO
ALTER TABLE [dbo].[tb_ConfigClient] CHECK CONSTRAINT [FK_tb_ConfigClient_tb_Config]
GO
ALTER TABLE [dbo].[tb_LoginLog]  WITH CHECK ADD  CONSTRAINT [FK_tb_LoginLog_tb_Administrator] FOREIGN KEY([IdAdministrator])
REFERENCES [dbo].[tb_Administrator] ([Id])
GO
ALTER TABLE [dbo].[tb_LoginLog] CHECK CONSTRAINT [FK_tb_LoginLog_tb_Administrator]
GO
ALTER TABLE [dbo].[tb_SourceToDest]  WITH CHECK ADD  CONSTRAINT [FK_tb_SourceToDest_tb_Config] FOREIGN KEY([IdConfig])
REFERENCES [dbo].[tb_Config] ([Id])
GO
ALTER TABLE [dbo].[tb_SourceToDest] CHECK CONSTRAINT [FK_tb_SourceToDest_tb_Config]
GO
ALTER TABLE [dbo].[tb_Task_List]  WITH CHECK ADD  CONSTRAINT [FK_tb_TaskList_tb_ConfigClient] FOREIGN KEY([IdConfigClient])
REFERENCES [dbo].[tb_ConfigClient] ([ID])
GO
ALTER TABLE [dbo].[tb_Task_List] CHECK CONSTRAINT [FK_tb_TaskList_tb_ConfigClient]
GO
ALTER TABLE [dbo].[tb_Config]  WITH CHECK ADD  CONSTRAINT [CK_tb_Config_Format] CHECK  (([BackUpFormat]='FB' OR [BackUpFormat]='DI' OR [BackUpFormat]='IN'))
GO
ALTER TABLE [dbo].[tb_Config] CHECK CONSTRAINT [CK_tb_Config_Format]
GO
ALTER TABLE [dbo].[tb_Config]  WITH CHECK ADD  CONSTRAINT [CK_tb_Config_Type] CHECK  (([BackUpType]='PT' OR [BackUpType]='AR'))
GO
ALTER TABLE [dbo].[tb_Config] CHECK CONSTRAINT [CK_tb_Config_Type]
GO
ALTER TABLE [dbo].[tb_SourceToDest]  WITH CHECK ADD  CONSTRAINT [CK_tb_SourceToDest_DestType] CHECK  (([DestType]='FTP' OR [DestType]='DRV'))
GO
ALTER TABLE [dbo].[tb_SourceToDest] CHECK CONSTRAINT [CK_tb_SourceToDest_DestType]
GO
ALTER TABLE [dbo].[tb_Task_List]  WITH CHECK ADD  CONSTRAINT [CK_tb_Task_List_State] CHECK  (([State]='ER' OR [State]='NR' OR [State]='SC'))
GO
ALTER TABLE [dbo].[tb_Task_List] CHECK CONSTRAINT [CK_tb_Task_List_State]
GO
USE [master]
GO
ALTER DATABASE [dbPain] SET  READ_WRITE 
GO

use dbPain
go
insert into tb_Administrator (login,password,accountCreation,admin) values ('admin','admin',GETDATE(),1)
