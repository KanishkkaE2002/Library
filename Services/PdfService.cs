using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using LibraryManagementApi.Data;
using iText.Layout.Borders;
using iText.Kernel.Pdf.Canvas.Draw;

namespace LibraryManagementApi.Services
{
    public class PdfService
    {
        private readonly LibraryContext _context;
        public PdfService(LibraryContext context)
        {
            _context = context;
        }

        public string GeneratePdf(string username, string phoneNumber, string email, string bookName, DateTime borrowDate, DateTime dueDate)
        {
            // Specify the path where the PDF will be saved
            var pdfPath = Path.Combine(Path.GetTempPath(), $"{username}_BorrowedBookDetails.pdf");

            // Create a PDF document
            using (var writer = new PdfWriter(pdfPath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    var darkBlueColor = new DeviceRgb(0, 0, 139);
                    var headerBgColor = new DeviceRgb(173, 216, 230); // Light blue background for headers

                    // Create a table for the logo and library name
                    var headerTable = new Table(2);
                    headerTable.SetWidth(UnitValue.CreatePercentValue(100)); // Set table width to 100%
                    headerTable.SetMarginBottom(10); // Space below the header

                    // Add logo image
                    string logoPath = "Images/logo.jpg"; // Update this path to your logo file
                    var img = new Image(ImageDataFactory.Create(logoPath))
                        .SetWidth(50) // Set the width of the logo
                        .SetHeight(50); // Set the height of the logo
                    headerTable.AddCell(new Cell().Add(img).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

                    var libraryName = new Paragraph("SERENITY LIBRARY")
                        .SetFontSize(24)
                        .SetBold()
                        .SetFontColor(darkBlueColor)
                        .SetTextAlignment(TextAlignment.CENTER);
                    libraryName.Add(new Paragraph("21, Book Street, CrossCut Nagar, Chennai, Tamil Nadu")
                        .SetFontSize(15)
                        .SetItalic()
                        .SetTextAlignment(TextAlignment.CENTER));
                    headerTable.AddCell(new Cell().Add(libraryName).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));

                    // Add the header table to the document
                    document.Add(headerTable);

                    var line = new LineSeparator(new SolidLine(1)); // Create a line with 1-point thickness
                    line.SetMarginTop(5); // Optional: Add some space above the line
                    document.Add(line);
                    document.Add(new Paragraph("\n"));

                    // Title
                    var receiptTitle = new Paragraph("Borrowed Book Receipt:")
                        .SetFontSize(18)
                        .SetBold()
                        .SetFontColor(darkBlueColor)
                        .SetTextAlignment(TextAlignment.LEFT);
                    document.Add(receiptTitle);

                    // Add space before the table
                    document.Add(new Paragraph("\n"));

                    // Create a table with 2 columns
                    var table = new Table(2);
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Set table width to 100%
                    table.SetPadding(5); // Set padding for table cells

                    // Add table headers with background color
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Field"))
                        .SetBackgroundColor(headerBgColor)
                        .SetBold()
                        .SetPadding(5)
                        .SetTextAlignment(TextAlignment.CENTER));

                    table.AddHeaderCell(new Cell().Add(new Paragraph("Details"))
                        .SetBackgroundColor(headerBgColor)
                        .SetBold()
                        .SetPadding(5)
                        .SetTextAlignment(TextAlignment.CENTER));

                    // Add data to the table with borders and padding
                    table.AddCell(new Cell().Add(new Paragraph("Username")).SetPadding(5));
                    table.AddCell(new Cell().Add(new Paragraph(username)).SetPadding(5));

                    table.AddCell(new Cell().Add(new Paragraph("Phone Number")).SetPadding(5));
                    table.AddCell(new Cell().Add(new Paragraph(phoneNumber)).SetPadding(5));

                    table.AddCell(new Cell().Add(new Paragraph("Email")).SetPadding(5));
                    table.AddCell(new Cell().Add(new Paragraph(email)).SetPadding(5));

                    table.AddCell(new Cell().Add(new Paragraph("Book Name")).SetPadding(5));
                    table.AddCell(new Cell().Add(new Paragraph(bookName)).SetPadding(5));

                    table.AddCell(new Cell().Add(new Paragraph("Borrow Date")).SetPadding(5));
                    table.AddCell(new Cell().Add(new Paragraph(borrowDate.ToShortDateString())).SetPadding(5));

                    table.AddCell(new Cell().Add(new Paragraph("Due Date")).SetPadding(5));
                    table.AddCell(new Cell().Add(new Paragraph(dueDate.ToShortDateString())).SetPadding(5));

                    document.Add(table);

                    // Add some space before adding the next paragraph
                    document.Add(new Paragraph("\n"));

                    // Add additional details
                    var receiptDetails = new Paragraph("Here are the details of the book you have borrowed. Please take a moment to review the information above.")
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.LEFT);
                    document.Add(receiptDetails);
                    document.Add(new Paragraph("\n"));
                    var receiptDetail = new Paragraph("To avoid overdue fines please return the book before the DueDate")
                       .SetFontSize(12)
                       .SetTextAlignment(TextAlignment.LEFT);
                    document.Add(receiptDetail);

                    // Add a footer section with a line separator and a thank you message
                    document.Add(new Paragraph("\n"));
                    var footerLine = new LineSeparator(new SolidLine(1));
                    document.Add(footerLine);

                    var footer = new Paragraph("Thank you for using Serenity Library. We hope you enjoy your book!")
                        .SetFontSize(12)
                        .SetItalic()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(20);
                    document.Add(footer);

                    // Set document margins
                    document.SetMargins(20, 20, 20, 20);

                    document.Close();
                }
            }

            return pdfPath;
        }
    }
}
