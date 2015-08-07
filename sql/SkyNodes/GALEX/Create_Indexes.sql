CREATE NONCLUSTERED INDEX [IX_PhotoObjAll_htmID] ON [dbo].[PhotoObjAll]
(
	[htmID] ASC
)
INCLUDE
(
	[cx], [cy], [cz], [ra], [dec],
	[mode]
)
WITH (SORT_IN_TEMPDB = ON)
ON [PHOTOIDX]
GO
