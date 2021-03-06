USE [master]
GO
/****** Object:  Database [Practica_2]    Script Date: 19/12/2021 17:30:52 ******/
CREATE DATABASE [Practica_2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Practica_2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Practica_2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Practica_2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Practica_2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Practica_2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Practica_2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Practica_2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Practica_2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Practica_2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Practica_2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Practica_2] SET ARITHABORT OFF 
GO
ALTER DATABASE [Practica_2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Practica_2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Practica_2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Practica_2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Practica_2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Practica_2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Practica_2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Practica_2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Practica_2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Practica_2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Practica_2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Practica_2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Practica_2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Practica_2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Practica_2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Practica_2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Practica_2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Practica_2] SET RECOVERY FULL 
GO
ALTER DATABASE [Practica_2] SET  MULTI_USER 
GO
ALTER DATABASE [Practica_2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Practica_2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Practica_2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Practica_2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Practica_2] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Practica_2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Practica_2', N'ON'
GO
ALTER DATABASE [Practica_2] SET QUERY_STORE = OFF
GO
USE [Practica_2]
GO
USE [Practica_2]
GO
/****** Object:  Sequence [dbo].[numero_pedido]    Script Date: 19/12/2021 17:30:53 ******/
CREATE SEQUENCE [dbo].[numero_pedido] 
 AS [int]
 START WITH 1
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 2147483647
 NO CACHE 
