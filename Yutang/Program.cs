using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Yutang.Forms;
using Yutang.Settings;

namespace Yutang
{
    static class Program
    {
        public static Main MainSettings { get; set; }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string json = File.ReadAllText("appsettings.json");
            MainSettings = JsonConvert.DeserializeObject<Main>(json);
            //string json = JsonConvert.SerializeObject(MainSettings);
            //System.IO.File.WriteAllText("appsettings.json", ConvertJsonString(json));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RenderForm());
        }

        private static string ConvertJsonString(string str)
        {
            //格式化json字符串  
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }
}
