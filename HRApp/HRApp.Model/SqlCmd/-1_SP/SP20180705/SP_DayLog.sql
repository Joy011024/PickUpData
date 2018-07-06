use hrapp
go
if(object_id('SP_QueryDayLog','P') is not null)
	drop procedure SP_QueryDayLog
go
create procedure SP_QueryDayLog
(
	@day int,
	@beginRow int,
	@endRow int,
	@total int output
) 
as
select   @total=count(Id)  from eventlog where dayint=@day
select [Id],[Category],[Note],[Title],[IsError],[CreateTime],[DayInt]
from
(
	 select  [Id],[Category],[Note],[Title],[IsError],[CreateTime],[DayInt]
	 ,row_number() over(order by dayint)  rn
	 from eventlog where dayint=@day
) t 
where t.rn>=@beginRow and t.rn<=@endRow
 