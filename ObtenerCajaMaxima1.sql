USE [Insumos]
GO
/****** Object:  StoredProcedure [dbo].[ObtenerCajaMaxima1]    Script Date: 22/04/2025 12:17:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[ObtenerCajaMaxima1] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @num int=(SELECT numero from Numeromaximo)+1
	declare @date date=getdate()
	if (@date=(SELECT dia from Numeromaximo))
		begin
			-- Insert statements for procedure here
			update Numeromaximo set numero=@num where dia=@date
			SELECT numero from Numeromaximo
		end	
	else 
		begin 
			update Numeromaximo set numero='0' 
			update  Numeromaximo set dia=@date
			update Numeromaximo set numero=(0) where numero='1'
			SELECT numero from Numeromaximo
		end
		end