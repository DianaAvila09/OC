using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Root.Model
{
    
    public class ET_RETURN
    {
        protected String _TYPE;
        protected String _ID;
        protected Int32 _NUMBER;
        protected String _MESSAGE;

        public String TYPE
        {
            get { return _TYPE; }
            set { _TYPE = value; }
        }

        public String ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public Int32 NUMBER
        {
            get { return _NUMBER; }
            set { _NUMBER = value; }
        }

        public String MESSAGE
        {
            get { return _MESSAGE; }
            set { _MESSAGE = value; }
        }
    }
}