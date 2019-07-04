CREATE TABLE [dbo].[Orders] (
    [Id]                      INT              IDENTITY (1, 1) NOT NULL,
    [IdentityGuid]            UNIQUEIDENTIFIER NOT NULL,
    [Quantity]                INT              NOT NULL,
    [UnitPrice]               DECIMAL (18)     NOT NULL,
    [OrderDate]               DATETIME         NOT NULL,
    [RejectionReason]         INT              NULL,
    [OrderStatusId]             INT              NOT NULL,
    [UserId]                  INT              NOT NULL,
    [ProductId]               INT              NOT NULL,
    [DeliveryAddress_Street]  VARCHAR (100)    NOT NULL,
    [DeliveryAddress_City]    VARCHAR (100)    NOT NULL,
    [DeliveryAddress_State]   VARCHAR (100)    NOT NULL,
    [DeliveryAddress_Country] VARCHAR (100)    NOT NULL,
    [DeliveryAddress_ZipCode] VARCHAR (100)    NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Orders_OrderStatus] FOREIGN KEY ([OrderStatusId]) REFERENCES [OrderStatus]([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_IdentityGuid]
    ON [dbo].[Orders]([IdentityGuid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[Orders]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[Orders]([ProductId] ASC);

