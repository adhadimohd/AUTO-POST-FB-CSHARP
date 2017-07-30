select g.Id, g.Name, g.fbid from GroupRaws g 
where g.id not in (select groupid from GroupManagerSets where GroupManagerId = 1)