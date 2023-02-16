using GetTaoBaoCommodity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;


namespace GetTaoBaoCommodity
{
    public class HtmlHelper
    {

        public static void WriteFile(List<DataInfoModel> resultList, string tmppath, string resultpath)
        {
            //---------------------读html模板页面到stringbuilder对象里----
            StringBuilder htmltext = new StringBuilder();
            if (Directory.Exists(tmppath))//判断是否存在
            {
                Directory.CreateDirectory(tmppath);//创建新路径

            }
            try
            {
                using (StreamReader sr = new StreamReader(tmppath)) //模板页路径
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        htmltext.Append(line);
                    }

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // ignored
            }


         


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < resultList.Count; i++)
            {
                var dataInfoModel =resultList[i]; 
                //遍历获取成员名称
                foreach (PropertyInfo p in dataInfoModel.GetType().GetProperties())
                {
                    //var xx = p.Name;
                    //var yy = p.GetValue(dataInfoModel);
                    //Console.WriteLine(p.Name);

                    if (p.Name == "PicUrl")
                    {
                        sb.Append($"<tr><td width=\"100%\" valign=\"middle\" align=\"left\">第{i + 1}个-{DictHelper.DataInfoPropertyDict[p.Name]}-<img src='{"http:" + resultList[i].PicUrl}' width='128' height='128' /></td></tr>\r\n");
                    }
                    else if (p.Name == "DetailUrl")
                    {
                        sb.Append($"<tr><td width=\"100%\" valign=\"middle\" align=\"left\">第{i + 1}个-<a href='https:{resultList[i].DetailUrl}'>{DictHelper.DataInfoPropertyDict[p.Name]}</a></td></tr>\r\n");
                    }
                    else
                    {
                        sb.Append($"<tr><td width=\"100%\" valign=\"middle\" align=\"left\">第{i + 1}个-{DictHelper.DataInfoPropertyDict[p.Name]}-{p.GetValue(dataInfoModel)}</td></tr>\r\n");
                    }

                }
                sb.Append("<tr><td width=\"100%\" bgcolor='#30D5C8' valign=\"middle\" align=\"left\"></td></tr>\r\n");
            }

            //----------替换htm里的标记为你想加的内容
            htmltext.Replace("$htmlformat[1]", sb.ToString());

            //----------生成htm文件------------------――
            try
            {
                using (StreamWriter sw = new StreamWriter(resultpath, false, System.Text.Encoding.GetEncoding("utf-8"))) //保存地址
                {
                    sw.WriteLine(htmltext);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // ignored
            }
        }


        public static void WriteFile(List<List<string>> resultAarry, string tmppath, string resultpath)
        {
            //---------------------读html模板页面到stringbuilder对象里----
            StringBuilder htmltext = new StringBuilder();
            if (Directory.Exists(tmppath))//判断是否存在
            {
                Directory.CreateDirectory(tmppath);//创建新路径

            }
            try
            {
                using (StreamReader sr = new StreamReader(tmppath)) //模板页路径
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        htmltext.Append(line);
                    }

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // ignored
            }

            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < resultAarry.Count; x++)
            {
                for (int y = 0; y < resultAarry[x].Count; y++)
                {
                    if (y == 1)
                    {
                        sb.Append(
                            $"<tr><td width=\"100%\" valign=\"middle\" align=\"left\">第{x + 1}个-{Rule.Information[y][1]}-<img src='{"http:" + resultAarry[x][y]}' width='128' height='128' /></td></tr>\r\n");
                    }
                    else if (y == 2)
                    {
                        sb.Append(String.Format(
                            "<tr><td width=\"100%\" valign=\"middle\" align=\"left\">第{0}个-<a href='{2}'>{1}</a></td></tr>\r\n",
                            x + 1, Rule.Information[y][1], "https:" + resultAarry[x][y]));
                    }
                    else
                    {
                        sb.Append(
                            $"<tr><td width=\"100%\" valign=\"middle\" align=\"left\">第{x + 1}个-{Rule.Information[y][1]}-{resultAarry[x][y]}</td></tr>\r\n");
                    }
                }

                sb.Append(String.Format(
                    "<tr><td width=\"100%\" bgcolor='#30D5C8' valign=\"middle\" align=\"left\"></td></tr>\r\n"));
            }

            //----------替换htm里的标记为你想加的内容
            htmltext.Replace("$htmlformat[1]", sb.ToString());

            //----------生成htm文件------------------――
            try
            {
                using (StreamWriter sw = new StreamWriter(resultpath, false, System.Text.Encoding.GetEncoding("utf-8"))) //保存地址
                {
                    sw.WriteLine(htmltext);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // ignored
            }
        }

        /// <summary>
        /// 自动产生编号，最多一天产生99个
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string ConvertDate(int index)
        {
            string Res = string.Empty;
            Res = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day;
            if (index < 10)
            {
                Res = Res + "0" + index;
            }
            else
            {
                Res = Res + index;
            }

            return Res;
        }
    }
}