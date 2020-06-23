using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CreateFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please press any key to create sample PDF invoice");
            //Console.ReadLine();
            //initialize content
            var r = new Random();
            var invoiceNo = r.Next(1, 100);
            var companyInfo = new string[] { "Company Name", "Street Address", "City, ST ZIP Code", "Phone", "Customer ID ABC12345"};
            var tableHeaders = new string[] { "Qty", "Description", "Unit Price", "Line Total"};
            string[,] tableData = new string[2, 4] { { "10", "Vanilla Icecream", "10 USD" , "100 USD"}, { "2", "Chocolate", "20 USD", "40 USD" } };
            string[,] totalData = new string[3, 2] { { "Subtotal", "140 USD" }, { "Sales Tax", "" }, { "Total", "140 USD" } };

            using (Document document = new Document(PageSize.A4, 50, 50, 100, 100))
            {
                PdfWriter.GetInstance(document, new FileStream($"{Environment.CurrentDirectory}\\invoice{invoiceNo}.pdf", FileMode.Create));
                document.Open();
                //common styles
                var baseColor = new BaseColor(90, 90, 90);
                Font title = FontFactory.GetFont("Arial", size: 30.0f, baseColor);
                Font mainFont = FontFactory.GetFont("Arial", 10.0f, baseColor);

                //INVOICE header table
                var table = new PdfPTable(2);
                table.WidthPercentage = 100;
                //image cell
                var image = Image.GetInstance("tvtable.jpg");
                image.Alignment = Image.ALIGN_LEFT;
                image.ScaleToFit(100, 50);
                var cell = new PdfPCell(image);cell.HorizontalAlignment = Element.ALIGN_LEFT;cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                table.AddCell(cell);
                //INVOICE
                Paragraph p = new Paragraph();
                p.Alignment = Element.ALIGN_RIGHT;
                cell = new PdfPCell(new Phrase($"INVOICE", title));cell.HorizontalAlignment = Element.ALIGN_RIGHT; cell.VerticalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                table.AddCell(cell);

                table.SpacingAfter = 20;    
                document.Add(table);

                //company info table
                table = new PdfPTable(4);
                table.WidthPercentage = 40;
                table.SpacingAfter = 30;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                p = new Paragraph($"Date: {DateTime.Now:dd-MM-yyyy}", font: mainFont); p.Alignment = Element.ALIGN_RIGHT;
                document.Add(p);
                p = new Paragraph($"INVOICE # {invoiceNo}", font: mainFont); p.Alignment = Element.ALIGN_RIGHT; p.SpacingAfter = 50;
                document.Add(p);

                cell = new PdfPCell(new Phrase($"To", mainFont)); cell.HorizontalAlignment = Element.ALIGN_LEFT; cell.Border = 0; 
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase($"Name", mainFont)); cell.HorizontalAlignment = Element.ALIGN_RIGHT;cell.Colspan = 3; cell.Border = 0; 

                table.AddCell(cell);
                foreach(var info in companyInfo)
                {
                    cell = new PdfPCell(new Phrase($"")); cell.Border = 0;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase($"{info}", mainFont)); cell.HorizontalAlignment = Element.ALIGN_RIGHT;cell.Colspan = 3; cell.Border = 0; 
                    table.AddCell(cell);
                }
                document.Add(table);

                //invoice data table
                table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new int[] { 1, 3, 1, 1 });
                Font boldFont = FontFactory.GetFont("Arial", 10.0f, Font.BOLD, baseColor);
                //headers
                foreach(var head in tableHeaders)
                {
                    cell = new PdfPCell(new Phrase(head, boldFont)); cell.HorizontalAlignment = Element.ALIGN_LEFT; cell.Border = 0; cell.PaddingBottom = 5;
                    table.AddCell(cell);
                }
                //values
                foreach(var data in tableData)
                {
                    cell = new PdfPCell(new Phrase(data, mainFont)); cell.HorizontalAlignment = data.Contains("USD") ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT; cell.PaddingBottom = 5;
                    table.AddCell(cell);
                }
                //totals
                for (var i = 0; i < totalData.GetLength(0); i++) 
                {
                    cell = new PdfPCell(new Phrase("")); cell.Border = 0;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase("")); cell.Border = 0;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(totalData[i, 0], mainFont)); cell.HorizontalAlignment = Element.ALIGN_LEFT; cell.Border = 0; cell.PaddingBottom = 5;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(totalData[i, 1], mainFont)); cell.HorizontalAlignment = Element.ALIGN_RIGHT; cell.PaddingBottom = 5;
                    table.AddCell(cell);
                }

                document.Add(table);
            }



            Console.WriteLine($"sample {invoiceNo} just got created.");
        }
    }

}
