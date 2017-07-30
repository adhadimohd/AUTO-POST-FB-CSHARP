SELECT 
    [Extent1].[Id] AS [Id], 
    [Extent1].[Name] AS [Name], 
    [Extent1].[fbid] AS [fbid]
    FROM  [dbo].[GroupRAWs] AS [Extent1]
    INNER JOIN [dbo].[GroupManagerSets] AS [Extent2] ON	 [Extent1].[Id] = [Extent2].[GroupManagerId]
    WHERE [Extent2].[GroupManagerId]=1


	select GM.ID, GM from GroupManagerSets GM where GroupManagerId=1
