using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogosLTH.Web
{
    public class CloudinaryAPI
    {
        Account account = new Account(
            //"pesca",
            //"269154332296237",
            //"k_GaPkueqBJW85De9aRGhZLgwJ8"
            "dpkyawi7g",
            "223562278248639",
            "EdNGyM7zwpMfsr2OxHnVDU9-Jcc"
            );

    public Account Account
        {
            set
            {
                this.Account = account;
            }
            get { return account; }
        }
    }
}