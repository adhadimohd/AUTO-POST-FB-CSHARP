--insert into posttasks (status, groupid, fbid, groupname) select distinct 0, g.id,g.fbid, g.Name from groupraws g 
--inner join groupmanagersets gm on gm.GroupId = g.id 
--where gm.GroupManagerId in (1)


--Select * from posttasks


--update p set p.contentid =1
--from  posttasks p 



--where contentid =0


--select p.id, p.contentid, c.id from posttasks p left join contents c 
--on p.contentid = c.id
declare @counttask int
declare @contentsize int
select @counttask = count(*) from posttasks where contentid = 0 or contentid is null
select @contentsize = count(*) from contents where campaignid = 1
--select @counttask
declare @a int
set @a=@counttask 

while @a != 0
begin
--select @
update top(@contentsize) posttasks 
set contentid = 


select @a=count(*) from posttasks where contentid = 0 or contentid is null
end


select top 3 * from posttasks, (select distinct id from contents) c
