USE [master]
GO
/****** Object:  Database [OnlineShop]    Script Date: 2/14/2017 5:02:59 PM ******/
CREATE DATABASE [OnlineShop]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OnlineShop', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\OnlineShop.mdf' , SIZE = 13312KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'OnlineShop_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\OnlineShop_log.ldf' , SIZE = 22144KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [OnlineShop] SET COMPATIBILITY_LEVEL = 120
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
ALTER DATABASE [OnlineShop] SET  ENABLE_BROKER 
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
ALTER DATABASE [OnlineShop] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OnlineShop] SET  MULTI_USER 
GO
ALTER DATABASE [OnlineShop] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OnlineShop] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OnlineShop] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OnlineShop] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [OnlineShop] SET DELAYED_DURABILITY = DISABLED 
GO
USE [OnlineShop]
GO
/****** Object:  Table [dbo].[Operations]    Script Date: 2/14/2017 5:03:00 PM ******/
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
	[IsSelled] [bit] NOT NULL,
 CONSTRAINT [PK_Operations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 2/14/2017 5:03:00 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2/14/2017 5:03:00 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/14/2017 5:03:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](60) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[ActivationCode] [nvarchar](100) NULL,
	[Password] [nvarchar](200) NOT NULL,
	[PasswordSalt] [nvarchar](50) NULL,
	[PasswordHash] [nvarchar](50) NULL,
	[RoleId] [int] NOT NULL,
	[Balance] [money] NOT NULL,
	[IsBlocked] [bit] NOT NULL,
	[IsEmailConfirmed] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSessions]    Script Date: 2/14/2017 5:03:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](100) NOT NULL,
	[IsExpired] [bit] NOT NULL,
 CONSTRAINT [PK_UserSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Operations] ON 

INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (1, 1, 1, 1, 13.0000, CAST(N'2016-02-11' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (2, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (3, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (4, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (5, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (6, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (7, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (8, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (9, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (10, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (11, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (12, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (13, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (14, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (15, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (16, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (17, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (18, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (19, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (20, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (21, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (22, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (23, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (24, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (25, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (26, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (27, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (28, 1, 3, 1, 100.0000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (29, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (30, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (31, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (32, 1, 2, 1, 40.3000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (33, 1, 1, 1, 50.4000, CAST(N'2016-11-03' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (34, 1, 1, 1, 50.4000, CAST(N'2016-11-17' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (35, 1, 1, 1, 50.4000, CAST(N'2016-11-18' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (36, 1, 1, 1, 50.4000, CAST(N'2016-11-18' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (37, 1, 2, 1, 40.3000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (38, 1, 2, 1, 40.3000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (39, 1, 1, 1, 50.4000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (40, 1, 2, 1, 40.3000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (41, 1, 1, 1, 50.4000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (42, 1, 2, 1, 40.3000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (43, 1, 1, 1, 50.4000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (44, 1, 3, 1, 100.0000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (45, 1, 3, 1, 100.0000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (46, 1, 3, 1, 100.0000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (47, 1, 3, 1, 100.0000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (48, 1, 2, 1, 40.3000, CAST(N'2016-11-21' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (49, 1, 1, 1, 50.4000, CAST(N'2017-01-02' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (50, 1, 2, 1, 40.3000, CAST(N'2017-01-02' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (51, 1, 3, 1, 100.0000, CAST(N'2017-01-02' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (52, 1, 1, 1, 50.4000, CAST(N'2017-01-02' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (53, 1, 2, 1, 40.3000, CAST(N'2017-01-02' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (54, 1, 3, 1, 100.0000, CAST(N'2017-01-02' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (55, 2, 0, 1, 0.0000, CAST(N'2017-01-16' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (56, 2, 0, 1, 0.0000, CAST(N'2017-01-16' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (57, 2, 0, 1, 0.0000, CAST(N'2017-01-16' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (58, 2, 0, 1, 50.4000, CAST(N'2017-01-16' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (59, 2, 1, 1, 50.4000, CAST(N'2017-01-16' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (60, 2, 1, 1, 50.4000, CAST(N'2017-01-16' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (61, 2, 1, 1, 50.4000, CAST(N'2017-01-16' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (62, 2, 3, 1, 100.0000, CAST(N'2017-01-16' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (63, 2, 2, 1, 40.3000, CAST(N'2017-01-16' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (64, 2, 3, 1, 100.0000, CAST(N'2017-01-17' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (65, 2, 3, 1, 100.0000, CAST(N'2017-01-17' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (66, 2, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (67, 2, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (68, 2, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (69, 2, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (70, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (71, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (72, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (73, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (74, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (75, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (76, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (77, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (78, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (79, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (80, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (81, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (82, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (83, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (84, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (85, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (86, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (87, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (88, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (89, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (90, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (91, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (92, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (93, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (94, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (95, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (96, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (97, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (98, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (99, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
GO
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (100, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (101, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (102, 4, 3, 1, 123.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (103, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (104, 4, 1, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (105, 4, 3, 1, 1000.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (106, 4, 3, 1, 1000.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (107, 4, 3, 1, 1000.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (108, 4, 3, 1, 1000.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (109, 4, 3, 1, 1000.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (110, 4, 3, 1, 1000.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (111, 4, 2, 1, 213.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (112, 4, 2, 1, 1.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (113, 4, 2, 1, 1.0000, CAST(N'2017-01-19' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (114, 4, 2, 1, 1.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (115, 4, 1, 1, 213.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (116, 4, 1, 1, 213.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (117, 4, 2, 1, 1.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (118, 4, 2, 1, 1.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (119, 4, 2, 1, 1.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (120, 4, 3, 1, 1000.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (121, 4, 3, 1, 1000.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (122, 4, 2, 1, 1.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (123, 4, 3, 1, 1000.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (124, 4, 1, 1, 213.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (125, 4, 3, 1, 1000.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (126, 4, 3, 1, 1000.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (127, 4, 3, 1, 1000.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (128, 4, 3, 1, 100.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (129, 4, 3, 1, 100.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (130, 4, 3, 1, 100.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (131, 4, 3, 1, 100.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (132, 4, 3, 1, 100.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (133, 4, 3, 1, 100.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (134, 4, 2, 1, 1.0000, CAST(N'2017-01-20' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (135, 2, 2, 1, 1000.0000, CAST(N'2017-01-23' AS Date), 0)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (136, 4, 2, 1, 1000.0000, CAST(N'2017-01-23' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (137, 4, 2, 1, 1000.0000, CAST(N'2017-01-23' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (138, 4, 2, 1, 1000.0000, CAST(N'2017-01-24' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (139, 4, 1, 1, 213.0000, CAST(N'2017-01-24' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (140, 4, 3, 1, 2000.0000, CAST(N'2017-01-24' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (141, 4, 2, 1, 1000.0000, CAST(N'2017-01-24' AS Date), 1)
INSERT [dbo].[Operations] ([Id], [UserID], [ProductId], [Quantity], [Amount], [Date], [IsSelled]) VALUES (142, 4, 3, 1, 1500.0000, CAST(N'2017-02-10' AS Date), 0)
SET IDENTITY_INSERT [dbo].[Operations] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [Cost]) VALUES (1, N'Iron', 213.0000)
INSERT [dbo].[Products] ([Id], [Name], [Cost]) VALUES (2, N'Wood', 1000.0000)
INSERT [dbo].[Products] ([Id], [Name], [Cost]) VALUES (3, N'Oil', 1500.0000)
SET IDENTITY_INSERT [dbo].[Products] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Admin               ')
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'User                ')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (1, N'asd', N'nikolaidenis2012@gmail.com', NULL, N'1', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (2, N'admin', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 1, 8546.6000, 0, 1)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (3, N'olya', N'show.must.go.on@mail.ru', N'1908cda669e73c586fd9e577f888d3f6', N'123', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (4, N'1', N'nikolaidenis2012@gmail.com', N'5fa71634ea4c0991507cacd8e2f2aefc', N'1', NULL, NULL, 2, 16817.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (5, N'2', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 1, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (6, N'3', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (7, N'4', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (8, N'5', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (9, N'6', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (10, N'7', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (11, N'8', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (12, N'9', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (13, N'10', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (14, N'11', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (15, N'12', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (16, N'13', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (17, N'141', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (18, N'15', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (19, N'16', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (20, N'17', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (21, N'18', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (22, N'19', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (23, N'20', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (24, N'21', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (25, N'22', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (26, N'23', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
INSERT [dbo].[Users] ([Id], [UserName], [Email], [ActivationCode], [Password], [PasswordSalt], [PasswordHash], [RoleId], [Balance], [IsBlocked], [IsEmailConfirmed]) VALUES (27, N'24', N'nikolaidenis2012@gmail.com', N'efbf3a06e8245c68a06f68f1398e838f', N'admina', NULL, NULL, 2, 0.0000, 0, 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[UserSessions] ON 

INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (1, 2, N'JG/T6ao41EgDncHLXxeSQbMrFxB3kaO3', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (2, 2, N'9p5RIrE41EhK17E4/SrIR6yDlb4u5w6B', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (3, 2, N'0zL9JbE41EhyeAGFKXK+SpQNoX1gyome', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (4, 2, N'QxUtKrE41EiczYZhJibWT7AyPioo2ZOb', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (5, 2, N'GHHDU7E41EiSKxM+Vl5vTZdWeeZY35zn', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (6, 2, N'sj9psbQ41EhE6LO4L0hrT57zaQka1d%2bK', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (7, 0, N'0l7CHbU41Ei0nwQE7I2ffTrlD8jtdPss6', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (8, 2, N'YkbEsrU41Eiwvp8efB68SpO2ezatdlly', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (9, 2, N'Mm2b007U41EhOYiVz6atOQYDfKvYRNLxz', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (10, 2, N'Erqg%2fXc51Ej%2fw3yQ1ORjRat2TwvZIsS1', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (11, 0, N'atQF8s461EgvSNUcvpplTpXoY05AW%2fGS', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (12, 0, N'OqyUGs861Eigwku%2bpD8bToqoAlVR%2fyem', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (13, 2, N'Uhopkc861Eh3ySqhUrrUTKJxbB8uz4YA', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (14, 2, N'4jl44dA61Ejm2hoXQ7FrT7cqCpNQBMuw', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (15, 2, N'dJf3zQo71EiExgowvuUAS77HWvGxT8Gy', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (16, 2, N'bUxCXA871EhfalU2Xd%2bOSagPypJXRFZ4', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (17, 2, N'XbDutQ871Ei1GUoaRaFITZZe7AKIuINP', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (18, 2, N'7zqlG5Q71EgAEgcEED5QSYAfH%2fEjaiAD', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (19, 2, N'YdCfQJQ71EhNyz%2flft4sQrNzLDB42JUt', 0)
INSERT [dbo].[UserSessions] ([Id], [UserId], [Token], [IsExpired]) VALUES (20, 2, N'gsxClJQ71Ejmg45N5aj%2bTaMWeIYiJEGb', 0)
SET IDENTITY_INSERT [dbo].[UserSessions] OFF
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Operations] ADD  CONSTRAINT [DF_Operations_IsSelled]  DEFAULT ((0)) FOR [IsSelled]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Balance]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsBlocked]  DEFAULT ((0)) FOR [IsBlocked]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsEmailConfirmed]  DEFAULT ((0)) FOR [IsEmailConfirmed]
GO
ALTER TABLE [dbo].[UserSessions] ADD  CONSTRAINT [DF_UserSessions_IsExpired]  DEFAULT ((0)) FOR [IsExpired]
GO
USE [master]
GO
ALTER DATABASE [OnlineShop] SET  READ_WRITE 
GO
