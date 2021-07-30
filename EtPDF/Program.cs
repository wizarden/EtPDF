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







        public static bool PrintPDF(string printer, string paperName, int wi, int he, string filename, int copies)
        {
            
            try
            {
                //printer = Console.ReadLine();

                using (var document = PdfiumViewer.PdfDocument.Load(filename))
                {
                    using (var printDocument = document.CreatePrintDocument(PdfPrintMode.CutMargin))
                    {

                        try
                        {


                            printDocument.PrinterSettings.PrinterName = printer;
                            printDocument.OriginAtMargins = true;

                            var printerSettings = new PrinterSettings
                            {
                                PrinterName = printer,
                                Copies = (short)copies,
                            };

                            var pageSettings = new PageSettings(printerSettings);

                            foreach (PaperSize paperSize in printerSettings.PaperSizes)
                            {

                                if (paperSize.PaperName == paperName)
                                {
                                    Console.WriteLine("+++");
                                    printDocument.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;
                                    printDocument.PrinterSettings.DefaultPageSettings.Landscape = false; ////??? 


                                    //PaperSize paperSize = new PaperSize("Test", (int)(wi / 0.254), (int)(he / 0.254));
                                    printDocument.DefaultPageSettings.PaperSize.RawKind = (int)PaperKind.Custom;

                                    printDocument.PrinterSettings.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
                                    printDocument.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
                                    break;
                                }
                            }







                            printDocument.PrintController = new StandardPrintController();
                            Console.WriteLine(">>" + printDocument.PrinterSettings.DefaultPageSettings);


                            printDocument.Print();


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


        //------------------------------------------------------------------------------------------------------------phm 20x13
        //------------------------------------------------------------------------------------------------------------svlogo 50x9
        //------------------------------------------------------------------------------------------------------------znaki 15x19
        //------------------------------------------------------------------------------------------------------------



        static Document document;
        static BaseFont TimesNewRomanBase;
        static BaseFont TimesNewRomanBaseBold;
        static PdfContentByte cb;
        static int OX = 0, OY = 0;
        const int wmargin = 1;

        static string PDFName = Path.GetTempPath() + "my.pdf";
        static string barcode = "";
        static string title = "";
        static string name = "";
        static string modul = "";
        static string color = "";
        static string up = "";
        static string ups = "";
        static string typeup = "";
        static bool furn = false;
        static string packer = "";
        static int upcol = 1;
        static string trademark = "";
        static string garant = "";
        static string regnum = "";
        static string sertifikatdata = "";
        static string data = "";
        static int print = 0;
        static int razmer = 9;
        static Dictionary<string, object> d;


        static int mm(double t)
        {
            return (int)Math.Round(t * 72 / 25.4);
        }

        static void Main(string[] args)
        {
            //consPrinterName();
          

            if (args.Length > 0)
            {
                //init JSON STRING
                {
                    string json = String.Join(" ", args).Replace(@"\'", "\"");
                    Console.WriteLine(json);
                    d = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (d.ContainsKey("pdfname")) PDFName = d["pdfname"].ToString();
                    if (d.ContainsKey("barcode")) barcode = d["barcode"].ToString();
                    if (d.ContainsKey("title")) title = d["title"].ToString();
                    if (d.ContainsKey("name")) name = d["name"].ToString();
                    if (d.ContainsKey("modul")) modul = d["modul"].ToString();
                    if (d.ContainsKey("color")) color = d["color"].ToString();
                    if (d.ContainsKey("up")) up = d["up"].ToString();
                    if (d.ContainsKey("ups")) ups = d["ups"].ToString();
                    if (d.ContainsKey("typeup")) typeup = d["typeup"].ToString();
                    if (d.ContainsKey("furn")) furn = (bool)d["furn"];
                    if (d.ContainsKey("packer")) packer = d["packer"].ToString();
                    if (d.ContainsKey("upcol")) upcol = Convert.ToInt32(d["upcol"]);
                    if (d.ContainsKey("trademark")) trademark = d["trademark"].ToString();
                    if (d.ContainsKey("garant")) garant = d["garant"].ToString();
                    if (d.ContainsKey("regnum")) regnum = d["regnum"].ToString();
                    if (d.ContainsKey("sertifikatdata")) sertifikatdata = d["sertifikatdata"].ToString();
                    if (d.ContainsKey("data")) data = d["data"].ToString();
                    if (d.ContainsKey("print")) print = Convert.ToInt32(d["print"]);
                    if (d.ContainsKey("razmer")) razmer = Convert.ToInt32(d["razmer"]);
                }

            }


            Console.WriteLine(PDFName);
            //Console.ReadKey();


            float wi = (float)(90 / 25.4 * 72);
            float he = (float)(70 / 25.4 * 72);
            if (razmer == 9)
            {
                wi = (float)(210 / 25.4 * 72);
                he = (float)(297 / 25.4 * 72);
            }

            //-------------------------------------------------------------------------------------------------------------
            string t = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            TimesNewRomanBase = BaseFont.CreateFont(t, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            t = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "timesbd.TTF");
            TimesNewRomanBaseBold = BaseFont.CreateFont(t, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //-------------------------------------------------------------------------------------------------------------



            var pgSize = new iTextSharp.text.Rectangle(wi, he);

            document = new Document(pgSize, 0f, 0f, 0f, 0f);





            using (PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(PDFName, FileMode.Create)))
            {
                document.Open();
                cb = writer.DirectContent;
                for (int i = 0; i < 4; i++)
                {
                    OX = 10; OY = 5 + i * mm(72);
                    E90X60(writer);
                    OX = mm(100); OY = 5 + i * mm(72);
                    E90X60(writer);
                }
                document.Close();
                writer.Close();
            }




            // принтер -- имя бумаги -- ширина бумаги -- высота бумаги -- имя файла -- количество копий
            // PrintPDF("NPIAE4A8A (HP LaserJet M402n)", "A4", 28, 150, PDFName, 1);


            if (print == 1) PrintPDF("NPIAE4A8A (HP LaserJet M402n)", "A4", 28, 150, PDFName, 1);
           // if (print == 1) PrintPDF("Microsoft Print to PDF", "A4", 28, 150, PDFName, 1);



            // Console.ReadLine();



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

            cb.AddTemplate(page, (w / page.Width) / 25.4 * 72, 0, 0, (h / page.Height) / 25.4 * 72, mm(x) + OX, mm(y) + OY);
            Console.WriteLine("---" + page.Width);
            Console.WriteLine("---" + page.Height);

        }

        static void addRectangle(int x, int y, int w, int h, double border)
        {
            cb.SetLineWidth(mm(border));
            cb.Rectangle(mm(x) + OX, mm(y) + OY, mm(w), mm(h));
            cb.Stroke();
        }

        static void addRoundRectangle(int x, int y, int w, int h, double border, double radius)
        {
            cb.SetLineWidth(border / 24.5 * 72);
            cb.RoundRectangle(mm(x) + OX, mm(y) + OY, mm(w), mm(h), mm(radius));
            cb.Stroke();
        }
        static void addText(string text, int x, int y, float w, float fontSize, int ug = 0, int align = Element.ALIGN_LEFT, BaseFont font = null, double border = 0)
        {
            float ww = 0;
            cb.SetHorizontalScaling(100);
            cb.BeginText();

            if (font == null)
            {
                cb.SetFontAndSize(TimesNewRomanBase, mm(fontSize));
                ww = TimesNewRomanBase.GetWidthPoint(text, fontSize);
            }
            else
            {
                cb.SetFontAndSize(font, mm(fontSize));
                ww = font.GetWidthPoint(text, fontSize);
            }
            Console.WriteLine(ww + " " + mm(ww) + " " + mm(w));


            if (mm(ww) > mm(w)) cb.SetHorizontalScaling(w / ww * 100);

            if (align == Element.ALIGN_CENTER)
                cb.ShowTextAligned(align, text, mm(x) + OX + mm(w) / 2, mm(y) + OY, ug);
            else
                cb.ShowTextAligned(align, text, mm(x) + OX, mm(y) + OY, ug);
            cb.EndText();

            //if (border > 0) { addRectangle(mm(x) + OX, mm(y) + OY, mm(w), 1, border); }

            cb.SetHorizontalScaling(100);
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



            cb.ShowTextAligned(Element.ALIGN_CENTER, text, mm(x) + mm(w) / 2 + OX, mm(y) + mm(h / 2) - mm(fontSize / 3) + OY, ug);
            cb.EndText();

            addRectangle(x + OX, y + OY, w, h, 1);


            cb.SetHorizontalScaling(100);

        }

        static void addWrapText(string text, int x, int y, int w, int h, float f)
        {
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(mm(x) + OX, mm(y) + OY, mm(x) + mm(w) + OX, mm(y) + mm(h) + OY);


            Font font = new iTextSharp.text.Font(TimesNewRomanBase, mm(f), Font.NORMAL);

            Paragraph tp = new Paragraph(text, font);
            tp.Alignment = Element.ALIGN_CENTER;
            tp.SetLeading(1, 1);

            ct.AddElement(tp);
            ct.Go();
        }

        static void addEAN13(string cod, int x, int y, float k = 1)
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
            imageEAN.SetAbsolutePosition(mm(x) + OX, mm(y) + OY);
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

            imageEAN.SetAbsolutePosition(mm(x) + OX, mm(y) + OY);
            document.Add(imageEAN);


        }

        static void consPrinterName()
        {
            PrinterSettings printer = new PrinterSettings();
            foreach (var item in PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine(item);
                Console.WriteLine("------------------");
                consPaperName(item.ToString());
                Console.WriteLine();
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



        static void E90X60(PdfWriter writer)
        {
           

            addRoundRectangle(1, 1, 88, 68, 0.5, 3);
            addResourcesPDF(writer, Properties.Resources.svlogo, 2, 56, 50, 9);
            addEAN13(barcode, 55, 52, 31, 15);

            if (title.Length > 50)
            {
                addWrapText(title, 1, 44, 88, 10, 4);
            }
            else
            {
                addText(title, 3, 45, 85, 5, 0, Element.ALIGN_CENTER);
            }

            addText(name, 3, 36, 84, 10, 0, Element.ALIGN_CENTER, TimesNewRomanBaseBold);
            addText(modul, 3, 29, 84, 8, 0, Element.ALIGN_CENTER);
            addText(color, 3, 16, 84, 10, 0, Element.ALIGN_CENTER, TimesNewRomanBaseBold);
            addRectangle(2, 14, 86, 12, 0.8);
            addText(up + "/" + ups, 3, 5, 20, 10, 0, Element.ALIGN_CENTER, TimesNewRomanBaseBold);
            addRectangle(2, 3, 22, 10, 0.2);
            addText(packer, 26, 5, 30, 8, 0, Element.ALIGN_CENTER, TimesNewRomanBaseBold);
            addText(data, 58, 5, 30, 10, 0, Element.ALIGN_CENTER, TimesNewRomanBaseBold);
            addRectangle(57, 3, 31, 10, 0.2);


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


        }

    
    
    
    }
}
