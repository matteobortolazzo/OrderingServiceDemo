PRINT 'Populating Lookup Table [dbo].[OrderStatus]'

MERGE INTO [dbo].[OrderStatus] as t
    USING (
        VALUES(1,'Submitted')
             ,(2,'Rejected')
             ,(3,'Confirmed')
    ) s ([Id],[Name])
        ON t.[Id] = s.[Id]
WHEN MATCHED AND (
        s.[Name] <> t.[Name]
    )
    THEN 
        UPDATE
            SET [Name] = s.[Name]
WHEN NOT MATCHED BY TARGET
    THEN
        INSERT 
        (
            [Id]
            ,[Name]
        )
        VALUES 
        (
            [Id]
            ,[Name]
        )
WHEN NOT MATCHED BY SOURCE 
   THEN 
       DELETE;

GO