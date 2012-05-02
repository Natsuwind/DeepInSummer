--3.0
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_invitation') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_invitation](
	[inviteid] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_dnt_invitation_inviteid] PRIMARY KEY([inviteid]),
	[invitecode] [nchar](7) NOT NULL,
	[creatorid] [int] NOT NULL,
	[creator] [nchar](20) NOT NULL,
	[successcount] [int] NOT NULL CONSTRAINT [DF_dnt_invitation_usecount]  DEFAULT ((0)),
	[createdtime] [smalldatetime] NOT NULL,
	[expiretime] [smalldatetime] NOT NULL,
	[maxcount] [int] NOT NULL,
	[invitetype] [int] NOT NULL,
	[isdeleted] [int] NOT NULL CONSTRAINT [DF_dnt_invitation_isdelete]  DEFAULT ((0))
)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_invitation') AND NAME = N'code')
CREATE NONCLUSTERED INDEX [code] ON [dnt_invitation]
(
	[invitecode] ASC
)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_invitation') AND NAME = N'creatorid')
CREATE NONCLUSTERED INDEX [creatorid] ON [dnt_invitation]
(
	[creatorid] ASC
)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_invitation') AND NAME = N'invitetype')
CREATE NONCLUSTERED INDEX [invitetype] ON [dnt_invitation]
(
	[invitetype] ASC
)
GO

--ALTER TABLE [dnt_invitation] ADD  CONSTRAINT [DF_dnt_invitation_usecount]  DEFAULT ((0)) FOR [successcount]
--GO

--ALTER TABLE [dnt_invitation] ADD  CONSTRAINT [DF_dnt_invitation_isdelete]  DEFAULT ((0)) FOR [isdeleted]
--GO
IF EXISTS(select * from syscolumns where id=object_id('dnt_orders') and name='status')
EXEC sp_rename 'dnt_orders','dnt_ordersnew'
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_orders') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_orders](
	[orderid] [int] IDENTITY(10000,1) NOT NULL CONSTRAINT [PK_dnt_orders_orderid] PRIMARY KEY([orderid]),
	[ordercode] [char](32) NOT NULL,
	[uid] [int] NOT NULL,
	[buyer] [char](20) NOT NULL,
	[paytype] [tinyint] NOT NULL,
	[tradeno] [char](32) NULL,
	[price] [decimal](18, 2) NOT NULL,
	[orderstatus] [tinyint] NOT NULL,
	[createdtime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_orders_createdtime]  DEFAULT (getdate()),
	[confirmedtime] [smalldatetime] NULL,
	[credit] [tinyint] NOT NULL,
	[amount] [int] NOT NULL
)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_orders') AND NAME = N'dnt_orders_ordercode')
CREATE NONCLUSTERED INDEX [dnt_orders_ordercode] ON [dnt_orders] 
(
	[ordercode] ASC
)
GO

--ALTER TABLE [dnt_orders] ADD  CONSTRAINT [DF_dnt_orders_createdtime]  DEFAULT (getdate()) FOR [createdtime]
--GO

-------------创建索引--------------------------------------------

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topictagcaches') AND NAME = N'IX_dnt_topictagcaches_tid')
CREATE INDEX [IX_dnt_topictagcaches_tid] ON dnt_topictagcaches(tid)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_polls') AND NAME = N'dnt_polls_tid')
CREATE INDEX [dnt_polls_tid] ON dnt_polls(tid)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_attachpaymentlog') AND NAME = N'IX_dnt_attachpaymentlog_uid')
CREATE INDEX [IX_dnt_attachpaymentlog_uid] ON dnt_attachpaymentlog([uid],[aid])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_forums') AND NAME = N'IX_dnt_forums_status')
CREATE INDEX [IX_dnt_forums_status] ON dnt_forums([status],[layer],[parentid])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_forumfields') AND NAME = N'dnt_forumfields_fid')
CREATE INDEX [dnt_forumfields_fid] ON dnt_forumfields(fid)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_smilies') AND NAME = N'IX_dnt_smilies_type')
CREATE INDEX [IX_dnt_smilies_type] ON dnt_smilies ([type], [displayorder],[id])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_bbcodes') AND NAME = N'dnt_bbcodes_available')
CREATE INDEX [dnt_bbcodes_available] ON dnt_bbcodes(available)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_moderatormanagelog') AND NAME = N'dnt_moderatormanagelog_tid')
CREATE INDEX [dnt_moderatormanagelog_tid] ON dnt_moderatormanagelog(tid)
GO

