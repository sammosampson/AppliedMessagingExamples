USE [AppliedMessagingExamples.SubscriberWithServerEventIndexStorage]
GO

CREATE TABLE [dbo].[Risk](
	[PolicyNumber] [VARCHAR](50) NOT NULL,
	[TenantId] [NVARCHAR](100) NOT NULL,
	[Risk] [NVARCHAR](1000) NOT NULL,
    CONSTRAINT [PK_Risk] PRIMARY KEY CLUSTERED 
(
	[PolicyNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE PROCEDURE [dbo].[sproc_upsert_risk] 
	@PolicyNumber AS NVARCHAR(50),
	@TenantID AS NVARCHAR(100),
	@Risk AS NVARCHAR(1000)
AS
IF(SELECT COUNT(*) FROM [Risk] WHERE PolicyNumber = @PolicyNumber) = 0
BEGIN
    INSERT INTO [Risk](PolicyNumber, TenantId, Risk) VALUES(@PolicyNumber, @TenantID, @Risk)
END
GO