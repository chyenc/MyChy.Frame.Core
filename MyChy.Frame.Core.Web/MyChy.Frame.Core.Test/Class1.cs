using MyChy.Frame.Core.Common.Helper;
using NUnit.Framework;
using System;

namespace MyChy.Frame.Core.Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void CreatLogin()
        {
            var ss1= "{\"result\":[{\"originTo\":\"+8613810565156\",\"createTime\":\"2019-03-11T07:37:41Z\",\"from\":\"csms12345678\",\"smsMsgId\":\"98864723-9047-4790-a384-f602f310eeb5_145580135\",\"status\":\"E200033\"}],\"code\":\"E200037\",\"description\":\"The SMS fails to be sent. For details, see status.\"}";

            //var model = new BatchSendSmsReq
            //{
            //    code = "E200037",
            //    description = "The SMS fails to be sent. For details, see status.",
            //    result = new BatchSendSmsReqResult
            //    {
            //        createTime = "2019-03-11T07:37:41Z",
            //        originTo = "+8613810565156",
            //        from = "csms12345678",
            //        smsMsgId = "98864723-9047-4790-a384-f602f310eeb5_145580135",
            //        status = "E200033"
            //    }
            //};

            var s1 = StringHelper.Serialize(ss1);

            //var ss = FileHelper.CheckFileNmae(name, out string namex, out string exname);
            //name = "aa.jpg.jpge";
            //ss = FileHelper.CheckFileNmae(name, out  namex, out exname);

            ////string openid = "o8KB3uEwEIye3ceovZRej1uG1rMM";

            ////UserLoginServer.SentRegByOpenId(openid);


            //FileHelper.DeleteFolder(@"F:\MyChy\HuiYuan\MyChy.T4\MyChy.Core.T4\CoreData");
        }

    }



}
