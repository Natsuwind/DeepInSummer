using System;
using System.Collections.Generic;
using System.Reflection;


namespace Discuz.Web.Services.API.Commands
{
    public class Command
    {
        private string _name = "";
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private bool _callIdCheck = true;
        /// <summary>
        /// 任务是否校验请求的CallId
        /// </summary>
        public bool CallIdCheck
        {
            get { return _callIdCheck; }
        }

        public Command(string commandName, bool callIdCheck)
        {
            _name = commandName;
            _callIdCheck = callIdCheck;
        }

        public Command(string commandName)
        {
            _name = commandName;
        }

        /// <summary>
        /// 上一次请求的CallId
        /// </summary>
        public long LastCallId = -1;
        /// <summary>
        /// 任务对象资源锁
        /// </summary>
        public object lockHelper = new object();

        public virtual bool Run(CommandParameter commandParam, ref string result) { return true; }
    }

    public class CommandManager
    {
        static Dictionary<string, Command> _commandsDic = new Dictionary<string, Command>();

        /// <summary>
        /// 首次开启WebService时自动加载任务类对象到任务字典中
        /// </summary>
        static CommandManager()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type t in types)
            {
                if (t.IsSubclassOf(typeof(Command)))
                    AddCommand((Command)Activator.CreateInstance(t));
            }
        }


        private static void AddCommand(Command command)
        {
            if (command != null && !_commandsDic.ContainsKey(command.Name))
                _commandsDic.Add(command.Name, command);
        }


        private static Command FindCommand(string commandName)
        {
            Command com = null;
            if (_commandsDic != null && _commandsDic.Count > 0)
                _commandsDic.TryGetValue(commandName, out com);

            return com;
        }

        public static string Run(CommandParameter commandParam)
        {
            string result = "";
            try
            {
                Command command = FindCommand(commandParam.CommandName);

                if (command != null)
                {
                    lock (command.lockHelper)
                    {
                        #region 任务的通用校验
                        //如果任务需要校验callId
                        if (command.CallIdCheck)
                        {
                            //如果CallId为不合法的值,或者当前CallId代表的时间是上次CallId的10秒(估算值)以前,表示该CallId已经超过时限
                            if (commandParam.CallId <= 0 || commandParam.CallId <= command.LastCallId - GetDelayValue(command.LastCallId.ToString().Length, 10))
                                return Util.CreateErrorMessage(ErrorType.API_EC_CALLID, commandParam.ParamList);

                            command.LastCallId = commandParam.CallId;
                        }
                        #endregion
                        command.Run(commandParam, ref result);
                    }
                }
                else
                    result = Util.CreateErrorMessage(ErrorType.API_EC_METHOD, commandParam.ParamList);
            }
            catch
            {
                result = Util.CreateErrorMessage(ErrorType.API_EC_UNKNOWN, commandParam.ParamList);
            }
            return result;
        }

        /// <summary>
        /// 获取CallId的延时系数
        /// </summary>
        /// <param name="callIdLength"></param>
        /// <param name="baseDelayValue"></param>
        /// <returns></returns>
        private static long GetDelayValue(int callIdLength, int baseDelayValue)
        {
            //若Callid位长过短,则没有延时系数
            if (callIdLength < 3)
                return 0;

            long returnValue = 1;
            //延时系数的位数约是CallId长度的一小半
            for (int i = 0; i < (callIdLength - 1) / 2; i++)
            {
                returnValue = returnValue * 10;
            }
            //乘以延时基数
            returnValue = returnValue * baseDelayValue;
            //如果Callid长度为奇数,则再加上一半的延时系数
            returnValue = callIdLength % 2 == 0 ? returnValue : returnValue + returnValue / 2;
            return returnValue;
        }
    }
}
