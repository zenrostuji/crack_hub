USE [master]
GO
/****** Object:  Database [WebDownloadGame]    Script Date: 09/06/2025 12:37:26 CH ******/
CREATE DATABASE [WebDownloadGame]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebDownloadGame', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\WebDownloadGame.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebDownloadGame_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\WebDownloadGame_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [WebDownloadGame] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebDownloadGame].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebDownloadGame] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebDownloadGame] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebDownloadGame] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebDownloadGame] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebDownloadGame] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebDownloadGame] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebDownloadGame] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebDownloadGame] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebDownloadGame] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebDownloadGame] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebDownloadGame] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebDownloadGame] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebDownloadGame] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebDownloadGame] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebDownloadGame] SET  ENABLE_BROKER 
GO
ALTER DATABASE [WebDownloadGame] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebDownloadGame] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebDownloadGame] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebDownloadGame] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebDownloadGame] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebDownloadGame] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebDownloadGame] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebDownloadGame] SET RECOVERY FULL 
GO
ALTER DATABASE [WebDownloadGame] SET  MULTI_USER 
GO
ALTER DATABASE [WebDownloadGame] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebDownloadGame] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebDownloadGame] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebDownloadGame] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebDownloadGame] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WebDownloadGame] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [WebDownloadGame] SET QUERY_STORE = ON
GO
ALTER DATABASE [WebDownloadGame] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [WebDownloadGame]
GO
/****** Object:  Table [dbo].[AvatarFrames]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvatarFrames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FrameName] [nvarchar](100) NOT NULL,
	[FrameUrl] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[RarityLevel] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[RequiredLevel] [int] NULL,
	[IsPremium] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Slug] [nvarchar](100) NOT NULL,
	[IconClass] [nvarchar](50) NULL,
	[GameCount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CrackInfo]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrackInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[Version] [nvarchar](50) NULL,
	[Group] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[DownloadUrl] [nvarchar](max) NULL,
	[FileSize] [nvarchar](50) NULL,
	[ReleaseDate] [datetime] NULL,
	[IsRecommended] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DownloadHistory]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DownloadHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[GameId] [int] NOT NULL,
	[DownloadDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FavoriteGames]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavoriteGames](
	[UserId] [nvarchar](450) NOT NULL,
	[GameId] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Features]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Features](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[FeatureDescription] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameLinks]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameLinks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[LinkName] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[FileSize] [nvarchar](50) NULL,
	[PartNumber] [int] NULL,
	[TotalParts] [int] NULL,
	[Password] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[DateAdded] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Games]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Games](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[ShortDescription] [nvarchar](500) NULL,
	[FullDescription] [nvarchar](max) NULL,
	[CoverImageUrl] [nvarchar](max) NULL,
	[Developer] [nvarchar](100) NULL,
	[Publisher] [nvarchar](100) NULL,
	[ReleaseDate] [datetime] NULL,
	[CategoryId] [int] NULL,
	[Rating] [float] NULL,
	[Downloads] [nvarchar](50) NULL,
	[DownloadUrl] [nvarchar](max) NULL,
	[Size] [nvarchar](50) NULL,
	[AverageRating] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameTags]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameTags](
	[GameId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GameId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocalizationInfo]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalizationInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[LocalizationType] [nvarchar](50) NOT NULL,
	[LocalizedBy] [nvarchar](100) NULL,
	[LocalizationVersion] [nvarchar](50) NULL,
	[LocalizationDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[InstallationGuide] [nvarchar](max) NULL,
	[DownloadUrl] [nvarchar](max) NULL,
	[FileSize] [nvarchar](50) NULL,
	[IsComplete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PopularTags]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PopularTags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,
	[SearchCount] [int] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RelatedGames]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelatedGames](
	[GameId] [int] NOT NULL,
	[RelatedGameId] [int] NOT NULL,
	[RelationStrength] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[GameId] ASC,
	[RelatedGameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Content] [nvarchar](max) NULL,
	[Rating] [float] NOT NULL,
	[DatePosted] [datetime] NOT NULL,
	[HelpfulCount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Screenshots]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Screenshots](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchHistory]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[SearchTerm] [nvarchar](255) NOT NULL,
	[SearchDate] [datetime] NOT NULL,
	[ResultCount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemRequirements]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemRequirements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[RequirementType] [nvarchar](50) NOT NULL,
	[OS] [nvarchar](100) NULL,
	[Processor] [nvarchar](100) NULL,
	[Memory] [nvarchar](100) NULL,
	[Graphics] [nvarchar](100) NULL,
	[DirectX] [nvarchar](100) NULL,
	[Storage] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Slug] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAvatarFrames]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAvatarFrames](
	[UserId] [nvarchar](450) NOT NULL,
	[FrameId] [int] NOT NULL,
	[IsEquipped] [bit] NOT NULL,
	[ObtainedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[FrameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 09/06/2025 12:37:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[DisplayName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[Bio] [nvarchar](max) NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[RoleId] [int] NOT NULL,
	[PremiumExpiryDate] [datetime] NULL,
	[RememberMe] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AvatarFrames] ON 

INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (1, N'Khung Cơ Bản', N'/img/frameAvartar/basic_frame.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 1, NULL, 0, CAST(N'2025-05-01T21:39:16.720' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (2, N'Khung Bạc', N'/img/frameAvartar/silver.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (3, N'Khung Vàng', N'/img/frameAvartar/gold.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (4, N'Khung Sắt', N'/img/frameAvartar/nomal_frame.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (5, N'Khung Bạch Dương', N'/img/frameAvartar/Aries.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:27:10.660' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (6, N'Khung Kim Ngưu', N'/img/frameAvartar/Taurus.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (7, N'Khung Song Tử', N'/img/frameAvartar/Gemini.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (8, N'Khung Cự Giải', N'/img/frameAvartar/Cancer.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (9, N'Khung Sư Tử', N'/img/frameAvartar/Leo.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (10, N'Khung Xử Nữ', N'/img/frameAvartar/Virgo.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (11, N'Khung Thiên Bình', N'/img/frameAvartar/Libra.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (12, N'Khung Thiên Yết', N'/img/frameAvartar/Scorpio.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (13, N'Khung Nhân Mã', N'/img/frameAvartar/Sagittarius.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (14, N'Khung Ma Kết', N'/img/frameAvartar/Capricorn.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (15, N'Khung Bảo Bình', N'/img/frameAvartar/Aquarius.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
INSERT [dbo].[AvatarFrames] ([Id], [FrameName], [FrameUrl], [Description], [RarityLevel], [IsDefault], [RequiredLevel], [IsPremium], [CreatedAt], [IsActive]) VALUES (16, N'Khung Song Ngư', N'/img/frameAvartar/Pisces.png', N'Khung avatar mặc định cho tất cả người dùng', 1, 0, NULL, 0, CAST(N'2025-05-01T22:39:23.357' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[AvatarFrames] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (1, N'Hành Động', N'Game hành động với nhịp độ nhanh và kích thích', N'hanh-dong', N'fas fa-running', 25)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (2, N'Nhập Vai', N'Game nhập vai với cốt truyện sâu sắc và phát triển nhân vật', N'nhap-vai', N'fas fa-dragon', 18)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (3, N'Phiêu Lưu', N'Game phiêu lưu với các thử thách và khám phá', N'phieu-luu', N'fas fa-mountain', 15)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (4, N'Chiến Thuật', N'Game chiến thuật đòi hỏi tư duy và lập kế hoạch', N'chien-thuat', N'fas fa-chess', 12)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (5, N'Thể Thao', N'Game thể thao mô phỏng các môn thể thao thực tế', N'the-thao', N'fas fa-football-ball', 10)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (6, N'Mô Phỏng', N'Game mô phỏng các hoạt động và trải nghiệm thực tế', N'mo-phong', N'fas fa-gamepad', 14)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (7, N'Giải Đố', N'Game giải đố thử thách trí tuệ', N'giai-do', N'fas fa-puzzle-piece', 8)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (8, N'Kinh Dị', N'Game kinh dị với bầu không khí căng thẳng và đáng sợ', N'kinh-di', N'fas fa-ghost', 9)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (9, N'Indie', N'Game độc lập từ các nhà phát triển nhỏ', N'indie', N'fas fa-university', 20)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (10, N'Sinh Tồn', N'Game sinh tồn với các cơ chế quản lý tài nguyên', N'sinh-ton', N'fas fa-tree', 11)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (11, N'Việt Hóa', N'Game đã được dịch và Việt hóa sang tiếng Việt', N'viet-hoa', N'fas fa-language', 30)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (12, N'FPS', N'Game bắn súng góc nhìn thứ nhất', N'fps', N'fas fa-crosshairs', 15)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (13, N'Battle Royale', N'Game battle royale với hình thức sống sót cuối cùng', N'battle-royale', N'fas fa-users-slash', 7)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (14, N'Đua Xe', N'Game đua xe với nhiều loại phương tiện khác nhau', N'dua-xe', N'fas fa-car', 8)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (15, N'Sandbox', N'Game thế giới mở cho phép tự do sáng tạo', N'sandbox', N'fas fa-cubes', 9)
INSERT [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [Slug], [IconClass], [GameCount]) VALUES (16, N'RPG', N'Game nhập vai với hệ thống nâng cấp nhân vật', N'rpg', N'fas fa-hat-wizard', 22)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[CrackInfo] ON 

INSERT [dbo].[CrackInfo] ([Id], [GameId], [Version], [Group], [Description], [DownloadUrl], [FileSize], [ReleaseDate], [IsRecommended]) VALUES (1, 1, N'1.32', N'CODEX', N'Bản crack hoàn chỉnh bao gồm tất cả DLC và nội dung bổ sung. Có thể chơi ngay sau khi cài đặt.', N'/downloads/witcher3-codex.zip', N'48.5 GB', CAST(N'2019-06-15T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[CrackInfo] ([Id], [GameId], [Version], [Group], [Description], [DownloadUrl], [FileSize], [ReleaseDate], [IsRecommended]) VALUES (2, 2, N'1.0.2245', N'RELOADED', N'Bản crack đầy đủ, không cần cài đặt Social Club. Chạy tốt trên hầu hết các cấu hình.', N'/downloads/gta5-reloaded.zip', N'70.2 GB', CAST(N'2021-03-10T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[CrackInfo] ([Id], [GameId], [Version], [Group], [Description], [DownloadUrl], [FileSize], [ReleaseDate], [IsRecommended]) VALUES (3, 3, N'1.6', N'EMPRESS', N'Bản crack mới nhất với hiệu suất được cải thiện, bao gồm tất cả các bản vá.', N'/downloads/cyberpunk-empress.zip', N'65.8 GB', CAST(N'2022-09-20T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[CrackInfo] ([Id], [GameId], [Version], [Group], [Description], [DownloadUrl], [FileSize], [ReleaseDate], [IsRecommended]) VALUES (4, 4, N'1.0', N'CPY', N'Bản crack đầy đủ, đã khắc phục lỗi kết nối server, chơi offline tốt.', N'/downloads/fifa23-cpy.zip', N'45.6 GB', CAST(N'2022-10-30T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[CrackInfo] ([Id], [GameId], [Version], [Group], [Description], [DownloadUrl], [FileSize], [ReleaseDate], [IsRecommended]) VALUES (5, 5, N'1.19', N'TLAUNCHER', N'Launcher tùy chỉnh cho phép cài đặt và chơi Minecraft miễn phí với nhiều tính năng bổ sung.', N'/downloads/minecraft-tlauncher.zip', N'250 MB', CAST(N'2022-08-12T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[CrackInfo] ([Id], [GameId], [Version], [Group], [Description], [DownloadUrl], [FileSize], [ReleaseDate], [IsRecommended]) VALUES (6, 38, N'1.1', N'PKAV', N'vi', N'https://drive.google.com/file/d/1jv0gtLVChifBLuGGSDADc8QY1uEKgFKt/view', N'500 MB', CAST(N'2025-04-30T00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[CrackInfo] OFF
GO
SET IDENTITY_INSERT [dbo].[DownloadHistory] ON 

INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (1, N'2', 1, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (2, N'2', 3, CAST(N'2021-08-15T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (3, N'2', 2, CAST(N'2022-03-20T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (4, N'3', 1, CAST(N'2021-06-20T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (5, N'3', 5, CAST(N'2022-01-05T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (6, N'4', 2, CAST(N'2021-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (7, N'4', 4, CAST(N'2022-10-15T00:00:00.000' AS DateTime))
INSERT [dbo].[DownloadHistory] ([Id], [UserId], [GameId], [DownloadDate]) VALUES (11, N'3', 2, CAST(N'2022-10-22T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[DownloadHistory] OFF
GO
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'2', 1, CAST(N'2021-07-10T00:00:00.000' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'2', 3, CAST(N'2021-08-15T00:00:00.000' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'3', 1, CAST(N'2021-06-20T00:00:00.000' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'3', 5, CAST(N'2022-01-05T00:00:00.000' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'4', 2, CAST(N'2021-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'4', 4, CAST(N'2022-10-15T00:00:00.000' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'4616f820-795b-4673-ab82-b4a7e34a12ea', 38, CAST(N'2025-05-05T12:15:14.367' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'5c65906c-16a2-4cf4-80b5-8d4c1e916feb', 34, CAST(N'2025-05-05T07:55:09.333' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'8b32518f-646a-4917-98d3-778d4f5b20af', 38, CAST(N'2025-04-30T09:26:20.113' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'af2e399f-0969-4c72-8760-1b3e64e9c072', 1, CAST(N'2025-04-29T19:58:05.450' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'af2e399f-0969-4c72-8760-1b3e64e9c072', 35, CAST(N'2025-04-29T18:15:47.470' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'af2e399f-0969-4c72-8760-1b3e64e9c072', 38, CAST(N'2025-04-30T00:50:31.243' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', 37, CAST(N'2025-04-29T19:28:50.267' AS DateTime))
INSERT [dbo].[FavoriteGames] ([UserId], [GameId], [DateAdded]) VALUES (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', 38, CAST(N'2025-05-01T10:17:36.887' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Features] ON 

INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (1, 1, N'Thế giới mở rộng lớn với hơn 100 giờ chơi')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (2, 1, N'Hệ thống chiến đấu đầy thử thách và sâu sắc')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (3, 1, N'Đồ họa tuyệt đẹp với chu kỳ ngày đêm và thời tiết thay đổi')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (4, 1, N'Cốt truyện hấp dẫn với nhiều lựa chọn ảnh hưởng đến diễn biến của game')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (5, 2, N'Thành phố Los Santos rộng lớn để khám phá')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (6, 2, N'Ba nhân vật chính có thể chuyển đổi qua lại')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (7, 2, N'Hơn 100 nhiệm vụ chính và phụ')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (8, 2, N'Chế độ multiplayer GTA Online cực kỳ phong phú')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (9, 3, N'Thế giới cyberpunk chi tiết và đầy sống động')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (10, 3, N'Hệ thống tùy biến nhân vật sâu rộng')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (11, 3, N'Nhiều cách tiếp cận khác nhau cho từng nhiệm vụ')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (12, 3, N'Cốt truyện phức tạp với nhiều kết thúc khác nhau')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (13, 4, N'Hơn 700 đội bóng được cấp phép chính thức')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (14, 4, N'Chế độ Ultimate Team với nhiều cải tiến')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (15, 4, N'Công nghệ HyperMotion2 mang đến chuyển động cầu thủ chân thực')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (16, 4, N'Chế độ Career Mode được cập nhật toàn diện')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (17, 5, N'Thế giới vô hạn để khám phá và xây dựng')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (18, 5, N'Hai chế độ chơi chính: Sáng tạo và Sinh tồn')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (19, 5, N'Hệ thống crafting đa dạng')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (20, 5, N'Cập nhật thường xuyên với nội dung mới')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (21, 38, N'Hỗ trợ mod từ cộng đồng người chơi')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (22, 38, N'Thế giới mở rộng lớn để khám phá')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (24, 6, N'trải nghiệm tay to :v')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (25, 6, N'tăng trải nghiệm khó khăn')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (26, 36, N'Hỗ trợ mod từ cộng đồng người chơi')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (27, 10, N'Đồ họa chân thực với công nghệ ray-tracing')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (28, 10, N'Thế giới mở rộng lớn để khám phá')
INSERT [dbo].[Features] ([Id], [GameId], [FeatureDescription]) VALUES (29, 39, N'Đồ họa chân thực với công nghệ ray-tracing')
SET IDENTITY_INSERT [dbo].[Features] OFF
GO
SET IDENTITY_INSERT [dbo].[GameLinks] ON 

INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (1, 1, N'Google Drive', N'https://drive.google.com/file/d/1abc123/view', N'25 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (2, 1, N'Google Drive', N'https://drive.google.com/file/d/2def456/view', N'25 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (3, 1, N'MediaFire', N'https://mediafire.com/file/witcher3_part1', N'25 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (4, 1, N'MediaFire', N'https://mediafire.com/file/witcher3_part2', N'25 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (5, 2, N'Google Drive', N'https://drive.google.com/file/d/3ghi789/view', N'35 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (6, 2, N'Google Drive', N'https://drive.google.com/file/d/4jkl012/view', N'37 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (7, 2, N'MEGA', N'https://mega.nz/file/gta5_part1', N'35 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (8, 2, N'MEGA', N'https://mega.nz/file/gta5_part2', N'37 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (9, 3, N'Google Drive', N'https://drive.google.com/file/d/5mno345/view', N'34 GB', 1, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (10, 3, N'Google Drive', N'https://drive.google.com/file/d/6pqr678/view', N'36 GB', 2, 2, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (11, 3, N'MEGA', N'https://mega.nz/file/cyberpunk_part1', N'34 GB', 1, 2, N'cp2077', 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (12, 3, N'MEGA', N'https://mega.nz/file/cyberpunk_part2', N'36 GB', 2, 2, N'cp2077', 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (13, 4, N'Google Drive', N'https://drive.google.com/file/d/7stu901/view', N'50 GB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (14, 4, N'MEGA', N'https://mega.nz/file/fifa23_full', N'50 GB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (15, 4, N'MediaFire', N'https://mediafire.com/file/fifa23_full', N'50 GB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (16, 5, N'Google Drive', N'https://drive.google.com/file/d/8vwx234/view', N'250 MB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (17, 5, N'MediaFire', N'https://mediafire.com/file/minecraft_launcher', N'250 MB', 1, 1, NULL, 1, CAST(N'2025-04-27T16:22:45.187' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (18, 6, N'pixeldrain', N'https://pixeldrain.com/u/SeZ9mSnW', N'645 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (19, 7, N'pixeldrain', N'https://pixeldrain.com/u/ds3qKj57', N'1.72 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (22, 10, N'gofile', N'https://gofile.io/d/5HX4ea', N'6 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (23, 11, N'pixeldrain', N'https://pixeldrain.com/u/v5PWQ8H5', N'340 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (24, 12, N'pixeldrain', N'https://pixeldrain.com/u/FrQC6Utv', N'12.1 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (25, 13, N'pixeldrain', N'https://pixeldrain.com/u/21G5wNyN', N'350 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (26, 14, N'pixeldrain', N'https://pixeldrain.com/u/xNs3zBpK', N'570 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (27, 15, N'pixeldrain', N'https://pixeldrain.com/u/7VoLE3oM', N'2 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (28, 15, N'MediaFire', N'https://mediafire.com/file/findlove_full', N'2 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (29, 16, N'mega', N'https://mega.nz/file/0StHnSqQ#8Q8dWb3FArjXdFPZ35QETF8jAKWVg1Tf0X1S_hPlHQA', N'420 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (30, 17, N'pixeldrain', N'https://pixeldrain.com/u/4AhVkV8A', N'9.2 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (31, 18, N'mixdrop', N'https://mixdrop.sb/f/xodkddg1uld1wnm', N'1 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (32, 19, N'pixeldrain', N'https://pixeldrain.com/u/aZa1kZSc', N'2 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (33, 20, N'MEGA', N'https://mega.nz/file/u4AhyABC#R70uIoiuzUamyWRpUdsm7D4zgfUozsToTGeZfbI3F_w', N'140 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (34, 21, N'pixeldrain', N'https://pixeldrain.com/u/VmEqSA4d', N'2.4 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (35, 22, N'pixeldrain', N'https://pixeldrain.com/u/B8Mkw9yX', N'1.3 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (36, 23, N'pixeldrain', N'https://pixeldrain.com/u/qoMPwV61', N'9 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (37, 24, N'pixeldrain', N'https://pixeldrain.com/u/z9byGKqx', N'255 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (38, 25, N'steam', N'https://store.steampowered.com/app/1289310/Helltaker/', N'350 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (39, 26, N'pixeldrain', N'https://pixeldrain.com/u/NvULjVXy', N'7 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (40, 27, N'MEGA', N'https://mega.nz/file/AbslCBZT#9XiUx0cCMtwqfX6Kn2BzKEIewElY_ouR8unvV1hgDaE', N'152 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (41, 28, N'pixeldrain', N'https://pixeldrain.com/u/mqaduXew', N'5.8 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (42, 29, N'MEGA', N'https://mega.nz/file/MBhWyayC#Fg1JgZkxYQAWBl9uVCFivI_0pVTyVO9IG0EaOoIR0E8', N'305 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (43, 30, N'Google Drive', N'https://drive.google.com/file/d/1Y7eTgkSV8NDABKxehos2Lm7_p0ont6fq/view', N'341 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (44, 31, N'MEGA', N'https://mega.nz/file/5qAGTCgC#140dx-b1E8qV3mh3Rb14snj-ZzxwyLXpvU6GgtShQOE', N'360 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (45, 32, N'pixeldrain', N'https://pixeldrain.com/u/JMhqHy18', N'262 MB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (46, 32, N'MEGA', N'https://mega.nz/file/jIUwXIZQ#i3Z8Rn-Z79JTCjTDHH72GqIqm2wH8gEkODQDmtBu7TA', N'14 MB', 1, 1, N'sg0003', 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (47, 33, N'pixeldrain', N'https://pixeldrain.com/u/KMDVWjXV', N'8.3 GB', 1, 2, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (48, 34, N'MEGA', N'https://mega.nz/file/5mRx1aIJ#Av8iC5YOFRrjc-Mldpaal2GERE7-A6CEajzmtRpn2Zw', N'10 GB', 1, 1, N'larry456', 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (49, 35, N'pixeldrain', N'https://pixeldrain.com/u/2eScxKLN', N'1.4 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (50, 36, N'pixeldrain', N'https://pixeldrain.com/u/A5PeKEbG', N'333 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (51, 37, N'pixeldrain', N'https://pixeldrain.com/u/ezKWZfCP', N'1.5 GB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (52, 38, N'terabox', N'https://dm.terabox.com/vietnamese/sharing/link?surl=iGrn4LI711z1baxz9wMCtQ&clearCache=1', N'73 MB', 1, 1, NULL, 1, CAST(N'2025-04-29T15:07:16.700' AS DateTime))
INSERT [dbo].[GameLinks] ([Id], [GameId], [LinkName], [Url], [FileSize], [PartNumber], [TotalParts], [Password], [IsActive], [DateAdded]) VALUES (54, 38, N'Google Drive', N'https://drive.google.com/file/d/1jv0gtLVChifBLuGGSDADc8QY1uEKgFKt/view', N'500 MB', 1, 1, NULL, 1, CAST(N'2025-04-30T16:46:57.083' AS DateTime))
SET IDENTITY_INSERT [dbo].[GameLinks] OFF
GO
SET IDENTITY_INSERT [dbo].[Games] ON 

INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (1, N'The Witcher 3: Wild Hunt', N'Một trò chơi nhập vai thế giới mở hoành tráng với cốt truyện phong phú và thế giới rộng lớn để khám phá.', N'The Witcher 3: Wild Hunt là một trò chơi nhập vai thế giới mở năm 2015 được phát triển bởi CD Projekt Red. Lấy bối cảnh trong một thế giới giả tưởng dựa trên thần thoại Slav, người chơi điều khiển Geralt xứ Rivia, một thợ săn quái vật được gọi là "witcher" phải tìm con gái nuôi của anh ta, Ciri, đang bị Wildchase truy đuổi, một lực lượng ma quái bí ẩn đe dọa thế giới.', N'/img/games/thewitcher3.jpg', N'CD Projekt Red', N'CD Projekt', CAST(N'2015-05-19T00:00:00.000' AS DateTime), 2, 9.5, N'1', NULL, N'50 GB', 9.3)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (2, N'Grand Theft Auto V', N'Một trò chơi hành động phiêu lưu thế giới mở, diễn ra tại thành phố hư cấu Los Santos.', N'Grand Theft Auto V là một tựa game hành động-phiêu lưu năm 2013 được phát triển bởi Rockstar North và được xuất bản bởi Rockstar Games. Là phần chính thứ bảy trong series Grand Theft Auto, trò chơi có ba nhân vật chính: tên tội phạm đã về hưu Michael De Santa, gangster đường phố Franklin Clinton và gã buôn ma túy Trevor Philips, kể về việc họ thực hiện các vụ cướp dưới áp lực của một cơ quan chính phủ tham nhũng và những tên tội phạm mạnh mẽ.', N'/img/games/gta5.jpg', N'Rockstar North', N'Rockstar Games', CAST(N'2013-09-17T00:00:00.000' AS DateTime), 1, 9.6, N'1', NULL, N'72 GB', 9.5)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (3, N'Cyberpunk 2077', N'Một trò chơi nhập vai hành động thế giới mở lấy bối cảnh trong một thành phố đầy rẫy tội phạm và rất nhiều công nghệ cao.', N'Cyberpunk 2077 là một trò chơi nhập vai hành động thế giới mở lấy bối cảnh ở Night City, một đô thị cyberpunk nơi quyền lực, sự xa hoa, và những phẫu thuật cơ thể đang là những thứ được săn đón. Bạn đóng vai V, một lính đánh thuê ngoài vòng pháp luật đang theo đuổi một bộ phận cấy ghép độc nhất vô nhị là chìa khóa dẫn đến sự bất tử.', N'/img/games/cyberpunk2077.jpg', N'CD Projekt Red', N'CD Projekt', CAST(N'2020-12-10T00:00:00.000' AS DateTime), 1, 8.7, N'3', NULL, N'70 GB', 7.9)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (4, N'FIFA 23', N'Trò chơi bóng đá mô phỏng mới nhất từ EA Sports với các giải đấu, câu lạc bộ và cầu thủ chính thức.', N'FIFA 23 là một trò chơi mô phỏng bóng đá được phát triển bởi EA Vancouver và EA Romania và được xuất bản bởi EA Sports. Đây là phần thứ 30 và cũng là phần cuối cùng trong loạt trò chơi FIFA do EA phát triển. Được hỗ trợ bởi công nghệ HyperMotion2, FIFA 23 mang đến trải nghiệm bóng đá thực tế hơn với các hoạt ảnh cầu thủ chân thực được xây dựng từ dữ liệu ghi nhận từ các trận đấu thực tế.', N'/img/games/fifa23.jpg', N'EA Vancouver', N'EA Sports', CAST(N'2022-09-30T00:00:00.000' AS DateTime), 5, 8.5, N'10', NULL, N'50 GB', 8.2)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (5, N'Minecraft', N'Một trò chơi sandbox cho phép người chơi khám phá, xây dựng và sinh tồn trong một thế giới khối hình học.', N'Minecraft là một trò chơi sandbox được phát triển bởi Mojang Studios. Trò chơi được tạo ra bởi Markus "Notch" Persson trong ngôn ngữ lập trình Java. Trong Minecraft, người chơi khám phá một thế giới block 3D được tạo ra theo cách khởi tạo thủ tục, khám phá và khai thác nguyên liệu thô, chế tạo các công cụ và vật phẩm, xây dựng các công trình, và tùy chọn có thể chiến đấu với quái vật máy tính.', N'/img/games/minecraft.jpg', N'Mojang Studios', N'Mojang Studios', CAST(N'2011-11-18T00:00:00.000' AS DateTime), 15, 9.2, N'5', NULL, N'2 GB', 9.4)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (6, N'Fear & Hunger', N'Game RPG kinh dị tâm lý với gameplay hardcore và cơ chế sinh tồn khắc nghiệt.', N'Fear & Hunger là một trò chơi nhập vai kinh dị tâm lý độc lập. Game lấy bối cảnh trong một hầm ngục cổ đại, nơi người chơi phải đối mặt với những sinh vật kinh dị và cạm bẫy chết người. Với cơ chế permadeath và hệ thống chiến đấu khắc nghiệt, game mang đến trải nghiệm kinh dị và đầy thách thức.', N'/img/games/449a0b91-1dc2-4d2c-84b7-3194c00d3d91_fear_and_hunger.jpg', N'Miro Haverinen', N'Miro Haverinen', CAST(N'2018-10-30T00:00:00.000' AS DateTime), 8, 8.7, N'420K', NULL, N'500 MB', 8.5)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (7, N'Fear & Hunger 2', N'Phần tiếp theo của series RPG kinh dị với những cải tiến về đồ họa và gameplay.', N'Fear & Hunger 2 là phần tiếp theo của tựa game kinh dị cult classic. Game mang đến một thế giới mới rộng lớn hơn với nhiều địa điểm để khám phá, các sinh vật kinh dị mới và cốt truyện sâu sắc hơn. Vẫn giữ nguyên tinh thần hardcore và bầu không khí ám ảnh của phần đầu.', N'/img/games/fear_and_hunger_2.jpg', N'Miro Haverinen', N'Miro Haverinen', CAST(N'2021-12-24T00:00:00.000' AS DateTime), 8, 8.9, N'1', NULL, N'800 MB', 8.7)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (10, N'嗜血印 Bloody Spell', N'Game hành động võ hiệp Trung Quốc với hệ thống chiến đấu đẹp mắt và chặt chém đã tay.', N'Bloody Spell là một game hành động võ hiệp phong cách Trung Quốc với hệ thống chiến đấu nhanh, mãn nhãn. Game lấy bối cảnh thế giới cổ đại nơi người chơi vào vai một môn đồ võ lâm trên hành trình tìm kiếm báo thù và khám phá bí ẩn về "Bloody Spell" - một bùa chú cổ xưa đầy quyền năng.', N'/img/games/590378c5-7df6-4446-ab67-c166219b1dab_bloody_spell.jpg', N'Yooreka Studio', N'Yooreka Studio', CAST(N'2019-01-16T00:00:00.000' AS DateTime), 1, 7.9, N'420K', NULL, N'25 GB', 7.6)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (11, N'Boo Party', N'Game phiêu lưu hành động vui nhộn với nhân vật chính là những con ma đáng yêu.', N'Boo Party là một trò chơi phiêu lưu hành động đáng yêu với các nhân vật ma dễ thương. Người chơi sẽ điều khiển những con ma với khả năng đặc biệt khác nhau để giải các câu đố, vượt qua chướng ngại vật và khám phá thế giới đầy màu sắc. Game phù hợp với mọi lứa tuổi.', N'/img/games/boo_party.jpg', N'Ghost Games Studio', N'Playful Entertainment', CAST(N'2022-04-15T00:00:00.000' AS DateTime), 3, 7.5, N'120K', NULL, N'4 GB', 7.2)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (12, N'Steins;Gate', N'Visual novel khoa học viễn tưởng nổi tiếng về du hành thời gian với cốt truyện sâu sắc.', N'Steins;Gate là một visual novel khoa học viễn tưởng kinh điển kể về nhóm bạn trẻ tình cờ phát minh ra máy du hành thời gian và những hậu quả không lường trước được. Với cốt truyện phức tạp, nhân vật được xây dựng chặt chẽ và nhiều nhánh kết thúc khác nhau, Steins;Gate được coi là một trong những visual novel hay nhất mọi thời đại.', N'/img/games/steins_gate.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2009-10-15T00:00:00.000' AS DateTime), 16, 9.4, N'1.2M', NULL, N'6 GB', 9.2)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (13, N'Enthralled', N'Visual novel kinh dị tâm lý với nhiều lựa chọn ảnh hưởng đến cốt truyện.', N'Enthralled là một visual novel kinh dị tâm lý với những lựa chọn có ảnh hưởng sâu rộng đến diễn biến cốt truyện. Game lấy bối cảnh một thị trấn nhỏ bị ám ảnh bởi những hiện tượng siêu nhiên, nơi người chơi phải làm sáng tỏ những bí mật đen tối trong khi cố gắng giữ mạng sống của mình.', N'/img/games/enthralled.jpg', N'Dark Tale Studios', N'Sekai Project', CAST(N'2021-07-23T00:00:00.000' AS DateTime), 8, 8, N'190K', NULL, N'3 GB', 7.8)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (14, N'Occult Rewrite', N'Visual novel ma thuật với phong cách anime và nhiều nhánh cốt truyện.', N'Occult Rewrite là một visual novel về ma thuật và thế giới siêu nhiên. Người chơi vào vai một học viên ma thuật với khả năng "viết lại" thực tại, phải sử dụng sức mạnh này để ngăn chặn một thảm họa sắp xảy ra. Game nổi bật với artwork đẹp mắt, hệ thống nhân vật đa dạng và nhiều nhánh cốt truyện khác nhau.', N'/img/games/occult_rewrite.jpg', N'Mystery Circle', N'Moonlight Games', CAST(N'2022-01-10T00:00:00.000' AS DateTime), 16, 8.3, N'210K', NULL, N'5 GB', 8.1)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (15, N'Find Love or Die Trying', N'Dating sim với yếu tố kinh dị và nhiều twist bất ngờ trong cốt truyện.', N'Find Love or Die Trying là một trò chơi hẹn hò với twist kinh dị, nơi người chơi không chỉ tìm kiếm tình yêu mà còn phải sống sót. Bên dưới vẻ ngoài của một dating sim thông thường là những bí mật đen tối và những tình tiết rùng rợn. Mỗi lựa chọn đều có thể dẫn đến tình yêu hoặc cái chết.', N'/img/games/find_love_or_die.jpg', N'Twisted Date Studios', N'Horror Dating Inc', CAST(N'2021-02-14T00:00:00.000' AS DateTime), 8, 7.7, N'150K', NULL, N'2 GB', 7.5)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (16, N'Sable''s Grimoire', N'Visual novel fantasy về một học viện ma thuật với các nhân vật là sinh vật huyền bí.', N'Sable''s Grimoire là một visual novel fantasy lấy bối cảnh tại một học viện ma thuật, nơi con người học tập cùng các sinh vật huyền bí như rồng, tiên, người sói. Người chơi vào vai Sable, một học viên ma thuật con người, trong hành trình học tập và xây dựng mối quan hệ với các bạn học kỳ lạ của mình.', N'/img/games/sables_grimoire.jpg', N'Zetsubou Games', N'Sekai Project', CAST(N'2018-01-30T00:00:00.000' AS DateTime), 16, 8.2, N'280K', NULL, N'2 GB', 8)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (17, N'Tokyo Babel', N'Visual novel kịch tính về cuộc chiến giữa thiên đường và địa ngục.', N'Tokyo Babel là một visual novel kịch tính với bối cảnh ngày tận thế, nơi thiên đường và địa ngục đã bị phong ấn và các linh hồn tụ họp tại Tokyo Babel - một tháp Babel mới. Người chơi sẽ theo dõi câu chuyện của nhiều nhân vật với những động cơ khác nhau trong một cuộc chiến sinh tồn khốc liệt.', N'/img/games/tokyo_babel.jpg', N'PropellerTeam', N'MangaGamer', CAST(N'2011-05-27T00:00:00.000' AS DateTime), 16, 8.5, N'320K', NULL, N'4 GB', 8.3)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (18, N'My Lovely Wife', N'Mô phỏng quản lý hẹn hò độc đáo với yếu tố ma thuật đen tối và đạo đức xám.', N'My Lovely Wife là một trò chơi mô phỏng quản lý độc đáo kết hợp với yếu tố hẹn hò và ma thuật đen tối. Người chơi vào vai một người đàn ông tìm cách hồi sinh người vợ đã mất bằng cách triệu hồi và "hẹn hò" với các ác quỷ. Game thách thức người chơi với những lựa chọn đạo đức xám và kết quả không lường trước.', N'/img/games/my_lovely_wife.jpg', N'GameChanger Studio', N'Toge Productions', CAST(N'2021-08-10T00:00:00.000' AS DateTime), 6, 7.9, N'150K', NULL, N'1 GB', 7.6)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (19, N'The Letter - Horror Visual Novel', N'Visual novel kinh dị tâm lý về một biệt thự bị nguyền rủa với bảy nhân vật chính.', N'The Letter là một visual novel kinh dị tâm lý kể về bảy cá nhân liên quan đến một biệt thự cổ bị nguyền rủa. Sau khi nhận được một lá thư bí ẩn, họ bị cuốn vào một chuỗi sự kiện kinh hoàng. Game nổi bật với đồ họa đẹp mắt, âm thanh rùng rợn và cốt truyện đan xen giữa nhiều nhân vật với những góc nhìn khác nhau.', N'/img/games/the_letter.jpg', N'Yangyang Mobile', N'Yangyang Mobile', CAST(N'2017-07-24T00:00:00.000' AS DateTime), 8, 9, N'480K', NULL, N'7 GB', 8.8)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (20, N'Friday Night Funkin'' Remake', N'Phiên bản làm lại của trò chơi nhịp điệu độc lập nổi tiếng với nhiều bài hát và nhân vật mới.', N'Friday Night Funkin'' Remake là phiên bản nâng cấp của trò chơi nhịp điệu độc lập nổi tiếng. Game kể về một chàng trai phải đấu rap với nhiều đối thủ khác nhau để giành được trái tim của bạn gái. Phiên bản remake bổ sung thêm nhiều bài hát, nhân vật mới và cải thiện đáng kể về mặt đồ họa và gameplay.', N'/img/games/fnf_remake.jpg', N'Funkin'' Crew', N'Funkin'' Crew', CAST(N'2022-03-15T00:00:00.000' AS DateTime), 7, 8.5, N'1.5M', NULL, N'500 MB', 8.3)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (21, N'Utawarerumono: Prelude to the Fallen + All DLC', N'Visual novel chiến thuật với cốt truyện sâu sắc về một thế giới fantasy đầy bí ẩn.', N'Utawarerumono: Prelude to the Fallen là phần đầu tiên trong series visual novel kết hợp với game chiến thuật theo lượt. Game kể về một người đàn ông mất trí nhớ được cứu bởi một cô gái trẻ trong một ngôi làng nhỏ. Khi làng bị tấn công, anh buộc phải dẫn dắt dân làng chống lại kẻ thù và dần khám phá ra bí mật về bản thân và thế giới xung quanh.', N'/img/games/utawarerumono_prelude.jpg', N'Aquaplus', N'Shiravune', CAST(N'2020-05-26T00:00:00.000' AS DateTime), 16, 8.7, N'280K', NULL, N'10 GB', 8.5)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (22, N'Leisure Suit Larry - Wet Dreams Dry Twice', N'Game phiêu lưu hài hước người lớn với nhân vật chính là Larry Laffer cùng các tình huống hài hước.', N'Leisure Suit Larry - Wet Dreams Dry Twice là phần tiếp theo của series phiêu lưu hài hước dành cho người lớn. Game tiếp tục câu chuyện của Larry Laffer trong hành trình tìm kiếm tình yêu đích thực của mình. Với đồ họa hoạt hình sặc sỡ, các câu đố thông minh và rất nhiều tình huống hài hước, game mang đến nhiều tiếng cười cho người chơi.', N'/img/games/larry_wet_dreams.jpg', N'CrazyBunch', N'Assemble Entertainment', CAST(N'2020-10-23T00:00:00.000' AS DateTime), 3, 7.5, N'220K', NULL, N'8 GB', 7.3)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (23, N'Death end re;Quest 2', N'JRPG kinh dị với hệ thống chiến đấu độc đáo và cốt truyện ám ảnh.', N'Death end re;Quest 2 là một JRPG kết hợp với yếu tố kinh dị, kể về Mai Toyama - một cô gái trẻ đến trường nội trú Wordsworth để tìm kiếm người chị mất tích. Nhưng đằng sau vẻ ngoài yên bình của trường học là những bí mật đen tối và sinh vật kinh dị xuất hiện vào ban đêm. Game nổi bật với hệ thống chiến đấu kết hợp turn-based và action, cùng cốt truyện nhiều twist bất ngờ.', N'/img/games/death_end_request2.jpg', N'Compile Heart', N'Idea Factory', CAST(N'2020-08-18T00:00:00.000' AS DateTime), 2, 8.2, N'190K', NULL, N'15 GB', 8)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (24, N'SPACE-FRIGHT', N'Game kinh dị sinh tồn trong không gian với yếu tố stealth và nỗi sợ cô đơn.', N'SPACE-FRIGHT là một trò chơi kinh dị sinh tồn lấy bối cảnh trên một trạm không gian bị bỏ hoang. Người chơi vào vai một phi hành gia cô đơn phải tìm cách sống sót và thoát khỏi trạm không gian trong khi bị săn đuổi bởi một thực thể bí ẩn. Game tạo nên không khí căng thẳng thông qua yếu tố stealth và âm thanh đáng sợ.', N'/img/games/space_fright.jpg', N'Cosmic Horror Games', N'Deep Space Studios', CAST(N'2021-10-31T00:00:00.000' AS DateTime), 8, 8, N'1', NULL, N'5 GB', 7.7)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (25, N'Helltaker', N'Game giải đố ngắn với phong cách anime và nhiều ác quỷ xinh đẹp.', N'Helltaker là một trò chơi giải đố ngắn và miễn phí với phong cách anime. Người chơi vào vai một người đàn ông quyết tâm xây dựng một hậu cung toàn ác quỷ. Mỗi màn chơi, người chơi phải giải các câu đố dựa trên lưới để tiếp cận và thuyết phục một ác quỷ khác nhau gia nhập hậu cung của mình.', N'/img/games/helltaker.jpg', N'Vanripper', N'Vanripper', CAST(N'2020-05-11T00:00:00.000' AS DateTime), 7, 8.6, N'2.2M', NULL, N'100 MB', 8.7)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (26, N'Utawarerumono: Mask of Truth', N'Phần cuối của trilogy visual novel chiến thuật với cốt truyện sâu sắc về chiến tranh và chính trị.', N'Utawarerumono: Mask of Truth là phần cuối cùng của trilogy, tiếp nối trực tiếp sự kiện của Mask of Deception. Game kết hợp giữa visual novel và chiến thuật theo lượt, với cốt truyện tập trung vào cuộc nội chiến và những âm mưu chính trị trong một thế giới fantasy. Người chơi sẽ theo dõi câu chuyện về Haku và những đồng đội trong cuộc chiến sinh tồn và giành lại hòa bình.', N'/img/games/utawarerumono_mot.jpg', N'Aquaplus', N'Atlus', CAST(N'2017-09-05T00:00:00.000' AS DateTime), 16, 9, N'260K', NULL, N'12 GB', 8.9)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (27, N'Love at First Sight', N'Visual novel lãng mạn về mối tình giữa một chàng trai bình thường và cô gái có một mắt.', N'Love at First Sight là một visual novel lãng mạn kể về mối tình giữa Hiroki và Sachi - một cô gái đeo băng che mắt. Khi tình cảm phát triển, Hiroki dần khám phá ra bí mật đằng sau băng che mắt của Sachi và những khó khăn mà cô phải đối mặt. Một câu chuyện cảm động về việc vượt qua những rào cản về ngoại hình và xã hội.', N'/img/games/love_at_first_sight.jpg', N'Creepy Cute', N'Sekai Project', CAST(N'2015-10-07T00:00:00.000' AS DateTime), 16, 7.8, N'180K', NULL, N'1 GB', 7.5)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (28, N'Steins;Gate: My Darling''s Embrace', N'Visual novel lãng mạn trong vũ trụ Steins;Gate với các tuyến tình cảm khác nhau.', N'Steins;Gate: My Darling''s Embrace là một spin-off lãng mạn của series Steins;Gate nổi tiếng. Game tập trung vào các mối quan hệ và tình cảm giữa nhân vật chính Okabe Rintaro với các thành viên nữ trong phòng thí nghiệm. Với nhiều tuyến truyện khác nhau dành cho từng nhân vật, game mang đến một khía cạnh nhẹ nhàng và vui vẻ hơn so với cốt truyện chính.', N'/img/games/steinsgate_darling.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2019-12-10T00:00:00.000' AS DateTime), 16, 8.5, N'240K', NULL, N'4 GB', 8.2)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (29, N'Grotesque Beauty', N'Game kinh dị tâm lý về một họa sĩ bị mắc kẹt trong một gallery tranh biến dạng.', N'Grotesque Beauty là một trò chơi kinh dị tâm lý ngắn về một họa sĩ bị mắc kẹt trong một gallery tranh kỳ lạ. Khi khám phá không gian biến dạng của gallery, người chơi dần khám phá ra sự thật đen tối về chính họa sĩ và những tác phẩm của anh ta. Game nổi bật với phong cách nghệ thuật độc đáo và bầu không khí ám ảnh.', N'/img/games/grotesque_beauty.jpg', N'Distorted Art Games', N'Twisted Visions', CAST(N'2022-02-28T00:00:00.000' AS DateTime), 8, 7.6, N'130K', NULL, N'800 MB', 7.3)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (30, N'The Spiral Scouts', N'Game phiêu lưu giải đố với hình ảnh dễ thương nhưng nội dung hài hước dành cho người lớn.', N'The Spiral Scouts là một trò chơi phiêu lưu giải đố với phong cách hình ảnh đáng yêu nhưng nội dung hài hước và đôi khi hơi "đen". Người chơi vào vai Remae, một thành viên của tổ chức Spiral Scouts, phải giải các câu đố phức tạp để kiếm được huy hiệu và khám phá thế giới. Game nổi bật với các câu đố thông minh và lối chơi không theo tuyến tính.', N'/img/games/spiralscouts.jpg', N'Cantaloupe', N'DANG!', CAST(N'2018-09-24T00:00:00.000' AS DateTime), 3, 7.8, N'150K', NULL, N'900 MB', 7.5)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (31, N'I Love You, Colonel Sanders! A Finger Lickin'''' Good Dating Simulator', N'Dating sim hài hước lấy bối cảnh trường dạy nấu ăn với nam chính là Colonel Sanders nổi tiếng của KFC.', N'I Love You, Colonel Sanders! A Finger Lickin'''' Good Dating Simulator là một visual novel hẹn hò hài hước chính thức từ thương hiệu KFC. Người chơi vào vai một sinh viên trường nấu ăn, phải chinh phục trái tim của Colonel Sanders trẻ tuổi, đẹp trai. Game nổi bật với phong cách anime sống động, cốt truyện hài hước và những tình huống kỳ quặc đúng phong cách marketing độc đáo của KFC.', N'/img/games/colonel_sanders.jpg', N'Psyop', N'KFC', CAST(N'2019-09-24T00:00:00.000' AS DateTime), 16, 6.8, N'280K', NULL, N'500 MB', 10)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (32, N'Steins;Gate - Spin Offs', N'Tổng hợp các ngoại truyện của series Steins;Gate nổi tiếng với nhiều câu chuyện mới về các nhân vật quen thuộc.', N'Steins;Gate - Spin Offs là tuyển tập các visual novel ngoại truyện trong vũ trụ Steins;Gate, bao gồm "Steins;Gate: Linear Bounded Phenogram" và "Steins;Gate: Darling of Loving Vows". Các spin-off này mang đến những góc nhìn mới về các nhân vật quen thuộc, với nhiều câu chuyện phụ và kết thúc thay thế không xuất hiện trong cốt truyện chính.', N'/img/games/steinsgate_spinoffs.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2013-03-14T00:00:00.000' AS DateTime), 16, 8.6, N'2', NULL, N'5 GB', 6)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (33, N'Steins;Gate 0', N'Phần tiếp theo của visual novel khoa học viễn tưởng nổi tiếng, khám phá một dòng thời gian thay thế đầy bi kịch.', N'Steins;Gate 0 là phần tiếp theo của visual novel Steins;Gate đình đám, lấy bối cảnh trong một dòng thời gian nơi Okabe Rintarou đã từ bỏ nỗ lực cứu Makise Kurisu và đang phải vật lộn với hội chứng PTSD. Game đưa người chơi vào một câu chuyện tăm tối hơn, với nhiều biến cố và thử thách mới, đồng thời giải thích nhiều bí ẩn còn tồn tại từ phần đầu tiên.', N'/img/games/steinsgate0.jpg', N'5pb.', N'Spike Chunsoft', CAST(N'2015-12-10T00:00:00.000' AS DateTime), 16, 9.2, N'310K', NULL, N'7 GB', 9)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (34, N'Leisure Suit Larry - Wet Dreams Don''t Dry', N'Game phiêu lưu hài hước người lớn với nhân vật Larry Laffer cố gắng thích nghi với thời đại hiện đại.', N'Leisure Suit Larry - Wet Dreams Don''t Dry đưa nhân vật huyền thoại Larry Laffer từ thập niên 1980s vào thế giới hiện đại. Sau khi thức dậy bí ẩn sau nhiều thập kỷ, Larry phải học cách thích nghi với thế giới của smartphone, mạng xã hội và hẹn hò trực tuyến. Game là một trò chơi phiêu lưu point-and-click đầy hài hước và châm biếm về văn hóa hiện đại.', N'/img/games/larry_wet_dreams_dont_dry.jpg', N'CrazyBunch', N'Assemble Entertainment', CAST(N'2018-11-07T00:00:00.000' AS DateTime), 3, 7.3, N'1', NULL, N'6 GB', 7.1)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (35, N'Needy Streamer Overload', N'Mô phỏng quản lý một streamer với yếu tố tâm lý sâu sắc và nhiều kết thúc khác nhau.', N'Needy Streamer Overload là một trò chơi mô phỏng quản lý độc đáo, nơi người chơi đóng vai "P-chan" - người quản lý và bạn trai của một streamer mới nổi có vấn đề tâm lý tên là "OMGkawaiiAngel". Nhiệm vụ của người chơi là giúp cô ấy trở nên nổi tiếng trên internet, nhưng đồng thời phải cân bằng giữa sự nổi tiếng và sức khỏe tâm thần ngày càng xấu đi của cô. Game nổi bật với 20+ kết thúc khác nhau, từ hạnh phúc đến vô cùng đen tối.', N'/img/games/needy_streamer.jpg', N'WSS Playground', N'WSS Playground', CAST(N'2022-01-21T00:00:00.000' AS DateTime), 6, 8.5, N'1', NULL, N'1 GB', 8.3)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (36, N'Amelie', N'Visual novel kinh dị tâm lý về một cô gái trẻ khám phá những bí mật đen tối trong gia đình mình.', N'Amelie là một visual novel kinh dị tâm lý, kể về Amelie Blake - một cô gái trở về nhà sau nhiều năm xa cách để tham dự đám tang của mẹ mình. Trong quá trình ở lại ngôi nhà cũ, cô bắt đầu khám phá ra những bí mật đáng sợ về gia đình mình và quá khứ bị chôn vùi. Game nổi bật với không khí u ám, cốt truyện sâu sắc và nhiều lựa chọn ảnh hưởng đến kết cục của câu chuyện.', N'/img/games/amelie.jpg', N'Blueheart Studios', N'Dark Path Games', CAST(N'2021-07-30T00:00:00.000' AS DateTime), 8, 7.9, N'140K', NULL, N'2 GB', 6)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (37, N'Policenauts', N'Visual novel khoa học viễn tưởng của Hideo Kojima về một cảnh sát không gian điều tra vụ án bí ẩn.', N'Policenauts là một visual novel phiêu lưu khoa học viễn tưởng được phát triển bởi Hideo Kojima - cha đẻ series Metal Gear. Game kể về Jonathan Ingram, một cựu "Policenaut" (cảnh sát không gian) thức dậy sau 25 năm ngủ đông và phải điều tra vụ mất tích của vợ cũ. Cốt truyện phức tạp, đầy bất ngờ và mang phong cách điện ảnh đặc trưng của Kojima đã khiến game trở thành tác phẩm cult classic trong lòng người hâm mộ.', N'/img/games/c5115c4c-4d72-4f6b-80d9-21a03df8bb9d_policenauts.jpg', N'Konami', N'Konami', CAST(N'1996-01-19T00:00:00.000' AS DateTime), 1, 8.8, N'3', NULL, N'1.5 GB', 4)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (38, N'Pokemon Black 2', N'Phần tiếp theo của series Pokémon với vùng đất Unova được mở rộng và nhiều tính năng mới.', N'Pokemon Black 2 là phần tiếp theo trực tiếp của Pokemon Black, diễn ra hai năm sau các sự kiện ở game gốc. Người chơi khám phá vùng đất Unova đã thay đổi, với những thành phố mới, gym leader mới và tổ chức phản diện Team Plasma tái xuất với mục tiêu mới. Game bổ sung nhiều tính năng mới như Pokémon World Tournament, PokéStar Studios và Medal system, đồng thời mở rộng Pokédex với nhiều Pokémon từ các thế hệ trước.', N'/img/games/7e6d53b2-1742-483c-af59-0b50ed804ea3_4cfb1575-c629-4909-b5a4-2187e44ba440_pokemon_black2.jpg', N'Game Freak', N'Nintendo', CAST(N'2012-10-07T00:00:00.000' AS DateTime), 4, 8.7, N'5', NULL, N'512 MB', 6.666666666666667)
INSERT [dbo].[Games] ([Id], [Title], [ShortDescription], [FullDescription], [CoverImageUrl], [Developer], [Publisher], [ReleaseDate], [CategoryId], [Rating], [Downloads], [DownloadUrl], [Size], [AverageRating]) VALUES (39, N'yessssssssss', N'hehe boy', N'vcl dài rồi', N'/img/games/e7d22ca1-9d5e-41b5-b4b6-43f113da2322_z6126961936990_d00acf9ea290a2dfef7df5562fe67bde.jpg', N'fzstudio', N'bandai', CAST(N'2025-05-05T00:00:00.000' AS DateTime), 9, 1.8, N'5', NULL, N'25 GB', 0)
SET IDENTITY_INSERT [dbo].[Games] OFF
GO
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (1, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (1, 3)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (1, 5)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (1, 11)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (1, 12)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (1, 14)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (2, 1)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (2, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (2, 3)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (2, 5)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (2, 8)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (3, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (3, 3)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (3, 4)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (3, 11)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (3, 12)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (3, 14)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (4, 1)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (4, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (4, 13)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (4, 20)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 1)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 3)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 4)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 6)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 8)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (5, 11)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (6, 18)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (7, 18)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (10, 22)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (28, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (29, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (31, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (31, 25)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (32, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (33, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (35, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (36, 18)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (36, 23)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (37, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (37, 17)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (38, 2)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (38, 16)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (38, 17)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (39, 1)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (39, 3)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (39, 4)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (39, 16)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (39, 17)
INSERT [dbo].[GameTags] ([GameId], [TagId]) VALUES (39, 18)
GO
SET IDENTITY_INSERT [dbo].[LocalizationInfo] ON 

INSERT [dbo].[LocalizationInfo] ([Id], [GameId], [LocalizationType], [LocalizedBy], [LocalizationVersion], [LocalizationDate], [Description], [InstallationGuide], [DownloadUrl], [FileSize], [IsComplete]) VALUES (1, 1, N'Đầy đủ', N'VietHoaGame Team', N'1.2', CAST(N'2019-08-20T00:00:00.000' AS DateTime), N'Việt hóa đầy đủ các hội thoại, menu và nội dung game. Chất lượng dịch thuật cao.', N'Giải nén file và copy vào thư mục cài đặt game, ghi đè khi được hỏi.', N'/downloads/viet-hoa/witcher3-viet-hoa.zip', N'1.2 GB', 1)
INSERT [dbo].[LocalizationInfo] ([Id], [GameId], [LocalizationType], [LocalizedBy], [LocalizationVersion], [LocalizationDate], [Description], [InstallationGuide], [DownloadUrl], [FileSize], [IsComplete]) VALUES (2, 2, N'Menu & UI', N'GTA5-VN', N'2.1', CAST(N'2020-05-15T00:00:00.000' AS DateTime), N'Việt hóa các menu và giao diện người dùng, không bao gồm hội thoại.', N'Cài đặt bằng installer đi kèm, chọn thư mục cài đặt game khi được hỏi.', N'/downloads/viet-hoa/gta5-viet-hoa.exe', N'350 MB', 0)
INSERT [dbo].[LocalizationInfo] ([Id], [GameId], [LocalizationType], [LocalizedBy], [LocalizationVersion], [LocalizationDate], [Description], [InstallationGuide], [DownloadUrl], [FileSize], [IsComplete]) VALUES (3, 3, N'Một phần', N'CP2077-VN Collective', N'0.9 Beta', CAST(N'2022-02-28T00:00:00.000' AS DateTime), N'Việt hóa một phần các menu và nhiệm vụ chính, đang trong quá trình hoàn thiện.', N'1. Sao lưu thư mục game\n2. Giải nén và copy vào thư mục game\n3. Chạy script cài đặt kèm theo', N'/downloads/viet-hoa/cp2077-viet-hoa.rar', N'800 MB', 0)
SET IDENTITY_INSERT [dbo].[LocalizationInfo] OFF
GO
SET IDENTITY_INSERT [dbo].[PopularTags] ON 

INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (1, N'Open World', 1250, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (2, N'Việt Hóa', 980, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (3, N'Multiplayer', 870, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (4, N'FPS', 760, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (5, N'RPG', 650, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (6, N'Story Rich', 580, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (7, N'Horror', 450, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (8, N'Free to Play', 620, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (9, N'Racing', 380, 1)
INSERT [dbo].[PopularTags] ([Id], [TagName], [SearchCount], [IsActive]) VALUES (10, N'Co-op', 490, 1)
SET IDENTITY_INSERT [dbo].[PopularTags] OFF
GO
INSERT [dbo].[RelatedGames] ([GameId], [RelatedGameId], [RelationStrength]) VALUES (1, 2, 4)
INSERT [dbo].[RelatedGames] ([GameId], [RelatedGameId], [RelationStrength]) VALUES (1, 3, 7)
INSERT [dbo].[RelatedGames] ([GameId], [RelatedGameId], [RelationStrength]) VALUES (2, 1, 4)
INSERT [dbo].[RelatedGames] ([GameId], [RelatedGameId], [RelationStrength]) VALUES (2, 3, 5)
INSERT [dbo].[RelatedGames] ([GameId], [RelatedGameId], [RelationStrength]) VALUES (3, 1, 7)
INSERT [dbo].[RelatedGames] ([GameId], [RelatedGameId], [RelationStrength]) VALUES (3, 2, 5)
GO
SET IDENTITY_INSERT [dbo].[Reviews] ON 

INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (1, 1, N'2', N'Game nhập vai hay nhất từ trước đến nay', N'The Witcher 3 là một kiệt tác về mặt cốt truyện và gameplay. Thế giới mở rộng lớn với các nhiệm vụ phụ có chiều sâu, không hề nhàm chán như nhiều game khác. Hệ thống chiến đấu đầy thách thức nhưng rất thỏa mãn khi làm chủ. Đồ họa tuyệt đẹp ngay cả khi game đã ra mắt được vài năm.', 9.8, CAST(N'2021-06-15T00:00:00.000' AS DateTime), 45)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (2, 1, N'3', N'Tuyệt phẩm không thể bỏ qua', N'Từ cốt truyện, âm nhạc đến gameplay đều hoàn hảo. Các DLC như Blood and Wine còn hay hơn cả game gốc. Một trong những trải nghiệm chơi game tuyệt vời nhất của tôi.', 9.5, CAST(N'2021-08-20T00:00:00.000' AS DateTime), 32)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (3, 2, N'4', N'Los Santos sống động đến không ngờ', N'GTA 5 mang đến một thế giới sống động với chi tiết đáng kinh ngạc. Cốt truyện hấp dẫn với 3 nhân vật có tính cách khác biệt hoàn toàn. Tôi đặc biệt thích hệ thống nhiệm vụ cướp ngân hàng. GTA Online thì vô tận và luôn có điều mới mẻ.', 9.2, CAST(N'2020-12-10T00:00:00.000' AS DateTime), 28)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (5, 3, N'2', N'Đẹp nhưng chưa hoàn thiện', N'Cyberpunk 2077 có đồ họa tuyệt đẹp và thế giới Night City vô cùng chi tiết, nhưng game vẫn còn nhiều lỗi và chưa đạt được kỳ vọng ban đầu. Cốt truyện và nhân vật thì rất tốt, đặc biệt là Johnny Silverhand.', 7.5, CAST(N'2021-02-18T00:00:00.000' AS DateTime), 20)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (6, 3, N'3', N'Quá nhiều tiềm năng bị bỏ phí', N'Game có tiềm năng rất lớn nhưng bị phát hành quá sớm. Thế giới đẹp và nhân vật thú vị, nhưng gameplay còn nhiều điểm cần cải thiện. Các bản cập nhật gần đây đã làm game tốt hơn nhiều.', 7, CAST(N'2021-03-25T00:00:00.000' AS DateTime), 15)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (8, 4, N'4', N'Tiến bộ nhưng vẫn còn vấn đề', N'Game đã có những cải tiến về gameplay nhưng vẫn còn một số vấn đề về cân bằng và trí tuệ nhân tạo của cầu thủ. Career mode được cải thiện đáng kể so với các phiên bản trước.', 7.8, CAST(N'2022-11-15T00:00:00.000' AS DateTime), 12)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (9, 5, N'3', N'Sáng tạo vô tận', N'Minecraft là game mà bạn có thể chơi hàng nghìn giờ mà không chán. Khả năng sáng tạo gần như vô hạn và cộng đồng mod khổng lồ luôn mang đến những trải nghiệm mới.', 9.5, CAST(N'2020-07-20T00:00:00.000' AS DateTime), 40)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (10, 5, N'2', N'Đơn giản nhưng sâu sắc', N'Đồ họa đơn giản nhưng gameplay sâu sắc đến ngạc nhiên. Có thể chơi theo nhiều cách khác nhau, từ xây dựng, khám phá đến chiến đấu. Hoàn hảo để chơi với bạn bè.', 9, CAST(N'2021-09-05T00:00:00.000' AS DateTime), 25)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (11, 37, N'System.Security.Cryptography.RandomNumberGeneratorImplementation', N'hay', N'heheboy', 4, CAST(N'2025-04-29T21:08:22.730' AS DateTime), 0)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (12, 38, N'af2e399f-0969-4c72-8760-1b3e64e9c072', N'hay', N'game hay nha :3', 5, CAST(N'2025-04-30T00:51:20.920' AS DateTime), 0)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (13, 38, N'System.Security.Cryptography.RandomNumberGeneratorImplementation', N'tôi yêu mew', N'cho tui 1 mod có mew đê :v', 5, CAST(N'2025-04-30T19:51:39.143' AS DateTime), 0)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (14, 36, N'af2e399f-0969-4c72-8760-1b3e64e9c072', N'ko giòn', N'game đã căng ...... ko giòn :v', 6, CAST(N'2025-05-01T20:46:56.857' AS DateTime), 0)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (15, 32, N'System.Security.Cryptography.RandomNumberGeneratorImplementation', N'ko giòn', N'game vcl :v', 6, CAST(N'2025-05-03T13:16:39.560' AS DateTime), 0)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (16, 31, N'System.Security.Cryptography.RandomNumberGeneratorImplementation', N'vcl', N'game vcl thật :v', 10, CAST(N'2025-05-05T07:47:46.173' AS DateTime), 0)
INSERT [dbo].[Reviews] ([Id], [GameId], [UserId], [Title], [Content], [Rating], [DatePosted], [HelpfulCount]) VALUES (17, 38, N'4616f820-795b-4673-ab82-b4a7e34a12ea', N'...', N'Quá xuất sắc 😆', 10, CAST(N'2025-05-05T12:15:57.447' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Reviews] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name], [Description], [CreatedAt]) VALUES (1, N'Regular', N'Người dùng thường với quyền truy cập cơ bản vào trang web', CAST(N'2025-04-27T16:21:19.007' AS DateTime))
INSERT [dbo].[Roles] ([Id], [Name], [Description], [CreatedAt]) VALUES (2, N'Moderator', N'Người điều hành', CAST(N'2025-04-27T16:21:19.007' AS DateTime))
INSERT [dbo].[Roles] ([Id], [Name], [Description], [CreatedAt]) VALUES (3, N'Admin', N'Quản trị viên với toàn quyền kiểm soát trang web', CAST(N'2025-04-27T16:21:19.007' AS DateTime))
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Screenshots] ON 

INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (5, 2, N'/img/screenshots/gta5_ss1.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (6, 2, N'/img/screenshots/gta5_ss2.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (7, 2, N'/img/screenshots/gta5_ss3.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (8, 2, N'/img/screenshots/gta5_ss4.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (9, 3, N'/img/screenshots/cyberpunk_ss1.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (10, 3, N'/img/screenshots/cyberpunk_ss2.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (11, 3, N'/img/screenshots/cyberpunk_ss3.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (12, 4, N'/img/screenshots/fifa23_ss1.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (13, 4, N'/img/screenshots/fifa23_ss2.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (14, 4, N'/img/screenshots/fifa23_ss3.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (15, 5, N'/img/screenshots/minecraft_ss1.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (16, 5, N'/img/screenshots/minecraft_ss2.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (17, 5, N'/img/screenshots/minecraft_ss3.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (18, 38, N'/img/games/screenshots/8179fea7-7d51-4f4d-9df9-d197306365bd_cobalion-ow.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (20, 6, N'/img/games/screenshots/62f872db-3f64-4cb5-88bc-0acbcd100ab9_31605d23d34990ec3e0956537b4be36336125420.gif')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (23, 10, N'/img/games/screenshots/5ac0fa99-d83b-44d5-9f50-215cee13b00d_sddefault.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (24, 10, N'/img/games/screenshots/03885944-e296-400d-a3e4-536c00fe1b94_ss_f0fbbaa5e31e477f4f131664fbd064ec61302504.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (25, 37, N'/img/games/screenshots/f125156c-5c0d-4d9b-9ad3-02803c2940fc_policenauts_3.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (26, 37, N'/img/games/screenshots/824529d1-bed6-430e-b5f2-5c38b085a3f4_policenauts_6.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (27, 38, N'/img/games/screenshots/228593b8-4999-49c2-a413-137172db1f6f_pkmb2.jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (28, 6, N'/img/games/screenshots/aee605f7-0f9c-42a4-83c6-90ebcb952a19_ss_673b04dcbc3f1eb26e3ae8f0aae5520df03f9adf.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (29, 6, N'/img/games/screenshots/d60976d3-56f4-4139-af2e-b716aaf4057a_34GAou.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (30, 35, N'/img/games/screenshots/25c8234e-a1a7-47b9-aaf7-d75e7b2970f1_Needy_Streamer_Overload_screenshot.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (31, 35, N'/img/games/screenshots/ce14c881-8ce5-42cd-96e3-102f43757cb2_ffbfb80cab807ebf43aeeadbb21e5fb280c8cb39.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (32, 35, N'/img/games/screenshots/89a5d755-504b-4c51-ba81-e07762ac8aa7_images.jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (33, 7, N'/img/games/screenshots/33584dd5-4a9a-4b9a-9ee2-5e210ae5d28a_images (1).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (34, 7, N'/img/games/screenshots/fa506b20-0667-4a06-a565-a9ab7484a6e1_ss_587cae4e1429e5bd6da6932c2cccb466443eb35d.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (35, 7, N'/img/games/screenshots/d757c8b0-53a2-468f-86d8-1123326d5f6b_images (2).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (42, 10, N'/img/games/screenshots/37f52231-a9f0-44d0-a181-fab977c452df_1616378931222928.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (43, 10, N'/img/games/screenshots/891637ec-a970-4403-8b6e-01e8b94daee2_b86e76916241b6dec23acee513998dd599f418f1.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (44, 14, N'/img/games/screenshots/57f5ccee-b1a8-4eb0-9e6b-d4ba396229eb_images (4).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (45, 14, N'/img/games/screenshots/1ba82ed7-de35-44d0-bfb9-b54d70fd874c_f53a62950b715e9453bef83fd079f321998f4790.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (46, 15, N'/img/games/screenshots/ac310150-d696-423a-811b-aaca06bf5470_ss_95cb486fd5f25637e8486edd92bb961ca5768114.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (47, 15, N'/img/games/screenshots/b1e1818c-48fc-4dfe-b765-9080f6cf5d96_find-love-or-die-trying-not-an-amazing-game-but-a-couple-of-v0-2u4bgtr3ye6d1.webp')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (48, 15, N'/img/games/screenshots/8e5f7e83-ca6a-4f20-a484-97b2b69169e3_hq720.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (49, 16, N'/img/games/screenshots/ea90366c-a153-496f-98e6-31fe91252990_14e6e826829eb2316ae08aae4450cefd5a26b485d83ae8e117c8da03dc54a4cd.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (50, 16, N'/img/games/screenshots/33f05340-89e0-423b-b319-32c2a5ae94e6_NSwitchDS_SablesGrimoire_03.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (51, 16, N'/img/games/screenshots/f8e93656-4d73-49a6-ac5c-4323b2bd3868_a43750e696249f5389a77635fab973bd3efba2b339406278f2e208bb80c9f17a.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (52, 17, N'/img/games/screenshots/870f88d9-2477-47f5-b6c1-faad57caaed0_maxresdefault.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (53, 17, N'/img/games/screenshots/e9c4deb7-9ade-4c99-acc7-a6eef02bdb10_hq720 (1).jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (54, 17, N'/img/games/screenshots/ec0f13be-5e5a-4536-ace6-6ff9c0d0b72c_images (5).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (55, 18, N'/img/games/screenshots/11d4cec1-7a59-4f5b-ba56-fab70af03bd4_0.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (56, 18, N'/img/games/screenshots/ffc28a51-87e0-4aad-8925-f8ea375e2f7f_ss_07cc686c1273acbc0402ce8e973f75221866a46b.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (57, 18, N'/img/games/screenshots/b7edac9b-ea52-475a-ae2c-cd585cc97b69_d56693139a72c70179bafa484315bc9e.webp')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (58, 18, N'/img/games/screenshots/2e370bde-a883-44cd-8a5d-512700bab9e5_afkmobi-wife-01.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (59, 19, N'/img/games/screenshots/d315aaa2-8b46-44bd-912a-b5ee5e9073ce_images (6).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (60, 19, N'/img/games/screenshots/f8a9b818-9347-4fc6-9c12-c08499fcfc2d_hq720 (2).jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (61, 20, N'/img/games/screenshots/ae42ed1a-d577-4762-b13d-49a4ae187c76_images (8).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (62, 20, N'/img/games/screenshots/d00f0073-7da4-48f6-a105-78cff3669563_6453801909fd7.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (63, 20, N'/img/games/screenshots/cdf32ca8-f9e1-496b-b5a8-b93ee315488b_images (7).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (64, 21, N'/img/games/screenshots/2022bfb2-fa09-4c00-a062-370a1f44c598_ss_f8e2127602bca4a86b11b1b4498e8d51d3d02885.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (65, 21, N'/img/games/screenshots/d5cbce86-852c-422a-9fc5-2a310d612fee_images (9).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (66, 21, N'/img/games/screenshots/995603a1-0b29-4f48-918d-8e2777c25a43_sys11_02.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (67, 22, N'/img/games/screenshots/3eaf9572-5d4b-4e17-91d0-aac65a361873_ss_b72c057a35a38d49e3cc7638a7e13179d47cfb41.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (68, 22, N'/img/games/screenshots/7987ad50-38c9-4601-bfbd-37b61c48fbfd_528513_w926.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (69, 22, N'/img/games/screenshots/d6fd4d18-a5d1-4437-9d5c-84f0146da41d_81TM86cuJIL._AC_UF350,350_QL80_.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (70, 23, N'/img/games/screenshots/215cb2fd-4d3a-4fc4-a8b4-5ddd94653345_download.jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (71, 23, N'/img/games/screenshots/7779d9c2-0698-4d55-b674-414dcb228ce3_death-end-request-2_2020-11-07_009.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (72, 23, N'/img/games/screenshots/60b8100f-e933-46c6-bf02-2b3449e1fe47_maxresdefault (1).jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (73, 24, N'/img/games/screenshots/adff078e-e3a7-42e9-b669-2886c6f37304_ss_a501ae6e3fe90546dece59f02878a42264658b6a.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (74, 24, N'/img/games/screenshots/e2105c76-25f5-46a3-9441-920887aebe1b_maxresdefault (2).jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (75, 25, N'/img/games/screenshots/cfd5c8e9-7899-43c0-bc8a-fb5a5e945f0f_ss_0682c86b6a140bb293f86f03fa0404369c0bba25.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (76, 25, N'/img/games/screenshots/dba0513c-61cb-47e1-bff5-33be67e8990a_ss_0e0102242f02f583133f373dc46c74535e5bdbb0.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (77, 25, N'/img/games/screenshots/e1bda22f-6fd2-43e5-a73c-ca0296991d6f_pjomc5nkbrm81.gif')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (78, 26, N'/img/games/screenshots/47b143ec-8463-4f9a-978e-ef27d5b50dfa_ss_b9b3a809e53f430bd090e7a10f26264e466fda0a.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (79, 27, N'/img/games/screenshots/7acbfff2-b328-44ce-881a-3170cc8c877d_images (10).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (80, 27, N'/img/games/screenshots/58f211e1-924a-41a0-ac06-397741919817_ydolaivrxczx4uam90qm.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (81, 28, N'/img/games/screenshots/8c1a8b65-1d61-420f-9577-08bfff46b6c4_unknown-1024x576.png')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (82, 28, N'/img/games/screenshots/113b9fae-dce6-4df5-ae9b-4df36d286376_tumblr_69d7800cb7b429a721ea2406013e9d8d_e3785bfc_1280.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (83, 28, N'/img/games/screenshots/6b1255f7-a1e6-4316-912e-f486ff531569_images (11).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (84, 29, N'/img/games/screenshots/42d56672-76c2-4dbc-b74a-6b15df75ab23_images (13).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (85, 29, N'/img/games/screenshots/2af67994-b225-42e4-a099-eee64110ee1a_ss_963ca52160314a24dd5abfde61400f56fdebcff3.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (86, 29, N'/img/games/screenshots/f4e7018b-80c6-41a1-adc0-c58012d40c57_images (12).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (87, 30, N'/img/games/screenshots/b6566ee4-af08-4b64-a007-d0609d73598a_ss_7a86ace56747e7d79363daef04fbd035bc7a9c93.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (88, 30, N'/img/games/screenshots/139ddeaa-1e4f-40d7-9b37-eca62a63cc40_download (1).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (89, 30, N'/img/games/screenshots/90a2997f-7c71-485e-bd6a-1a76aef69763_ss_3ee702361fdbc2e6cbb305dd7225251e9a882325.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (90, 32, N'/img/games/screenshots/657eafa0-a3a9-45ac-aec0-c72b0029bd31_1728989464-ef259d43b94b7ca08f643491a02da17b.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (91, 32, N'/img/games/screenshots/732583fe-e59a-4607-9411-aaf7824c224a_images (15).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (92, 32, N'/img/games/screenshots/8b5bf58f-8e97-47ab-8643-42e67255c0a5_images (14).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (93, 33, N'/img/games/screenshots/57f78f83-3fe9-43ec-861f-eac4cef050f7_images (11).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (94, 33, N'/img/games/screenshots/d83ef5e9-e34d-496a-a0ef-2f70bc1f50d3_images (14).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (95, 33, N'/img/games/screenshots/086fcbe1-8e4a-4fb4-8801-7f9944f32f38_1728989464-ef259d43b94b7ca08f643491a02da17b.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (96, 34, N'/img/games/screenshots/a261bae4-2559-4e02-9b28-cecc82c921eb_leisure_suit_larry_wet_dreams_don''t_dry_announcement-550x309.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (97, 34, N'/img/games/screenshots/1459ae4a-7239-4a68-8066-3fb983993222_AT-cm_339731902-preview-480x272.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (98, 34, N'/img/games/screenshots/96ab1257-2c71-4aa3-9c54-f9fe3bd1a9dd_images (16).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (99, 38, N'/img/games/screenshots/3eb9041f-1ce5-4095-b0a1-8ad47f72bdba_images (17).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (100, 36, N'/img/games/screenshots/b891b390-f705-4227-91a4-7f8cb97a6581_download (3).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (101, 36, N'/img/games/screenshots/24f82459-109f-4b2a-980c-c87f496c1660_download (2).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (102, 36, N'/img/games/screenshots/91d433d7-f50e-4e05-870b-7bf69eabd80a_images (18).jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (103, 31, N'/img/games/screenshots/03942b8d-4cc2-486f-b785-0e743f5402c6_ss_5f56f3553e910786e010e27b5c575e1bdee24cc5.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (104, 31, N'/img/games/screenshots/baaca86d-ad90-4599-ad1c-8921b1b1a0be_ss_8bb57fb03d5e7ef2078f323a745e33332b73e605.1920x1080.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (105, 31, N'/img/games/screenshots/d9464ffb-25f7-4bf9-aa38-d565f52e7067_chest.jpeg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (106, 1, N'/img/games/screenshots/02720126-26c2-4d07-ad42-e578b2dadb8b_48064.gif')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (107, 1, N'/img/games/screenshots/c728a957-e973-4f55-b660-465df3a9a441_hq720 (4).jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (108, 1, N'/img/games/screenshots/0c286552-e1bd-498b-a4c0-724bdccfedc2_hq720 (3).jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (109, 1, N'/img/games/screenshots/9ef4dfbe-b03d-463e-a9fd-634709cf65fa_game-the-witcher-3-wild-hunt-complete-edition-ps5-3_706291_63eb3b4f7781e7.19334861.jpg')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (110, 38, N'/img/games/screenshots/e3d252b2-685c-4902-a0e1-ca367877e596_bw-battle.gif')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (111, 38, N'/img/games/screenshots/631b6141-eae1-410a-86f5-67996090ce1f_tumblr_mkjgu4jP5H1qlh7rxo1_500.gif')
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (112, 39, N'/img/games/screenshots/e1880cb1-57e6-4d51-919c-00c94cfa307c_chest.jpeg')
GO
INSERT [dbo].[Screenshots] ([Id], [GameId], [ImageUrl]) VALUES (113, 39, N'/img/games/screenshots/9c189e1c-667a-48c5-a03a-113dffe03599_48064.gif')
SET IDENTITY_INSERT [dbo].[Screenshots] OFF
GO
SET IDENTITY_INSERT [dbo].[SearchHistory] ON 

INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (1, N'2', N'witcher 3', CAST(N'2022-10-15T00:00:00.000' AS DateTime), 5)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (2, N'2', N'game việt hóa', CAST(N'2022-09-20T00:00:00.000' AS DateTime), 30)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (3, N'3', N'game offline hay 2022', CAST(N'2022-11-05T00:00:00.000' AS DateTime), 15)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (4, N'3', N'cyberpunk 2077 crack', CAST(N'2022-10-10T00:00:00.000' AS DateTime), 3)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (5, N'4', N'game đua xe', CAST(N'2022-09-18T00:00:00.000' AS DateTime), 8)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (7, NULL, N'game sinh tồn', CAST(N'2022-11-10T00:00:00.000' AS DateTime), 11)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (8, NULL, N'gta 6 release date', CAST(N'2022-11-11T00:00:00.000' AS DateTime), 0)
INSERT [dbo].[SearchHistory] ([Id], [UserId], [SearchTerm], [SearchDate], [ResultCount]) VALUES (9, NULL, N'game việt hóa hay nhất', CAST(N'2022-10-25T00:00:00.000' AS DateTime), 25)
SET IDENTITY_INSERT [dbo].[SearchHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemRequirements] ON 

INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (1, 1, N'Minimum', N'Windows 7/8/10 (64-bit)', N'Intel CPU Core i5-2500K 3.3GHz / AMD CPU Phenom II X4 940', N'6 GB RAM', N'Nvidia GPU GeForce GTX 660 / AMD GPU Radeon HD 7870', N'11', N'35 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (2, 1, N'Recommended', N'Windows 7/8/10 (64-bit)', N'Intel CPU Core i7 3770 3.4 GHz / AMD CPU AMD FX-8350 4 GHz', N'8 GB RAM', N'Nvidia GPU GeForce GTX 770 / AMD GPU Radeon R9 290', N'11', N'50 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (3, 2, N'Minimum', N'Windows 10 64-bit', N'Intel Core i5-4460 (4 CPU) @ 3.40GHz / AMD Ryzen 3 1200', N'8 GB RAM', N'NVIDIA GTX 750 Ti 2GB / AMD Radeon R7 260x 2GB', N'11', N'72 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (4, 2, N'Recommended', N'Windows 10 64-bit', N'Intel Core i7-4770 / AMD Ryzen 5 1500X', N'16 GB RAM', N'NVIDIA GTX 1060 6GB / AMD RX 580 4GB', N'11', N'72 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (5, 3, N'Minimum', N'Windows 10 (64-bit)', N'Intel Core i5-3570K / AMD FX-8310', N'8 GB RAM', N'NVIDIA GeForce GTX 780 3GB / AMD Radeon RX 470', N'12', N'70 GB SSD')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (6, 3, N'Recommended', N'Windows 10 (64-bit)', N'Intel Core i7-4790 / AMD Ryzen 3 3200G', N'12 GB RAM', N'NVIDIA GeForce GTX 1060 6GB / AMD Radeon R9 Fury', N'12', N'70 GB SSD')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (7, 4, N'Minimum', N'Windows 10 64-bit', N'Intel Core i5-6600K / AMD Ryzen 5 1600', N'8 GB RAM', N'NVIDIA GeForce GTX 1050 Ti / AMD Radeon RX 570', N'12', N'50 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (8, 4, N'Recommended', N'Windows 10 64-bit', N'Intel Core i7-6700 / AMD Ryzen 7 2700X', N'12 GB RAM', N'NVIDIA GeForce GTX 1660 / AMD Radeon RX 5600 XT', N'12', N'50 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (9, 5, N'Minimum', N'Windows 7 or later', N'Intel Core i3-3210 / AMD A8-7600', N'4 GB RAM', N'Integrated: Intel HD Graphics 4000 / AMD Radeon R5', N'11', N'1 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (10, 5, N'Recommended', N'Windows 10', N'Intel Core i5-4690 / AMD A10-7800', N'8 GB RAM', N'NVIDIA GeForce GTX 700 Series / AMD Radeon Rx 200 Series', N'11', N'4 GB')
INSERT [dbo].[SystemRequirements] ([Id], [GameId], [RequirementType], [OS], [Processor], [Memory], [Graphics], [DirectX], [Storage]) VALUES (11, 10, N'Minimum', N'Windows 7/8/10 64-bit.', N'Intel i5 Processor.', N'8 GB RAM', N'Nvidia GeForce GTX 770', N'Phiên bản 10', N'25 GB chỗ trống')
SET IDENTITY_INSERT [dbo].[SystemRequirements] OFF
GO
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (1, N'Multiplayer', N'multiplayer')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (2, N'Single Player', N'single-player')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (3, N'Open World', N'open-world')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (4, N'First Person', N'first-person')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (5, N'Third Person', N'third-person')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (6, N'Co-op', N'co-op')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (7, N'Battle Royale', N'battle-royale')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (8, N'Sandbox', N'sandbox')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (9, N'Stealth', N'stealth')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (10, N'VR Support', N'vr-support')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (11, N'Offline', N'offline')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (12, N'Story Rich', N'story-rich')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (13, N'Competitive', N'competitive')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (14, N'Action RPG', N'action-rpg')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (15, N'Simulation', N'simulation')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (16, N'Free to Play', N'free-to-play')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (17, N'Việt Hóa', N'viet-hoa')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (18, N'Horror', N'horror')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (19, N'Racing', N'racing')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (20, N'Sports', N'sports')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (22, N'Nhập vai', N'nhap_vai')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (23, N'visual', N'visual')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (24, N'16+', N'16+')
INSERT [dbo].[Tags] ([Id], [Name], [Slug]) VALUES (25, N'anime', N'ani')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'af2e399f-0969-4c72-8760-1b3e64e9c072', 6, 1, CAST(N'2025-05-02T19:43:12.913' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 1, 0, CAST(N'2025-05-07T17:36:00.880' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 2, 0, CAST(N'2025-05-07T17:30:22.947' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 3, 0, CAST(N'2025-05-07T17:45:09.550' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 4, 0, CAST(N'2025-05-07T18:14:50.177' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 5, 0, CAST(N'2025-05-09T19:47:28.543' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 6, 0, CAST(N'2025-05-07T19:09:30.887' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 7, 0, CAST(N'2025-06-05T22:28:37.240' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 8, 0, CAST(N'2025-06-05T22:28:40.030' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 9, 0, CAST(N'2025-06-05T22:28:44.080' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 10, 0, CAST(N'2025-06-05T21:59:42.097' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 11, 1, CAST(N'2025-06-05T22:28:54.550' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 12, 0, CAST(N'2025-06-05T23:25:49.357' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 13, 0, CAST(N'2025-06-05T22:29:11.543' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 14, 0, CAST(N'2025-06-05T21:59:48.667' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 15, 0, CAST(N'2025-06-08T20:42:49.107' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', 16, 0, CAST(N'2025-06-06T15:56:05.237' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', 1, 0, CAST(N'2025-05-01T21:40:16.617' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', 5, 0, CAST(N'2025-05-01T22:30:14.817' AS DateTime))
INSERT [dbo].[UserAvatarFrames] ([UserId], [FrameId], [IsEquipped], [ObtainedDate]) VALUES (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', 6, 1, CAST(N'2025-05-01T22:39:58.010' AS DateTime))
GO
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'2', N'Hồng', N'Nguyễn', N'Hong_Premium', N'hong@gmail.com', N'Game thủ chuyên nghiệp', N'/img/avatars/premium1.jpg', CAST(N'2025-04-27T16:21:53.160' AS DateTime), N'AQAAAAIAAYagAAAAEN5D0cB++VJP3F1XKdU/tc9S/AkJNG8FUQdXVLLw9RBmGLvEfR2XtcB71RffMyJK6Q==', N'YUKQDZVJSMA5PPRWWPTIO2D5OJAEZM7K', N'HONG@GMAIL.COM', 1, 2, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'3', N'Minh', N'Lê', N'MinhGamer', N'minh@hotmail.com', N'Đam mê game AAA', N'/img/avatars/user1.jpg', CAST(N'2025-04-27T16:21:53.160' AS DateTime), N'AQAAAAIAAYagAAAAECVfZoIDaUBgqAZvQZGFP0fQjAyryXytdzlTIPXqU04x4Esx+WBGpGLqTb7ty96Yjg==', N'BLGA3XZXJ2LI7CRXBWCMFDSSK4MSD7YD', N'MINH@HOTMAIL.COM', 1, 1, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'4', N'Thảo', N'Trần', N'ThaoGaming', N'thao@yahoo.com', N'Chuyên game indie', N'/img/default-avatar.png', CAST(N'2025-04-27T16:21:53.160' AS DateTime), N'AQAAAAIAAYagAAAAEGrf+P5iJ3XYdYR+np2ci+yBKFMJXkQ9YVpNcz2aGo0d2cwemzehId+nUJ388zANsA==', N'Z4KCUCISL6BQ6LVBPFMKVU4KSCGZMNYX', N'THAO@YAHOO.COM', 1, 1, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'4616f820-795b-4673-ab82-b4a7e34a12ea', N'Tuong', N'Pham', N'PT', N'tuongpham@gmail.com', NULL, N'/img/avartars/PT.jpg', CAST(N'2025-05-05T12:14:09.460' AS DateTime), N'71a749b5dcf73fb9bcfcd3facee7ace21301f6f7c1b36c15a67f692fc27d99c3', NULL, NULL, 0, 1, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'4724c7e5-f291-4d88-8557-d89d4391b4b9', N'Tan Loc', N'Nguyen', N'GiaoHopChanNhan', N'locLoLi991@gmail.com', NULL, N'/img/avartars/GiaoHopChanNhan.jpg', CAST(N'2025-04-30T18:39:32.543' AS DateTime), N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', NULL, N'LOCLOLI991@GMAIL.COM', 0, 1, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'5c65906c-16a2-4cf4-80b5-8d4c1e916feb', N'Femboy', N'Killer', N'FemboyKiller', N'7554ntl@gmail.com', NULL, N'/img/avartars/FemboyKiller.jpg', CAST(N'2025-05-01T20:26:41.883' AS DateTime), N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', NULL, N'7554NTL@GMAIL.COM', 1, 2, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'8b32518f-646a-4917-98d3-778d4f5b20af', N'ngoc vy', N'huynh', N'vy', N'ngocvylegend1412@gmail.com', NULL, NULL, CAST(N'2025-04-30T09:26:14.130' AS DateTime), N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', NULL, NULL, 0, 1, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'af2e399f-0969-4c72-8760-1b3e64e9c072', N'ngoc lam', N'huynh', N'lam', N'ngoclamlegend1412@gmail.com', NULL, N'/img/avartars/lam.jpg', CAST(N'2025-04-29T18:12:54.240' AS DateTime), N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', NULL, N'NGOCLAMLEGEND1412@GMAIL.COM', 1, 2, CAST(N'2026-05-01T20:48:01.590' AS DateTime), 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'cd38c5e6-b3e4-4b79-9d7b-aaf037902a95', N'DUC', N'NGUYEN THANH', N'Rinri', N'nguyenthanhduc70pleiku@gmail.com', NULL, N'/img/avartars/Rinri.jpg', CAST(N'2025-05-01T17:14:49.650' AS DateTime), N'2c224e981bc7c409a95083fb418d5b51fc880b088ba2834c997d28d49a377d2c', NULL, N'NGUYENTHANHDUC70PLEIKU@GMAIL.COM', 1, 3, NULL, 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'ce60ba0e-c2d2-43b6-b119-5eba44844563', N'mesouya', N'mesouya', N'mesouya', N'mesouya@gmail.com', NULL, N'/img/avartars/mesouya.jpg', CAST(N'2025-05-01T20:21:04.133' AS DateTime), N'17a9de098ee3622efafe4d69b57802a2389f3887cd62a29763413dda82bef68a', NULL, N'MESOUYA@GMAIL.COM', 1, 2, CAST(N'2025-07-11T21:28:02.080' AS DateTime), 0)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [DisplayName], [Email], [Bio], [AvatarUrl], [CreatedAt], [PasswordHash], [SecurityStamp], [NormalizedEmail], [EmailConfirmed], [RoleId], [PremiumExpiryDate], [RememberMe]) VALUES (N'System.Security.Cryptography.RandomNumberGeneratorImplementation', N'zhou', N'lu', N'long', N'pkav113@gmail.com', NULL, N'/img/avartars/long.jpg', CAST(N'2025-04-28T00:06:13.460' AS DateTime), N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', NULL, NULL, 1, 3, CAST(N'2025-05-28T00:06:13.457' AS DateTime), 0)
GO
ALTER TABLE [dbo].[AvatarFrames] ADD  DEFAULT ((1)) FOR [RarityLevel]
GO
ALTER TABLE [dbo].[AvatarFrames] ADD  DEFAULT ((0)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[AvatarFrames] ADD  DEFAULT (NULL) FOR [RequiredLevel]
GO
ALTER TABLE [dbo].[AvatarFrames] ADD  DEFAULT ((0)) FOR [IsPremium]
GO
ALTER TABLE [dbo].[AvatarFrames] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[AvatarFrames] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [GameCount]
GO
ALTER TABLE [dbo].[CrackInfo] ADD  DEFAULT ((0)) FOR [IsRecommended]
GO
ALTER TABLE [dbo].[DownloadHistory] ADD  DEFAULT (getdate()) FOR [DownloadDate]
GO
ALTER TABLE [dbo].[FavoriteGames] ADD  DEFAULT (getdate()) FOR [DateAdded]
GO
ALTER TABLE [dbo].[GameLinks] ADD  DEFAULT ((1)) FOR [PartNumber]
GO
ALTER TABLE [dbo].[GameLinks] ADD  DEFAULT ((1)) FOR [TotalParts]
GO
ALTER TABLE [dbo].[GameLinks] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[GameLinks] ADD  DEFAULT (getdate()) FOR [DateAdded]
GO
ALTER TABLE [dbo].[Games] ADD  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Games] ADD  DEFAULT ('0') FOR [Downloads]
GO
ALTER TABLE [dbo].[Games] ADD  DEFAULT ((0)) FOR [AverageRating]
GO
ALTER TABLE [dbo].[LocalizationInfo] ADD  DEFAULT ((0)) FOR [IsComplete]
GO
ALTER TABLE [dbo].[PopularTags] ADD  DEFAULT ((0)) FOR [SearchCount]
GO
ALTER TABLE [dbo].[PopularTags] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[RelatedGames] ADD  DEFAULT ((1)) FOR [RelationStrength]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT (getdate()) FOR [DatePosted]
GO
ALTER TABLE [dbo].[Reviews] ADD  DEFAULT ((0)) FOR [HelpfulCount]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SearchHistory] ADD  DEFAULT (getdate()) FOR [SearchDate]
GO
ALTER TABLE [dbo].[SearchHistory] ADD  DEFAULT ((0)) FOR [ResultCount]
GO
ALTER TABLE [dbo].[UserAvatarFrames] ADD  DEFAULT ((0)) FOR [IsEquipped]
GO
ALTER TABLE [dbo].[UserAvatarFrames] ADD  DEFAULT (getdate()) FOR [ObtainedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [RoleId]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [RememberMe]
GO
ALTER TABLE [dbo].[CrackInfo]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[DownloadHistory]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[DownloadHistory]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[FavoriteGames]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[FavoriteGames]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Features]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[GameLinks]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[Games]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[GameTags]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[GameTags]  WITH CHECK ADD FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
GO
ALTER TABLE [dbo].[LocalizationInfo]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[RelatedGames]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[RelatedGames]  WITH CHECK ADD FOREIGN KEY([RelatedGameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Screenshots]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[SearchHistory]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SystemRequirements]  WITH CHECK ADD FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[UserAvatarFrames]  WITH CHECK ADD FOREIGN KEY([FrameId])
REFERENCES [dbo].[AvatarFrames] ([Id])
GO
ALTER TABLE [dbo].[UserAvatarFrames]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
USE [master]
GO
ALTER DATABASE [WebDownloadGame] SET  READ_WRITE 
GO
