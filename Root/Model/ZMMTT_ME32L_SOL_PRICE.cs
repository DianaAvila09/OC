using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Root.Model
{
    
    public class ZMMTT_ME32L_SOL_PRICE
    {
        protected String _PURCHASINGDOCUMENT;
        protected String _MATERIAL;
        protected Double _PRICE;
        protected Int32 _BASE;
        protected DateTime _VALID_FROM;

        public String PURCHASINGDOCUMENT
        {
            get { return _PURCHASINGDOCUMENT; }
            set { _PURCHASINGDOCUMENT = value; }
        }

        public String MATERIAL
        {
            get { return _MATERIAL; }
            set { _MATERIAL = value; }
        }

        public Double PRICE
        {
            get { return _PRICE; }
            set { _PRICE = value; }
        }

        public Int32 BASE
        {
            get { return _BASE; }
            set { _BASE = value; }
        }

        public DateTime VALID_FROM
        {
            get { return _VALID_FROM; }
            set { _VALID_FROM = value; }
        }
    }
}