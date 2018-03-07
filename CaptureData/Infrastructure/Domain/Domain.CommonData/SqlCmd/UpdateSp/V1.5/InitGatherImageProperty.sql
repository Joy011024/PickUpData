 update TecentQQData  set imgtype=-1 where  headimageUrl is null;
 update  TecentQQData  set   imgtype=0 where len(headimageurl)>0 and imgtype is null;
 update TecentQQData  set GatherImageErrorNum=0 where GatherImageErrorNum is null;
 update TecentQQData  set IsGatherImage=0 where IsGatherImage is null;