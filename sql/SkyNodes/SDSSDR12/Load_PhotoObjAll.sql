BULK INSERT [dbo].[PhotoObjAll_01]
FROM '\\SCISERVER04\data\data1\users\dobos\01.bcp'
WITH (
	TABLOCK, 
	DATAFILETYPE = 'native',
	ORDER (objID)
)

GO

BULK INSERT [dbo].[PhotoObjAll_02]
FROM '\\SCISERVER04\data\data1\users\dobos\02.bcp'
WITH (
	TABLOCK, 
	DATAFILETYPE = 'native',
	ORDER (objID)
)

GO

BULK INSERT [dbo].[PhotoObjAll_03]
FROM '\\SCISERVER04\data\data1\users\dobos\03.bcp'
WITH (
	TABLOCK, 
	DATAFILETYPE = 'native',
	ORDER (objID)
)

GO

BULK INSERT [dbo].[PhotoObjAll_04]
FROM '\\SCISERVER04\data\data1\users\dobos\04.bcp'
WITH (
	TABLOCK, 
	DATAFILETYPE = 'native',
	ORDER (objID)
)

GO

BULK INSERT [dbo].[PhotoObjAll_05]
FROM '\\SCISERVER04\data\data1\users\dobos\05.bcp'
WITH (
	TABLOCK, 
	DATAFILETYPE = 'native',
	ORDER (objID)
)