--3.1
--[dnt_forums]表中添加字段[modnewtopics]
IF NOT EXISTS(select * from syscolumns where id=object_id('dnt_forums') and name='modnewtopics')
	ALTER TABLE [dnt_forums] ADD [modnewtopics] [int] NOT NULL CONSTRAINT [DF_dnt_forums_modnewtopics] DEFAULT (0)
GO

--[dnt_usergroups]表中添加字段[modnewtopics]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='modnewtopics')
	ALTER TABLE [dnt_usergroups] ADD [modnewtopics] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_modnewtopics] DEFAULT (0)
GO

--[dnt_usergroups]表中添加字段[modnewposts]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='modnewposts')
	ALTER TABLE [dnt_usergroups] ADD [modnewposts] [smallint] NOT NULL CONSTRAINT [DF_dnt_usergroups_modnewposts] DEFAULT (0)
GO

--[dnt_templates]表中添加字段[templateurl]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_templates') AND NAME='templateurl')
	ALTER TABLE [dnt_templates] ADD [templateurl] [nvarchar] (100) NOT NULL CONSTRAINT [DF_dnt_templates_templateurl] DEFAULT('')
GO

--[dnt_polls]表中添加字段[allowview]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_polls') AND NAME='allowview')
	ALTER TABLE [dnt_polls] ADD [allowview] [tinyint] NOT NULL CONSTRAINT [DF_dnt_polls_allowview] DEFAULT(0)
GO

--[dnt_notices]表中添加字段[fromid]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_notices') AND NAME='fromid')
	ALTER TABLE [dnt_notices] ADD [fromid] [int] NOT NULL CONSTRAINT [DF_dnt_notices_fromid] DEFAULT(0)
GO

--[dnt_favorites]表中添加字段[favtime]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_favorites') AND NAME='favtime')
	ALTER TABLE [dnt_favorites] ADD [favtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_favorites_favtime] DEFAULT(getdate())
GO

--[dnt_favorites]表中添加字段[viewtime]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_favorites') AND NAME='viewtime')
	ALTER TABLE [dnt_favorites] ADD [viewtime] [datetime] NOT NULL CONSTRAINT [DF_dnt_favorites_viewtime] DEFAULT(getdate())
GO

--[dnt_attachments]表中添加字段[isimage]
IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_attachments') AND NAME='isimage')
	ALTER TABLE [dnt_attachments] ADD [isimage] [tinyint] NOT NULL CONSTRAINT [DF_dnt_attachments_isimage] DEFAULT (0)
GO

ALTER TABLE [dnt_attachments] ALTER COLUMN [attachment] nchar(255)  NOT NULL
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='allowhtmltitle')
	ALTER TABLE [dnt_usergroups] ADD [allowhtmltitle] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_allowhtmltitle] DEFAULT (0)
GO

IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_usergroups') AND NAME='ignoreseccode')
	ALTER TABLE [dnt_usergroups] ADD [ignoreseccode] [int] NOT NULL CONSTRAINT [DF_dnt_usergroups_ignoreseccode] DEFAULT (0)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_trendstat') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dnt_trendstat](
		[daytime]	[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_daytime]	DEFAULT (0),
		[login]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_login]		DEFAULT (0),
		[register]	[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_register]	DEFAULT (0),
		[topic]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_topic]		DEFAULT (0),
		[post]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_post]		DEFAULT (0),
		[poll]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_poll]		DEFAULT (0),
		[debate]	[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_debate]		DEFAULT (0),
		[bonus]		[int] NOT NULL CONSTRAINT [DF_dnt_trendstat_bonus]		DEFAULT (0),
	 CONSTRAINT [PK_dnt_trendstat] PRIMARY KEY CLUSTERED 
	(
		[daytime]
	) ON [PRIMARY]
	)ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_postdebatefields') AND name = N'tid_opinion')
