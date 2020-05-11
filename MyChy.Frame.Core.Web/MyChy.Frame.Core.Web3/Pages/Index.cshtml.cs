using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MyChy.Frame.Core.Common.Helper;
using MyChy.Frame.Core.Common.Model;

namespace MyChy.Frame.Core.Web3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            var url = "Encrypt=eyJLZXkiOiJBQ0VBOTMzREZCM0IzRDg1IiwiVGlja3MiOjYzNzI0NzkxODU1MDU2NDg3OX0=&Sign=f2fa563206856e18b4606ca7154dfce6e2e1b9eb";
            var res = StringHelper.DeserializeParameter<ReceiptEncryptModel>(url);


            string vv = "";
        }
    }
}
