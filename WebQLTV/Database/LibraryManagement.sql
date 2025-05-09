USE [master]
GO
/****** Object:  Database [LibraryManagement]    Script Date: 4/8/2025 8:59:49 PM ******/
CREATE DATABASE [LibraryManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryManagement', FILENAME = N'D:\SQL\MSSQL16.SQLEXPRESS01\MSSQL\DATA\LibraryManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryManagement_log', FILENAME = N'D:\SQL\MSSQL16.SQLEXPRESS01\MSSQL\DATA\LibraryManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LibraryManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LibraryManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LibraryManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LibraryManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LibraryManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LibraryManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LibraryManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LibraryManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LibraryManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LibraryManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LibraryManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LibraryManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LibraryManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LibraryManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LibraryManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LibraryManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LibraryManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LibraryManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LibraryManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LibraryManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LibraryManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LibraryManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LibraryManagement] SET  MULTI_USER 
GO
ALTER DATABASE [LibraryManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LibraryManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LibraryManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LibraryManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LibraryManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LibraryManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [LibraryManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [LibraryManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LibraryManagement]
GO
/****** Object:  Table [dbo].[author]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[author](
	[AuthorID] [int] IDENTITY(1,1) NOT NULL,
	[AuthorName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AuthorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[book]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[book](
	[BookID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[TypeID] [int] NULL,
	[AuthorID] [int] NULL,
	[PublisherID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[description] [nvarchar](max) NULL,
	[total_borrow] [int] NULL,
	[imgpath] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[bookborrow]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bookborrow](
	[BorrowID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[BookID] [int] NULL,
	[BorrowDate] [date] NOT NULL,
	[ReturnDate] [date] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BorrowID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[publisher]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[publisher](
	[PublisherID] [int] IDENTITY(1,1) NOT NULL,
	[PublisherName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PublisherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[type]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[type](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 4/8/2025 8:59:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[RoleID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[author] ON 

INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (2, N'Albert Einstein')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (8, N'Fyodor Dostoevsky')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (7, N'George Orwell')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (1, N'Isaac Newton')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (9, N'J.K. Rowling')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (10, N'J.R.R. Tolkien')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (6, N'Leo Tolstoy')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (5, N'Mark Twain')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (3, N'Stephen Hawking')
INSERT [dbo].[author] ([AuthorID], [AuthorName]) VALUES (4, N'William Shakespeare')
SET IDENTITY_INSERT [dbo].[author] OFF
GO
SET IDENTITY_INSERT [dbo].[book] ON 

INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (2, N'The Theory of Relativity', 1, 2, 2, 4, N'A revolutionary theory of space and time by Albert Einstein', 0, N'lib/img/imgbook/book2.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (3, N'A Brief History of Time', 1, 3, 3, 3, N'Stephen Hawking’s exploration of cosmology and the universe', 0, N'lib/img/imgbook/book3.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (4, N'Hamlet', 2, 4, 4, 7, N'A classic tragedy play by William Shakespeare', 0, N'lib/img/imgbook/book4.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (5, N'Adventures of Huckleberry Finn', 2, 5, 5, 6, N'A novel about the adventures of a boy on the Mississippi River, written by Mark Twain', 1, N'lib/img/imgbook/book5.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (6, N'War and Peace', 2, 6, 6, 4, N'A historical epic set in Russia during the Napoleonic Wars, by Leo Tolstoy', 0, N'lib/img/imgbook/book6.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (7, N'The Innovators', 3, 3, 7, 5, N'A history of technology and innovation by Walter Isaacson', 0, N'lib/img/imgbook/book7.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (8, N'Clean Code', 3, 8, 8, 8, N'A handbook for writing clean, maintainable code, by Robert C. Martin', 0, N'lib/img/imgbook/book8.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (9, N'The Pragmatic Programmer', 3, 9, 9, 7, N'A guide for software developers on best practices and practical solutions', 0, N'lib/img/imgbook/book9.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (10, N'Sapiens: A Brief History of Humankind', 4, 7, 1, 5, N'A history of humankind from prehistoric times to the modern era, by Yuval Noah Harari', 0, N'lib/img/imgbook/book10.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (11, N'The Silk Roads', 4, 8, 2, 3, N'A historical exploration of the Silk Roads and their impact on global trade and culture', 0, N'lib/img/imgbook/book11.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (12, N'Guns, Germs, and Steel', 4, 9, 3, 6, N'A study of the role of geography, technology, and biology in shaping history', 0, N'lib/img/imgbook/book12.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (13, N'Mathematics for the Nonmathematician', 5, 1, 4, 6, N'A non-technical introduction to mathematics and its applications', 0, N'lib/img/imgbook/book13.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (14, N'Introduction to Probability', 5, 2, 5, 4, N'An accessible guide to probability and its real-world applications', 0, N'lib/img/imgbook/book14.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (15, N'Calculus Made Easy', 5, 3, 6, 7, N'A simplified introduction to calculus for beginners', 0, N'lib/img/imgbook/book15.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (16, N'1984', 6, 7, 7, 5, N'A dystopian novel about totalitarianism, by George Orwell', 2, N'lib/img/imgbook/book16.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (17, N'Animal Farm', 6, 7, 8, 3, N'A political allegory using farm animals to explore the dangers of dictatorship, by George Orwell', 0, N'lib/img/imgbook/book17.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (18, N'The Hobbit', 6, 10, 9, 4, N'A fantasy novel set in Middle-earth, following the adventures of Bilbo Baggins, by J.R.R. Tolkien', 0, N'lib/img/imgbook/book18.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (19, N'The Republic', 7, 8, 10, 4, N'A philosophical dialogue on justice, society, and the ideal state, by Plato', 0, N'lib/img/imgbook/book19.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (20, N'Beyond Good and Evil', 7, 9, 1, 6, N'A philosophical work exploring morality, truth, and the human condition, by Friedrich Nietzsche', 0, N'lib/img/imgbook/book20.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (21, N'Meditations', 7, 10, 2, 5, N'A collection of personal reflections by the Roman Emperor Marcus Aurelius', 0, N'lib/img/imgbook/book21.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (22, N'The Wealth of Nations', 8, 1, 3, 4, N'A foundational work on free-market economics by Adam Smith', 0, N'lib/img/imgbook/book22.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (23, N'Capital in the Twenty-First Century', 8, 2, 4, 3, N'An exploration of wealth inequality in the 21st century, by Thomas Piketty', 0, N'lib/img/imgbook/book23.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (24, N'The General Theory of Employment, Interest, and Money', 8, 3, 5, 5, N'A work on macroeconomics and economic theory by John Maynard Keynes', 3, N'lib/img/imgbook/book24.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (25, N'Thinking, Fast and Slow', 9, 4, 6, 6, N'An exploration of cognitive biases and human decision-making by Daniel Kahneman', 0, N'lib/img/imgbook/book25.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (26, N'Man’s Search for Meaning', 9, 5, 7, 7, N'A memoir about finding meaning and purpose in life, by Viktor Frankl', 1, N'lib/img/imgbook/book26.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (27, N'The Interpretation of Dreams', 9, 6, 8, 4, N'A groundbreaking work on psychoanalysis and dreams, by Sigmund Freud', 0, N'lib/img/imgbook/book27.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (28, N'The Story of Art', 10, 7, 9, 4, N'A comprehensive guide to the history of art from prehistoric to modern times', 1, N'lib/img/imgbook/book28.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (29, N'Ways of Seeing', 10, 8, 10, 3, N'A thought-provoking exploration of how we see and interpret art', 0, N'lib/img/imgbook/book29.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (30, N'The Art Book', 10, 9, 1, 6, N'A visually stunning collection of artworks from different periods and cultures', 0, N'lib/img/imgbook/book30.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (31, N'A Phylosophy to Mathematics', 1, 2, 2, 6, N'The tips of a phylogenetic tree can be living taxa or fossils, which represent the present time or "end" of an evolutionary lineage, respectively. A phylogenetic diagram can be rooted or unrooted. A rooted tree diagram indicates the hypothetical common ancestor of the tree. An unrooted tree diagram (a network) makes no assumption about the ancestral line, and does not show the origin or "root" of the taxa in question or the direction of inferred evolutionary transformations.', 0, N'lib/img/imgbook/book1.jpg')
INSERT [dbo].[book] ([BookID], [Title], [TypeID], [AuthorID], [PublisherID], [Quantity], [description], [total_borrow], [imgpath]) VALUES (32, N'One Thousand and One Nights', 6, 4, 3, 20, N'Common to all the editions of the Nights is the framing device of the story of the ruler Shahryar being narrated the tales by his wife Scheherazade, with one tale told over each night of storytelling. The stories proceed from this original tale; some are framed within other tales, while some are self-contained. Some editions contain only a few hundred nights of storytelling, while others include 1001 or more. The bulk of the text is in prose, although verse is occasionally used for songs and riddles and to express heightened emotion. Most of the poems are single couplets or quatrains, although some are longer.', 0, N'lib/img/imgbook/book31.jpg')
SET IDENTITY_INSERT [dbo].[book] OFF
GO
SET IDENTITY_INSERT [dbo].[bookborrow] ON 

INSERT [dbo].[bookborrow] ([BorrowID], [UserID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (2, 1, 24, CAST(N'2025-03-27' AS Date), CAST(N'2025-04-06' AS Date), N'Returned')
INSERT [dbo].[bookborrow] ([BorrowID], [UserID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (3, 1, 24, CAST(N'2025-03-27' AS Date), CAST(N'2025-04-06' AS Date), N'Returned')
INSERT [dbo].[bookborrow] ([BorrowID], [UserID], [BookID], [BorrowDate], [ReturnDate], [Status]) VALUES (25, 1, 24, CAST(N'2025-04-08' AS Date), CAST(N'2025-04-18' AS Date), N'Approved')
SET IDENTITY_INSERT [dbo].[bookborrow] OFF
GO
SET IDENTITY_INSERT [dbo].[publisher] ON 

INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (2, N'Cambridge University Press')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (8, N'Hachette Livre')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (3, N'HarperCollins')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (7, N'Macmillan Publishers')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (1, N'Oxford University Press')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (4, N'Penguin Books')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (5, N'Random House')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (6, N'Simon & Schuster')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (9, N'Springer')
INSERT [dbo].[publisher] ([PublisherID], [PublisherName]) VALUES (10, N'Wiley')
SET IDENTITY_INSERT [dbo].[publisher] OFF
GO
SET IDENTITY_INSERT [dbo].[role] ON 

INSERT [dbo].[role] ([RoleID], [RoleName]) VALUES (2, N'admin')
INSERT [dbo].[role] ([RoleID], [RoleName]) VALUES (1, N'customer')
SET IDENTITY_INSERT [dbo].[role] OFF
GO
SET IDENTITY_INSERT [dbo].[type] ON 

INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (10, N'Art')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (8, N'Economics')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (6, N'Fiction')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (4, N'History')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (2, N'Literature')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (5, N'Mathematics')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (7, N'Philosophy')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (9, N'Psychology')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (1, N'Science')
INSERT [dbo].[type] ([TypeID], [TypeName]) VALUES (3, N'Technology')
SET IDENTITY_INSERT [dbo].[type] OFF
GO
SET IDENTITY_INSERT [dbo].[user] ON 

INSERT [dbo].[user] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID]) VALUES (1, N'khakham', N'12312312', N'Peter bé nhỏ ', N'pvpgammingpro@gmail.com', 1)
INSERT [dbo].[user] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID]) VALUES (4, N'admin', N'123456', N'Administrator', N'admin@email.com', 2)
SET IDENTITY_INSERT [dbo].[user] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__author__4A1A120BB0A2C614]    Script Date: 4/8/2025 8:59:50 PM ******/
ALTER TABLE [dbo].[author] ADD UNIQUE NONCLUSTERED 
(
	[AuthorName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__publishe__5F0E2249BD40241B]    Script Date: 4/8/2025 8:59:50 PM ******/
ALTER TABLE [dbo].[publisher] ADD UNIQUE NONCLUSTERED 
(
	[PublisherName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__role__8A2B6160A4D6264F]    Script Date: 4/8/2025 8:59:50 PM ******/
ALTER TABLE [dbo].[role] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__type__D4E7DFA8B46C2C22]    Script Date: 4/8/2025 8:59:50 PM ******/
ALTER TABLE [dbo].[type] ADD UNIQUE NONCLUSTERED 
(
	[TypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__user__536C85E458567B3C]    Script Date: 4/8/2025 8:59:50 PM ******/
ALTER TABLE [dbo].[user] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[book] ADD  DEFAULT ((0)) FOR [total_borrow]
GO
ALTER TABLE [dbo].[book]  WITH CHECK ADD FOREIGN KEY([AuthorID])
REFERENCES [dbo].[author] ([AuthorID])
GO
ALTER TABLE [dbo].[book]  WITH CHECK ADD FOREIGN KEY([PublisherID])
REFERENCES [dbo].[publisher] ([PublisherID])
GO
ALTER TABLE [dbo].[book]  WITH CHECK ADD FOREIGN KEY([TypeID])
REFERENCES [dbo].[type] ([TypeID])
GO
ALTER TABLE [dbo].[bookborrow]  WITH CHECK ADD FOREIGN KEY([BookID])
REFERENCES [dbo].[book] ([BookID])
GO
ALTER TABLE [dbo].[bookborrow]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[user] ([UserID])
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[role] ([RoleID])
GO
ALTER TABLE [dbo].[book]  WITH CHECK ADD CHECK  (([Quantity]>=(0)))
GO
ALTER TABLE [dbo].[bookborrow]  WITH CHECK ADD CHECK  (([Status]='Overdue' OR [Status]='Returned' OR [Status]='Approved' OR [Status]='Pending'))
GO
USE [master]
GO
ALTER DATABASE [LibraryManagement] SET  READ_WRITE 
GO
