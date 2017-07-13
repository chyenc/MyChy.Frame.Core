using Microsoft.AspNetCore.DataProtection.Repositories;
using MyChy.Frame.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MyChy.Frame.Core.Repository
{
    /// <summary>
    /// machineKey 实现.net Core 负载均衡登录
    /// </summary>
    public class MachineXmlRepository : IXmlRepository
    {

        private readonly string filePath = FileHelper.GetFileMapPath(@"config/MachineKey.config");

        public virtual IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            yield return XElement.Load(filePath);

        }


        public virtual void StoreElement(XElement element, string friendlyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            StoreElementCore(element, friendlyName);
        }


        private void StoreElementCore(XElement element, string filename)
        {


        }
    }
}
