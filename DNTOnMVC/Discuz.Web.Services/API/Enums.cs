using System;
#if NET1
#else
using System.Collections.Generic;
#endif
using System.Text;

namespace Discuz.Web.Services.API
{
    public enum FormatType
    {
        XML,
        JSON
    }

    public enum ApplicationType
    {
        WEB = 1,
        DESKTOP = 2
    }

    public enum ErrorType
    {
        /*
         * 错误提示信息码(Code)的赋值规则为
         * 1-99为系统级异常信息
         * 100-999为接口权限级异常信息
         * 1000以上为任务级(Commmand)异常信息,且为了方便区分归类,
         * 1K段为Auth类
         * 2K段为Users
         * 3K段为Forums
         * 4K段为Topics
         * 5K段为Message
         * 6K段为Notification
         * 
         */

        API_EC_NONE = 0,
        #region System Error
        API_EC_UNKNOWN = 1, //未知错误,请再次提交该任务. 
        API_EC_SERVICE = 2, //服务目前不可使用.
        API_EC_METHOD = 3, //调用了不存在的方法或方法内部错误 
        API_EC_TOO_MANY_CALLS = 4, //整合程序已达到允许的最大同时请求数
        API_EC_PARAM = 5, //指定参数不存在或不是有效参数，请检查是否有必要参数未提交，或者提交的参数值不是合法的.

        #endregion

        #region Authrity Error
        API_EC_APPLICATION = 100, //所提交的api_key未关联到任何设定程序
        API_EC_BAD_IP = 101, //请求来自一个未被当前整合程序允许的远程地址
        API_EC_PERMISSION_DENIED = 102, //应用程序的请求由于权限问题被拒绝
        API_EC_CALLID = 103, //当前会话所提交的call_id没有大于前一次的call_id
        API_EC_SIGNATURE = 104, //签名(sig)参数不正确.
        API_EC_SESSIONKEY = 105, //session_key已过期或失效,请重定向让用户重新登录并获得新的session_key

        #endregion

        #region Application Error

        API_EC_REGISTER_NOT_ALLOW = 1000,//不允许注册
        API_EC_EMAIL = 1001, //Email已存在或非法
        API_EC_USER_ALREADY_EXIST = 1002,//用户名称已被注册
        API_EC_USERNAME_ILLEGAL = 1003,//用户名称不合法
        API_EC_USER_ONLINE = 1004,//当前已有用户登录,不能注册新用户
        API_EC_MORE_LOGIN_FAILED = 1005,//当前用户多次登录失败,15分钟内无法登录
        API_EC_SAME_USER_EMAIL = 1006,//多个用户使用了该登录E-mail,无法登录
        API_EC_WRONG_PASSWORD = 1007,//密码错误,登录失败
        API_EC_BANNED_USERGROUP = 1008,//用户的用户组禁止访问,登录失败

        API_EC_USER_NOT_EXIST = 2000,//当前指定的用户不存在,无法操作
        API_EC_ORI_PASSWORD_EQUAL_FALSE = 2001,//原密码输入不正确,无法修改密码

        API_EC_REWRITENAME = 3000, //版块RewriteName已存在或非法
        API_EC_FORUM_NOT_EXIST = 3001,//指定版块不存在

        API_EC_SPAM = 4001, //信息中存在非法,垃圾信息,任务被拒绝
        API_EC_TOPIC_CLOSED = 4002, //主题已关闭,无法通过API进行回复
        API_EC_TOPIC_READ_PERM = 4003, //当前用户阅读权限不足,无法查看主题或回复
        API_EC_FORUM_PASSWORD = 4004, //版块设置了密码,无法访问和操作资源
        API_EC_FORUM_PERM = 4005, //没有访问版块权限
        API_EC_REPLY_PERM = 4006, //没有回复权限
        API_EC_FRESH_USER = 4007, //见习期用户无法发帖
        API_EC_TITLE_INVALID = 4008, //标题太长或含有非法字符
        API_EC_MESSAGE_LENGTH = 4009, //内容长度不符合系统要求
        API_EC_POST_PERM = 4010, //没有发主题的权限
        API_EC_EDIT_PERM = 4011, //没有编辑的权限
        API_EC_EDIT_NOUSER = 4012,//未能成功关联到一个用户,无法编辑
        API_EC_REPOST_MESSAGE = 4013,//重复提交的信息,任务被拒绝
        API_EC_TOPIC_NOT_EXIST = 4014,//指定主题不存在
        API_EC_POST_TOOFAST = 4015,//未超出发帖时间间隔

        API_EC_PM_FROMID_NOT_EXIST = 5000,//发件人uid无效,系统中不存在指定uid的用户
        API_EC_PM_TOID_OVERFLOW = 5001,//设置了过多的收件人ID,任务被拒绝
        API_EC_PM_VISIT_TOOFAST = 5002//未超出指定调用间隔时间
        #endregion
    }
}
