using Web_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using Web_Store.Model;

namespace Web_Store.Controllers
{
    public class ProductViewModel
    {
        dbQLGiayDataContext data = new dbQLGiayDataContext();
        public int MAGIAY { get; set; }
        public string TENGIAY { get; set; }
        public decimal? DONGIABAN { get; set; }
        public string HINHANH { get; set; }
        public int MATH { get; set; }
        public int MALOAI { get; set; }
        public String TENTH { get; set; }
        public String TENLOAI { get; set; }
        public int SOLUONG { get; set; }
        public string MOTA { get; set; }
        public int TENKICHTHUOC { get; set; }
        public string HINH1 { get; set; }
        public string LOGO { get; set; }
        public string THANHTOANON { get; set; }
        
    }
}