
CREATE TABLE       wy_column       (
      columnid       INTEGER PRIMARY KEY  NOT NULL  DEFAULT '',
      columnname       CHAR NOT NULL  DEFAULT '',
      description       CHAR NOT NULL  DEFAULT '',
      allowpost       CHAR NOT NULL  DEFAULT '',
      allowedit       CHAR NOT NULL  DEFAULT '', 
      allowdel       CHAR NOT NULL  DEFAULT '', 
      shenghe       INTEGER NOT NULL  DEFAULT '0')
      
      
title
columnid
hightlight
content
postdate
uid
username

articleinfo.Title
articleinfo.Columnid
articleinfo.Hightlight
articleinfo.Content
articleinfo.Postdate
articleinfo.Uid
articleinfo.Username
articleinfo.


uid
username
password
groupid
adminid
qq
email
msn
hi
nickname
realname
regip
del
articlecount
topiccount
replycount
lastlogip
bdday
lastlogdate
regdate

info.Uid
info.Username
info.Password
info.Groupid
info.Adminid
info.Qq
info.Email
info.Msn
info.Hi
info.Nickname
info.Realname
info.Regip
info.Del
info.Articlecount
info.Topiccount
info.Replycount
info.Lastlogip
info.Bdday
info.Lastlogdate
info.Regdate
























uid,username,password,groupid,adminid,qq,email,msn,hi,nickname,realname,regip,del,articlecount,topiccount,replycount,lastlogip,bdday,lastlogdate,regdate
uid,@username,@password,@groupid,@adminid,@qq,@email,@msn,@hi,@nickname,@realname,@regip,@del,@articlecount,@topiccount,@replycount,@lastlogip,@bdday,@lastlogdate,@regdate


attachmentid,@filename,@filepath,@filetype,@posterid,@description
Attachmentid
Filename
Filepath
Filetype
Posterid
Description

commentid,articleid,uid,username,postdate,del,content,goodcount,badcount


commentid
articleid
uid
username
postdate
del
content
goodcount
badcount

Commentid
Articleid
Uid
Username
Postdate
Del
Content
Goodcount
Badcount