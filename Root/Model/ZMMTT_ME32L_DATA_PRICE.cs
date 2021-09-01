using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Root.Model
{
    public class ZMMTT_ME32L_DATA_PRICE
    {
        protected String _PURCHASINGDOCUMENT;
        protected Int32 _ITEM_NO;
        protected String _MATERIAL;
        protected Double _KTMNG;
        protected String _MEINS;
        protected Double _NETPR;
        protected Double _PEINH;

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

        public Double KTMNG
        {
            get { return _KTMNG; }
            set { _KTMNG = value; }
        }

        public String MEINS
        {
            get { return _MEINS; }
            set { _MEINS = value; }
        }

        public Double NETPR
        {
            get { return _NETPR; }
            set { _NETPR = value; }
        }

        public Double PEINH
        {
            get { return _PEINH; }
            set { _PEINH = value; }
        }
    }
}