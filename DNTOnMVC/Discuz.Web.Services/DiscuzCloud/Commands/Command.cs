using System;
using System.Collections.Generic;
using System.Reflection;

namespace Discuz.Web.Services.DiscuzCloud.Commands
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

        public Command(string commandName)
        {
            _name = commandName;
        }

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
                Command command = FindCommand(commandParam.Method);
                if (command != null)
                {
                    command.Run(commandParam, ref result);
                }
            }
            catch
            {
            }
            return result;
        }
    }
}
