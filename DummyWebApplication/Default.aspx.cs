﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DummyWebApplication
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnRunException.Click += btnRunException_Click;
        }

        void btnRunException_Click(object sender, EventArgs e)
        {
            throw new NullReferenceException();
        }
    }
}