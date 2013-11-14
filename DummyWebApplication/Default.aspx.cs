using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;

namespace DummyWebApplication
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnRunException.Click += btnRunException_Click;
            btnRunExceptionWithErrorSignal.Click += btnRunExceptionWithErrorSignal_Click;
        }

        void btnRunExceptionWithErrorSignal_Click(object sender, EventArgs e)
        {
            ltrResponse.Text = "";
            ErrorSignal.FromCurrentContext().Raise(new NullReferenceException());
            ltrResponse.Text = "Exception set :)";
        }

        void btnRunException_Click(object sender, EventArgs e)
        {
            ltrResponse.Text = "";
            throw new NullReferenceException();
        }
    }
}