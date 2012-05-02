IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_posts1') AND NAME = N'showtopic')
CREATE  UNIQUE  INDEX [showtopic] ON [dnt_posts1]([tid], [invisible], [pid]) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID('dnt_posts1') AND name='postdatetime' AND type=58)
BEGIN
	EXEC sp_rename '[dnt_posts1].[postdatetime]',  'postdatetime2',  'COLUMN'
	IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts1_postdatetime')
		ALTER TABLE [dnt_posts1] DROP CONSTRAINT [DF_dnt_posts1_postdatetime]
	IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_postdatetime')
		ALTER TABLE [dnt_posts1] DROP CONSTRAINT [DF_dnt_posts_postdatetime]
	IF EXISTS(SELECT * FROM sysobjects WHERE name='DF__dnt_posts__postd__0CBBB3CE')
		ALTER TABLE [dnt_posts1] DROP CONSTRAINT [DF__dnt_posts__postd__0CBBB3CE]
	IF EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_posts1') AND NAME = N'IX_dnt_posts1_fid')
		DROP INDEX [dnt_posts1].[IX_dnt_posts1_fid]
	IF EXISTS (SELECT * FROM SYSINDEXES WHERE ID = OBJECT_ID(N'dnt_posts1') AND NAME = N'IX_dnt_posts1_posterid')
		DROP INDEX [dnt_posts1].[IX_dnt_posts1_posterid]
	ALTER TABLE [dnt_posts1] ADD [postdatetime] [datetime]  NULL
	EXEC('UPDATE [dnt_posts1] SET [postdatetime]=[postdatetime2]')
	ALTER TABLE [dnt_posts1] ALTER COLUMN [postdatetime] [datetime]  NOT NULL
END
GO

IF EXISTS (select * from syscolumns where id=object_id('dnt_posts1') and name='postdatetime2')
	ALTER TABLE [dnt_posts1] DROP COLUMN [postdatetime2]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts1_postdatetime')
	ALTER TABLE [dnt_posts1] WITH NOCHECK ADD CONSTRAINT [DF_dnt_posts1_postdatetime] DEFAULT (getdate()) FOR [postdatetime]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_attachment')
	EXEC sp_rename '[DF_dnt_posts_attachment]','DF_dnt_posts1_attachment','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_bbcodeoff')
	EXEC sp_rename '[DF_dnt_posts_bbcodeoff]','DF_dnt_posts1_bbcodeoff','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_htmlon')
	EXEC sp_rename '[DF_dnt_posts_htmlon]','DF_dnt_posts1_htmlon','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_invisible')
	EXEC sp_rename '[DF_dnt_posts_invisible]','DF_dnt_posts1_invisible','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_ip')
	EXEC sp_rename '[DF_dnt_posts_ip]','DF_dnt_posts1_ip','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_lastedit')
	EXEC sp_rename '[DF_dnt_posts_lastedit]','DF_dnt_posts1_lastedit','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_layer')
	EXEC sp_rename '[DF_dnt_posts_layer]','DF_dnt_posts1_layer','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_message')
	EXEC sp_rename '[DF_dnt_posts_message]','DF_dnt_posts1_message','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_parentid')
	EXEC sp_rename '[DF_dnt_posts_parentid]','DF_dnt_posts1_parentid','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_parseurloff')
	EXEC sp_rename '[DF_dnt_posts_parseurloff]','DF_dnt_posts1_parseurloff','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_pid')
	EXEC sp_rename '[DF_dnt_posts_pid]','DF_dnt_posts1_pid','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_poster')
	EXEC sp_rename '[DF_dnt_posts_poster]','DF_dnt_posts1_poster','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_posterid')
	EXEC sp_rename '[DF_dnt_posts_posterid]','DF_dnt_posts1_posterid','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_rate')
	EXEC sp_rename '[DF_dnt_posts_rate]','DF_dnt_posts1_rate','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_ratetimes')
	EXEC sp_rename '[DF_dnt_posts_ratetimes]','DF_dnt_posts1_ratetimes','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_smileyoff')
	EXEC sp_rename '[DF_dnt_posts_smileyoff]','DF_dnt_posts1_smileyoff','OBJECT'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='DF_dnt_posts_usesig')
	EXEC sp_rename '[DF_dnt_posts_usesig]','DF_dnt_posts1_usesig','OBJECT'
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE id=OBJECT_ID('dnt_posts1') AND NAME = 'IX_dnt_posts1_fid')
	CREATE INDEX [IX_dnt_posts1_fid] ON [dnt_posts1](fid,tid,posterid)
GO

IF NOT EXISTS (SELECT * FROM SYSINDEXES WHERE id=OBJECT_ID('dnt_posts1') AND NAME = 'IX_dnt_posts1_posterid')
	CREATE INDEX [IX_dnt_posts1_posterid] ON [dnt_posts1] (posterid,tid,pid)
GO

IF EXISTS (SELECT * FROM SYSINDEXES WHERE name = 'treelist' AND id = OBJECT_ID('dnt_posts1'))
DROP INDEX [dnt_posts1].[treelist]
GO

IF EXISTS (SELECT * FROM SYSINDEXES WHERE name = 'parentid' AND id = OBJECT_ID('dnt_posts1'))
DROP INDEX [dnt_posts1].[parentid]
GO

IF EXISTS (SELECT * FROM SYSINDEXES WHERE name = 'tid' AND id = OBJECT_ID('dnt_posts1'))
DROP INDEX [dnt_posts1].[tid]