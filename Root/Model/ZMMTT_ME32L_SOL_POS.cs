using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Root.Model
{
    public class ZMMTT_ME32L_SOL_POS
    {
        protected String _PURCHASINGDOCUMENT;
        protected String _LIFNR;
        protected String _ITEM_CAT;
        protected String _MATERIAL;
        protected Double _TARGET_QTY;
        protected Double _PRICE;
        protected Int32 _BASE;
        protected DateTime _VALID_FROM;

        public String PURCHASINGDOCUMENT
        {
            get { return _PURCHASINGDOCUMENT; }
            set { _PURCHASINGDOCUMENT = value; }
        }

        public String LIFNR
        {
            get { return _LIFNR; }
            set { _LIFNR = value; }
        }

        public String ITEM_CAT
        {
            get { return _ITEM_CAT; }
            set { _ITEM_CAT = value; }
        }

        public String MATERIAL
        {
            get { return _MATERIAL; }
            set { _MATERIAL = value; }
        }

        public Double TARGET_QTY
        {
            get { return _TARGET_QTY; }
            set { _TARGET_QTY = value; }
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
    }
}