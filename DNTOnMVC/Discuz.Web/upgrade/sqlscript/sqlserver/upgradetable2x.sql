--2.5
--表dnt_forumfields中添加两个字段，seokeywords和seodescription，类型：ntext
IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_forumfields') AND name='seokeywords')
	ALTER TABLE [dnt_forumfields] ADD [seokeywords] [nvarchar] (500) NULL
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_forumfields') AND name='seodescription')
	ALTER TABLE [dnt_forumfields] ADD [seodescription] [nvarchar] (500) NULL
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_forumfields') AND name='rewritename')
	ALTER TABLE [dnt_forumfields] ADD [rewritename] [nvarchar] (20)  NULL
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_userfields') AND name='ignorepm')
	ALTER TABLE [dnt_userfields] ADD [ignorepm] [nvarchar] (1000) NOT NULL CONSTRAINT [DF_dnt_userfields_ignorepm] DEFAULT('')
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_online') AND name='newpms')
	ALTER TABLE [dnt_online] ADD [newpms] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_newpms] DEFAULT(0)
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_online') AND name='newnotices')
	ALTER TABLE [dnt_online] ADD [newnotices] [smallint] NOT NULL CONSTRAINT [DF_dnt_online_newnotices] DEFAULT(0)
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_topics') AND name='attention')
	ALTER TABLE [dnt_topics] ADD [attention] [int] NOT NULL CONSTRAINT [DF_dnt_topics_attention] DEFAULT(0)
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_attachments') AND name='attachprice')
	ALTER TABLE [dnt_attachments] ADD [attachprice] [int] NOT NULL CONSTRAINT [DF_dnt_attachments_attachprice] DEFAULT(0)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_attachpaymentlog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_attachpaymentlog] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[uid] [int] NOT NULL ,
	[username] [nchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[aid] [int] NOT NULL ,
	[authorid] [int] NOT NULL ,
	[postdatetime] [datetime] NOT NULL CONSTRAINT [DF_dnt_attachpaymentlog_postdatetime] DEFAULT (getdate()),
	[amount] [int] NOT NULL ,
	[netamount] [int] NOT NULL,
	CONSTRAINT [PK_dnt_attachpaymentlog] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

--ALTER TABLE [dnt_attachpaymentlog] WITH NOCHECK ADD 
--	CONSTRAINT [PK_dnt_attachpaymentlog] PRIMARY KEY  CLUSTERED 
--	(
--		[id]
--	)  ON [PRIMARY] 
--GO

--ALTER TABLE [dnt_attachpaymentlog] ADD 
--	CONSTRAINT [DF_dnt_attachpaymentlog_postdatetime] DEFAULT (getdate()) FOR [postdatetime]
--GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_navs') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_navs]
(
	[id] [int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_id primary key(id),
	[parentid] [int] NOT NULL CONSTRAINT [DF_dnt_navs_parentid] DEFAULT(0),
	[name] [char](50) NOT NULL,
	[title] [char](255) NOT NULL,
	[url] [char](255) NOT NULL,
	[target] [tinyint] NOT NULL CONSTRAINT [DF_dnt_navs_target] DEFAULT(0),
	[type] [tinyint] NOT NULL CONSTRAINT [DF_dnt_navs_type] DEFAULT(0),
	[available] [tinyint] NOT NULL CONSTRAINT [DF_dnt_navs_available] DEFAULT(0),
	[displayorder] [int] NOT NULL,
	[highlight] [tinyint] NOT NULL CONSTRAINT [DF_dnt_navs_highlight] DEFAULT(0),
	[level] [tinyint] NOT NULL CONSTRAINT [DF_dnt_navs_level] DEFAULT(0) 
)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_banned') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_banned](
	[id] [smallint]  NOT NULL,
	[ip1] [smallint]  NOT NULL,
	[ip2] [smallint]  NOT NULL,
	[ip3] [smallint]  NOT NULL,
	[ip4] [smallint]  NOT NULL,
	[admin] [nvarchar] (50) NOT NULL,
	[dateline] [datetime]  NOT NULL,
	[expiration] [datetime]  NOT NULL,			
	CONSTRAINT [PK_dnt_banned] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY]
)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_notices') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_notices]
(
[nid] [int] NOT NULL IDENTITY(1, 1),
[uid] [int] NOT NULL,
[type] [smallint] NOT NULL,
[new] [tinyint] NOT NULL,
[posterid] [int] NOT NULL,
[poster] [nchar] (20) COLLATE Chinese_PRC_CI_AS NOT NULL,
[note] [ntext] COLLATE Chinese_PRC_CI_AS NOT NULL,
[postdatetime] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_notices') AND NAME = N'uid')
CREATE  INDEX [uid] ON [dnt_notices]([uid] DESC ) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PK_dnt_notices_nid')
ALTER TABLE [dnt_notices] ADD CONSTRAINT [PK_dnt_notices_nid] PRIMARY KEY CLUSTERED  ([nid]) ON [PRIMARY]
GO

--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_tradeoptionvars') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--CREATE TABLE [dnt_tradeoptionvars]
--(
--[typeid] [smallint] NOT NULL,
--[pid] [int] NULL,
--[optionid] [smallint] NULL,
--[value] [ntext] COLLATE Chinese_PRC_CI_AS NULL
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
--GO

UPDATE [dnt_usergroups] SET [allowhtml]=1 WHERE [groupid]=1
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_words') AND name='id' AND type=56)
ALTER TABLE [dnt_words] ALTER COLUMN [id] [int] NOT NULL
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PK_dnt_words')
ALTER TABLE [dnt_words] ADD CONSTRAINT [PK_dnt_words] PRIMARY KEY CLUSTERED ([id]) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_forums_parentid')
BEGIN
	ALTER TABLE [dnt_forums] DROP CONSTRAINT [DF_dnt_forums_parentid]
	ALTER TABLE [dnt_forums] ALTER COLUMN [parentid] [int] NOT NULL
	ALTER TABLE [dnt_forums] ADD CONSTRAINT [DF_dnt_forums_parentid]  DEFAULT (0) FOR [parentid]
END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_forumfields') AND name='fid' AND type=56)
BEGIN
	IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_forumfields') AND name='PK_dnt_forumfields')
		ALTER TABLE [dnt_forumfields] DROP CONSTRAINT [PK_dnt_forumfields]
	ALTER TABLE [dnt_forumfields] ALTER COLUMN [fid] [int] NOT NULL
END
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND name='creditslower' AND type=56)
ALTER TABLE [dnt_usergroups] ALTER COLUMN [creditslower] [int] NOT NULL
GO

UPDATE [dnt_templates] SET [createdate]='2008-12-1',[ver]=2.6,[fordntver]=2.6
GO

-- 建立索引--
IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_pms') AND NAME = N'msgtoid')
CREATE INDEX [msgtoid] ON [dnt_pms] ([msgtoid])	ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_searchcaches') AND NAME = N'getsearchid')
CREATE INDEX [getsearchid] ON [dnt_searchcaches] ([searchstring], [groupid]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topics') AND NAME = N'list_date')
CREATE  INDEX [list_date] ON [dnt_topics]([fid], [displayorder], [postdatetime], [lastpostid] DESC ) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topics') AND NAME = N'list_tid')
 CREATE  INDEX [list_tid] ON [dnt_topics]([fid], [displayorder], [tid]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topics') AND NAME = N'list_replies')
CREATE  INDEX [list_replies] ON [dnt_topics]([fid], [displayorder], [postdatetime], [replies]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topics') AND NAME = N'list_views')
 CREATE  INDEX [list_views] ON [dnt_topics]([fid], [displayorder], [postdatetime], [views]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topics') AND NAME = N'fid_displayorder')
CREATE  INDEX [fid_displayorder] ON [dnt_topics]([fid], [displayorder]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topics') AND NAME = N'tid')
 CREATE  INDEX [tid] ON [dnt_topics]([tid] DESC ) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_attachments') AND NAME = N'tid')
 CREATE  INDEX [tid] ON [dnt_attachments]([tid]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_attachments') AND NAME = N'pid')
 CREATE  INDEX [pid] ON [dnt_attachments]([pid]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_attachments') AND NAME = N'uid')
 CREATE  INDEX [uid] ON [dnt_attachments]([uid]) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_users') AND NAME = N'email')
CREATE NONCLUSTERED INDEX [email] ON [dnt_users] 
(
	[email] ASC
)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_users') AND NAME = N'regip')
CREATE NONCLUSTERED INDEX [regip] ON [dnt_users] 
(
	[regip] ASC
)
GO

---- 初始化表数据--
--INSERT INTO [dnt_navs] VALUES(0,'短消息','短消息','usercpinbox.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'用户中心','用户中心','usercp.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'系统设置','系统设置','admin/index.aspx',1,0,1,0,0,3)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'我的','我的','#',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'标签','标签','tags.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'统计','统计','stats.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'会员','会员','showuser.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'搜索','搜索','search.aspx',1,0,0,0,0,2)
--GO

--INSERT INTO [dnt_navs] VALUES(0,'帮助','帮助','help.aspx',1,0,0,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'基本状况','基本状况','usercpinbox.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'流量统计','流量统计','stats.aspx?type=views',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'客户软件','客户软件','stats.aspx?type=client',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'发帖量记录','发帖量记录','stats.aspx?type=posts',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'版块排行','版块排行','stats.aspx?type=forumsrank',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'主题排行','主题排行','stats.aspx?type=topicsrank',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'发帖排行','发帖排行','stats.aspx?type=postsrank',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'积分排行','积分排行','stats.aspx?type=creditsrank',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(6,'在线时间','在线时间','stats.aspx?type=onlinetime',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的主题','我的主题','mytopics.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的回复','我的回复','myposts.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的精华','我的精华','search.aspx?posterid=current&type=digest',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的附件','我的附件','myattachment.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的收藏','我的收藏','usercpsubscribe.aspx',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的空间','我的空间','space/',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的相册','我的相册','showalbumlist.aspx?uid=-1',0,0,1,0,0,0)
--GO

--INSERT INTO [dnt_navs] VALUES(4,'我的商品','我的商品','usercpmygoods.aspx',0,0,1,0,0,0)
--GO

--2.6
-------------添加字段--------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_attachments') AND name='width')
ALTER TABLE [dnt_attachments] ADD [width] INT NOT NULL DEFAULT (0)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_attachments') AND name='height')
ALTER TABLE [dnt_attachments] ADD [height] INT NOT NULL DEFAULT (0)
GO
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_users') AND name='salt')
ALTER TABLE [dnt_users] ADD [salt] NCHAR(6) NOT NULL CONSTRAINT [DF_dnt_users_salt] DEFAULT ('')
GO

-------------修改初始值--------------------------------------------
UPDATE [dnt_bbcodes] 
SET replacement='<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" width="550" height="400"><param name="allowScriptAccess" value="sameDomain"/><param name="wmode" value="opaque"/><param name="movie" value="{1}"/><param name="quality" value="high"/><param name="bgcolor" value="#ffffff"/><embed src="{1}" quality="high" bgcolor="#ffffff" width="550" height="400" allowScriptAccess="sameDomain" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" wmode="transparent"/></object>'
WHERE id=1
GO

DELETE FROM [dnt_navs] 
WHERE [name]='短消息' OR [name]='用户中心' OR [name]='系统设置' OR [name]='我的' OR [name]='统计'
GO

IF EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_banned') AND name='dataline')
BEGIN
	ALTER TABLE [dnt_banned] ADD [dateline] [datetime] NULL
	EXEC('UPDATE [dnt_banned] SET [dateline] = [dataline]')
	ALTER TABLE [dnt_banned] ALTER COLUMN [dateline] [datetime] NOT NULL
	ALTER TABLE [dnt_banned] DROP COLUMN [dataline]
END
GO
-------------删除表--------------------------------------------
IF OBJECT_ID('catchsoftstatics') IS NOT NULL
DROP TABLE catchsoftstatics