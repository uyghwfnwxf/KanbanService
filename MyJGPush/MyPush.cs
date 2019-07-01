using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JPushAPI.push.mode;
using JPushAPI;
using JPushAPI.common;
using JPushAPI.common.resp;
using JPushAPI.push.notification;
namespace MyJGPush
{
    public class MyPush
    {
        public static String TITLE = "测试标题";
        public static String ALERT = "测试内容";
        public static String MSG_CONTENT = "Test from C# v3 sdk - msgContent---.NET3.5";
        public static String REGISTRATION_ID = "0900e8d85ef";
        public static String TAG = "tag_api";
        public static String app_key_ios = System.Configuration.ConfigurationSettings.AppSettings["app_key_ios"];
        public static String master_secret_ios = System.Configuration.ConfigurationSettings.AppSettings["master_secret_ios"];
        public static String app_key_android = System.Configuration.ConfigurationSettings.AppSettings["app_key_android"];
        public static String master_secret_android = System.Configuration.ConfigurationSettings.AppSettings["master_secret_android"];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content">发送内容</param>
        /// <param name="Platform">1:ios,0:Android,3群发</param>调用钱，先在WebConfig中的参数
        /// <returns></returns>
        public static string SendPush(string Title, string Content, int Platform, string s_registrationId)
        {
            TITLE = Title;
            ALERT = Content;
            string msg = string.Empty;
            if (Platform == 1)
            {
                JPushClient client = new JPushClient(app_key_ios, master_secret_ios);//ios
                PushPayload payload = PushObject_IOSMsg(s_registrationId, Content);//ios推送
                msg = JGPushing(client, payload);
            }
            else if (Platform == 0)
            {
                JPushClient client = new JPushClient(app_key_android, master_secret_android);//andriod
                PushPayload payload = PushObject_AndroidMsg(s_registrationId, Content);
                msg = JGPushing(client, payload);
            }
            else if (Platform == 3)
            {
                JPushClient client = new JPushClient(app_key_ios, master_secret_ios);//ios群发
                PushPayload payload = PushObject_All_All_Aler_ios();
                msg = JGPushing(client, payload);
                client = new JPushClient(app_key_android, master_secret_android);//andriod群发
                payload = PushObject_All_All_Alert_Android();
                msg = JGPushing(client, payload);
            }
            return msg;
            //

            // PushPayload payload = PushObject_All_All_Alert();//群发
            //  PushPayload payload = PushObject_IOSMsg("191e35f7e040cd09c20", Content);//ios推送
            //PushPayload payload = PushObject_AndroidMsg("1a0018970aa3a3b76b7", Content);//Android推送


        }
        public static string JGPushing(JPushClient client, PushPayload payload)
        {
            var results = string.Empty;
            try
            {
                var result = client.SendPush(payload);
                //var result = client.SendPush("4422569071705780711");
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(1000);
                /*如需查询上次推送结果执行下面的代码*/
                // var receive = client.getReceivedApi(result.msg_id.ToString());
                //var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());
                /*如需查询某个messageid的推送结果执行下面的代码*/
                //var queryResultWithV2 = client.getReceivedApi("1739302794");
                //var querResultWithV3 = client.getReceivedApi_v3("1739302794");
                results = result.msg_id.ToString();
            }
            catch (APIRequestException e)
            {
                results += "Error response from JPush server. Should review and fix it. ";
                results += "HTTP Status: " + e.Status;
                results += "Error Code: " + e.ErrorCode;
                results += "Error Message: " + e.ErrorCode;
            }
            return results;
        }
        /// <summary>
        /// 推送到单个App  IOS设备 
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="MsgContent"></param>
        /// <returns></returns>
        public static PushPayload PushObject_IOSMsg(string registrationId, string MsgContent)
        {
            string[] register = registrationId.Split(',');
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.ios();
            pushPayload.audience = Audience.s_registrationId(register);
            var notification = new IosNotification().setAlert(MsgContent);
            pushPayload.notification = new Notification().setIos(notification);
            pushPayload.options.apns_production = true;
            return pushPayload;
        }
        /// <summary>
        ///  推送到单个App Android设备 
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="MsgContent"></param>
        /// <returns></returns>
        public static PushPayload PushObject_AndroidMsg(string registrationId, string MsgContent)
        {
            string[] register = registrationId.Split(',');

           
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_registrationId(register);
            var notification = new AndroidNotification().setAlert(MsgContent);
          

            var message = Message.content(MsgContent);
            message.content_type = "text";
            message.msg_content = "aaaaaaaaaaa";
            message.setTitle("aaaaa");

            pushPayload.message = message;


            pushPayload.notification = new Notification().setAndroid(notification);
            pushPayload.options.apns_production = true;
            return pushPayload;
        }
        /// <summary>
        /// 安卓群发
        /// </summary>
        /// <returns></returns>
        public static PushPayload PushObject_All_All_Alert_Android()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.all();
            var notification = new AndroidNotification().setAlert(ALERT);
            pushPayload.notification = new Notification().setAndroid(notification);
            var message = Message.content(ALERT);
            message.content_type = "text";
            message.msg_content = ALERT;
            message.setTitle("aaaaa");

            pushPayload.message = message;
            pushPayload.options.apns_production = true;
            return pushPayload;
        }
        /// <summary>
        /// IOS群发
        /// </summary>
        /// <returns></returns>
        public static PushPayload PushObject_All_All_Aler_ios()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.ios();
            pushPayload.audience = Audience.all();
            var notification = new IosNotification().setAlert(ALERT);
            pushPayload.notification = new Notification().setIos(notification);
            pushPayload.options.apns_production = true;
            return pushPayload;
        }
    }
}
