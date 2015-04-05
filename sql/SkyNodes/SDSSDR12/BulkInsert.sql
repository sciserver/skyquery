SELECT 
'BULK INSERT ' + t.name + '
FROM ''\\sciserver01\data\data1\deoyani\compiled_skyquery\bcpGenerated\' + t.name + '.bcp''
WITH (TABLOCK, DATAFILETYPE=''native''' + 

CASE WHEN c.name IS NULL THEN ''
ELSE ', ORDER(' + c.name + ')'
END
+ ')
GO'
FROM sys.tables t
LEFT OUTER JOIN sys.indexes i ON t.object_id = i.object_id
LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = t.object_id AND ic.index_id = i.index_id
LEFT OUTER JOIN sys.columns c ON c.object_id = t.object_id AND c.column_id = ic.column_id
WHERE t.type = 'U' and t.name <> 'PhotoObjAll'
    AND (ic.key_ordinal = 1 OR ic.key_ordinal IS NULL)

