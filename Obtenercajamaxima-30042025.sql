ALTER PROCEDURE [dbo].[ObtenerCajaMaxima1] 
AS
BEGIN
    SET NOCOUNT ON;
    declare @num int=(SELECT numero from Numeromaximo)+1
    declare @date date=getdate()
    declare @datetime datetime=getdate() 
    
    if (@date=(SELECT dia from Numeromaximo))
    begin
        update Numeromaximo set numero=@num where dia=@date
        
        SELECT numero, @datetime as FechaHoraServidor from Numeromaximo
    end    
    else 
    begin 
        update Numeromaximo set numero='0' 
        update Numeromaximo set dia=@date
        update Numeromaximo set numero=(0) where numero='1'
        
        
        SELECT numero, @datetime as FechaHoraServidor from Numeromaximo
    end
END