GO
/****** Object:  UserDefinedFunction [dbo].[FUNC_SelectIdAutor]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FUNC_SelectIdAutor] (@autor varchar(50)) 
RETURNS INT
AS
BEGIN
DECLARE @idAutor int;
SELECT @idAutor = Id FROM Autores WHERE Autor = @autor;
RETURN @idAutor;
END;
GO
/****** Object:  Table [dbo].[Libro]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Libro](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](50) NULL,
	[IdAutor] [int] NULL,
	[IdEditorial] [int] NULL,
	[ISBN] [varchar](20) NULL,
	[Edicion] [varchar](50) NULL,
	[Anio] [int] NULL,
	[Paginas] [int] NULL,
	[IdCategoria] [int] NULL,
	[Precio] [money] NULL,
	[Stock] [int] NULL,
 CONSTRAINT [PK_Libro] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetallePedidos]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetallePedidos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPedido] [bigint] NULL,
	[Detalle] [varchar](100) NULL,
	[Cantidad] [int] NULL,
 CONSTRAINT [PK_DetallePedidos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Editoriales]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Editoriales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Editorial] [varchar](50) NULL,
	[IdProveedor] [int] NULL,
 CONSTRAINT [PK_Editoriales] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdProveedor] [int] NULL,
	[NumPedido] [int] NULL,
	[Fecha] [datetime] NULL,
	[Baja] [bit] NOT NULL,
	[FechaBaja] [datetime] NULL,
 CONSTRAINT [PK_Pedidos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedores]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Razon_Social] [varchar](50) NULL,
	[Direccion] [varchar](50) NULL,
	[Codigo_Postal] [varchar](50) NULL,
	[Telefono] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
 CONSTRAINT [PK_Proveedores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[PedidosCompleto]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE   VIEW [dbo].[PedidosCompleto] (PedidoNum, Proveedor, Fecha, ISBN, Cantidad, Titulo, Editorial) AS
SELECT pe.NumPedido, pro.Nombre, pe.Fecha, det.Detalle, det.Cantidad, Li.Titulo, Ed.Editorial FROM Pedidos pe
INNER JOIN Proveedores pro
ON pe.IdProveedor = pro.Id
INNER JOIN DetallePedidos det
ON pe.Id = det.IdPedido
INNER JOIN Libro Li
ON det.Detalle = Li.ISBN
INNER JOIN Editoriales Ed
ON Li.IdEditorial = Ed.Id 
GO
/****** Object:  Table [dbo].[Agencia]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agencia](
	[Id_Agencia] [int] IDENTITY(1,1) NOT NULL,
	[Agencia] [varchar](50) NULL,
 CONSTRAINT [PK_Agencia] PRIMARY KEY CLUSTERED 
(
	[Id_Agencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Autores]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Autores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Autor] [varchar](50) NULL,
 CONSTRAINT [PK_Autores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Categoria] [varchar](50) NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ciudad]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ciudad](
	[Id_Ciudad] [int] IDENTITY(1,1) NOT NULL,
	[Ciudad] [varchar](50) NULL,
 CONSTRAINT [PK_Ciudad] PRIMARY KEY CLUSTERED 
(
	[Id_Ciudad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[Id_Cliente] [int] IDENTITY(1,1) NOT NULL,
	[Cliente] [varchar](50) NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Id_Cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientePaquete]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientePaquete](
	[Id_Cliente] [int] NULL,
	[Id_Paquete] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dias]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dias](
	[Id_Dias] [int] IDENTITY(1,1) NOT NULL,
	[TipoDias] [varchar](50) NULL,
 CONSTRAINT [PK_Dias] PRIMARY KEY CLUSTERED 
(
	[Id_Dias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hotel]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hotel](
	[Id_Hotel] [int] IDENTITY(1,1) NOT NULL,
	[Hotel] [varchar](50) NULL,
 CONSTRAINT [PK_Hotel] PRIMARY KEY CLUSTERED 
(
	[Id_Hotel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paquete]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paquete](
	[Id_Paquete] [int] IDENTITY(1,1) NOT NULL,
	[Id_Ciudad] [int] NULL,
	[Id_Dias] [int] NULL,
	[Id_Hotel] [int] NULL,
	[Id_Agencia] [int] NULL,
	[PrecioDol] [int] NULL,
	[Id_Promo] [int] NULL,
 CONSTRAINT [PK_Paquete] PRIMARY KEY CLUSTERED 
(
	[Id_Paquete] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promocion]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promocion](
	[Id_promo] [int] IDENTITY(1,1) NOT NULL,
	[Promo] [varchar](50) NULL,
	[Descuento] [int] NULL,
 CONSTRAINT [PK_Promocion] PRIMARY KEY CLUSTERED 
(
	[Id_promo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Agencia] ON 

INSERT [dbo].[Agencia] ([Id_Agencia], [Agencia]) VALUES (1, N'Turimir')
INSERT [dbo].[Agencia] ([Id_Agencia], [Agencia]) VALUES (2, N'Pezatti')
INSERT [dbo].[Agencia] ([Id_Agencia], [Agencia]) VALUES (3, N'Argentur')
INSERT [dbo].[Agencia] ([Id_Agencia], [Agencia]) VALUES (4, N'AvianPlace')
INSERT [dbo].[Agencia] ([Id_Agencia], [Agencia]) VALUES (5, N'LapachoTrip')
SET IDENTITY_INSERT [dbo].[Agencia] OFF
GO
SET IDENTITY_INSERT [dbo].[Autores] ON 

INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (1, N'Stephen King')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (2, N'Rick Riordan')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (3, N'Andrzej Sapkowski')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (4, N'PEPE')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (5, N'Juan Carlos')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (6, N'Juan perez')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (7, N'oaki')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (8, N'asdasdasdasd')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (9, N'asdasdasdasdaa')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (10, N'asdasd')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (11, N'juan carlos batman')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (12, N'Jeuan perez')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (13, N'14134')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (14, N'carlos')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (15, N'Drew Karpyshyn')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (16, N'Margarita Maine')
INSERT [dbo].[Autores] ([Id], [Autor]) VALUES (17, N'preuba')
SET IDENTITY_INSERT [dbo].[Autores] OFF
GO
SET IDENTITY_INSERT [dbo].[Categorias] ON 

INSERT [dbo].[Categorias] ([Id], [Categoria]) VALUES (1, N'Fantasía')
INSERT [dbo].[Categorias] ([Id], [Categoria]) VALUES (2, N'Terror')
INSERT [dbo].[Categorias] ([Id], [Categoria]) VALUES (3, N'Ciencia Ficción')
INSERT [dbo].[Categorias] ([Id], [Categoria]) VALUES (4, N'Novela')
INSERT [dbo].[Categorias] ([Id], [Categoria]) VALUES (5, N'Ficción')
SET IDENTITY_INSERT [dbo].[Categorias] OFF
GO
SET IDENTITY_INSERT [dbo].[Ciudad] ON 

INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (1, N'Lisboa')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (2, N'Berlin')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (3, N'Paris')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (4, N'Madrid')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (5, N'Nueva York')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (6, N'Bariloche')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (7, N'San Martin de los Andes')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (8, N'Iguazu')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (9, N'Salta')
INSERT [dbo].[Ciudad] ([Id_Ciudad], [Ciudad]) VALUES (10, N'Mar del Plata')
SET IDENTITY_INSERT [dbo].[Ciudad] OFF
GO
SET IDENTITY_INSERT [dbo].[Cliente] ON 

INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (1, N'Pedro Wanch')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (2, N'Juan Fernandez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (3, N'Francisco Alvarez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (4, N'Juana Gonzalez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (5, N'David Oncangia')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (6, N'Juan Manso')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (7, N'Roxana Lopez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (8, N'Pedro Sanchez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (9, N'Rodrigo Juarez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (10, N'Paula Rodriguez')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (11, N'Cristian Sancho')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (12, N'Florencia Rossi')
INSERT [dbo].[Cliente] ([Id_Cliente], [Cliente]) VALUES (13, N'Graciela Hermida')
SET IDENTITY_INSERT [dbo].[Cliente] OFF
GO
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (1, 1)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (2, 2)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (3, 3)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (3, 4)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (4, 5)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (5, 6)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (6, 7)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (6, 8)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (7, 9)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (8, 9)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (9, 10)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (10, 11)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (11, 12)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (12, 12)
INSERT [dbo].[ClientePaquete] ([Id_Cliente], [Id_Paquete]) VALUES (13, 13)
GO
SET IDENTITY_INSERT [dbo].[DetallePedidos] ON 

INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (1, 1, N'9789877250244', 10)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (2, 1, N'9789500731652', 7)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (3, 1, N'9788401336478', 5)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (4, 1, N'9789506442637', 10)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (5, 1, N'9789875668287', 4)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (6, 1, N'9789506440718', 3)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (7, 1, N'9788401336522', 5)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (8, 1, N'9789506443214', 8)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (9, 1, N'9789875666276', 4)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (10, 1, N'9789877251760', 6)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (11, 1, N'9789877251272', 8)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (20, 65, N'9789877250244', 7)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (21, 65, N'9789500731652', 17)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (22, 65, N'9788401336478', 4)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (23, 65, N'9789506442637', 5)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (24, 65, N'9788498386264', 6)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (25, 65, N'9789878000206', 7)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (26, 65, N'9786073192408', 4)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (27, 65, N'9788498386295', 44)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (28, 65, N'9788498386301', 3)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (29, 65, N'9789875668287', 8)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (30, 65, N'9789506440718', 5)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (31, 65, N'9788401336522', 9)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (34, 65, N'9789877251760 ', 8)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (36, 66, N'9786073192408', 12)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (42, 1, N'9788498386264', 7)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (46, 66, N'9786073192408', 4)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (47, 66, N'9788498386295', 7)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (52, 68, N'9788498386264', 7)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (60, 68, N'9789877250244', 5)
INSERT [dbo].[DetallePedidos] ([Id], [IdPedido], [Detalle], [Cantidad]) VALUES (61, 68, N'9789506443214', 3)
SET IDENTITY_INSERT [dbo].[DetallePedidos] OFF
GO
SET IDENTITY_INSERT [dbo].[Dias] ON 

INSERT [dbo].[Dias] ([Id_Dias], [TipoDias]) VALUES (1, N'corto')
INSERT [dbo].[Dias] ([Id_Dias], [TipoDias]) VALUES (2, N'semanal')
INSERT [dbo].[Dias] ([Id_Dias], [TipoDias]) VALUES (3, N'quincenal')
SET IDENTITY_INSERT [dbo].[Dias] OFF
GO
SET IDENTITY_INSERT [dbo].[Editoriales] ON 

INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (1, N'Debolsillo', 1)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (2, N'Plaza y Janes', 1)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (3, N'Salamandra', 1)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (4, N'Artifex', 3)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (5, N'Alamut', 4)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (6, N'Del Rey', 1)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (7, N'Planeta Comics', 2)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (33, N'Editorial Hola Chicos', 1019)
INSERT [dbo].[Editoriales] ([Id], [Editorial], [IdProveedor]) VALUES (34, N'Planeta Libros', 2)
SET IDENTITY_INSERT [dbo].[Editoriales] OFF
GO
SET IDENTITY_INSERT [dbo].[Hotel] ON 

INSERT [dbo].[Hotel] ([Id_Hotel], [Hotel]) VALUES (1, N'1 Estrellas')
INSERT [dbo].[Hotel] ([Id_Hotel], [Hotel]) VALUES (2, N'2 Estrellas')
INSERT [dbo].[Hotel] ([Id_Hotel], [Hotel]) VALUES (3, N'3 Estrellas')
INSERT [dbo].[Hotel] ([Id_Hotel], [Hotel]) VALUES (4, N'4 Estrellas')
INSERT [dbo].[Hotel] ([Id_Hotel], [Hotel]) VALUES (5, N'5 Estrellas')
INSERT [dbo].[Hotel] ([Id_Hotel], [Hotel]) VALUES (6, N'Hostel')
SET IDENTITY_INSERT [dbo].[Hotel] OFF
GO
SET IDENTITY_INSERT [dbo].[Libro] ON 

INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1, N'IT', 1, 1, N'9789877250244', N'2', 2015, 1504, 2, 3749.0000, 10)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (2, N'Cementerio de animales', 1, 1, N'9789500731652', N'1', 2010, 488, 2, 3149.0000, 5)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (3, N'La Llegada de Los Tres La Torre Oscura', 1, 2, N'9788401336478', N'14', 2007, 544, 3, 4500.0000, 1)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (4, N'El Viento Por la Cerradura', 1, 2, N'9789506442637', N'1', 2012, 368, 3, 7161.0000, 3)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (5, N'Percy Jackson El Ladron del Rayo', 2, 3, N'9788498386264', N'1', 2014, 288, 1, 3517.0000, 4)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (6, N'Percy Jackson El Mar de Los Monstruos', 2, 3, N'9789878000206', N'1', 2020, 256, 1, 1999.0000, 8)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (7, N'Percy Jackson La Maldicion del Titan', 2, 3, N'9786073192408', N'1', 2020, 288, 1, 3989.0000, 15)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (8, N'Percy Jackson La Batalla del Laberinto', 2, 3, N'9788498386295', N'1', 2015, 320, 1, 3391.0000, 7)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (9, N'Percy Jackson El Ultimo Heroe del Olimpo', 2, 3, N'9788498386301', N'1', 2015, 352, 1, 2819.0000, 20)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (10, N'El Ultimo Deseo', 3, 4, N'9788498891041', N'1', 2015, 264, 5, 3703.0000, 4)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (11, N'La Espada del Destino', 3, 4, N'9788498891058', N'1', 2018, 288, 5, 1895.0000, 6)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (12, N'Estacion de Tormentas', 3, 5, N'9788498891027', N'1', 2015, 312, 5, 6308.0000, 3)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (13, N'La Sangre de Los Elfos', 3, 4, N'978498891065', N'1', 2018, 264, 5, 3300.0000, 5)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (14, N'Tiempo de Odio', 3, 4, N'9788498891072', N'1', 2016, 288, 5, 2095.0000, 9)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (15, N'Bautismo de Fuego', 3, 5, N'9788498890549', N'1', 2010, 256, 5, 6191.0000, 3)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (16, N'La Torre de La Golondrina', 3, 5, N'9788498890570', N'1', 2012, 336, 5, 6423.0000, 4)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (17, N'La Dama del Lago', 3, 4, N'9788498891102', N'1', 2016, 480, 5, 2650.0000, 11)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1045, N'La Torre Oscura Mago y Cristal', 1, 1, N'9789875668287', N'1', 2012, 918, 5, 3104.0000, 5)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1046, N'La Torre Oscura Canción de Susannah', 1, 2, N'9789506440718', N'1', 2005, 496, 5, 2250.0000, 3)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1047, N'La Torre Oscura Las Tierras Baldías', 1, 2, N'9788401336522', N'1', 2008, 640, 5, 4000.0000, 2)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1048, N'Mr Mercedes', 1, 2, N'9789506443214', N'1', 2014, 496, 5, 2834.0000, 3)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1049, N'La Torre Oscura El Pistolero', 1, 1, N'9789875666276', N'1', 2011, 304, 5, 1259.0000, 2)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1050, N'La Torre Oscura Lobos del Calla', 1, 1, N'9789877251760 ', N'1', 2018, 794, 5, 2834.0000, 4)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1051, N'La Torre Oscura La Torre Oscura VII', 1, 1, N'9789877251272', N'1', 2016, 992, 5, 3104.0000, 4)
INSERT [dbo].[Libro] ([Id], [Titulo], [IdAutor], [IdEditorial], [ISBN], [Edicion], [Anio], [Paginas], [IdCategoria], [Precio], [Stock]) VALUES (1067, N'Star Wars: The Old Republic Revan', 15, 7, N'9788491739074', N'1', 2021, 352, 3, 6652.0000, 5)
SET IDENTITY_INSERT [dbo].[Libro] OFF
GO
SET IDENTITY_INSERT [dbo].[Paquete] ON 

INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (1, 1, 2, 3, 1, 1500, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (2, 2, 1, 2, 1, 650, 1)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (3, 2, 2, 3, 2, 1000, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (4, 3, 1, 3, 2, 1200, 2)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (5, 4, 1, 2, 1, 1000, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (6, 5, 2, 2, 2, 1000, 3)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (7, 6, 1, 4, 3, 500, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (8, 7, 1, 3, 3, 600, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (9, 8, 1, 5, 4, 350, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (10, 8, 1, 1, 5, 150, NULL)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (11, 9, 1, 3, 4, 400, 4)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (12, 9, 2, 6, 4, 150, 5)
INSERT [dbo].[Paquete] ([Id_Paquete], [Id_Ciudad], [Id_Dias], [Id_Hotel], [Id_Agencia], [PrecioDol], [Id_Promo]) VALUES (13, 10, 3, 2, 5, 300, 4)
SET IDENTITY_INSERT [dbo].[Paquete] OFF
GO
SET IDENTITY_INSERT [dbo].[Pedidos] ON 

INSERT [dbo].[Pedidos] ([Id], [IdProveedor], [NumPedido], [Fecha], [Baja], [FechaBaja]) VALUES (1, 1, 109, CAST(N'2021-11-11T20:27:13.900' AS DateTime), 0, NULL)
INSERT [dbo].[Pedidos] ([Id], [IdProveedor], [NumPedido], [Fecha], [Baja], [FechaBaja]) VALUES (65, 1, 177, CAST(N'2021-11-12T00:18:34.253' AS DateTime), 0, NULL)
INSERT [dbo].[Pedidos] ([Id], [IdProveedor], [NumPedido], [Fecha], [Baja], [FechaBaja]) VALUES (66, 1, 178, CAST(N'2021-11-12T00:30:35.663' AS DateTime), 0, NULL)
INSERT [dbo].[Pedidos] ([Id], [IdProveedor], [NumPedido], [Fecha], [Baja], [FechaBaja]) VALUES (68, 1, 180, CAST(N'2021-11-17T16:51:15.840' AS DateTime), 0, NULL)
SET IDENTITY_INSERT [dbo].[Pedidos] OFF
GO
SET IDENTITY_INSERT [dbo].[Promocion] ON 

INSERT [dbo].[Promocion] ([Id_promo], [Promo], [Descuento]) VALUES (1, N'Europa Classic', 10)
INSERT [dbo].[Promocion] ([Id_promo], [Promo], [Descuento]) VALUES (2, N'Europa Gold', 20)
INSERT [dbo].[Promocion] ([Id_promo], [Promo], [Descuento]) VALUES (3, N'American Good', 20)
INSERT [dbo].[Promocion] ([Id_promo], [Promo], [Descuento]) VALUES (4, N'Mi Pais Verano', 30)
INSERT [dbo].[Promocion] ([Id_promo], [Promo], [Descuento]) VALUES (5, N'Joven Sub 25', 25)
SET IDENTITY_INSERT [dbo].[Promocion] OFF
GO
SET IDENTITY_INSERT [dbo].[Proveedores] ON 

INSERT [dbo].[Proveedores] ([Id], [Nombre], [Razon_Social], [Direccion], [Codigo_Postal], [Telefono], [Email]) VALUES (1, N'Penguin Random House Grupo Editorial', N'Penguin Random House Grupo Editorial', N'Humberto Primo 555', N'C1103ACK', N'541152354400', N'penguinar@penguin.com')
INSERT [dbo].[Proveedores] ([Id], [Nombre], [Razon_Social], [Direccion], [Codigo_Postal], [Telefono], [Email]) VALUES (2, N'Planeta de Libros', N'PlanetadeLibros Argentina', N'Av. Independencia 1682', N'C1100ABQ', N'541141249100', N'planeta@editorialplaneta.com')
INSERT [dbo].[Proveedores] ([Id], [Nombre], [Razon_Social], [Direccion], [Codigo_Postal], [Telefono], [Email]) VALUES (3, N'Editorial Oceano Argentina', N'Editorial Oceano Argentina', N'Carlos Pellegrini 855 12/13vo', N'1009', N'541152637133', N'oceano@oceano.com.ar')
INSERT [dbo].[Proveedores] ([Id], [Nombre], [Razon_Social], [Direccion], [Codigo_Postal], [Telefono], [Email]) VALUES (4, N'Alamut', N'Alamut', N'c/ Alcalá 387 - Madrid', N'28027', N'913771344', N'infoed@alamutediciones.com')
INSERT [dbo].[Proveedores] ([Id], [Nombre], [Razon_Social], [Direccion], [Codigo_Postal], [Telefono], [Email]) VALUES (1019, N'Editorial Hola Chicos', N'Editorial Hola Chicos', N'Callao 1121 Piso 4D - CABA', N'1023', N'4812-1800/4815-1998', N'web@editorialholachicos.com.ar')
SET IDENTITY_INSERT [dbo].[Proveedores] OFF
GO
ALTER TABLE [dbo].[Pedidos] ADD  CONSTRAINT [DF_Pedidos_Baja]  DEFAULT ((0)) FOR [Baja]
GO
ALTER TABLE [dbo].[ClientePaquete]  WITH CHECK ADD  CONSTRAINT [FK_ClientePaquete_Cliente] FOREIGN KEY([Id_Cliente])
REFERENCES [dbo].[Cliente] ([Id_Cliente])
GO
ALTER TABLE [dbo].[ClientePaquete] CHECK CONSTRAINT [FK_ClientePaquete_Cliente]
GO
ALTER TABLE [dbo].[ClientePaquete]  WITH CHECK ADD  CONSTRAINT [FK_ClientePaquete_Paquete] FOREIGN KEY([Id_Paquete])
REFERENCES [dbo].[Paquete] ([Id_Paquete])
GO
ALTER TABLE [dbo].[ClientePaquete] CHECK CONSTRAINT [FK_ClientePaquete_Paquete]
GO
ALTER TABLE [dbo].[DetallePedidos]  WITH CHECK ADD  CONSTRAINT [FK_DetallePedidos_Pedidos] FOREIGN KEY([IdPedido])
REFERENCES [dbo].[Pedidos] ([Id])
GO
ALTER TABLE [dbo].[DetallePedidos] CHECK CONSTRAINT [FK_DetallePedidos_Pedidos]
GO
ALTER TABLE [dbo].[Editoriales]  WITH CHECK ADD  CONSTRAINT [FK_Editoriales_Proveedores] FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedores] ([Id])
GO
ALTER TABLE [dbo].[Editoriales] CHECK CONSTRAINT [FK_Editoriales_Proveedores]
GO
ALTER TABLE [dbo].[Libro]  WITH CHECK ADD  CONSTRAINT [FK_Libro_Autores] FOREIGN KEY([IdAutor])
REFERENCES [dbo].[Autores] ([Id])
GO
ALTER TABLE [dbo].[Libro] CHECK CONSTRAINT [FK_Libro_Autores]
GO
ALTER TABLE [dbo].[Libro]  WITH CHECK ADD  CONSTRAINT [FK_Libro_Categorias] FOREIGN KEY([IdCategoria])
REFERENCES [dbo].[Categorias] ([Id])
GO
ALTER TABLE [dbo].[Libro] CHECK CONSTRAINT [FK_Libro_Categorias]
GO
ALTER TABLE [dbo].[Libro]  WITH CHECK ADD  CONSTRAINT [FK_Libro_Editoriales] FOREIGN KEY([IdEditorial])
REFERENCES [dbo].[Editoriales] ([Id])
GO
ALTER TABLE [dbo].[Libro] CHECK CONSTRAINT [FK_Libro_Editoriales]
GO
ALTER TABLE [dbo].[Paquete]  WITH CHECK ADD  CONSTRAINT [FK_Paquete_Agencia] FOREIGN KEY([Id_Agencia])
REFERENCES [dbo].[Agencia] ([Id_Agencia])
GO
ALTER TABLE [dbo].[Paquete] CHECK CONSTRAINT [FK_Paquete_Agencia]
GO
ALTER TABLE [dbo].[Paquete]  WITH CHECK ADD  CONSTRAINT [FK_Paquete_Ciudad] FOREIGN KEY([Id_Ciudad])
REFERENCES [dbo].[Ciudad] ([Id_Ciudad])
GO
ALTER TABLE [dbo].[Paquete] CHECK CONSTRAINT [FK_Paquete_Ciudad]
GO
ALTER TABLE [dbo].[Paquete]  WITH CHECK ADD  CONSTRAINT [FK_Paquete_Dias] FOREIGN KEY([Id_Dias])
REFERENCES [dbo].[Dias] ([Id_Dias])
GO
ALTER TABLE [dbo].[Paquete] CHECK CONSTRAINT [FK_Paquete_Dias]
GO
ALTER TABLE [dbo].[Paquete]  WITH CHECK ADD  CONSTRAINT [FK_Paquete_Hotel] FOREIGN KEY([Id_Hotel])
REFERENCES [dbo].[Hotel] ([Id_Hotel])
GO
ALTER TABLE [dbo].[Paquete] CHECK CONSTRAINT [FK_Paquete_Hotel]
GO
ALTER TABLE [dbo].[Paquete]  WITH CHECK ADD  CONSTRAINT [FK_Paquete_Promocion] FOREIGN KEY([Id_Promo])
REFERENCES [dbo].[Promocion] ([Id_promo])
GO
ALTER TABLE [dbo].[Paquete] CHECK CONSTRAINT [FK_Paquete_Promocion]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_Pedidos_Proveedores] FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedores] ([Id])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_Pedidos_Proveedores]
GO
/****** Object:  StoredProcedure [dbo].[SP_AbreVentana]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AbreVentana] AS
SELECT NumPedido, Fecha FROM Pedidos WHERE Baja=0;
GO
/****** Object:  StoredProcedure [dbo].[SP_BorrarPedido]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BorrarPedido] @numeroPedido int AS
BEGIN
DELETE FROM Pedidos WHERE NumPedido = @numeroPedido;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BorrarPedidoDetalle]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BorrarPedidoDetalle] @numeroPedido int AS
BEGIN
DELETE FROM DetallePedidos WHERE IdPedido = (SELECT Id FROM Pedidos WHERE NumPedido = @numeroPedido);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BorrarRegistroPedidoDetalle]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BorrarRegistroPedidoDetalle] @numeroPedido int, @isbn varchar(100) AS
BEGIN
DELETE FROM DetallePedidos WHERE IdPedido = (SELECT Id FROM Pedidos WHERE NumPedido = @numeroPedido) AND Detalle = @isbn;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BuscarLibroNuevoPedidoISBN]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_BuscarLibroNuevoPedidoISBN] @ISBN VARCHAR(20) AS
BEGIN
DECLARE @isbn1 VARCHAR(20) = '%' + @ISBN + '%'
SELECT L.Titulo, L.ISBN, L.Stock, P.Nombre as Proveedor FROM Libro L 
INNER JOIN Editoriales Ed
ON L.IdEditorial = Ed.Id
INNER JOIN Proveedores P
ON Ed.IdProveedor = P.Id
WHERE L.ISBN LIKE @isbn1;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BuscarLibroNuevoPedidoTitulo]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_BuscarLibroNuevoPedidoTitulo] @Titulo VARCHAR(50) AS
BEGIN
DECLARE @tit VARCHAR(50) = '%' + @Titulo + '%'
SELECT L.Titulo, L.ISBN, L.Stock, P.Nombre as Proveedor FROM Libro L 
INNER JOIN Editoriales Ed
ON L.IdEditorial = Ed.Id
INNER JOIN Proveedores P
ON Ed.IdProveedor = P.Id
WHERE L.Titulo LIKE @tit;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BuscarPedido]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_BuscarPedido] (@Numero int) AS
BEGIN
--DECLARE @pedido VARCHAR(150) = '%' + @Numero + '%'
SELECT NumPedido, Fecha FROM Pedidos WHERE NumPedido LIKE @Numero;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Editorial_Alta]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Editorial_Alta] @Editorial varchar(50), @idProveedor int AS
BEGIN
INSERT INTO Editoriales (Editorial, IdProveedor)
VALUES (@Editorial, @idProveedor)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Editorial_Borrar]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Editorial_Borrar] @idEditorial int AS
BEGIN
UPDATE Libro Set IdEditorial = NULL WHERE IdEditorial = @idEditorial;
DELETE FROM Editoriales WHERE Id = @idEditorial;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Editorial_Modificar]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Editorial_Modificar] @Editorial varchar (50), @id int AS
BEGIN
UPDATE Editoriales SET Editorial = @Editorial WHERE Id = @id;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GenerarPedido]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GenerarPedido] @Proveedor varchar(50), @idpedido bigint OUTPUT AS
BEGIN
--DECLARE @Prov VARCHAR(50) =  @Proveedor;
DECLARE @numeroPedido int;
SET @numeroPedido = NEXT VALUE FOR numero_pedido;
DECLARE @idproveedor int;
SELECT @idproveedor = Id FROM Proveedores WHERE Nombre = @Proveedor;
INSERT INTO Pedidos (IdProveedor, NumPedido, Fecha)
VALUES (@idproveedor, @numeroPedido, GETDATE());
SELECT @idpedido = id FROM Pedidos WHERE NumPedido = @numeroPedido;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GenerarPedidoDetalle]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GenerarPedidoDetalle] (@pedidoId bigint, @ISBN varchar(100), @cant int) AS
BEGIN
INSERT INTO DetallePedidos (IdPedido, Detalle, Cantidad)
VALUES (@pedidoId, @ISBN, @cant)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListaDetalles]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_ListaDetalles] (@idPedido int) AS
SELECT Titulo, Proveedor, ISBN, Cantidad FROM PedidosCompleto WHERE PedidoNum=@idPedido;
GO
/****** Object:  StoredProcedure [dbo].[SP_NuevoLibro]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NuevoLibro] @titulo VARCHAR(50), @autor VARCHAR(50) , @editorial VARCHAR(50), @isbn VARCHAR(20),
@edicion VARCHAR(50), @anio INT, @paginas INT, @categoria VARCHAR(50), @precio MONEY, @stock INT, @existe int OUTPUT AS
BEGIN
DECLARE @resultado int;
DECLARE @idAutor int;
SELECT @idAutor = Id FROM Autores WHERE Autor = @autor;
IF NOT EXISTS (SELECT * FROM Libro WHERE ISBN = @isbn)
	BEGIN
		IF (@idAutor IS NULL)
			BEGIN
			INSERT INTO Autores (Autor) VALUES (@autor);
			SELECT @idAutor = Id FROM Autores WHERE Autor = @autor;
			END
		BEGIN
		INSERT INTO Libro (Titulo, IdAutor, IdEditorial, ISBN, Edicion, Anio, Paginas, IdCategoria, Precio, Stock) 
		VALUES (@titulo, @idAutor, (SELECT Id FROM Editoriales WHERE Editorial=@editorial), @isbn, @edicion, @anio, @paginas,
		(SELECT Id FROM Categorias WHERE Categoria=@categoria), @precio, @stock);
		END
		SET @existe = 0;
	END
ELSE
	BEGIN
		SET @existe = 1;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_NuevoProveedor]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_NuevoProveedor] @nombre varchar(50), @razonSocial varchar(50), @direccion varchar(50), @codigoPostal varchar(50), @telefono varchar(50),
@email varchar(50), @idProveedor int OUTPUT AS
BEGIN
INSERT INTO Proveedores (Nombre, Razon_Social, Direccion, Codigo_Postal, Telefono, Email)
VALUES (@nombre, @razonSocial, @direccion, @codigoPostal, @telefono, @email);
SELECT @idProveedor = Id FROM Proveedores WHERE Nombre = @nombre;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Ver_Pedidos]    Script Date: 19/12/2021 17:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Ver_Pedidos] @numeroPedido int AS
SELECT * FROM PedidosCompleto WHERE PedidoNum = @numeroPedido;
GO
USE [master]
GO
ALTER DATABASE [Practica_2] SET  READ_WRITE 
GO
