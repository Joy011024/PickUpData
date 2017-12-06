--  ingnore  no image of data
update TecentQQData  set imgtype=-1 where  headimageUrl is null
--init image type 
update  TecentQQData  set   imgtype=0 where len(headimageurl)>0 and imgtype is null
-- init error pick up num
update TecentQQData  set GatherImageErrorNum=0 where GatherImageErrorNum is null