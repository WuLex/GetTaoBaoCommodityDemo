using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTaoBaoCommodity.Models
{
    public static class DictHelper
    {

        public static Dictionary<string,string> DataInfoPropertyDict =new Dictionary<string, string>()
        {
             { "TitleName" , "商品名称"},
             { "PicUrl" , "图片地址"},
             { "DetailUrl" , "商品地址"},
             { "Price" , "价格"},
             { "Address" , "店铺所在地"},
             { "BuyNumber" , "购买人数"},
             { "UserId" , "卖家ID"},
             { "ShopName" , "店铺名称"},
        };
    }
}
