using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using iTextSharp.tool.xml.html;
using static iTextSharp.text.pdf.events.IndexEvents;
using System.Drawing;
using Font = iTextSharp.text.Font;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Contexts;
using iTextSharp.text.pdf.qrcode;

namespace pdfmaker
{
    class Program
    {
        //pdf主体
        static void Main(string[] args)
        {
            //创建document实例
            Document document = new Document();
            //创建writer实例
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("C:\\Users\\Administrator\\Desktop\\pdf\\wbd.pdf", FileMode.Create));
            //打开document
            document.Open();
            //中文字体
            BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/SIMYOU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //正文和标题字体
            iTextSharp.text.Font font = new Font(baseFont, 12, Font.NORMAL);
            Font font1 = new Font(baseFont, 20, Font.BOLD);
            Font font2 = new Font(baseFont, 16, Font.BOLD);
            Font font3 = new Font(baseFont, 14, Font.BOLD);
            //添加属性
            document.AddTitle("Sample Technical Report");
            document.AddSubject("PDF报告");
            document.AddKeywords("测试");
            document.AddCreator("visual studio 2022 preview");
            document.AddAuthor("KXH");

            //添加标题、单位、时间
            Paragraph title = new Paragraph("Sample Technical Report\n\n", font1);
            title.Alignment = 1; //0 = Left, 1 = Centre, 2 = Right
            document.Add(title);

            Paragraph author = new Paragraph("Example Organization\n\n", font3);
            author.Alignment = 2;
            document.Add(author);

            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            int 年 = currentTime.Year;
            int 月 = currentTime.Month;
            int 日 = currentTime.Day;
            int 时 = currentTime.Hour;
            int 分 = currentTime.Minute;
            int 秒 = currentTime.Second;
            string time = 年.ToString() + "年" + 月.ToString() + "月" + 日.ToString() + "日" + 时.ToString() + "时" + 分.ToString() + "分" + 秒.ToString() + "秒";
            Paragraph date = new Paragraph(time, font3);
            date.Alignment = 2;
            document.Add(date);

            //添加介绍
            string path = "C:\\Users\\Administrator\\Desktop\\pdf\\txt\\Introduction.txt";
            string[] lines = System.IO.File.ReadAllLines(path, Encoding.GetEncoding("utf-8"));
            Paragraph p = new Paragraph();
            Phrase p1 = new Phrase("一、背景介绍\n\n", font2);
            p.Add(p1);
            foreach (string line in lines)
            {
                Paragraph p2 = new Paragraph(line, font);
                p.Add(p2);
                document.Add(p);
            }

            //添加img1
            iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance("C:\\Users\\Administrator\\Desktop\\pdf\\img\\img1.jpg");
            float percentage1 = 1;
            //这里都是图片最原始的宽度与高度  
            float resizedWidht1 = img1.Width;
            float resizedHeight1 = img1.Height;
            //这时判断图片宽度是否大于页面宽度减去也边距，如果是，那么缩小，如果还大，继续缩小，  
            //这样这个缩小的百分比percentage会越来越小  
            while (resizedWidht1 > (document.PageSize.Width - document.LeftMargin - document.RightMargin) * 0.8)
            {
                 percentage1 = percentage1 * 0.9f;
                 resizedHeight1 = img1.Height * percentage1;
                 resizedWidht1 = img1.Width * percentage1;
            }

            while (resizedHeight1 > (document.PageSize.Height - document.TopMargin - document.BottomMargin) * 0.8)
            {
                  percentage1 = percentage1 * 0.9f;
                  resizedHeight1 = img1.Height * percentage1;
                  resizedWidht1 = img1.Width * percentage1;
            }
            //这里用计算出来的百分比来缩小图片  
            img1.ScalePercent(percentage1 * 100);
            //图片定位，页面总宽283，高416；这里设置0,0的话就是页面的左下角 让图片的中心点与页面的中心店进行重合  
            img1.SetAbsolutePosition(document.PageSize.Width / 2 - resizedWidht1 / 2, document.PageSize.Height / 3 - resizedHeight1 / 2);
            document.Add(img1);
            //插入页眉页脚水印
            writer.PageEvent = new IsHandF();

            //第二页
            document.NewPage();
            document.Add(new Paragraph("二、探测数据\n\n", font2));
            //添加标题
            Paragraph title1 = new Paragraph("可疑点经纬度\n\n", font3);
            title1.Alignment = 1;
            document.Add(title1);

            //添加表格
            PdfPTable table = new PdfPTable(3);
            //添加单元格
            PdfPCell cell = new PdfPCell();
            cell.Colspan = 200;
            cell.HorizontalAlignment = 1; //居中
            table.AddCell(cell);
            table.AddCell("Site");
            table.AddCell("Longitude");
            table.AddCell("Latitude");

            //json解析

            using (StreamReader reader = File.OpenText("C:\\Users\\Administrator\\Desktop\\pdf\\txt\\data.json"))
            {
                JArray ja = (JArray)JToken.ReadFrom(new JsonTextReader(reader));
                for (int i = 0; i < ja.Count; i++)
                {
                    JObject jo = JObject.Parse(ja[i].ToString());
                    string Site = jo["Site"].ToString();
                    Paragraph pS = new Paragraph(Site, font);
                    table.AddCell(pS);
                    string Longitude = jo["Longitude"].ToString();
                    Paragraph pLo = new Paragraph(Longitude, font);
                    table.AddCell(pLo);
                    string Latitude = jo["Latitude"].ToString();
                    Paragraph pLa = new Paragraph(Latitude, font);
                    table.AddCell(pLa);
                }
            }
            document.Add(table);

