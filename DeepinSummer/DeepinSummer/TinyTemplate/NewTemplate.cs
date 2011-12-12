/*****************
// NewTemplate.cs
// Edit by wysky
 *****************/

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace Natsuhime
{
    public class NewTemplate
    {
        static Regex[] r = new Regex[25];
        static Dictionary<string, string> reftemplatecache;
        static NewTemplate()
        {
            RegexOptions options = RegexOptions.Compiled;

            //解析模板引用
            r[0] = new Regex(@"<%template ([^\[\]\{\}\s]+)%>", options);

            r[1] = new Regex(@"<%loop ((\(([a-zA-Z\<\>\,]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);

            r[2] = new Regex(@"<%\/loop%>", options);

            r[3] = new Regex(@"<%while ([^\[\]\{\}\s]+)%>", options);

            r[4] = new Regex(@"<%\/while ([^\[\]\{\}\s]+)%>", options);

            r[5] = new Regex(@"<%if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?)(?:\s*)%>", options);
            //r[5] = new Regex(@"<%if ([^\s]+)%>", options);

            r[6] = new Regex(@"<%else(( (?:\s*)if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?))?)(?:\s*)%>", options);

            r[7] = new Regex(@"<%\/if%>", options);

            //解析{var.a}
            r[8] = new Regex(@"(\{strtoint\(([^\s]+?)\)\})", options);

            //解析{request[a]}
            r[9] = new Regex(@"(<%urlencode\(([^\s]+?)\)%>)", options);

            //解析{var[a]}
            r[10] = new Regex(@"(<%datetostr\(([^\s]+?),(.*?)\)%>)", options);
            r[11] = new Regex(@"(\{([^\.\[\]\{\}\s]+)\.([^\[\]\{\}\s]+)\})", options);

            //解析普通变量{}
            r[12] = new Regex(@"(\{request\[([^\[\]\{\}\s]+)\]\})", options);

            //解析==表达式
            r[13] = new Regex(@"(\{([^\[\]\{\}\s]+)\[([^\[\]\{\}\s]+)\]\})", options);

            //解析==表达式
            r[14] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

            //解析普通变量{}
            r[15] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

            //解析==表达式
            r[16] = new Regex(@"(([=|>|<|!]=)\\" + "\"" + @"([^\s]*)\\" + "\")", options);

            //命名空间
            r[17] = new Regex(@"<%namespace (?:""?)([\s\S]+?)(?:""?)%>", options);

            //C#代码
            r[18] = new Regex(@"<%csharp%>([\s\S]+?)<%/csharp%>", options);

            //set标签
            r[19] = new Regex(@"<%set ((\(([a-zA-Z]+)\))?)(?:\s*)\{([^\s]+)\}(?:\s*)=(?:\s*)(.*?)(?:\s*)%>", options);

            //截取字符串
            r[20] = new Regex(@"(<%getsubstring\(([^\s]+?),(.\d*?),([^\s]+?)\)%>)", options);

            //repeat标签
            r[21] = new Regex(@"<%repeat\(([^\s]+?)(?:\s*),(?:\s*)([^\s]+?)\)%>", options);

            //继承类Inherits
            r[22] = new Regex(@"<%inherits (?:""?)([\s\S]+?)(?:""?)%>", options);

            r[23] = new Regex(@"<%continue%>");
            r[24] = new Regex(@"<%break%>");
        }
        string InheritsPrefix_Default;
        string Inherits_Default;
        string Import_Default;

        #region 属性
        public string Productversion { get; set; }
        public string Productname { get; set; }
        #endregion
        public NewTemplate(string defaultinheritsprefix, string defaultimport)
        {
            this.InheritsPrefix_Default = defaultinheritsprefix.Trim();
            this.Import_Default = defaultimport.Trim();
        }
        public void CreateFromFolder(string templatefilefolder, string pagefilefolder)
        {
            Create(templatefilefolder, pagefilefolder, 1);
        }
        public void CreateFromFileList(List<KeyValuePair<string, string>> templateFileList)
        {
            Create(templateFileList, 1);
        }

        #region 入口

        /// <summary>
        /// 通过模板的目录生成目标页面(只支持layer层级嵌套为1)
        /// </summary>
        /// <param name="templatefilepath"></param>
        /// <param name="pagefilepath"></param>
        /// <param name="layer"></param>
        void Create(string templatefilepath, string pagefilepath, int layer)
        {
            //取得目录下的文件列表
            string[] templatefilelist = Directory.GetFiles(templatefilepath, "*.htm");
            System.Diagnostics.Debug.WriteLine(string.Format("目录:{0}下的文件列表载入完毕.", templatefilepath));

            //非引用模板列表
            List<string> maintemplatefilelist = new List<string>();
            //_开头的被引用模板内容缓存.转换后存入缓存._开头的模板是最深模板,里面不能再引用别人.
            reftemplatecache = new Dictionary<string, string>();

            foreach (string file in templatefilelist)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                if (filename.StartsWith("_"))
                {
                    LoadRefTemplateCache(file);
                }
                else
                {
                    maintemplatefilelist.Add(file);
                    System.Diagnostics.Debug.WriteLine(string.Format("主模板:{0}已载入列表.", file));
                }
            }

            //读取主模板,匹配后生成.
            foreach (string file in maintemplatefilelist)
            {
                this.Inherits_Default = this.InheritsPrefix_Default + "." + Path.GetFileNameWithoutExtension(file);
                string result = CreatMainTemplate(file);
                string aspxfilepath = Path.Combine(pagefilepath, Path.GetFileNameWithoutExtension(file) + ".aspx");
                //using (StreamWriter sw = new StreamWriter(aspxfilepath, false, Encoding.UTF8))
                //{
                //    sw.Write(result);
                //}
                File.WriteAllText(
                    aspxfilepath,
                    result,
                    new UTF8Encoding(true, true)
                    );
            }
            System.Diagnostics.Debug.WriteLine("生成完毕!");
        }

        /// <summary>
        /// 通过模板的文件列表生成页面(只支持layer层级嵌套为1)
        /// </summary>
        /// <param name="templateFileList"></param>
        /// <param name="layer"></param>
        void Create(List<KeyValuePair<string, string>> templateFileList, int layer)
        {
            //非引用模板列表
            List<KeyValuePair<string, string>> maintemplatefilelist = new List<KeyValuePair<string, string>>();
            //_开头的被引用模板内容缓存.转换后存入缓存._开头的模板是最深模板,里面不能再引用别人.
            reftemplatecache = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> file in templateFileList)
            {
                string filename = Path.GetFileNameWithoutExtension(file.Key);
                if (filename.StartsWith("_"))
                {
                    LoadRefTemplateCache(file.Key);
                }
                else
                {
                    maintemplatefilelist.Add(file);
                    System.Diagnostics.Debug.WriteLine(string.Format("主模板:{0}已载入列表.", file.Key));
                }
            }

            //读取主模板,匹配后生成.
            foreach (KeyValuePair<string, string> file in maintemplatefilelist)
            {
                this.Inherits_Default = this.InheritsPrefix_Default + "." + Path.GetFileNameWithoutExtension(file.Key);
                string result = CreatMainTemplate(file.Key);
                string aspxfilepath = Path.Combine(file.Value, Path.GetFileNameWithoutExtension(file.Key) + ".aspx");

                File.WriteAllText(
                    aspxfilepath,
                    result,
                    new UTF8Encoding(true, true)
                    );
            }
            System.Diagnostics.Debug.WriteLine("生成完毕!");
        }
        /// <summary>
        /// 载入子模板进入缓存的方法
        /// </summary>
        /// <param name="filename"></param>
        void LoadRefTemplateCache(string filepath)
        {
            string filename = Path.GetFileNameWithoutExtension(filepath);
            if (!reftemplatecache.ContainsKey(filename))
            {
                if (File.Exists(filepath))
                {
                    string content = GetRefTemplate(filepath);
                    reftemplatecache.Add(filename, content);
                    System.Diagnostics.Debug.WriteLine(string.Format("引用模板:{0}载入缓存完毕.", filename));
                }
                else
                {
                    throw new Exception(string.Format("Could NOT Find SubTemplate:{0} At Path:{1}.Please Check File", filepath, filepath));
                }
            }
        }
        string GetRefTemplate(string filepath)
        {
            //读取文件得到string
            string source = File.ReadAllText(filepath, Encoding.UTF8);
            //匹配得到结果
            return ConvertTags(source, 0);
        }
        string CreatMainTemplate(string filepath)
        {
            //读取文件得到string
            string source = File.ReadAllText(filepath, Encoding.UTF8);
            //匹配得到结果
            return ConvertTags(source, 1);
        }

        #endregion

        /// <summary>
        /// 匹配标签
        /// </summary>
        /// <param name="source"></param>
        /// <param name="layer">如果为0表示为引用模板,不会匹配ASP.NET页头和template子模板</param>
        /// <returns></returns>
        public string ConvertTags(string SourceHtml, int layer)
        {
            StringBuilder source = new StringBuilder();
            StringBuilder SourceText = new StringBuilder(SourceHtml);

            if (layer == 1)
            {
                //inherits
                string inherits = "";
                foreach (Match m in r[22].Matches(SourceText.ToString()))
                {
                    inherits = m.Groups[1].ToString();
                    SourceText.Replace(m.Groups[0].ToString(), string.Empty);
                    break;
                }
                if (inherits == "")
                {
                    inherits = this.Inherits_Default;
                }
                source.Append(
                    string.Format(
                    "<%@ Page language=\"c#\" AutoEventWireup=\"false\" EnableViewState=\"false\" Inherits=\"{0}\" %>",
                    inherits
                    )
                    );

                //命名空间
                string extNamespace;
                foreach (Match m in r[17].Matches(SourceText.ToString()))
                {
                    extNamespace = string.Format("\r\n<%@ Import namespace=\"{0}\" %>", m.Groups[1].ToString());
                    SourceText.Replace(m.Groups[0].ToString(), string.Empty);
                    source.Append(extNamespace);
                }
                if (this.Import_Default != string.Empty)
                {
                    string[] importnamespace = this.Import_Default.Split(',');
                    foreach (string names in importnamespace)
                    {
                        source.Append(string.Format("\r\n<%@ Import namespace=\"{0}\" %>", names));
                    }
                }

                string currentfile = inherits.Substring(inherits.LastIndexOf('.') + 1) + ".htm";
                //页面混合script入口
                source.Append("\r\n<script runat=\"server\">\r\noverride protected void OnInit(EventArgs e)\r\n{\r\n\t/*\r\n\tThis is a cached-file of template(\"\\templates\\templatename\\" + currentfile + "\"), it was created by LiteCMS.CN Template Engine.\r\n\tPlease do NOT edit it.\r\n\t此文件为模板文件的缓存(\"\\templates\\模板名\\" + currentfile + "\"),由 LiteCMS.CN 模板引擎生成.\r\n\t所以请不要编辑此文件.\r\n\t*/\r\n\tbase.OnInit(e);\r\n");
            }

            //处理Csharp语句
            //foreach (Match m in r[18].Matches(source.ToString()))
            //{
            //    SourceText.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("\r\n", "\r\r\t"));
            //}

            SourceText.Replace("\r\n", "\r\r\r");
            SourceText.Replace("\n", "\r\r\r");
            SourceText.Replace("\r", "\r\r\r");
            SourceText.Replace("<%", "\r\r\n<%");
            SourceText.Replace("%>", "%>\r\r\n");

            SourceText.Replace("<%csharp%>\r\r\n", "<%csharp%>").Replace("\r\r\n<%/csharp%>", "<%/csharp%>");

            string[] sourceline = SplitString(SourceText.ToString(), "\r\r\n");
            //int count = strlist.GetUpperBound(0);

            //for (int i = 0; i <= count; i++)
            //{
            //strReturn.Append(ConvertTags(nest, AspxfilePath, strlist[i]));
            //}
            foreach (string line in sourceline)
            {
                source.Append(ConvertTags(line));
            }


            if (layer == 1)
            {
                source.Append("\r\n\tResponse.Write(templateBuilder.ToString());");
                source.Append("\r\n}\r\n</script>");
            }

            return source.ToString();
        }

        private string ConvertTags(string SourceText)
        {
            bool iscodeline = false;
            StringBuilder source = new StringBuilder(SourceText);
            source.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("</script>", "</\"+ \"script>");

            //子模板
            foreach (Match m in r[0].Matches(source.ToString()))
            {
                //TODO 子模板载入问题,现在的子模板无法嵌套,因为有的子模板载入缓存顺序不一致.//暂时失效,因为现在是传入filepath而不是filename了
                string subtemplatename = m.Groups[1].ToString();
                if (!reftemplatecache.ContainsKey(subtemplatename))
                {
                    LoadRefTemplateCache(subtemplatename);
                    //throw new Exception(string.Format("Could NOT Find SubTemplate{0}.Please Check File", subtemplatename));

                }
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format("\r\n{0}\r\n", reftemplatecache[subtemplatename])
                    );
            }
            //<loop>
            foreach (Match m in r[1].Matches(source.ToString()))
            {
                iscodeline = true;
                if (m.Groups[3].ToString() == "")
                {
                    source.Replace(
                        m.Groups[0].ToString(),
                        string.Format(
                        "\r\n\tint {0}__loop__id=0;\r\n\tforeach(DataRow {0} in {1}.Rows)\r\n\t{{\r\n\t\t{0}__loop__id++;\r\n",
                        m.Groups[4].ToString(),
                        m.Groups[5].ToString()
                        )
                        );
                }
                else
                {
                    source.Replace(
                        m.Groups[0].ToString(),
                        string.Format(
                        "\r\n\tint {1}__loop__id=0;\r\n\tforeach({0} {1} in {2})\r\n\t{{\r\n\t\t{1}__loop__id++;\r\n",
                        m.Groups[3].ToString(),
                        m.Groups[4].ToString(),
                        m.Groups[5].ToString()
                        )
                        );
                }
            }
            //</loop>
            foreach (Match m in r[2].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    "\r\n\t}\t//end loop\r\n");
            }
            //<while>
            foreach (Match m in r[3].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format(
                    "\r\n\tint {0}__loop__id=0;\r\nwhile({0}.Read())\r\n\t{{\r\n{0}__loop__id++;\r\n",
                    m.Groups[1].ToString()
                    )
                    );
            }
            //</while>
            foreach (Match m in r[4].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format(
                    "\r\n\t}\t//end while\r\n{0}.Close();\r\n", m.Groups[1].ToString()
                    )
                    );
            }
            //<if>
            foreach (Match m in r[5].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(m.Groups[0].ToString(),
                    "\r\n\tif (" + m.Groups[1].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
            }
            //<%else%>
            foreach (Match m in r[6].Matches(source.ToString()))
            {
                iscodeline = true;
                if (m.Groups[1].ToString() == string.Empty)
                {
                    source.Replace(m.Groups[0].ToString(),
                    "\r\n\t}\r\n\telse\r\n\t{\r\n");
                }
                else
                {
                    source.Replace(m.Groups[0].ToString(),
                        "\r\n\t}\r\n\telse if (" + m.Groups[3].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
                }
            }
            //<%/if%>
            foreach (Match m in r[7].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(m.Groups[0].ToString(),
                    "\r\n\t}\t//end if\r\n");
            }
            //解析set
            foreach (Match m in r[19].Matches(source.ToString().ToString()))
            {
                iscodeline = true;
                string type = "";
                if (m.Groups[3].ToString() != string.Empty)
                {
                    type = m.Groups[3].ToString();
                }
                source.Replace(m.Groups[0].ToString(),
                    string.Format("\t{0} {1} = {2};\r\n\t", type, m.Groups[4].ToString(), m.Groups[5].ToString()).Replace("\\\"", "\"")
                    );
            }
            //解析repeat
            foreach (Match m in r[21].Matches(source.ToString().ToString()))
            {
                iscodeline = true;
                source.Replace(m.Groups[0].ToString(),
                "\tfor (int i = 0; i < " + m.Groups[2] + "; i++)\r\n\t{\r\n\t\ttemplateBuilder.Append(" + m.Groups[1].ToString().Replace("\\\"", "\"").Replace("\\\\", "\\") + ");\r\n\t}\r\n");
            }
            //StrToInt
            foreach (Match m in r[8].Matches(source.ToString()))
            {
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format("System.Convert.ToInt32({0})", m.Groups[2].ToString())
                    );
            }
            //urlencode()
            foreach (Match m in r[9].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format("System.Web.HttpUtility.UrlEncode({0});", m.Groups[2].ToString())
                    );
            }
            //DateToStr(,)
            foreach (Match m in r[10].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format(
                    "System.Convert.ToDateTime({0}).ToString(\"{1}\");",
                    m.Groups[2].ToString(),
                    m.Groups[3].ToString().Replace("\\\"", string.Empty)).Trim());
            }
            //substring() TODO
            foreach (Match m in r[20].Matches(source.ToString()))
            {
                iscodeline = true;
                source.Replace(
                    m.Groups[0].ToString(),
                    string.Format(
                    "Utils.GetSubString({0},{1},{2},\"{3}\");",
                    m.Groups[2].ToString(),
                    m.Groups[3].ToString(),
                    m.Groups[4].ToString(),
                    m.Groups[5].ToString().Replace("\\\"", string.Empty)
                    )
                    );
            }
            //{var.a}
            foreach (Match m in r[11].Matches(source.ToString()))
            {
                if (iscodeline)
                {
                    source.Replace(
                        m.Groups[0].ToString(),
                        string.Format(
                        "{0}.{1}{2}",
                        m.Groups[2].ToString(),
                        CutString(m.Groups[3].ToString(), 0, 1).ToUpper(),
                        m.Groups[3].ToString().Substring(1,
                        m.Groups[3].ToString().Length - 1)
                        )
                        );
                }
                else
                {
                    source.Replace(
                        m.Groups[0].ToString(),
                        string.Format(
                            "\");\r\n\ttemplateBuilder.Append({0}.{1}{2}.ToString().Trim());\r\n\ttemplateBuilder.Append(\"",
                            m.Groups[2].ToString(),
                            CutString(
                                m.Groups[3].ToString(),
                                0,
                                1).ToUpper(),
                            m.Groups[3].ToString().Substring(1, m.Groups[3].ToString().Length - 1)
                            )
                        );
#warning 这里如果是连续两个{var.a}链接的话,中间会产生一个空白的"",临时替换掉;
                    source.Replace("\r\n\ttemplateBuilder.Append(\"\");", "");
                }
            }

