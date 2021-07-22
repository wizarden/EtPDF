using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;


//https://docs.microsoft.com/ru-ru/troubleshoot/windows-client/shell-experience/command-line-string-limitation#:~:text=%D0%BD%D0%BE%D0%BC%D0%B5%D1%80%20%D0%9A%D0%91%3A%20830473-,%D0%94%D0%BE%D0%BF%D0%BE%D0%BB%D0%BD%D0%B8%D1%82%D0%B5%D0%BB%D1%8C%D0%BD%D1%8B%D0%B5%20%D1%81%D0%B2%D0%B5%D0%B4%D0%B5%D0%BD%D0%B8%D1%8F,%D0%BA%D0%BE%D0%BC%D0%B0%D0%BD%D0%B4%D0%BD%D0%B0%D1%8F%20%D1%81%D1%82%D1%80%D0%BE%D0%BA%D0%B0

namespace EtPDF
{
    class Program
    {
        static Document document;
        static BaseFont TimesNewRomanBase;
        static BaseFont TimesNewRomanBaseBold;
        static PdfContentByte cb;
        const int wmargin = 1;

        static int mm(double t)
        { 
            return Convert.ToInt32(t * 72 / 25.4);
        }
        static void Main(string[] args)
        {
            string json = String.Join(" ", args);   //args[0];
            Console.WriteLine(json);

            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            int wi = Convert.ToInt32(210 / 25.4 * 72);
            int he = Convert.ToInt32(297 / 25.4 * 72);


            //------------------------------------------------------------------------------------------------------------
            string t = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            TimesNewRomanBase = BaseFont.CreateFont(t, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            t = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "timesbd.TTF");
            TimesNewRomanBaseBold = BaseFont.CreateFont(t, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);


            //string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            //var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            


            //-------------------------------------------------------------------------------------------------------------



            var pgSize = new iTextSharp.text.Rectangle(wi, he);


            document = new Document(pgSize, 0f, 0f, 0f, 0f);

            
            string PDFName = values["pdfname"].ToString();
            Console.WriteLine(PDFName);

            using (PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(PDFName, FileMode.Create)))
            {
                document.Open();
                cb = writer.DirectContent;

               // addRectangle(50, 50, 50, 50, 1);
               // addRoundRectangle(50, 50, 100, 100, 1, 5);

               // addText("AAAAAA", 50, 50, 50, 20f, 45);
                addBorderText("ФАСАД ВЕРХ", 5, 100, 200, 100, 50f, false);
                //addBorderText("ФАСАД ВЕРХ", 5, 130, 200, 100, 50f, true);


                addWrapText("ИП Стеклянников В.М., Юр.адрес: Россия, 440045 г.Пенза, ул.Ладожская 155 - 77.Производство: Россия, 440015, г.Пенза, ул.Аустрина, д. 164.Хранить в крытых"
                    + "помещениях при температуре не ниже 10°С и отн. влажности воздуха от 40% до 60%. Гарант. срок эксплуатации-3 года. Срок службы-15 лет.ГОСТ 16371-2014,ГОСТ 19917-2014"
                    + "ТР ТС 025/2012, Декларация о соответствии ЕАЭС №RU Д-RU.РА01.B.58427/21, срок действия по 04.05.2026 г."
                    , 5, 5 , 200, 30, 7f);


                addEAN13("2520439000412", 10,50, 1.5f);

                document.Close();
                writer.Close();

            }


            Console.WriteLine(PageSize.A4.Width);
            Console.WriteLine(PageSize.A4.Height);
            Console.WriteLine(mm(210));
            Console.ReadLine();

           

        }


        ///////////////////////////////////
        ///////////////////////////////////
        ///////////////////////////////////
        ///////////////////////////////////
        ///////////////////////////////////




        static void addRectangle(int x, int y, int w, int h, int border)
        {
            cb.SetLineWidth(mm(border));
            cb.Rectangle(mm(x), mm(y), mm(w), mm(h));
            cb.Stroke();
        }

        static void addRoundRectangle(int x, int y, int w, int h, int border, int radius)
        {
            cb.SetLineWidth(mm(border));
            cb.RoundRectangle(mm(x), mm(y), mm(w), mm(h), mm(radius));
            cb.Stroke();
        }
        static void addText(string text, int x, int y, float w, float fontSize, int ug=0 )

        {
            float ww = TimesNewRomanBase.GetWidthPoint(text, fontSize);
            cb.BeginText();
            cb.SetFontAndSize(TimesNewRomanBase, mm(fontSize));
            if (ww > w) cb.SetHorizontalScaling(w / ww * 100);
            cb.ShowTextAligned(Element.ALIGN_LEFT, text, mm(x), mm(y), ug);
            cb.EndText();
        }

        static void addBorderText(string text, int x, int y, int w, int h, float fontSize, bool bold = false, int ug = 0)

        {
            float ww = TimesNewRomanBase.GetWidthPoint(text, fontSize);

            cb.BeginText();

            if (bold)
            { 
                cb.SetFontAndSize(TimesNewRomanBaseBold, mm(fontSize));
                if (ww > (w - wmargin * 5)) cb.SetHorizontalScaling((w - mm(wmargin * 5)) / ww * 100);
            }
            else
            {
                cb.SetFontAndSize(TimesNewRomanBase, mm(fontSize));
                if (ww > (w - wmargin)) cb.SetHorizontalScaling((w - mm(wmargin)) / ww * 100);
            }

            
            
            cb.ShowTextAligned(Element.ALIGN_CENTER, text, mm(x) + mm(w)/2, mm(y) + mm(h/2) - mm(fontSize/3), ug);
            cb.EndText();

            addRectangle(x, y, w, h, 1);


            cb.SetHorizontalScaling( 100 );

        }

        static void addWrapText(string text, int x, int y, int w, int h, float f) 
        {

            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(new Rectangle(mm(x), mm(y), mm(x + w), mm(y + h)));
            
            Font font = new iTextSharp.text.Font(TimesNewRomanBase, f, Font.NORMAL);

            Paragraph tp = new Paragraph(text, font);
            tp.SetLeading(1, 1);
            ct.AddElement(tp);
            ct.Go();
        }

        static void addEAN13(string cod, int x, int y, float k=1) 
        {
            BarcodeEAN ean = new BarcodeEAN
            {
                CodeType = Barcode.EAN13,
                Code = cod,
                Font = TimesNewRomanBase
            };

            Image imageEAN = ean.CreateImageWithBarcode(cb, null, null);
            float eanw = imageEAN.Width;
            float eanh = imageEAN.Height;

            imageEAN.ScaleAbsolute(eanw * k, eanh * k);
            imageEAN.SetAbsolutePosition(mm(x), mm(y));
            document.Add(imageEAN);


        }

    }
}
