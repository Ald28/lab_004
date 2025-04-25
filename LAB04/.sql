USE Neptuno
GO
CREATE PROC ListarPedidos
AS
BEGIN
SELECT IdPedido
      ,[IdCliente]
      ,[IdEmpleado]
      ,[FechaPedido]
      ,[FechaEntrega]
      ,[FechaEnvio]
      ,[FormaEnvio]
      ,[Cargo]
      ,[Destinatario]
      ,[DireccionDestinatario]
      ,[CiudadDestinatario]
      ,[RegionDestinatario]
      ,[CodPostalDestinatario]
      ,[PaisDestinatario]
  FROM [Neptuno].[dbo].[Pedidos]
END

CREATE PROC ListarPedidos
AS
BEGIN
SELECT [IdPedido]
      ,[IdCliente]
      ,[IdEmpleado]
      ,[FechaPedido]
      ,[FechaEntrega]
      ,[FechaEnvio]
      ,[FormaEnvio]
      ,[Cargo]
      ,[Destinatario]
      ,[DireccionDestinatario]
      ,[CiudadDestinatario]
      ,[RegionDestinatario]
      ,[CodPostalDestinatario]
      ,[PaisDestinatario]
  FROM [Neptuno].[dbo].[Pedidos]
  WHERE Destinatario LIKE '%' +@Destinatario + '%'
END

CREATE PROC ListarProductos
AS
BEGIN
SELECT [idproducto]
      ,[nombreProducto]
      ,[idProveedor]
      ,[idCategoria]
      ,[cantidadPorUnidad]
      ,[precioUnidad]
      ,[unidadesEnExistencia]
      ,[unidadesEnPedido]
      ,[nivelNuevoPedido]
      ,[suspendido]
      ,[categoriaProducto]
  FROM [Neptuno].[dbo].[productos]
END

CREATE PROC ListarProductos
AS
BEGIN
SELECT [idproducto]
      ,[nombreProducto]
      ,[idProveedor]
      ,[idCategoria]
      ,[cantidadPorUnidad]
      ,[precioUnidad]
      ,[unidadesEnExistencia]
      ,[unidadesEnPedido]
      ,[nivelNuevoPedido]
      ,[suspendido]
      ,[categoriaProducto]
  FROM [Neptuno].[dbo].[productos]
END

CREATE PROC ListarCategorias
AS
BEGIN
SELECT [idcategoria]
      ,[nombrecategoria]
      ,[descripcion]
      ,[Activo]
      ,[CodCategoria]
  FROM [Neptuno].[dbo].[categorias]
END

CREATE PROC ListarProveedores
AS
BEGIN
SELECT [idProveedor]
      ,[nombreCompañia]
      ,[nombrecontacto]
      ,[cargocontacto]
      ,[direccion]
      ,[ciudad]
      ,[region]
      ,[codPostal]
      ,[pais]
      ,[telefono]
      ,[fax]
      ,[paginaprincipal]
  FROM [Neptuno].[dbo].[proveedores]
END

EXEC ListarProveedores;

/*------------------------------------------------------*/

CREATE PROC BuscarProveedores
	@nombrecontacto NVARCHAR(100) = NULL,
    @ciudad NVARCHAR(100) = NULL
AS
BEGIN
    SELECT 
        idProveedor,
        nombreCompañia,
        nombrecontacto,
        cargocontacto,
        direccion,
        ciudad,
        region,
        codPostal,
        pais,
        telefono,
        fax,
        paginaprincipal
    FROM Neptuno.dbo.proveedores
    WHERE 
        (@nombrecontacto IS NULL OR nombrecontacto LIKE '%' + @nombrecontacto + '%') AND
        (@ciudad IS NULL OR ciudad LIKE '%' + @ciudad + '%')
END

/*------------------------------------------------------------------------*/
CREATE PROCEDURE ListarPedidosPorFecha
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT 
        p.IdPedido,
        p.IdCliente,
        c.NombreCompañia AS NombreCliente,  -- si tienes una tabla Clientes
        p.IdEmpleado,
        p.FechaPedido,
        p.FechaEntrega,
        p.FechaEnvio,
        p.FormaEnvio,
        p.Cargo,
        p.Destinatario,
        p.DireccionDestinatario,
        p.CiudadDestinatario,
        p.RegionDestinatario,
        p.CodPostalDestinatario,
        p.PaisDestinatario
    FROM Neptuno.dbo.Pedidos p
    INNER JOIN Neptuno.dbo.Clientes c ON p.IdCliente = c.IdCliente  -- usa INNER JOIN si deseas info del cliente
    WHERE p.FechaPedido BETWEEN @FechaInicio AND @FechaFin
    ORDER BY p.FechaPedido;
END

EXEC ListarPedidos;