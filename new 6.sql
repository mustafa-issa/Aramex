DECLARE @SQL AS VarChar(MAX)
SET @SQL = ''

SELECT @SQL = @SQL + 'SELECT ' + quotename(table_name,'''') + ' as table_name, ts, value FROM ' + '[' + TABLE_NAME + '] union '
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME LIKE '%100%' and table_name not like '%err%'
select @SQL = substring(@SQL, 1, (len(@SQL) - 6))
--EXEC (@SQL)

select * from(
select table_name, ts, value , row_number() over 
			(partition by table_name ORDER BY ts DESC) as row_num
from (
SELECT 'Aramex_100_TL26' as table_name, ts, value FROM [Aramex_100_TL26] 
union SELECT 'Aramex_100_TL27' as table_name, ts, value FROM [Aramex_100_TL26] ) d)e
where row_num =1
