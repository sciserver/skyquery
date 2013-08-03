-- SUBSAMPLING TABLE 'PhotoObj' ---

 -- Insert subset into destination table
 INSERT [SkyNode_TwoQZ_Mini].[dbo].[PhotoObj] WITH (TABLOCKX)
    ([objID], [name], [ra], [dec], [cat_num], [cat_name], [sector], [raB1950], [decB1950], [UKST], [x_apm], [y_apm], [raBrad], [decBrad], [bj], [u_bj], [bj_r], [n_obs], [z1], [q1], [id1], [date1], [fld1], [fibre1], [SN1], [z2], [q2], [id2], [date2], [fld2], [fibre2], [SN2], [zprev], [radio], [Xray], [dust], [comm1], [comm2], [cx], [cy], [cz], [htmid])
 SELECT sourcetablealias.[objID], sourcetablealias.[name], sourcetablealias.[ra], sourcetablealias.[dec], sourcetablealias.[cat_num], sourcetablealias.[cat_name], sourcetablealias.[sector], sourcetablealias.[raB1950], sourcetablealias.[decB1950], sourcetablealias.[UKST], sourcetablealias.[x_apm], sourcetablealias.[y_apm], sourcetablealias.[raBrad], sourcetablealias.[decBrad], sourcetablealias.[bj], sourcetablealias.[u_bj], sourcetablealias.[bj_r], sourcetablealias.[n_obs], sourcetablealias.[z1], sourcetablealias.[q1], sourcetablealias.[id1], sourcetablealias.[date1], sourcetablealias.[fld1], sourcetablealias.[fibre1], sourcetablealias.[SN1], sourcetablealias.[z2], sourcetablealias.[q2], sourcetablealias.[id2], sourcetablealias.[date2], sourcetablealias.[fld2], sourcetablealias.[fibre2], sourcetablealias.[SN2], sourcetablealias.[zprev], sourcetablealias.[radio], sourcetablealias.[Xray], sourcetablealias.[dust], sourcetablealias.[comm1], sourcetablealias.[comm2], sourcetablealias.[cx], sourcetablealias.[cy], sourcetablealias.[cz], sourcetablealias.[htmid]
 FROM   [SkyNode_TwoQZ].[dbo].[PhotoObj] sourcetablealias WITH (NOLOCK)
	;
 

GO

