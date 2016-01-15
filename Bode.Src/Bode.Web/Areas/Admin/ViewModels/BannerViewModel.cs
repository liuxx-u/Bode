using Bode.Services.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bode.Web.Areas.Admin.ViewModels
{
    public class BannerViewModel
    {
        public int OrderNo { get; set; }
        
        public string Path { get; set; }
        
        public bool IsDisplay { get; set; }
        
        public BannerType BannerType { get; set; }

        public BannerViewModel() { }

        public BannerViewModel(BannerEntityBase<int> banner)
        {
            OrderNo = banner.OrderNo;
            Path = banner.Path;
            IsDisplay = banner.IsDisplay;
            BannerType = banner.BannerType;
        }
    }
}