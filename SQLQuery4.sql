USE [OFFAL]
GO
/****** Object:  StoredProcedure [dbo].[ObtenerOrdenes]    Script Date: 1/3/2019 3:51:55 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[ObtenerCaja]
	-- Add the parameters for the stored procedure here
	--@cantidad as int
	@linea as int
	
AS
BEGIN
if @linea=0
	begin 
	select *from salida 
	END
else
	begin 
	select *from salida where linea=@linea
	end
