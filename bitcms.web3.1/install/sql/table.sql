SET FOREIGN_KEY_CHECKS = 0;

CREATE TABLE  IF NOT EXISTS `bitcms_adminmenu` (
  `adminmenuid` int(11) NOT NULL AUTO_INCREMENT COMMENT '管理菜单Id',
  `fatherid` int(11) DEFAULT NULL COMMENT '父管理菜单Id',
  `menuname` varchar(64) DEFAULT NULL COMMENT '菜单名',
  `area` varchar(128) DEFAULT NULL COMMENT 'Area区域',
  `controller` varchar(128) DEFAULT NULL COMMENT '控制器',
  `parm` varchar(64) DEFAULT NULL COMMENT '参数',
  `orderno` int(11) DEFAULT '0' COMMENT '排序',
  `roletype` varchar(64) DEFAULT '0' COMMENT '类型 0管理后台用，以后可以扩展',
  `display` tinyint(1) DEFAULT '0' COMMENT '显示',
  `explain` varchar(1024) DEFAULT NULL COMMENT '说明',
  `icon` varchar(32) DEFAULT NULL COMMENT '图标',
  PRIMARY KEY (`adminmenuid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_adminmenushortcut` (
  `adminmenuid` int(11) NOT NULL COMMENT '管理菜单Id',
  `userid` int(11) NOT NULL COMMENT '会员Id',
  `roleid` int(11) NOT NULL COMMENT '角色Id',
  PRIMARY KEY (`adminmenuid`,`userid`,`roleid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE  IF NOT EXISTS `bitcms_ads` (
  `adsid` int(11) NOT NULL AUTO_INCREMENT COMMENT '广告id',
  `adscode` varchar(64) DEFAULT NULL COMMENT '广告编码',
  `title` varchar(64) DEFAULT NULL COMMENT '广告名',
  `adstype` smallint(6) DEFAULT '0' COMMENT '类型 0文本 1图片 2falsh 3代码',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  `tags` varchar(2048) DEFAULT NULL COMMENT '标签',
  `width` varchar(32) DEFAULT '' COMMENT '宽度',
  `height` varchar(32) DEFAULT NULL COMMENT '高度',
  `introduce` text COMMENT '介绍',
  PRIMARY KEY (`adsid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_adsdetail` (
  `adsdetailid` int(11) NOT NULL AUTO_INCREMENT COMMENT '广告详情id',
  `adscode` varchar(64) DEFAULT NULL COMMENT '广告编码',
  `title` varchar(256) DEFAULT NULL COMMENT '广告标题',
  `tag` varchar(128) DEFAULT NULL COMMENT '标签',
  `width` varchar(32) DEFAULT '' COMMENT '宽度',
  `height` varchar(32) DEFAULT NULL COMMENT '高度',
  `adstype` smallint(6) DEFAULT '0' COMMENT '类型 0文本 1图片 2falsh 3代码',
  `link` varchar(256) DEFAULT NULL COMMENT '广告地址',
  `description` varchar(1024) DEFAULT NULL COMMENT '描述',
  `enabled` tinyint(4) DEFAULT '0' COMMENT '状态',
  `orderno` int(11) DEFAULT '0' COMMENT '排序',
  `shownum` bigint(20) DEFAULT NULL COMMENT '展示次数',
  `hitsnum` bigint(20) DEFAULT NULL COMMENT '点击次数',
  `path` varchar(256) DEFAULT NULL COMMENT '附件',
  `startdate` datetime DEFAULT NULL COMMENT '开始时间',
  `enddate` datetime DEFAULT NULL COMMENT '结束时间',
  `showdays` int(11) DEFAULT NULL COMMENT '时间长',
  PRIMARY KEY (`adsdetailid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_city` (
  `cityid` int(11) NOT NULL,
  `fathercityid` int(16) DEFAULT '0',
  `cityname` varchar(32) DEFAULT NULL,
  `orderno` int(11) DEFAULT '200',
  `postcode` varchar(16) DEFAULT NULL,
  `deep` int(11) DEFAULT '0',
  `show` smallint(6) DEFAULT '0' COMMENT '推荐圈子',
  `diliquhua` varchar(32) DEFAULT NULL COMMENT '地理区划',
  PRIMARY KEY (`cityid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_detail` (
  `detailid` int(11) NOT NULL AUTO_INCREMENT COMMENT '内容id',
  `itemid` int(11) DEFAULT '0' COMMENT '栏目id',
  `items` varchar(512) DEFAULT NULL,
  `userid` int(11) DEFAULT '0' COMMENT '会员Id',
  `channelcode` varchar(32) DEFAULT NULL COMMENT '频道编码',
  `detailcode` varchar(32) DEFAULT NULL COMMENT '编码',
  `title` varchar(256) DEFAULT NULL COMMENT '标题',
  `subtitle` varchar(256) DEFAULT NULL COMMENT '副标题',
  `display` tinyint(1) DEFAULT '0' COMMENT '展示',
  `displaytime` datetime DEFAULT NULL,
  `show` int(11) DEFAULT '0' COMMENT '推荐',
  `resume` varchar(512) DEFAULT NULL COMMENT '概要介绍',
  `keyword` varchar(512) DEFAULT NULL,
  `searchkey` varchar(2048) DEFAULT NULL COMMENT '查找关键字',
  `galleryid` int(11) DEFAULT '0' COMMENT '缩略图',
  `closereview` tinyint(4) DEFAULT '0' COMMENT '关闭评论',
  `readpower` int(11) DEFAULT '0' COMMENT '阅读权限值',
  `enabledlink` tinyint(1) DEFAULT '0' COMMENT '启用外链',
  `link` varchar(128) DEFAULT NULL COMMENT '链接地址',
  `againstnum` int(11) DEFAULT '0' COMMENT '反对数',
  `agreenum` int(11) DEFAULT '0' COMMENT '点赞数',
  `follownum` int(11) DEFAULT '0' COMMENT '收藏数',
  `hitsnum` int(11) DEFAULT '0' COMMENT '查看次数',
  `reviewnum` int(11) DEFAULT '0' COMMENT '回复次数',
  `recycle` tinyint(4) DEFAULT '0',
  `recycledate` datetime DEFAULT NULL COMMENT '删除时间',
  `author` varchar(64) DEFAULT NULL COMMENT '作者',
  `source` varchar(64) DEFAULT NULL COMMENT '出处',
  `indate` datetime COMMENT '添加时间',
  `gallerystatistics` varchar(512) DEFAULT '' COMMENT '图库统计',
  `stars` decimal(18,1) DEFAULT NULL COMMENT '评星',
  PRIMARY KEY (`detailid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_detailchannel` (
  `channelcode` varchar(32) NOT NULL COMMENT '频道编码',
  `channelname` varchar(64) DEFAULT NULL COMMENT '频道名',
  `path` varchar(128) DEFAULT NULL COMMENT '前台路径',
  `detailpath` varchar(128) DEFAULT NULL COMMENT '页面路径模板',
  `enabled` tinyint(1) DEFAULT '0' COMMENT '启用',
  `enableddetailcode` tinyint(1) DEFAULT '0',
  `enabledreadpower` tinyint(1) DEFAULT '0' COMMENT '启用阅读权限',
  `itemdeep` int(11) DEFAULT '1' COMMENT '栏目深度',
  `galleryset` smallint(6) DEFAULT '0' COMMENT '图库设置',
  `enabledpaging` tinyint(1) DEFAULT '0' COMMENT '启用内容分页',
  `shows` varchar(512) DEFAULT NULL COMMENT '内容推荐设置',
  `gallerytags` varchar(2018) DEFAULT NULL COMMENT '图库标签',
  `orderno` int(11) DEFAULT '200' COMMENT '排序',
  `coverset` smallint(6) DEFAULT '0' COMMENT '封面设置',
  `audioset` smallint(6) DEFAULT '0' COMMENT '音频设置',
  `videoset` smallint(6) DEFAULT '0' COMMENT '视频设置',
  `attachmentset` smallint(6) DEFAULT '0' COMMENT '附件设置',
  `width` int(11) DEFAULT '0' COMMENT '宽度',
  `height` int(11) DEFAULT '0' COMMENT '高度',
  `keywords` varchar(512) DEFAULT NULL COMMENT '栏目关键字',
  `resume` varchar(1024) DEFAULT NULL COMMENT '概要介绍',
  PRIMARY KEY (`channelcode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_detailcontent` (
  `contentid` int(11) NOT NULL AUTO_INCREMENT COMMENT '内容详情id',
  `detailid` int(11) DEFAULT NULL COMMENT '内容id',
  `title` varchar(255) DEFAULT NULL COMMENT '标题',
  `content` text COMMENT '内容',
  `orderno` int(11) DEFAULT '0' COMMENT '排序',
  `itemid` int(11) DEFAULT '0' COMMENT '栏目id',
  `channelcode` varchar(32) DEFAULT NULL COMMENT '频道编码',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`contentid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_detailgallery` (
  `galleryid` int(11) NOT NULL AUTO_INCREMENT COMMENT '图库id',
  `detailid` int(11) DEFAULT NULL COMMENT '内容id',
  `title` varchar(256) DEFAULT NULL COMMENT '标题',
  `readpower` int(11) DEFAULT '-1' COMMENT '阅读权限值',
  `agreenum` int(11) DEFAULT '0' COMMENT '点赞数',
  `follownum` int(11) DEFAULT '0' COMMENT '收藏数',
  `hitsnum` int(11) DEFAULT '0' COMMENT '查看数',
  `gallerytype` smallint(1) DEFAULT '0' COMMENT '类型 0 图片1音频 2视频 3附件',
  `cover` tinyint(1) DEFAULT '0' COMMENT '封面',
  `path` varchar(128) DEFAULT NULL COMMENT '地址',
  `resume` varchar(2048) DEFAULT NULL COMMENT '概要介绍',
  `tag` varchar(32) DEFAULT NULL COMMENT '图库标签',
  `orderno` int(11) DEFAULT '200' COMMENT '排序',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`galleryid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_detailhits` (
  `detailhitscode` varchar(32) NOT NULL COMMENT '点击编码',
  `useronlineid` varchar(32) DEFAULT NULL COMMENT '会员在线id',
  `detailid` int(11) NOT NULL COMMENT '内容id',
  `userid` int(11) NOT NULL COMMENT '会员Id',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`detailhitscode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_dictionary` (
  `dictionaryid` int(11) NOT NULL AUTO_INCREMENT COMMENT '字典id',
  `dictionarycode` varchar(32) NOT NULL COMMENT '编码',
  `key` varchar(64) DEFAULT NULL COMMENT 'Key1',
  `key2` varchar(64) DEFAULT NULL COMMENT 'Key1',
  `Key2type` varchar(64) DEFAULT NULL COMMENT 'Key2type',
  `key3` varchar(64) DEFAULT NULL COMMENT 'Key1',
  `Key3type` varchar(64) DEFAULT NULL COMMENT 'Key3type',
  `key4` varchar(64) DEFAULT NULL COMMENT 'Key1',
  `Key4type` varchar(64) DEFAULT NULL COMMENT 'Key4type',
  `name` varchar(64) DEFAULT NULL COMMENT '名称',
  `explain` varchar(2048) DEFAULT NULL COMMENT '说明',
  PRIMARY KEY (`dictionaryid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_dictionarykey` (
  `keyid` int(11) NOT NULL AUTO_INCREMENT COMMENT 'KeyId',
  `dictionaryid` int(11) DEFAULT NULL COMMENT '字典id',
  `dictionarycode` varchar(32) DEFAULT NULL COMMENT '编码',
  `title` varchar(64) DEFAULT NULL COMMENT '标题',
  `value` varchar(64) DEFAULT NULL COMMENT '值',
  `value2` varchar(128) DEFAULT NULL COMMENT '值',
  `value3` varchar(128) DEFAULT NULL COMMENT '值',
  `value4` varchar(128) DEFAULT NULL COMMENT '值',
  `orderno` int(11) DEFAULT '200' COMMENT '排序',
  `explain` varchar(2048) DEFAULT NULL COMMENT '说明',
  `systemsetting` bigint(255) DEFAULT '0' COMMENT '系统设置',
  PRIMARY KEY (`keyid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_favorites` (
  `favoritesid` int(11) NOT NULL AUTO_INCREMENT COMMENT '收藏夹id',
  `userid` int(11) DEFAULT NULL COMMENT '会员Id',
  `favoritescode` varchar(32) DEFAULT NULL COMMENT '收藏夹编码',
  `targetid` int(11) DEFAULT '0' COMMENT '收藏目标id',
  `title` varchar(128) DEFAULT NULL COMMENT '收藏标题',
  `pic` varchar(256) DEFAULT NULL COMMENT '缩略图',
  `link` varchar(256) DEFAULT NULL COMMENT '地址',
  `describe` text COMMENT '描述',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`favoritesid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_follow` (
  `userid` int(11) NOT NULL COMMENT '会员Id',
  `followuserid` int(11) NOT NULL COMMENT '被关注者',
  `mutual` tinyint(11) DEFAULT '0',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`userid`,`followuserid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_item` (
  `itemid` int(11) NOT NULL AUTO_INCREMENT COMMENT '栏目id',
  `fatherid` int(11) DEFAULT NULL COMMENT '父栏目id',
  `channelcode` varchar(32) DEFAULT NULL COMMENT '频道编码',
  `itemcode` varchar(32) DEFAULT NULL COMMENT '栏目编码',
  `show` int(11) DEFAULT '0' COMMENT '推荐',
  `itemname` varchar(64) DEFAULT NULL COMMENT '栏目名',
  `orderno` int(11) DEFAULT '200' COMMENT '排序',
  `explain` text COMMENT '分类说明',
  `readpower` int(11) DEFAULT NULL COMMENT '阅读权限值',
  `icon` varchar(256) DEFAULT NULL COMMENT '图标',
  `keywords` varchar(512) DEFAULT NULL COMMENT '栏目关键字',
  `resume` varchar(1024) DEFAULT NULL COMMENT '概要介绍',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`itemid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_maproute` (
  `maprouteid` int(11) NOT NULL AUTO_INCREMENT COMMENT '规则Id',
  `fatherid` int(11) DEFAULT '0' COMMENT '父规则Id',
  `maproutename` varchar(64) DEFAULT NULL COMMENT '规则名',
  `lookfor` varchar(128) DEFAULT NULL COMMENT '重写地址',
  `sendto` varchar(128) DEFAULT NULL COMMENT '转向地址',
  `template` varchar(128) DEFAULT NULL COMMENT '模板',
  `cachetime` int(11) DEFAULT NULL COMMENT '缓存时间（分钟）',
  `enabled` tinyint(4) DEFAULT '0' COMMENT '启用',
  PRIMARY KEY (`maprouteid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_module` (
  `moduleid` int(11) NOT NULL AUTO_INCREMENT COMMENT '模块id',
  `modulename` varchar(64) DEFAULT NULL COMMENT '模块名',
  `moduletype` smallint(255) DEFAULT '0' COMMENT '模块类型',
  `modulecode` varchar(32) DEFAULT NULL COMMENT '编码',
  `appid` varchar(64) DEFAULT NULL COMMENT 'AppId',
  `appsecret` varchar(64) DEFAULT NULL COMMENT 'AppSecret',
  `enabled` smallint(6) DEFAULT '0' COMMENT '启用',
  `extend1` varchar(64) DEFAULT NULL COMMENT '扩展字段1',
  `extend2` varchar(64) DEFAULT NULL COMMENT '扩展字段2',
  `extend3` varchar(64) DEFAULT NULL COMMENT '扩展字段3',
  `extend4` varchar(64) DEFAULT NULL COMMENT '扩展字段4',
  `explain` text COMMENT '说明',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  `extendname` varchar(128) DEFAULT NULL COMMENT '扩展字段名，多个名称使用;分隔',
  `timestampexpired` int(11) DEFAULT '0' COMMENT '时间戳过期时间',
  PRIMARY KEY (`moduleid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_referee` (
  `userid` int(11) NOT NULL COMMENT '会员Id',
  `refereeid` int(11) NOT NULL DEFAULT '0' COMMENT '推荐会员',
  `referee` varchar(64) DEFAULT NULL COMMENT '推荐人',
  `indate` datetime DEFAULT NULL COMMENT '推荐时间',
  PRIMARY KEY (`userid`,`refereeid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_review` (
  `reviewid` int(11) NOT NULL AUTO_INCREMENT COMMENT '评论id',
  `userid` int(11) DEFAULT '0' COMMENT '会员Id',
  `detailid` int(11) DEFAULT '0' COMMENT '内容id',
  `channelcode` varchar(32) DEFAULT NULL COMMENT '频道编码',
  `replyid` int(11) DEFAULT '0' COMMENT '评论id',
  `title` varchar(256) DEFAULT NULL COMMENT '评语日期',
  `content` text COMMENT '评论内容',
  `show` int(11) DEFAULT '0' COMMENT '推荐',
  `verify` tinyint(4) DEFAULT '0' COMMENT '审核',
  `secrecy` tinyint(4) DEFAULT '0' COMMENT '保密评论',
  `replynum` int(11) DEFAULT '0' COMMENT '回复数',
  `hitsnum` int(11) DEFAULT '0' COMMENT '查看数',
  `againstnum` int(11) DEFAULT '0' COMMENT '反对数',
  `agreenum` int(11) DEFAULT '0' COMMENT '赞同数',
  `indate` varchar(32) DEFAULT NULL COMMENT '评语日期',
  PRIMARY KEY (`reviewid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_role` (
  `roleid` int(11) NOT NULL AUTO_INCREMENT COMMENT '角色Id',
  `rolename` varchar(32) DEFAULT NULL COMMENT '角色名',
  `roletype` varchar(64) DEFAULT '' COMMENT '角色类型',
  `readpower` smallint(4) DEFAULT '0' COMMENT '读权限',
  `lock` tinyint(4) DEFAULT '0' COMMENT '锁定',
  `minscore` int(11) DEFAULT '0' COMMENT '最大积分',
  `icon` varchar(255) DEFAULT NULL COMMENT '图标',
  `orderno` int(11) DEFAULT '200' COMMENT '排序',
  `resume` text COMMENT '介绍',
  PRIMARY KEY (`roleid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_rolepower` (
  `roleid` int(11) NOT NULL COMMENT '角色Id',
  `adminmenuid` int(11) NOT NULL COMMENT '管理菜单Id',
  `read` tinyint(4) DEFAULT '0' COMMENT '读权限',
  `edit` tinyint(4) DEFAULT '0' COMMENT '写权限',
  PRIMARY KEY (`roleid`,`adminmenuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_scoreevent` (
  `eventid` int(11) NOT NULL AUTO_INCREMENT COMMENT '事件id',
  `eventcode` varchar(64) DEFAULT NULL COMMENT '事件编码',
  `eventname` varchar(64) DEFAULT NULL COMMENT '事件名',
  `enabled` smallint(6) DEFAULT '1' COMMENT '启用',
  `score` int(11) DEFAULT NULL COMMENT '积分值',
  `direction` smallint(6) DEFAULT NULL COMMENT '方向 0加 1减',
  `eventtype` smallint(6) DEFAULT NULL COMMENT '事件类型 0系统事件 1人工操作',
  `explan` varchar(512) DEFAULT NULL COMMENT '说明',
  `times` int(11) DEFAULT NULL COMMENT '次数',
  `period` int(11) DEFAULT NULL COMMENT '次数',
  PRIMARY KEY (`eventid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_scorelog` (
  `logid` int(11) NOT NULL AUTO_INCREMENT COMMENT '记录id',
  `eventid` int(11) DEFAULT NULL COMMENT '事件id',
  `UserId` int(11) DEFAULT NULL COMMENT '会员Id',
  `score` int(11) DEFAULT '0' COMMENT '积分值 ',
  `Incomeorexpenses` smallint(6) DEFAULT '0' COMMENT '收入或支出 0收入 1支出',
  `reason` varchar(512) DEFAULT NULL COMMENT '理由',
  `indate` datetime DEFAULT NULL COMMENT '时间',
  PRIMARY KEY (`logid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_user` (
  `userid` int(11) NOT NULL AUTO_INCREMENT COMMENT '会员Id',
  `username` varchar(64) DEFAULT NULL COMMENT '会员名',
  `roleid` int(255) DEFAULT '0',
  `birthday` varchar(32) DEFAULT NULL COMMENT '生日',
  `gender` smallint(6) DEFAULT '0',
  `avatar` varchar(128) DEFAULT NULL COMMENT '头像',
  `location` varchar(256) DEFAULT NULL COMMENT '详细地址',
  `cityid` int(11) DEFAULT '0' COMMENT '城市id',
  `city` varchar(128) DEFAULT NULL COMMENT '城市',
  `lockuser` tinyint(4) DEFAULT '0' COMMENT '锁定会员',
  `score` int(11) DEFAULT '0' COMMENT '积分',
  `ip` varchar(32) DEFAULT NULL COMMENT '注册IP',
  `indate` datetime DEFAULT NULL COMMENT '注册时间',
  `introduce` text COMMENT '个人介绍',
  `lastLanddate` datetime DEFAULT NULL COMMENT '最后登陆时间',
  `verifymember` tinyint(4) DEFAULT '0' COMMENT '待审核会员',
  `comefrom` varchar(64) DEFAULT NULL COMMENT '来自',
  `show` int(11) DEFAULT '0' COMMENT '推荐',
  `deadline` datetime DEFAULT NULL COMMENT '过期时间',
  `follownum` int(11) DEFAULT '0' COMMENT '关注数',
  PRIMARY KEY (`userid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_userbind` (
  `userbindid` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT '-1' COMMENT '会员Id',
  `modulecode` varchar(64) NOT NULL COMMENT '模块编码',
  `nickname` varchar(64) DEFAULT NULL COMMENT '会员昵称',
  `usercode` varchar(64) DEFAULT NULL COMMENT '绑定会员编码',
  `verify` tinyint(4) DEFAULT '0' COMMENT '审核',
  `indate` datetime DEFAULT NULL COMMENT '注册时间',
  `verifycode` varchar(32) DEFAULT NULL COMMENT '验证码',
  `extend1` varchar(64) DEFAULT NULL COMMENT '扩展字段1',
  `extend2` varchar(64) DEFAULT NULL COMMENT '扩展字段2',
  `extend3` varchar(64) DEFAULT NULL COMMENT '扩展字段3',
  `deadline` datetime DEFAULT NULL COMMENT '过期时间',
  PRIMARY KEY (`userbindid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_userpassword` (
  `userid` int(11) NOT NULL COMMENT '会员Id',
  `password` char(32) DEFAULT NULL COMMENT '密码',
  `passwordtype` tinyint(1) NOT NULL DEFAULT '0' COMMENT '管理员密码',
  `lastdate` datetime DEFAULT NULL COMMENT '最后修改时间',
  PRIMARY KEY (`userid`,`passwordtype`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE  IF NOT EXISTS `bitcms_viewpoint` (
  `userid` int(11) NOT NULL COMMENT '会员Id',
  `detailid` int(11) NOT NULL COMMENT '内容id',
  `reviewid` int(11) NOT NULL COMMENT '评论id',
  `against` tinyint(4) DEFAULT '0' COMMENT '反对',
  `agree` tinyint(4) DEFAULT '0' COMMENT '赞同',
  `indate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`userid`,`detailid`,`reviewid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

SET FOREIGN_KEY_CHECKS = 1;