#warning    {request[a]} TODO:For Discuz!NT
            foreach (Match m in r[12].Matches(source.ToString()))
            {
                //throw new Exception("未完成");
                if (iscodeline)
                {
                    source.Replace(
                       m.Groups[0].ToString(),
                       "DNTRequest.GetString(\"" + m.Groups[2].ToString() + "\")"
                       );
                }
                else
                {
                    source.Replace(
                    m.Groups[0].ToString(),
                    string.Format("\");\r\n\ttemplateBuilder.Append(DNTRequest.GetString(\"{0}\"));\r\n\ttemplateBuilder.Append(\"", m.Groups[2])
                    );
                }
            }
#warning    解析{var[a]} TODU 未测试
            foreach (Match m in r[13].Matches(source.ToString()))
            {
                //throw new Exception("未测试的标签");
                if (iscodeline)
                {
                    if (IsNumeric(m.Groups[3].ToString()))
                    {
                        source.Replace(m.Groups[0].ToString(), m.Groups[2].ToString() + "[" + m.Groups[3].ToString() + "].ToString().Trim()");
                    }
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                        {
                            source.Replace(m.Groups[0].ToString(), m.Groups[2].ToString() + "__loop__id");
                        }
                        else
                        {
                            source.Replace(m.Groups[0].ToString(), m.Groups[2].ToString() + "[\"" + m.Groups[3].ToString() + "\"].ToString().Trim()");
                        }
                    }
                }
                else
                {
                    if (IsNumeric(m.Groups[3].ToString()))
                    {
                        source.Replace(m.Groups[0].ToString(), string.Format("\");\r\n\ttemplateBuilder.Append({0}[{1}].ToString().Trim());\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString(), m.Groups[3].ToString()));
                    }
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                        {
                            source.Replace(m.Groups[0].ToString(), string.Format("\");\r\n\ttemplateBuilder.Append({0}__loop__id.ToString());\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString()));
                        }
                        else
                        {
                            source.Replace(m.Groups[0].ToString(), string.Format("\");\r\n\ttemplateBuilder.Append({0}[\"{1}\"].ToString().Trim());\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString(), m.Groups[3].ToString()));
                        }
                    }
                }
            }
            //ReplaceSpecialTemplate(AspxfilePath, AspxfilePath, source.ToString()); TODO
            foreach (Match m in r[14].Matches(source.ToString()))
            {
                if (m.Groups[0].ToString() == "{productversion}" || m.Groups[0].ToString() == "{forumversion}")
                {
                    string productversion = (this.Productversion == null || this.Productversion.Trim() == string.Empty) ? GetAssemblyVersion() : this.Productversion;
                    source.Replace(m.Groups[0].ToString(), productversion);
                }
                else if (m.Groups[0].ToString() == "{productname}" || m.Groups[0].ToString() == "{forumproductname}")
                {
                    string productname = (this.Productname == null || this.Productname.Trim() == string.Empty) ? GetAssemblyProductName() : this.Productname;
                    source.Replace(m.Groups[0].ToString(), productname);
                }
            }
            //普通变量{}
            foreach (Match m in r[15].Matches(source.ToString()))
            {
                if (iscodeline)
                {
                    source.Replace(m.Groups[0].ToString(),
                        m.Groups[2].ToString());
                }
                else
                {
                    source.Replace(m.Groups[0].ToString(),
                        string.Format(
                        "\");\r\n\ttemplateBuilder.Append({0}.ToString());\r\n\ttemplateBuilder.Append(\"",
                        m.Groups[2].ToString().Trim()
                        )
                        );
                }
            }
            //解析==表达式            
            foreach (Match m in r[16].Matches(source.ToString()))
            {
                source.Replace(m.Groups[0].ToString(),
                    m.Groups[2].ToString() + "\"" + m.Groups[3].ToString() + "\"");

            }
            //解析csharpcode
            //
            foreach (Match m in r[18].Matches(source.ToString().ToString()))
            {
                iscodeline = true;
                source.Replace(m.Groups[0].ToString(),
                    m.Groups[1].ToString().Replace("\r\t\r", "\r\n\t").Replace("\\\"", "\""));
            }

            if (iscodeline)
            {
                return source.Append("\r\n").ToString();
            }
            else
            {
                if (source.Length > 0)
                {
                    StringBuilder htmltext = new StringBuilder();
                    string[] htmlline = SplitString(source.ToString(), "\r\r\r");
                    foreach (string line in htmlline)
                    {
                        if (line.Trim() == string.Empty)
                            continue;
                        //htmltext.Append(string.Format("\ttemplateBuilder.Append(\"{0}\\r\\n\");\r\n", line));
                        htmltext.Append(string.Format("\ttemplateBuilder.Append(\"{0}\\r\\n\");\r\n", line));
                    }
                    return htmltext.ToString();
                }
                else
                {
                    return string.Empty;
                }
                //return string.Format("\tResponse.Write(\"{0}\\r\\n\");\r\n", source.ToString());
            }
        }



        #region 工具
        /// <summary>
        /// 分割字符串
        /// </summary>
        private static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent != null && strContent != string.Empty)
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    string[] tmp = { strContent };
                    return tmp;
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        private static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        private static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        private static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }


        private static FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
        }
        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
            return AssemblyFileVersion.ProductName;
        }
        #endregion
    }
}
