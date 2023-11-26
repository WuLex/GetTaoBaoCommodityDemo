using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GetTaoBaoCommodity.Models
{
    /// <summary>
    /// 重构前的代码
    /// </summary>
    //public static class Rule
    //{
    //    public static string[] Name = { "raw_title", "商品名称" };
    //    public static string[] Pic = { "pic_url", "图片地址" };
    //    public static string[] Url = { "detail_url", "商品地址" };
    //    public static string[] Price = { "view_price", "价格" };
    //    public static string[] Address = { "item_loc", "店铺所在地" };
    //    public static string[] BuyNumber = { "view_sales", "购买人数" };
    //    public static string[] UserId = { "user_id", "卖家ID" };
    //    public static string[] ShopName = { "nick", "店铺名称" };
    //    public static int Inline = 58; //内容所在行数

    //    public static List<string[]> Information = new List<string[]>() {Name, Pic, Url, Price, Address, BuyNumber, UserId, ShopName};

    //    public static List<List<string>> GetAll(string html) //获取所有商品信息
    //    {
    //        List<List<string>> list = new List<List<string>>();
    //        for (int i = 1; i <= 10; i++)
    //        {
    //            List<string> item = new List<string>();
    //            for (int j = 0; j < Information.Count; j++)
    //            {
    //                int index = html.IndexOf(Information[j][0], StringComparison.Ordinal);
    //                if (index != -1)
    //                {
    //                    html = html.Substring(index + Information[j][0].Length + 3);
    //                    item.Add(j == 2
    //                        ? Regex.Unescape(html.Substring(0, html.IndexOf('"')))
    //                        : html.Substring(0, html.IndexOf('"')));
    //                }
    //            }

    //            list.Add(item);
    //        }

    //        return list;
    //    }
    //}

    public static class Rule
    {
        public static (string key, string label) Name = ("raw_title", "商品名称");
        public static (string key, string label) Pic = ("pic_url", "图片地址");
        public static (string key, string label) Url = ("detail_url", "商品地址");
        public static (string key, string label) Price = ("view_price", "价格");
        public static (string key, string label) Address = ("item_loc", "店铺所在地");
        public static (string key, string label) BuyNumber = ("view_sales", "购买人数");
        public static (string key, string label) UserId = ("user_id", "卖家ID");
        public static (string key, string label) ShopName = ("nick", "店铺名称");
        public static int Inline = 58; //内容所在行数

        public static Dictionary<string, (string key, string label)> Information = new Dictionary<string, (string key, string label)>
        {
            { "Name", Name },
            { "Pic", Pic },
            { "Url", Url },
            { "Price", Price },
            { "Address", Address },
            { "BuyNumber", BuyNumber },
            { "UserId", UserId },
            { "ShopName", ShopName },
        };


        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<List<string>> GetAll(string html)
        {
            var list = new List<List<string>>();
            for (int i = 1; i <= 10; i++)
            {
                var item = new List<string>();
                foreach (var info in Information.Values)
                {
                    int index = html.IndexOf(info.key, StringComparison.Ordinal);
                    if (index != -1)
                    {
                        html = html.Substring(index + info.key.Length + 3);
                        item.Add(info.key == "Url"
                            ? Regex.Unescape(html.Substring(0, html.IndexOf('"')))
                            : html.Substring(0, html.IndexOf('"')));
                    }
                }
                list.Add(item);
            }
            return list;
        }
    }
}