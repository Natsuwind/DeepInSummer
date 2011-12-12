CREATE TABLE `litecms`.`wy_admins` (
  `adminid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL DEFAULT '',
  `password` VARCHAR(32) NOT NULL DEFAULT '',
  `uid` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `allowip` VARCHAR(200) NOT NULL,
  `lastlogindate` DATETIME NOT NULL,
  `lastloginip` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`adminid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_articles` (
  `articleid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `columnid` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `title` VARCHAR(45) NOT NULL,
  `summary` VARCHAR(45) NOT NULL,
  `uid` INTEGER UNSIGNED NOT NULL,
  `username` VARCHAR(45) NOT NULL,
  `postdate` DATETIME NOT NULL,
  `commentcount` INTEGER UNSIGNED NOT NULL,
  `viewcount` INTEGER UNSIGNED NOT NULL,
  `sort` INTEGER UNSIGNED NOT NULL,
  `ip` VARCHAR(45) NOT NULL,
  `image` VARCHAR(200) NOT NULL,
  `del` INTEGER UNSIGNED NOT NULL,
  `content` TEXT NOT NULL,
  `goodcount` INTEGER UNSIGNED NOT NULL,
  `badcount` INTEGER UNSIGNED NOT NULL,
  `highlight` VARCHAR(45) NOT NULL,
  `recommend` INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY (`articleid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_articletype` (
  `typeid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `typename` VARCHAR(45) NOT NULL,
  `description` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`typeid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;


CREATE TABLE `litecms`.`wy_attachments` (
  `attachmentid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `filename` VARCHAR(45) NOT NULL,
  `filepath` VARCHAR(45) NOT NULL,
  `filetype` INTEGER UNSIGNED NOT NULL,
  `posterid` INTEGER UNSIGNED NOT NULL,
  `description` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`attachmentid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_columns` (
  `columnid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `columnname` VARCHAR(45) NOT NULL,
  `description` VARCHAR(200) NOT NULL,
  `allowpost` VARCHAR(45) NOT NULL,
  `allowedit` VARCHAR(45) NOT NULL,
  `allowdel` VARCHAR(45) NOT NULL,
  `shenghe` INTEGER UNSIGNED NOT NULL,
  `parentid` INTEGER UNSIGNED NOT NULL,
  `del` INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY (`columnid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_comments` (
  `commentid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `articleid` INTEGER UNSIGNED NOT NULL,
  `uid` INTEGER UNSIGNED NOT NULL,
  `username` VARCHAR(45) NOT NULL,
  `postdate` DATETIME NOT NULL,
  `del` INTEGER UNSIGNED NOT NULL,
  `content` VARCHAR(200) NOT NULL,
  `goodcount` INTEGER UNSIGNED NOT NULL,
  `badcount` INTEGER UNSIGNED NOT NULL,
  `articletitle` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`commentid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_groups` (
  `groupid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `groupname` VARCHAR(45) NOT NULL,
  `allowpost` INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY (`groupid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_templates` (
  `templateid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `folder` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`templateid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;

CREATE TABLE `litecms`.`wy_users` (
  `uid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(32) NOT NULL,
  `groupid` INTEGER UNSIGNED NOT NULL,
  `adminid` INTEGER UNSIGNED NOT NULL,
  `qq` VARCHAR(100) NOT NULL,
  `email` VARCHAR(100) NOT NULL,
  `msn` VARCHAR(100) NOT NULL,
  `hi` VARCHAR(100) NOT NULL,
  `nickname` VARCHAR(45) NOT NULL,
  `realname` VARCHAR(45) NOT NULL,
  `regip` VARCHAR(45) NOT NULL,
  `del` INTEGER UNSIGNED NOT NULL,
  `articlecount` INTEGER UNSIGNED NOT NULL,
  `topiccount` INTEGER UNSIGNED NOT NULL,
  `replycount` INTEGER UNSIGNED NOT NULL,
  `lastlogip` VARCHAR(45) NOT NULL,
  `bdday` DATETIME NOT NULL,
  `lastlogdate` DATETIME NOT NULL,
  `regdate` DATETIME NOT NULL,
  `secques` VARCHAR(45) NOT NULL,
  `secans` VARCHAR(32) NOT NULL,
  PRIMARY KEY (`uid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;


CREATE TABLE `litecms`.`wy_feeds` (
  `feed_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `typeid` INTEGER UNSIGNED NOT NULL,
  `accesslevel` INTEGER UNSIGNED NOT NULL,
  `title` VARCHAR(90) NOT NULL,
  `content` VARCHAR(180) NOT NULL,
  `senddate` DATETIME NOT NULL,
  PRIMARY KEY (`feed_id`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;
ALTER TABLE `litecms`.`wy_feeds` ADD COLUMN `url` VARCHAR(360) NOT NULL AFTER `senddate`;

CREATE TABLE `litecms`.`wy_comments` (
  `commentid` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `articleid` INTEGER UNSIGNED NOT NULL,
  `uid` INTEGER UNSIGNED NOT NULL,
  `username` VARCHAR(45) NOT NULL,
  `postdate` DATETIME NOT NULL,
  `del` INTEGER UNSIGNED NOT NULL,
  `content` VARCHAR(1000) NOT NULL,
  `goodcount` INTEGER UNSIGNED NOT NULL,
  `badcount` INTEGER UNSIGNED NOT NULL,
  `articletitle` VARCHAR(90) NOT NULL,
  PRIMARY KEY (`commentid`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;


CREATE TABLE  `litecms`.`wy_columns` (
  `columnid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `columnname` varchar(45) NOT NULL,
  `description` varchar(90) NOT NULL,
  `allowpost` int(10) unsigned NOT NULL,
  `allowedit` int(10) unsigned NOT NULL,
  `allowdel` int(10) unsigned NOT NULL,
  `shenghe` int(10) unsigned NOT NULL,
  `parentid` int(10) unsigned NOT NULL,
  `del` int(10) unsigned NOT NULL,
  PRIMARY KEY (`columnid`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;


CREATE TABLE `litecms`.`wy_blogs` (
  `blog_id` INTEGER UNSIGNED,
  `title` VARCHAR(90) NOT NULL,
  `content` TEXT NOT NULL,
  `uid` INTEGER UNSIGNED NOT NULL,
  `postdate` DATETIME NOT NULL,
  `blog_type` INTEGER UNSIGNED NOT NULL,
  `status` INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY (`blog_id`)
)
ENGINE = MyISAM
CHARACTER SET utf8 COLLATE utf8_general_ci;