            //第三页
            document.NewPage();
            document.Add(new Paragraph("三、航飞路线\n\n", font2));

            //添加img2,3
            iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance("C:\\Users\\Administrator\\Desktop\\pdf\\img\\img2.jpg");
            float percentage2 = 1;
            float resizedWidht2 = img2.Width;
            float resizedHeight2 = img2.Height;
            while (resizedWidht2 > (document.PageSize.Width - document.LeftMargin - document.RightMargin) * 0.8)
            {
                percentage2 = percentage2 * 0.9f;
                resizedHeight2 = img2.Height * percentage2;
                resizedWidht2 = img2.Width * percentage2;
            }

            while (resizedHeight2 > (document.PageSize.Height - document.TopMargin - document.BottomMargin) * 0.8)
            {
                percentage2= percentage2 * 0.9f;
                resizedHeight2 = img2.Height * percentage2;
                resizedWidht2 = img2.Width * percentage2;
            }  
            img2.ScalePercent(percentage2 * 100);
            img2.SetAbsolutePosition(document.PageSize.Width / 2- resizedWidht2 / 2, document.PageSize.Height / 3*2 - resizedHeight2 / 2);
            document.Add(img2);

            iTextSharp.text.Image img3 = iTextSharp.text.Image.GetInstance("C:\\Users\\Administrator\\Desktop\\pdf\\img\\img3.jpg");
            float percentage3 = 1;
            float resizedWidht3 = img3.Width;
            float resizedHeight3 = img3.Height;
            while (resizedWidht3 > (document.PageSize.Width - document.LeftMargin - document.RightMargin) * 0.8)
            {
                percentage3 = percentage3 * 0.9f;
                resizedHeight3 = img3.Height * percentage3;
                resizedWidht3 = img3.Width * percentage3;
            }

            while (resizedHeight3 > (document.PageSize.Height - document.TopMargin - document.BottomMargin) * 0.8)
            {
                percentage3 = percentage3 * 0.9f;
                resizedHeight3 = img3.Height * percentage3;
                resizedWidht3 = img3.Width * percentage3;
            }
            img3.ScalePercent(percentage3 * 100);
            img3.SetAbsolutePosition(document.PageSize.Width / 2 - resizedWidht3 / 2, document.PageSize.Height / 4 - resizedHeight3 / 2);
            document.Add(img3);

            //第四页
            document.NewPage();
            document.Add(new Paragraph("四、总结\n\n", font2));
            string path2 = "C:\\Users\\Administrator\\Desktop\\pdf\\txt\\Conclusion.txt";
            string[] lines2 = System.IO.File.ReadAllLines(path2, Encoding.GetEncoding("utf-8"));
            foreach (string line in lines2)
            {
                Paragraph p2 = new Paragraph(line, font);
                document.Add(p2);
            }

            //关闭document
            document.Close();
        }
    }
    public class IsHandF : PdfPageEventHelper, IPdfPageEvent
    {
        //插入页眉页脚背景水印
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            //页眉页脚使用字体
            BaseFont bsFont = BaseFont.CreateFont("C:/Windows/Fonts/SIMYOU.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font fontheader = new iTextSharp.text.Font(bsFont, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fontfooter = new iTextSharp.text.Font(bsFont, 12, iTextSharp.text.Font.BOLD);

            //获取文件流
            PdfContentByte cbs = writer.DirectContent;
            cbs.SetCharacterSpacing(1.3f); //设置文字显示时的字间距
            Phrase header = new Phrase("", fontheader);
            Phrase footer = new Phrase(writer.PageNumber.ToString(), fontfooter); //writer.PageNumber.ToString()为页码。
            //页眉显示的位置 
            ColumnText.ShowTextAligned(cbs, Element.ALIGN_CENTER, header,
                       document.Right / 2, document.Top + 20 , 0);
            //页脚显示的位置 
            ColumnText.ShowTextAligned(cbs, Element.ALIGN_CENTER, footer,
                       document.Right / 2, document.Bottom - 20 , 0);

            //添加背景色及水印，在内容下方添加
            PdfContentByte cba = writer.DirectContentUnder;
            //背景色
            Bitmap bmp = new Bitmap(1263, 893);
            Graphics g = Graphics.FromImage(bmp);
            Color c = Color.FromArgb(0x33ff33);
            SolidBrush b = new SolidBrush(c);//这里修改颜色
            g.FillRectangle(b, 0, 0, 1263, 893);
            System.Drawing.Image ig = bmp;
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ig, new BaseColor(0xFF, 0xFF, 0xFF));
            img.SetAbsolutePosition(0, 0);
            cba.AddImage(img);

            //水印
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("C:\\Users\\Administrator\\Desktop\\pdf\\img\\sy.jpg");
            image.RotationDegrees = 30;//旋转角度

            PdfGState gs = new PdfGState();
            gs.FillOpacity = 0.1f;//透明度
            cba.SetGState(gs);

            int x = -1000;
            for (int j = 0; j < 15; j++)
            {
                x = x + 200;
                int a = x;
                int y = - 100;
                for (int i = 0; i < 10; i++)
                {
                    a = a + 200;
                    y = y + 200;
                    image.SetAbsolutePosition(a, y);
                    cba.AddImage(image);
                }
            }
        }
    }
}