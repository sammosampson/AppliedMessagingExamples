USE [AppliedMessagingExamples.SubscriberWithLocalEventIndexStorage]
GO

CREATE TABLE [dbo].[EventIndexStore](
	[Stream] [NVARCHAR](100) NOT NULL,
	[OwningProcess] [NVARCHAR](1000) NOT NULL,
	[EventIndex] [INT] NOT NULL,
 CONSTRAINT [PK_EventIndexStore] PRIMARY KEY CLUSTERED 
(
	[Stream] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE PROCEDURE [dbo].[sproc_upsert_event_index] 
	@Stream AS NVARCHAR(100),
	@OwningProcess AS NVARCHAR(1000),
	@EventIndex AS INT
AS

IF NOT EXISTS (SELECT * FROM dbo.EventIndexStore WHERE [Stream] = @Stream AND [OwningProcess] = @OwningProcess)
	INSERT INTO [dbo].[EventIndexStore] ([Stream], [OwningProcess], [EventIndex]) VALUES(@Stream, @OwningProcess, @EventIndex)
ELSE
	UPDATE [dbo].[EventIndexStore] SET [EventIndex] = @EventIndex WHERE [Stream] = @Stream AND [OwningProcess] = @OwningProcess
GO

CREATE PROCEDURE [dbo].[sproc_get_event_index] 
	@Stream AS NVARCHAR(100),
	@OwningProcess AS NVARCHAR(1000)
AS
SELECT [EventIndex] FROM [dbo].[EventIndexStore] WHERE [Stream] = @Stream AND [OwningProcess] = @OwningProcess
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

CREATE PROCEDURE [dbo].[sproc_insert_risk] 
	@PolicyNumber AS NVARCHAR(50),
	@TenantID AS NVARCHAR(100),
	@Risk AS NVARCHAR(1000)
AS
INSERT INTO [Risk](PolicyNumber, TenantId, Risk) VALUES(@PolicyNumber, @TenantID, @Risk)
GO