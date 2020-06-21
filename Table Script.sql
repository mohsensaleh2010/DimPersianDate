
CREATE TABLE [dbo].[DimDate]
(
	MiladiDate DATETIME,
	ShamsiDate NVARCHAR(12),
	ShamsiYear INT,
	ShamsiMonth INT,
	ShamsiMonthName NVARCHAR(50),
	ShamsiDayInMonth TINYINT,
	ShamsiSesion TINYINT,
)
go

-- select * from DimDate