CREATE INDEX [tid_opinion] ON [dnt_postdebatefields] 
(
	[tid] ,
	[opinion]
) ON [PRIMARY]
GO

-------------删除索引------------------------------------------
IF EXISTS (SELECT * FROM SYSINDEXES WHERE name = 'displayorder' AND id = OBJECT_ID('dnt_topics'))
DROP INDEX [dnt_topics].[displayorder]
GO

IF EXISTS (SELECT * FROM SYSINDEXES WHERE name = 'displayorder_fid' AND id = OBJECT_ID('dnt_topics'))
DROP INDEX [dnt_topics].[displayorder_fid]
GO

IF EXISTS (SELECT * FROM SYSINDEXES WHERE name = 'fid' AND id = OBJECT_ID('dnt_topics'))
DROP INDEX [dnt_topics].[fid]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_topictags') AND NAME = N'tid')
CREATE CLUSTERED INDEX [tid] ON [dnt_topictags] ([tid])
GO

--[dnt_debates]表中添加字段\约束[positivevote],[positivevoterids],[negativevote],[negativevoterids]
IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_debates') AND name='positivevote')
ALTER TABLE [dnt_debates] ADD [positivevote] INT NOT NULL CONSTRAINT [DF_dnt_debates_positivevote] DEFAULT (0)
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_debates') AND name='positivevoterids')
ALTER TABLE [dnt_debates] ADD [positivevoterids] TEXT NOT NULL CONSTRAINT [DF_dnt_debates_positivevoterids] DEFAULT ('')
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_debates') AND name='negativevote')
ALTER TABLE [dnt_debates] ADD [negativevote] INT NOT NULL CONSTRAINT [DF_dnt_debates_negativevote] DEFAULT (0)
GO

IF NOT EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_debates') AND name='negativevoterids')
ALTER TABLE [dnt_debates] ADD [negativevoterids] TEXT NOT NULL CONSTRAINT [DF_dnt_debates_negativevoterids] DEFAULT ('')
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_verifyreg') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_verifyreg](
	[regid] [int] IDENTITY(1,1) NOT NULL,
	[ip] [char](15) NOT NULL,
	[email] [char](50) NOT NULL,
	[createtime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_verifyreg_createtime]  DEFAULT (getdate()),
	[expiretime] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_verifyreg_expiretime]  DEFAULT (getdate()),
	[invitecode] [nchar](7) NOT NULL,
	[verifycode] [nchar](16) NOT NULL
 CONSTRAINT [PK_regauth] PRIMARY KEY CLUSTERED 
(
	[regid] ASC
) ON [PRIMARY]
)ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_verifyreg') AND NAME = N'regid')
CREATE INDEX [regid] ON [dnt_verifyreg] ([regid])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_verifyreg') AND NAME = N'ip')
CREATE INDEX [ip] ON [dnt_verifyreg] ([ip])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_verifyreg') AND NAME = N'email')
CREATE INDEX [email] ON [dnt_verifyreg] ([email])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_verifyreg') AND NAME = N'createtime')
CREATE INDEX [createtime] ON [dnt_verifyreg] ([createtime])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_verifyreg') AND NAME = N'verifycode')
CREATE UNIQUE INDEX [verifycode] ON [dnt_verifyreg] ([verifycode])
GO

