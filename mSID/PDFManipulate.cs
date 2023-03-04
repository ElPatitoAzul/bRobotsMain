using BarcodeLib;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;

namespace BackRobotTDM.mSID
{
    public class PDFManipulate
    {

        private static Tools tools = new Tools();
        public string Reader(string src)
        {

            var pdfDocument = new PdfDocument(new PdfReader(src));
            var page = pdfDocument.GetPage(1);
            string text = PdfTextExtractor.GetTextFromPage(page);
            pdfDocument.Close();
            return text;
        }

        public string PrefsManagement(string src, Modelos.Preferences prefType, string estado, Guid UserId, string cadena)
        {
            var pdfDocument = new PdfDocument(new PdfReader(src));
            var strategy = new LocationTextExtractionStrategy();
            var page = pdfDocument.GetPage(1);
            var pageSize = page.GetPageSize();
            var heightPage = pageSize.GetHeight();
            var widthPage = pageSize.GetWidth();
            string text = PdfTextExtractor.GetTextFromPage(page);
            //Foliador Function here -->
            if (text.Contains("Acta de Nacimiento"))
            {
                var idPlace = text.IndexOf("Identificador Electrónico");
                var idContent = text.Substring(idPlace, 200);
                string id = idContent.Split('\n')[1];
                var statePlace = text.IndexOf("Acta de Nacimiento");
                var statePlaceEnd = text.IndexOf("Municipio de Registro");
                var stateSub = text.Substring(statePlace + "Acta de Nacimiento".Length, statePlaceEnd);
                string state = estado;

                var curpPlace = text.IndexOf("Clave Única de Registro de Población");
                var curpContent = text.Substring(curpPlace + "Clave Única de Registro de Población".Length, 19);
                string curp = curpContent.Split('\n')[1];
                if (prefType == Modelos.Preferences.Foliado)
                {
                    string folio = foliador(state, id, UserId);
                    string qrCodeText = $"CURP:{curp}\nIdentificador Electrónico: {id}";
                    QrGenerator(qrCodeText, curp, state, UserId);
                    pdfDocument.Close();
                    editFirstPage(folio, src, UserId);
                    File.Delete(src);
                    File.Move($@"tmp/{UserId}/new.pdf", src);
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else if (prefType == Modelos.Preferences.Reversado)
                {
                    string qrCodeText = $"CURP:{curp}\nIdentificador Electrónico: {id}";
                    QrGenerator(qrCodeText, curp, state, UserId);
                    pdfDocument.Close();
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else return src;


            }
            else if (text.Contains("Acta de Defunción"))
            {

                var idPlace = text.IndexOf("Identificador Electrónico");
                var idContent = text.Substring(idPlace, 200);
                string id = idContent.Split('\n')[1];
                var statePlace = text.IndexOf("Acta de Defunción");
                var statePlaceEnd = text.IndexOf("Municipio de Registro");
                var stateSub = text.Substring(statePlace + "Acta de Defunción".Length, statePlaceEnd);
                string state = estado;
                var curpPlace = text.IndexOf("Clave Única de Registro de Población");
                var curpContent = text.Substring(curpPlace + "Clave Única de Registro de Población".Length, 19);
                string curp = curpContent.Split('\n')[1];

                if (prefType == Modelos.Preferences.Foliado)
                {
                    string folio = foliador(state, id, UserId);
                    string qrCodeText = $"CURP:{curp}\nIdentificador Electrónico: {id}";
                    QrGenerator(qrCodeText, curp, state, UserId);
                    pdfDocument.Close();
                    editFirstPage(folio, src, UserId);
                    File.Delete(src);
                    File.Move($@"tmp/{UserId}/new.pdf", src);
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else if (prefType == Modelos.Preferences.Reversado)
                {
                    string qrCodeText = $"CURP:{curp}\nIdentificador Electrónico: {id}";
                    QrGenerator(qrCodeText, curp, state, UserId);
                    pdfDocument.Close();
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else return src;

            }
            else if (text.Contains("Acta de Matrimonio"))
            {
                var idPlace = text.IndexOf("Identificador Electrónico");
                var idContent = text.Substring(idPlace, 200);
                string id = idContent.Split('\n')[1];
                var statePlace = text.IndexOf("Entidad de Registro");
                var statePlaceEnd = text.IndexOf("Municipio de Registro");
                var stateSub = text.Substring(statePlace + "Entidad de Registro".Length, statePlaceEnd);
                string state = estado;
                if (prefType == Modelos.Preferences.Foliado)
                {
                    string folio = foliador(state, id, UserId);
                    string qrCodeText = $"Identificador Electrónico: {id}";
                    QrGenerator(qrCodeText, "", state, UserId);
                    pdfDocument.Close();
                    editFirstPage(folio, src, UserId);
                    File.Delete(src);
                    File.Move($@"tmp/{UserId}/new.pdf", src);
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else if (prefType == Modelos.Preferences.Reversado)
                {
                    string qrCodeText = $"Identificador Electrónico: {id}";
                    QrGenerator(qrCodeText, "", state, UserId);
                    pdfDocument.Close();
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else return src;

            }
            else if (text.Contains("Acta de Divorcio"))
            {
                var idPlace = text.IndexOf("Identificador Electrónico");
                var idContent = text.Substring(idPlace, 200);
                string id = idContent.Split('\n')[1];
                var statePlace = text.IndexOf("Acta de Divorcio");
                var statePlaceEnd = text.IndexOf("Municipio:");
                var stateSub = text.Substring(statePlace + "Acta de Divorcio".Length, statePlaceEnd);
                string[] dataSplit = stateSub.Split('\n');
                string stateTextdel = dataSplit[0];
                string state = estado;
                if (prefType == Modelos.Preferences.Foliado)
                {
                    string folio = foliador(state, id, UserId);
                    string qrCodeText = $"Identificador Electrónico: {id}";
                    QrGenerator(qrCodeText, "", state, UserId);
                    pdfDocument.Close();
                    editFirstPage(folio, src, UserId);
                    File.Delete(src);
                    File.Move($@"tmp/{UserId}/new.pdf", src);
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else if (prefType == Modelos.Preferences.Reversado)
                {
                    string qrCodeText = $"Identificador Electrónico: {id}";
                    QrGenerator(qrCodeText, "", state, UserId);
                    pdfDocument.Close();
                    var _path = MergerPDF(src, UserId, cadena);
                    return _path;
                }
                else return src;
            }

            pdfDocument.Close();
            return src;

        }


        private void editFirstPage(string folio, string src, Guid UserId)
        {
            tools._PATHER_($@"tmp/{UserId}");
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(src), new PdfWriter($@"tmp/{UserId}/new.pdf"));
            Document document = new Document(pdfDocument);
            var bcode = $@"tmp/{UserId}/barcode.png";
            iText.Layout.Element.Image lngImg = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(bcode));
            lngImg.ScaleAbsolute(100f, (float)14);
            lngImg.SetFixedPosition(60f, 700f);
            document.Add(lngImg);
            var arial = PdfFontFactory.CreateFont("fonts/ArialMT.ttf");
            var strVal = new Text("FOLIO");
            var val = new Paragraph();
            val.Add(strVal);
            val.SetFontSize((float)14);
            val.SetMarginLeft(57);
            val.SetMarginTop(13);
            document.Add(val);
            var fol = new Paragraph();
            var strFol = new Text(folio);
            fol.Add(strFol);
            fol.SetFontSize((float)12);
            fol.SetMarginLeft(40);
            fol.SetMarginTop(-11);
            document.Add(fol);
            document.Close();
        }

        private string foliador(string state, string id, Guid UserId)
        {
            var paraph = a00(state);
            var idLast = id.Substring(13);
            string data = paraph + " " + idLast;
            genCodebar(data, UserId);
            return data;
        }

        private void genCodebar(string data, Guid UserId)
        {
            tools._PATHER_($@"tmp/{UserId}");
            Barcode Code = new Barcode();
            Code.IncludeLabel = false;
            var code = Code.Encode(TYPE.CODE128, data, Color.Black, Color.White, 350, 5);

            System.Drawing.Image img = (System.Drawing.Image)code;
            img.Save($@"tmp/{UserId}/barcode.png", ImageFormat.Png);

        }
        private string a00(string state)
        {
            switch (state)
            {
                case "AGUASCALIENTES":
                    return "A01";
                case "BAJA CALIFORNIA":
                    return "A02";
                case "BAJA CALIFORNIA SUR":
                    return "A03";
                case "CAMPECHE":
                    return "A04";
                case "COAHUILA DE ZARAGOZA":
                    return "A05";
                case "COLIMA":
                    return "A06";
                case "CHIAPAS":
                    return "A07";
                case "CHIHUAHUA":
                    return "A08";
                case "DISTRITO FEDERAL":
                    return "A09";
                case "DURANGO":
                    return "A10";
                case "GUANAJUATO":
                    return "A11";
                case "GUERRERO":
                    return "A12";
                case "HIDALGO":
                    return "A13";
                case "JALISCO":
                    return "A14";
                case "MEXICO":
                    return "A15";
                case "CIUDAD DE MEXICO":
                    return "A15";
                case "MICHOACAN DE OCAMPO":
                    return "A16";
                case "MORELOS":
                    return "A17";
                case "NAYARIT":
                    return "A18";
                case "NUEVO LEON":
                    return "A19";
                case "OAXACA":
                    return "A20";
                case "PUEBLA":
                    return "A21";
                case "QUERETARO DE ARTEAGA":
                    return "A22";
                case "QUINTANA ROO":
                    return "A23";
                case "SAN LUIS POTOSI":
                    return "A24";
                case "SINALOA":
                    return "A25";
                case "SONORA":
                    return "A26";
                case "TABASCO":
                    return "A27";
                case "TAMAULIPAS":
                    return "A28";
                case "TLAXCALA":
                    return "A29";
                case "VERACRUZ":
                    return "A30";
                case "YUCATAN":
                    return "A31";
                case "ZACATECAS":
                    return "A32";
                case "NACIDO EN EL EXTRANJERO":
                    return "A39";
                default: return "";
            }
        }

        public void QrGenerator(string qrContent, string curpText, string stateText, Guid UserId)
        {
            tools._PATHER_($"tmp/{UserId.ToString()}");
            //QR CODE 1
            //QRCodeGenerator qrGen1 = new QRCodeGenerator();
            //QRCodeData qrDatos1 = qrGen1.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.H);
            //var qrCode1 = new QRCode(qrDatos1);
            //Bitmap Img1 = qrCode1.GetGraphic(5, Color.Black, Color.White, true);
            //Img1.Save(@"tmp/qrcode.png", ImageFormat.Png);
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            encoder.QRCodeBackgroundColor = Color.White;
            encoder.QRCodeForegroundColor = Color.Black;
            Bitmap qr1 = encoder.Encode(qrContent);
            qr1.Save($@"tmp/{UserId}/qrcode.png", ImageFormat.Png);

            //GeneratedBarcode barcode1 = QRCodeWriter.CreateQrCode(qrContent.ToString(), 5, QRCodeWriter.QrErrorCorrectionLevel.Highest);
            //barcode1.ChangeBackgroundColor(Color.White);
            //barcode1.ChangeBarCodeColor(Color.Black);
            //barcode1.SaveAsPng(@"tmp/qrcode.png");

            //QR CODE 2
            string urlBase = "https://cevar.registrocivil.gob.mx/eVAR/ConsultaFolio.jsp";
            //Url url = new Url(urlBase);
            QRCodeEncoder encoder2 = new QRCodeEncoder();
            encoder2.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            encoder2.QRCodeScale = 5;
            encoder2.QRCodeBackgroundColor = Color.White;
            encoder2.QRCodeForegroundColor = Color.Black;
            Bitmap qr2 = encoder2.Encode(urlBase);
            qr2.Save($@"tmp/{UserId}/qrcode2.png", ImageFormat.Png);


            string path = $@"tmp/{UserId}/generate.pdf";
            using (PdfWriter wpdf = new PdfWriter(path, new WriterProperties().SetPdfVersion(PdfVersion.PDF_1_0)))
            {
                var pdfDoc = new PdfDocument(wpdf);
                var doc = new Document(pdfDoc, PageSize.LETTER);
                //ArialMT, 10 Curp
                var arial = PdfFontFactory.CreateFont("fonts/ArialMT.ttf");
                var cpr = new Paragraph();
                cpr.SetFont(arial);
                cpr.SetFontSize((float)4.20);
                cpr.SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY);
                cpr.SetRelativePosition(-2, (float)50.5, 1, 1);
                cpr.Add(curpText);
                doc.Add(cpr);
                //QR 50x50
                var qrcod = $@"tmp/{UserId}/qrcode.png";

                iText.Layout.Element.Image img = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(qrcod));
                img.ScaleAbsolute(50, 50);
                img.SetFixedPosition(32f, 701f);
                doc.Add(img);
                //SEGOB LOGO
                var segob = @"img/segob.png";
                iText.Layout.Element.Image segobImg = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(segob));
                segobImg.ScaleAbsolute(213, (float)67.2);
                segobImg.SetFixedPosition(96f, 696.5f);
                doc.Add(segobImg);
                //LEGEND LOGO
                var lgn = @"img/legend.png";
                iText.Layout.Element.Image lngImg = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(lgn));
                //lngImg.Scale(0.2001f,0.2001f);
                lngImg.ScaleAbsolute(170.5f, (float)28.5);
                lngImg.SetFixedPosition(314.5f, 715f);
                doc.Add(lngImg);
                //CONAFREC LOGO
                var cnf = @"img/conafrec.png";
                iText.Layout.Element.Image cnfImg = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(cnf));
                cnfImg.ScaleAbsolute(88.5f, 89f);
                cnfImg.SetFixedPosition(498.5f, 687.5f);
                doc.Add(cnfImg);
                //STATE TEXT
                var stateSet = new Paragraph();
                stateSet.SetFont(arial);
                stateSet.SetFontSize(18f);
                stateSet.SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY);
                stateSet.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                stateSet.SetRelativePosition(1, (float)475, 1, 1);
                stateSet.Add(stateText);
                doc.Add(stateSet);
                //State Logo
                var stateLogoPath = $@"states/{stateText}.png";
                iText.Layout.Element.Image stateImg = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(stateLogoPath));
                stateImg.ScaleToFit(230f, 230f);
                float x = (PageSize.A4.GetHeight() - stateImg.GetImageHeight()) / 2;
                float y = (PageSize.A4.GetWidth() - stateImg.GetImageWidth()) / 2;
                iText.Layout.Element.Image image = stateImg.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                stateImg.SetMarginTop(200f);
                doc.Add(stateImg);
                //Ver
                string strVal = "Validación";
                var val = new Paragraph();
                val.SetFont(arial);
                val.SetFontSize((float)8.5);
                val.SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY);
                val.SetMarginLeft(8);
                val.SetMarginTop(122);
                val.Add(strVal);
                doc.Add(val);
                //QRCode 2
                var qr2Path = $@"tmp/{UserId}/qrcode2.png";
                iText.Layout.Element.Image qr2Img = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(qr2Path));
                qr2Img.ScaleAbsolute(50, 50);
                qr2Img.SetFixedPosition(38.5f, 75f);
                doc.Add(qr2Img);
                //GOB Logo
                var gobLogo = @"img/gob.png";
                iText.Layout.Element.Image gobImg = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(gobLogo));
                gobImg.ScaleToFit(44.4f, 18.5f);
                gobImg.SetFixedPosition(42f, 128f);
                doc.Add(gobImg);
                //FINISH
                doc.Close();
                pdfDoc.Close();
            }
        }

        private byte[] Bmp2Byte(Bitmap bpm)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bpm, typeof(byte[]))!;
        }

        public void QrCreator(string content)
        {
            //QR CODE 1
            //QRCodeGenerator qrGen1 = new QRCodeGenerator();
            //QRCodeData qrDatos1 = qrGen1.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
            //QRCode qrCode1 = new QRCode(qrDatos1);
            //Bitmap Img1 = qrCode1.GetGraphic(5, Color.Black, Color.White, true);
            //Img1.Save(@"tmp/protectQR.png", ImageFormat.Png);


            //QRCodeEncoder encoder2 = new QRCodeEncoder();
            //encoder2.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            //encoder2.QRCodeScale = 5;
            //encoder2.QRCodeBackgroundColor = Color.White;
            //encoder2.QRCodeForegroundColor = Color.Black;
            //Bitmap qr2 = encoder2.Encode(content);

            //System.IO.FileStream fs2 = System.IO.File.Open(@"tmp/qrcode.png", FileMode.Create);
            //qr2.Save(fs2, ImageFormat.Png);
            //qr2.Dispose();

        }



        private string MergerPDF(string fileUrl, Guid ID_REQ, string cadena)
        {
            string stc = $@"tmp/{ID_REQ}/{ID_REQ}-{cadena}.pdf";
            string[] src = fileUrl.Split('\\');
            string srcOut = "";
            for (int b = 0; b < src.Length - 1; b++)
            {
                srcOut += src[b] + '\\';
            }
            string nameOriginal = src.Last();
            string[] nameOriginalParts = nameOriginal.Split('.');
            string newName = nameOriginalParts[0] + "_completo.pdf";
            string[] files = { fileUrl, $@"tmp/{ID_REQ}/generate.pdf" };
            PdfDocument pdf = new PdfDocument(new PdfWriter(srcOut + newName));
            PdfMerger merger = new PdfMerger(pdf);
            PdfDocument firstSourcePdf = new PdfDocument(new PdfReader(files[0]));
            merger.Merge(firstSourcePdf, 1, firstSourcePdf.GetNumberOfPages());
            PdfDocument secondSourcePdf = new PdfDocument(new PdfReader(files[1]));
            merger.Merge(secondSourcePdf, 1, secondSourcePdf.GetNumberOfPages());
            firstSourcePdf.Close();
            secondSourcePdf.Close();
            pdf.Close();
            File.Delete(stc);
            File.Move(srcOut + newName, stc);
            return stc;
        }

        public void Enmarcar(string src, Guid ID_REQ, string srcOut)
        {
            tools._PATHER_($@"tmp/{ID_REQ.ToString()}/");
            //SOURCES
            String imageFile = "tmp/marco.png";
            String outputFile = $@"tmp/{ID_REQ.ToString()}/enmarcado.pdf";
            //LOGIC FORM
            var data = ImageDataFactory.Create(imageFile);
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(outputFile));
            PdfPage page = pdfDoc.GetPage(1);
            iText.Kernel.Geom.Rectangle pageSize = page.GetPageSize();
            pageSize.ApplyMargins(15, 12, 2, 15, false);
            PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            pdfCanvas.BeginText().AddImageFittedIntoRectangle(data, pageSize, true);
            pdfCanvas.Release();
            pdfDoc.Close();
            //File.Delete(src);
            File.Move(outputFile, srcOut);
        }
    }
}
