using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletKeeper
{
    class Pdf
    {
        public static void GeneratePDF()
        {
            //create a document object
            var doc = new Document();
            //get PdfWriter object
            PdfWriter.GetInstance(doc, new FileStream("../../DOCS/pdfdoc.pdf", FileMode.Create));
            //open the document for writing
            doc.Open();
            //write a paragraph to the document
            doc.Add(new Paragraph("Hello World"));
            //close the document
            doc.Close();
            //view the result pdf file
            //System.Diagnostics.Process.Start("../../DOCS/pdfdoc.pdf");
        }
    }
}
