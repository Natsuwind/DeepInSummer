namespace Discuz.Config
{
    /// <summary>
    ///  LLServer配置类
    /// </summary>
    public class LLServerConfigs
    {
        private static System.Timers.Timer LLServerConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static LLServerConfigInfo m_configinfo;

        static LLServerConfigs()
        {
            m_configinfo = LLServerConfigFileManager.LoadConfig();
            LLServerConfigTimer.AutoReset = true;
            LLServerConfigTimer.Enabled = true;
            LLServerConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            LLServerConfigTimer.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = LLServerConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static LLServerConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(LLServerConfigInfo configinfo)
        {
            LLServerConfigFileManager rcfm = new LLServerConfigFileManager();
            LLServerConfigFileManager.ConfigInfo = configinfo;
            return rcfm.SaveConfig();
        }
    }
}
