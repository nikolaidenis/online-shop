USE [master]
GO
/****** Object:  Database [OnlineShop]    Script Date: 10/28/2016 18:18:42 ******/
CREATE DATABASE [OnlineShop] ON  PRIMARY 
( NAME = N'OnlineShop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\OnlineShop.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'OnlineShop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\OnlineShop_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [OnlineShop] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OnlineShop].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OnlineShop] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [OnlineShop] SET ANSI_NULLS OFF
GO
ALTER DATABASE [OnlineShop] SET ANSI_PADDING OFF
GO
ALTER DATABASE [OnlineShop] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [OnlineShop] SET ARITHABORT OFF
GO
ALTER DATABASE [OnlineShop] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [OnlineShop] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [OnlineShop] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [OnlineShop] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [OnlineShop] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [OnlineShop] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [OnlineShop] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [OnlineShop] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [OnlineShop] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [OnlineShop] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [OnlineShop] SET  DISABLE_BROKER
GO
ALTER DATABASE [OnlineShop] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [OnlineShop] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [OnlineShop] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [OnlineShop] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [OnlineShop] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [OnlineShop] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [OnlineShop] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [OnlineShop] SET  READ_WRITE
GO
ALTER DATABASE [OnlineShop] SET RECOVERY FULL
GO
ALTER DATABASE [OnlineShop] SET  MULTI_USER
GO
ALTER DATABASE [OnlineShop] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [OnlineShop] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'OnlineShop', N'ON'
GO
USE [OnlineShop]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/28/2016 18:18:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](20) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Admin               ')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'User                ')
SET IDENTITY_INSERT [dbo].[Roles] OFF
/****** Object:  Table [dbo].[Products]    Script Date: 10/28/2016 18:18:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Cost] [money] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Products] ON
INSERT [dbo].[Products] ([Id], [Name], [Cost]) VALUES (1, N'Iron', 50.0000)
INSERT [dbo].[Products] ([Id], [Name], [Cost]) VALUES (2, N'Wood', 40.0000)
INSERT [dbo].[Products] ([Id], [Name], [Cost]) VALUES (3, N'Oil', 100.0000)
SET IDENTITY_INSERT [dbo].[Products] OFF
/****** Object:  Table [dbo].[Operations]    Script Date: 10/28/2016 18:18:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_Operations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/28/2016 18:18:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](60) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[PasswordSalt] [nvarchar](50) NULL,
	[PasswordHash] [nvarchar](50) NULL,
	[RoleId] [int] NOT NULL,
	[Balance] [money] NOT NULL,
	[IsBlocked] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Products_Cost]    Script Date: 10/28/2016 18:18:43 ******/
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Cost]  DEFAULT ((0)) FOR [Cost]
GO
/****** Object:  Default [DF_Operations_Quantity]    Script Date: 10/28/2016 18:18:43 ******/
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
/****** Object:  Default [DF_Operations_Amount]    Script Date: 10/28/2016 18:18:43 ******/
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_Amount]  DEFAULT ((0)) FOR [Amount]
GO
/****** Object:  Default [DF_Users_Balance]    Script Date: 10/28/2016 18:18:43 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Balance]  DEFAULT ((0)) FOR [Balance]
GO
/****** Object:  Default [DF_Users_IsBlocked]    Script Date: 10/28/2016 18:18:43 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsBlocked]  DEFAULT ((0)) FOR [IsBlocked]
GO
/****** Object:  ForeignKey [FK_Users_Roles]    Script Date: 10/28/2016 18:18:43 ******/
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
