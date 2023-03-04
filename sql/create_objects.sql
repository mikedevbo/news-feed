USE [NewsFeed]
GO

/*
drop table dbo.Accounts
drop table dbo.TwitterGroups
drop table dbo.TwitterUsers
drop table dbo.TwitterUsersApi
drop table dbo.TwitterTweets
drop table dbo.TwitterTweetsApi

drop table [nsb].[error]
drop table [nsb].[NewsFeed.Server]
drop table [nsb].[NewsFeed.Server.Delayed]
drop table [nsb].[NewsFeed_Server_OutboxData]
drop table [nsb].[SubscriptionRouting]

drop schema nsb_t
drop schema nsb_p
*/

CREATE SCHEMA nsb_t
GO

CREATE SCHEMA nsb_p
GO

CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--

CREATE TABLE [dbo].[TwitterGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[AccountId] int NOT NULL,
 CONSTRAINT [PK_TwitterGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--

CREATE TABLE [dbo].[TwitterUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[GroupId] [int] NOT NULL,
	[IsTweetsDownloading] [bit] NOT NULL
 CONSTRAINT [PK_TwitterUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--

CREATE TABLE [dbo].[TwitterUsersApi](
	[Id] [int] NOT NULL,
	[UserId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TwitterUsersApi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--

CREATE TABLE [dbo].[TwitterTweets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[IsFavorite] [bit] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[TweetIdApi] [varchar](50) NOT NULL,
	[TweetTextApi] [varchar](max) NOT NULL,
	[TweetCreatedAtApi] [datetime] NOT NULL
 CONSTRAINT [PK_TwitterTweets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO