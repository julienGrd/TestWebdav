using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebdav.Shared
{
    public class WebdavFileModel
    {
        public string Name { get; set; }

        public WebdavFileModel()
        {

        }

        public WebdavFileModel(string pName, string pUrl)
        {
            Name = pName;
            Url = pUrl;
        }

        public string Url { get; set; }
    }
}