IF NOT EXISTS(SELECT * FROM syscolumns WHERE id=OBJECT_ID('dnt_failedlogins') AND name='id')
BEGIN
	IF EXISTS (SELECT * FROM SYSINDEXES WHERE id=OBJECT_ID('dnt_failedlogins') AND NAME = 'PK_dnt_failedlogins')
		ALTER TABLE [dnt_failedlogins] DROP CONSTRAINT [PK_dnt_failedlogins]
	ALTER TABLE [dnt_failedlogins] ADD id [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_dnt_failedlogins] PRIMARY KEY  CLUSTERED (id) ON [PRIMARY]
	CREATE UNIQUE INDEX [ip] ON [dnt_failedlogins]([ip]) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_userconnect') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_userconnect](
	[openid] [char](32) NOT NULL,
	[uid] [int] NOT NULL  CONSTRAINT [DF_dnt_userconnect_uid]  DEFAULT ((-1)),
	[token] [char](16) NOT NULL,
	[secret] [char](16) NOT NULL,
	[allowvisitqquserinfo] [int] NOT NULL CONSTRAINT [DF_dnt_userconnect_allowvisitqquserinfo]  DEFAULT ((0)),
	[allowpushfeed] [int] NOT NULL CONSTRAINT [DF_dnt_userconnect_allowpushfeed]  DEFAULT ((0)),
	[issetpassword] [int] NOT NULL,
	[callbackinfo] [nvarchar](100) NOT NULL
	 CONSTRAINT [PK_dnt_userconnect] PRIMARY KEY CLUSTERED 
	(
		[openid] ASC
	) ON [PRIMARY]
	) ON [PRIMARY]
GO

--3.9beta版创建的是唯一索引,升级正式版时要先将原有的索引删除再创建不唯一索引
IF  EXISTS (SELECT * FROM SYSINDEXES WHERE id=OBJECT_ID('dnt_userconnect') AND NAME = 'uid')
	DROP INDEX  [dnt_userconnect].[uid]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE id=OBJECT_ID('dnt_userconnect') AND NAME = 'uid')
	CREATE INDEX [uid] ON [dnt_userconnect] ([uid])
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE id=OBJECT_ID('dnt_topicidentify') AND NAME = 'PK_dnt_topicidentify')
BEGIN
	ALTER TABLE [dnt_topicidentify] DROP COLUMN [identifyid]
	ALTER TABLE [dnt_topicidentify] ADD [identifyid] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_dnt_topicidentify] PRIMARY KEY  CLUSTERED ([identifyid]) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_pushfeedlog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_pushfeedlog](
	[tid] [int] NOT NULL,
	[uid] [int] NOT NULL,
	[authortoken] [char](16) NOT NULL,
	[authorsecret] [char](16) NOT NULL,
	[pushdate] [smalldatetime] NOT NULL CONSTRAINT [DF_dnt_pushfeedlog_pushdate]  DEFAULT (getdate()),
	 CONSTRAINT [PK_dnt_pushfeedlog] PRIMARY KEY CLUSTERED 
	(
		[tid] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'精华', N'001.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'热帖', N'002.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'美图', N'003.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'优秀', N'004.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'置顶', N'005.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'推荐', N'006.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'原创', N'007.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'版主推荐', N'008.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'爆料', N'009.gif')
GO

INSERT [dnt_topicidentify] ([name], [filename]) VALUES (N'编辑采用', N'010.gif')
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'dnt_connectbindlog') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [dnt_connectbindlog](
	[openid] [char](32) NOT NULL,
	[uid] [int] NOT NULL,
	[type] [smallint] NOT NULL CONSTRAINT [DF_dnt_connectbindlog_type]  DEFAULT ((1)),
	[bindcount] [smallint] NOT NULL CONSTRAINT [DF_dnt_connectbindlog_bindcount]  DEFAULT ((0)),
	 CONSTRAINT [PK_dnt_connectbindlog] PRIMARY KEY CLUSTERED 
	(
		[openid] ASC
	)ON [PRIMARY]
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_connectbindlog') AND NAME = N'uid')
CREATE INDEX uid ON [dnt_connectbindlog] (uid)
GO

UPDATE [dnt_smilies] SET [code] = ':m' WHERE [id] = 17