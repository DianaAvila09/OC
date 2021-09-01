using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job.PricesWF.Model
{
    public class ZMMTT_ME32L_ACT_PRICE
    {
        protected String _PURCHASINGDOCUMENT;
        protected Int32 _ITEM_NO;
        protected String _MATERIAL;
        protected Double _PRICE;
        protected Int32 _BASE;
        protected DateTime _VALID_FROM;

        public String PURCHASINGDOCUMENT
        {
            get { return _PURCHASINGDOCUMENT; }
            set { _PURCHASINGDOCUMENT = value; }
        }

        public Int32 ITEM_NO
        {
            get { return _ITEM_NO; }
            set { _ITEM_NO = value; }
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
