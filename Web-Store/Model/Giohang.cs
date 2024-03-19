using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Store.Model
{
    public class Giohang
    {
        dbQLGiayDataContext data = new dbQLGiayDataContext();

        public int iMaGiay { set; get; }

        public string sTenGiay { set; get; }
        public string gHINHANH { set; get; }

        public string sMoTa { set; get; }

        public double dDonGia { set; get; }

        public int iSoLuong { set; get; }
        
        public int iMaDH { set; get; }
        
        public int iMaCTDH { set; get; }

        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public Giohang(int MaGiay)
        {
            iMaGiay = MaGiay;
            Giay giay = data.Giays.Single(n => n.MaGiay == iMaGiay);
            sTenGiay = giay.TenGIay;
            sMoTa = giay.MoTa;
            dDonGia = double.Parse(giay.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}