--
-- 数据库: `guestbook`
--

-- --------------------------------------------------------

--
-- 表的结构 `members`
--

CREATE TABLE IF NOT EXISTS `members` (
  `uid` int(11) NOT NULL AUTO_INCREMENT COMMENT 'UID',
  `username` varchar(20) NOT NULL,
  `password` char(32) NOT NULL,
  `admin` int(11) NOT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `username` (`username`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=16 ;


--
-- 表的结构 `posts`
--

CREATE TABLE IF NOT EXISTS `posts` (
  `pid` int(11) NOT NULL AUTO_INCREMENT,
  `parentid` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `postuser` varchar(20) NOT NULL,
  `content` varchar(1000) NOT NULL,
  `postdate` date NOT NULL,
  `replyuid` int(11) DEFAULT NULL,
  `replyuser` varchar(20) DEFAULT NULL,
  `replycontent` text,
  `replydate` date DEFAULT NULL,
  PRIMARY KEY (`pid`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=17 ;
