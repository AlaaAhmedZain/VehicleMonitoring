USE [master]
GO
/****** Object:  Database [VehiclesMonitoring]    Script Date: 4/10/2018 11:08:46 PM ******/
CREATE DATABASE [VehiclesMonitoring]
go

USE [VehiclesMonitoring]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 4/10/2018 11:08:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[Address] [varchar](500) NOT NULL,
	[Created] [datetime2](7) NULL,
	[Creator] [varchar](50) NULL,
	[Updated] [datetime2](7) NULL,
	[Updator] [varchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
))

GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 4/10/2018 11:08:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[VIN] [varchar](50) NOT NULL,
	[RegNr] [varchar](50) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[LastPing] [datetime2](7) NOT NULL,
	[Created] [datetime2](7) NULL,
	[Creator] [varchar](50) NULL,
	[Updated] [datetime2](7) NULL,
	[Updator] [varchar](50) NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[VIN] ASC
))

GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_Created]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_Updated]  DEFAULT (getdate()) FOR [Updated]
GO
ALTER TABLE [dbo].[Vehicle] ADD  CONSTRAINT [DF_Vehicle_LastPing]  DEFAULT (getdate()) FOR [LastPing]
GO
ALTER TABLE [dbo].[Vehicle] ADD  CONSTRAINT [DF_Vehicle_Created]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Vehicle] ADD  CONSTRAINT [DF_Vehicle_Updated]  DEFAULT (getdate()) FOR [Updated]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [FK_Vehicle_Customer]
GO
/****** Object:  StoredProcedure [dbo].[sp_SearchVehicles]    Script Date: 4/10/2018 11:08:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		azain
-- Create date: 06-04-2018
-- Description:	get the vehicles for a specific customer or with certain status connected =1 or disconnected=0 
-- by sending no of ticks in seconds and current time
-- if seconds of(current time -lastping)>no of ticks -->vehicle diconnected else vehicle connected
--if last ping is null so the vehicle is disconnected
-- =============================================
CREATE PROCEDURE [dbo].[sp_SearchVehicles]  
	-- Add the parameters for the stored procedure here
	@TicksNo int =60,
	@CurrentTime datetime2 = null,
	@CustomerID int = null, 
	@Status bit = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @CurrentTime is null 
	BEGIN 
      set @CurrentTime =GETDATE();
	END ;
	
   
	select * 
	from 
	(
		SELECT c.id,c.name,c.[address],v.VIN,v.RegNr,v.CustomerId,v.LastPing ,	 
			  CASE 
				WHEN v.LastPing is null THEN CAST(0 AS BIT)  
				WHEN DATEDIFF(SECOND,v.LastPing ,@CurrentTime ) >  @TicksNo THEN CAST(0 AS BIT)  
				ELSE CAST(1 AS BIT)  
			  END as [Status]
		from Customer c inner join Vehicle v
		on c.ID=v.CustomerId 
		where
		( @CustomerID  is null or @CustomerID=0 or C.ID=@CustomerID)

	)as Vstatus
	where ( @Status  is null or Vstatus.[Status]=@Status)

END

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateVehicleStatus]    Script Date: 4/10/2018 11:08:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		azain
-- Create date: 06-04-2018
-- Description:	update the vehicle lastping  
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateVehicleStatus]
	
	@VIN varchar(50) , 
	@LastPing	datetime2= null	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT on;
	IF @LastPing is null 
	BEGIN 
      set @LastPing =GETDATE();
	END ;
    update Vehicle set LastPing = @LastPing where VIN=@VIN;
	 --return  @@ROWCOUNT 
END

GO
USE [master]
GO
ALTER DATABASE [VehiclesMonitoring2] SET  READ_WRITE 
GO
