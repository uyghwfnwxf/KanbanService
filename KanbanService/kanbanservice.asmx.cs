using MyJGPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace KanbanService
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld( String msg)
        {
            var Msg1 = string.Empty;
            //  Msg = MyJGPush.MyPush.SendPush("新年快乐", "新年快乐！", 1, "s_registrationId：你的设备ID");//IOS设备推送
            //  Msg = MyJGPush.MyPush.SendPush("新年快乐", "新年快乐！", 0, "aaa");//安卓设备推送
            Msg1 = MyPush.SendPush("新年快乐", msg, 3, "");//群发

            return Msg1;
        }
    }
}
