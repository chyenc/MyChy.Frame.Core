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

            var name = "aa.jpge";
            var ss = FileHelper.CheckFileNmae(name, out string namex, out string exname);
            name = "aa.jpg.jpge";
            ss = FileHelper.CheckFileNmae(name, out  namex, out exname);

            //string openid = "o8KB3uEwEIye3ceovZRej1uG1rMM";

            //UserLoginServer.SentRegByOpenId(openid);


            FileHelper.DeleteFolder(@"F:\MyChy\HuiYuan\MyChy.T4\MyChy.Core.T4\CoreData");
        }

    }


 
}
