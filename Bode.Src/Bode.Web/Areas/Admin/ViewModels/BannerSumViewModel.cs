using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bode.Web.Areas.Admin.ViewModels
{
    public class BannerSumViewModel
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string Tips { get; set; }

        public string ButtonName { get; set; }

        public List<BannerViewModel> Banners { get; set; }

        public BannerSumViewModel()
        {
            Banners = new List<BannerViewModel>();
        }

        public BannerSumViewModel(string buttonName,string tips, int width, int height) : this()
        {
            Width = width;
            Height = height;
            Tips = tips;
            ButtonName = buttonName;
        }
    }
}