using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatMVC25B.Models
{
    public class Sinifim
    {
        public List<TBLBLOG> bloglistesi {  get; set; }
        public List<TBLBLOGRESIMLER> blogresimleri {  get; set; }
        public List<TBLKATEGORI> kategoriler {  get; set; }
        public TBLHAKKIMDA hakkimda { get; set; }
    }
}