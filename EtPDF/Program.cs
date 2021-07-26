using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using PdfiumViewer;
using System.Drawing.Printing;


//https://docs.microsoft.com/ru-ru/troubleshoot/windows-client/shell-experience/command-line-string-limitation#:~:text=%D0%BD%D0%BE%D0%BC%D0%B5%D1%80%20%D0%9A%D0%91%3A%20830473-,%D0%94%D0%BE%D0%BF%D0%BE%D0%BB%D0%BD%D0%B8%D1%82%D0%B5%D0%BB%D1%8C%D0%BD%D1%8B%D0%B5%20%D1%81%D0%B2%D0%B5%D0%B4%D0%B5%D0%BD%D0%B8%D1%8F,%D0%BA%D0%BE%D0%BC%D0%B0%D0%BD%D0%B4%D0%BD%D0%B0%D1%8F%20%D1%81%D1%82%D1%80%D0%BE%D0%BA%D0%B0

namespace EtPDF
{
    class Program
    {



        public static bool PrintPDF(string printer, int wi, int he, string filename, int copies)
        {
            
            try
            {
                printer = Console.ReadLine();

                    using (var document = PdfiumViewer.PdfDocument.Load(filename))
                    {
                        using (var printDocument = document.CreatePrintDocument(PdfPrintMode.CutMargin))
                        {

                        try
                        {
                            

                            printDocument.PrinterSettings.PrinterName = printer;
                            printDocument.OriginAtMargins = true;
                           

                            PaperSize paperSize = new PaperSize("Test", (int)(wi/0.254), (int)(he/0.254));
                            //paperSize.RawKind = (int)PaperKind.Custom;
                            printDocument.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;
                            printDocument.DefaultPageSettings.PaperSize = paperSize;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);



                            printDocument.PrintController = new StandardPrintController();
                            Console.WriteLine(">>" + printDocument.PrinterSettings.DefaultPageSettings);
                            //printDocument.Print();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
        }


        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------



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

            consPrinterName();

            string PDFName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\my.pdf"; ;
            
            if (args.Length > 0)
            {
                string json = String.Join(" ", args);
                Console.WriteLine(json);
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (values.ContainsKey("pdfname"))
                    PDFName = values["pdfname"].ToString();
            }


           


            int wi = Convert.ToInt32(150 / 25.4 * 72);
            int he = Convert.ToInt32(28 / 25.4 * 72);


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

            
          
           
            //PdfReader reader;

            using (PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(PDFName, FileMode.Create)))
            {
                document.Open();
                cb = writer.DirectContent;

                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
                document.NewPage();
                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
                document.NewPage();
                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
                document.NewPage();
                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
                document.NewPage();
                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
                document.NewPage();
                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
                document.NewPage();
                addRoundRectangle(1, 1, 148, 26, 0.5, 3);
                addEAN13("2520439000412", 2, 2, 30, 20);
               


                //addWrapText("ИП Стеклянников В.М., Юр.адрес: Россия, 440045 г.Пенза, ул.Ладожская 155 - 77.Производство: Россия, 440015, г.Пенза, ул.Аустрина, д. 164.Хранить в крытых"
                //    + "помещениях при температуре не ниже 10°С и отн. влажности воздуха от 40% до 60%. Гарант. срок эксплуатации-3 года. Срок службы-15 лет.ГОСТ 16371-2014,ГОСТ 19917-2014"
                //    + "ТР ТС 025/2012, Декларация о соответствии ЕАЭС №RU Д-RU.РА01.B.58427/21, срок действия по 04.05.2026 г."
                //    , 10, 2, 190, 5, 5f);


                //PdfImportedPage page;

                //addResourcesPDF(writer, Properties.Resources.znaki, 0, 0, 105, 105);



                //reader = new PdfReader(Properties.Resources.svlogo);                
                //page = writer.GetImportedPage(reader, 1);
                //cb.AddTemplate(page, 2, 0, 0, 2, 50, 100);


                ////Properties.Resources.svlogo

                //// addRectangle(50, 50, 50, 50, 1);
                //// addRoundRectangle(50, 50, 100, 100, 1, 5);

                //// addText("AAAAAA", 50, 50, 50, 20f, 45);
                //addBorderText("ФАСАД ВЕРХ", 5, 100, 200, 100, 50f, false);
                ////addBorderText("ФАСАД ВЕРХ", 5, 130, 200, 100, 50f, true);





                //addEAN13("2520439000412", 10,50, 1.5f);

                document.Close();
                writer.Close();

            }

            PrintPDF("NPIAE4A8A (HP LaserJet M402n)", 28, 150, PDFName, 1);


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


        static void addResourcesPDF(PdfWriter writer, byte[] templatePDF, int x, int y, int w, int h)
        {
            PdfReader reader = new PdfReader(templatePDF);
            PdfImportedPage page = writer.GetImportedPage(reader, 1);

            cb.AddTemplate(page, (w / page.Width) / 25.4 * 72, 0, 0, (h / page.Height) / 25.4 * 72, x, y);
            Console.WriteLine("---" + page.Width);
            Console.WriteLine("---" + page.Height);

        }

        static void addRectangle(int x, int y, int w, int h, int border)
        {
            cb.SetLineWidth(mm(border));
            cb.Rectangle(mm(x), mm(y), mm(w), mm(h));
            cb.Stroke();
        }

        static void addRoundRectangle(int x, int y, int w, int h, double border, double radius)
        {
            cb.SetLineWidth(border/24.5*72);        
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


        static void addEAN13(string cod, int x, int y, int w, int h)
        {

            BarcodeEAN ean = new BarcodeEAN
            {
                CodeType = Barcode.EAN13,
                Code = cod,
                Font = TimesNewRomanBase
            };

            Image imageEAN = ean.CreateImageWithBarcode(cb, null, null);
            //float eanw = imageEAN.Width;
            //float eanh = imageEAN.Height;
            //Console.WriteLine(eanw / eanh); //2,372018

            imageEAN.ScaleAbsolute(mm(w), mm(h));

            imageEAN.SetAbsolutePosition(mm(x), mm(y));
            document.Add(imageEAN);


        }

        static void consPrinterName()
        {
             PrinterSettings printer = new PrinterSettings();
             foreach (var item in PrinterSettings.InstalledPrinters)
             {
                Console.WriteLine(item);
                Console.WriteLine("------------------");
                //consPaperName(item.ToString());
                //Console.WriteLine();
             }
        }


        static void consPaperName(string printer)
        {

            var printerSettings = new PrinterSettings
            {
                PrinterName = printer              
            };

            foreach (PaperSize paperSize in printerSettings.PaperSizes)
            {
                Console.WriteLine(paperSize.PaperName);
            }
        }

    }
}
