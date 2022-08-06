using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using StudyStipendCalcAPI.DTOs.FileDtos;
using StudyStipendCalcAPI.DTOs.StudentDtos;

namespace StudyStipendCalcAPI.Services.FileServices
{
    //Template file helye CentOS-en: /Resources/UniID/Templates
    public static class FileGeneratorFactory
    {
        //nem sablonra megy a generálás
        public static string GenerateTanulmányiBizonylat(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_bizonylat_tanulmanyi_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;

            //a lapon található táblázat
            var tables = body.Descendants<Table>();

            //iktatószám hozzáadása
            var rows = tables.FirstOrDefault().Descendants<TableRow>();
            TableCell cell = rows.ElementAt(1).Descendants<TableCell>().FirstOrDefault();
            Paragraph paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("XXX")).FirstOrDefault();
            string text = paragraph.InnerText;
            string modifiedText = Regex.Replace(text, "XXX", generateDto.Tan_BizonylatIkatoszam);
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //ma dátum hozzáadása
            rows = tables.ElementAt(1).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().Last();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Számla")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //összeg kitöltése
            rows = tables.ElementAt(2).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Összeg")).FirstOrDefault();
            text = paragraph.InnerText;
            string sumtext = generateDto.Month.Split("-").Count() > 1 ? (generateDto.StipendSum * 2).ToString("#,0", nfi) :
                generateDto.StipendSum.ToString("#,0", nfi);
            modifiedText = text + sumtext + " Ft";
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //fizetési határidő kitöltés
            cell = rows.FirstOrDefault().Descendants<TableCell>().LastOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("határidő")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = text + new DateTime(DateTime.Today.Year, DateTime.Now.Day < 15 ? DateTime.Today.Month : DateTime.Today.AddMonths(1).Month, 10).ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //jogcím kitöltése
            rows = tables.ElementAt(6).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Tanulmányi")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = Regex.Replace(text, "CCC", generateDto.MonthName);
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //témaszám kitöltése
            rows = tables.ElementAt(7).Descendants<TableRow>();
            cell = rows.ElementAt(1).Descendants<TableCell>().ElementAt(1);
            paragraph = cell.Descendants<Paragraph>().FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + generateDto.Tan_Temanumber;
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //funkcioterület kitöltése
            cell = rows.ElementAt(1).Descendants<TableCell>().ElementAt(3);
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("")).FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + generateDto.Funkcioterulet;
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //mai dátumok kitöltése
            rows = tables.ElementAt(8).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Dátum")).FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //mai dátum
            cell = rows.LastOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Dátum")).FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            string savepath = WordEditor.SaveFile(path, generateDto.Roleid, generateDto.Uniid, $"{generateDto.Tan_BizonylatIkatoszam.Replace('/','-')}_Tanulmányi bizonylatkisérő.docx");

            editeddocument.SaveAs(savepath).Close();
            return savepath;
        }
        public static string GenerateKözeletiBizonylat(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_bizonylat_kozeleti_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;

            //a lapon található táblázat
            var tables = body.Descendants<Table>();

            //iktatószám hozzáadása
            var rows = tables.FirstOrDefault().Descendants<TableRow>();
            TableCell cell = rows.ElementAt(1).Descendants<TableCell>().FirstOrDefault();
            Paragraph paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("XXX")).FirstOrDefault();
            string text = paragraph.InnerText;
            string modifiedText = Regex.Replace(text, "XXX", generateDto.Kozeleti_BizonylatIktatoszam);
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //ma dátum hozzáadása
            rows = tables.ElementAt(1).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().Last();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Számla")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //összeg kitöltése
            rows = tables.ElementAt(2).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Összeg")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = text + generateDto.KözeletiSum.ToString("#,0", nfi) + " Ft";
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //fizetési határidő kitöltés
            cell = rows.FirstOrDefault().Descendants<TableCell>().LastOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("határidő")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = text + new DateTime(DateTime.Today.Year, DateTime.Now.Day<15? DateTime.Today.Month : DateTime.Today.AddMonths(1).Month, 10).ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //jogcím kitöltése
            rows = tables.ElementAt(6).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Közéleti")).FirstOrDefault();
            text = paragraph.InnerText;
            modifiedText = Regex.Replace(text, "CCC", generateDto.MonthName);
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //témaszám kitöltése
            rows = tables.ElementAt(7).Descendants<TableRow>();
            cell = rows.ElementAt(1).Descendants<TableCell>().ElementAt(1);
            paragraph = cell.Descendants<Paragraph>().FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + generateDto.Koz_Temanumber;
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //funkcioterület kitöltése
            cell = rows.ElementAt(1).Descendants<TableCell>().ElementAt(3);
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("")).FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + generateDto.Funkcioterulet;
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //mai dátumok kitöltése
            rows = tables.ElementAt(8).Descendants<TableRow>();
            cell = rows.FirstOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Dátum")).FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            //mai dátum
            cell = rows.LastOrDefault().Descendants<TableCell>().FirstOrDefault();
            paragraph = cell.Descendants<Paragraph>().Where(q => q.InnerText.Contains("Dátum")).FirstOrDefault();
            text = cell.InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraph.RemoveAllChildren<Run>();
            paragraph.AppendChild<Run>(new Run(new RunProperties(new Italic(), new RunFonts() { Ascii = "Arial" }, new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "22" }), new Text(modifiedText)));

            string savepath = WordEditor.SaveFile(path, generateDto.Roleid, generateDto.Uniid, $"{generateDto.Tan_BizonylatIkatoszam.Replace('/', '-')}_Közéleti bizonylatkisérő.docx");

            editeddocument.SaveAs(savepath).Close();
            return savepath;
        }
        //Excel
        public static string GenerateNeptunba_Excel(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);
            string faculty = generateDto.facultyName.Split(" ").ElementAt(1);

            ExcelPackage neptunbaExcel = ExcelEditor.OpenFile(path, "oe_neptunba_template.xlsx");
            ExcelWorksheet worksheet = neptunbaExcel.Workbook.Worksheets.FirstOrDefault();

            worksheet = ExcelEditor.FillNeptunbaExcelWithData(worksheet, generateDto, faculty);
            
            string savepath = ExcelEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid,
                $"oe_neptunba_{faculty}_{ generateDto.Month}.xlsx");

            neptunbaExcel.SaveAs(new FileInfo(savepath));
            return savepath;
        }
        public static string GenerateNyomtatniForm_Excel(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            ExcelPackage formExcel = ExcelEditor.OpenFile(path, "oe_form_template.xlsx");
            ExcelWorksheet worksheet = formExcel.Workbook.Worksheets.ElementAt(1);

            worksheet.HeaderFooter.OddHeader.CenteredText = $"&\"Arial CE\"&16 {generateDto.facultyName.Replace(" HÖK","")}\nÖsztöndíj összefoglaló { generateDto.MonthName}";

            worksheet.Cells[1, 6].Value = $"tanulm_rendsz_{generateDto.Month}";
            worksheet.Cells[1, 7].Value = $"tanulm_1_{generateDto.Month}";
            worksheet.Cells[1, 8].Value = $"közeleti_rendsz_{generateDto.Month}";
            worksheet.Cells[1, 9].Value = $"kozeleti_egyszeri_{generateDto.Month}";

            worksheet.Cells[2, 5].Formula = "SUM(E4:E500)";
            worksheet.Cells[2, 6].Formula = "SUM(F4:F500)";
            worksheet.Cells[2, 7].Formula = "SUM(G4:G500)";
            worksheet.Cells[2, 8].Formula = "SUM(H4:H500)";
            worksheet.Cells[2, 9].Formula = "SUM(I4:I500)";

            for (int i = 0; i < generateDto.studentGetStipend.Count; i++)
            {
                var student = generateDto.studentGetStipend.ElementAt(i);
                int row = i + 4;
                worksheet.Cells[row, 1].Value = student.NeptunCode;
                worksheet.Cells[row, 4].Value = student.ModulCode;
                worksheet.Cells[row, 5].Formula = $"SUM(F{i + 4}:I{i + 4})";
                if (student.StipendAmmount != 0)
                {
                    if (generateDto.Month.Split("-").Count() > 1)
                    {
                        worksheet.Cells[row, 6].Value = student.StipendAmmount * 2;
                    }
                    else
                    {
                        worksheet.Cells[row, 6].Value = student.StipendAmmount;
                    }
                }
                if (student.PublicStipAmmount != 0)
                {
                    worksheet.Cells[row, 8].Value = student.PublicStipAmmount;
                }
                if (student.SinglePublicStipAmmount != 0)
                {
                    worksheet.Cells[row, 9].Value = student.SinglePublicStipAmmount;
                }
                
            }

            formExcel.Workbook.Calculate();

            string savepath = ExcelEditor.SaveFile(path,
                     generateDto.Roleid,
                     generateDto.Uniid,
                     $"{generateDto.facultyName.Replace(" HÖK","")} formNyomtatni { generateDto.Month}.xlsx");

            formExcel.SaveAs(new FileInfo(savepath));
            return savepath;
        }

        //sablonra megy a generálás
        public static string GenerateTanUtalas(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_utalas_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;

            var paragraphs = body.Descendants<Paragraph>();

            //ösztöndíj típusának kitöltése
            var runs = paragraphs.ElementAt(51).Descendants<Run>();
            var stipendTypeRun = runs.Where(q => q.InnerText.Contains("Utalás")).FirstOrDefault();
            string stipendTypeText = stipendTypeRun.InnerText;
            string modifiedText = Regex.Replace(stipendTypeText, "Utalás", "Tanulmányi");
            stipendTypeRun.RemoveAllChildren<Text>();
            stipendTypeRun.AppendChild<Text>(new Text(modifiedText));

            //ösztöndíjösszeg kitöltése
            var stipendSumRun = runs.Where(q => q.InnerText == "XXX ").FirstOrDefault();
            string stipendsumText = stipendSumRun.InnerText;
            modifiedText = Regex.Replace(stipendsumText, "XXX",
                generateDto.Month.Split("-").Count() > 1 ? (generateDto.StipendSum * 2).ToString("#,0", nfi) :
                generateDto.StipendSum.ToString("#,0", nfi));
            stipendSumRun.RemoveAllChildren<Text>();
            stipendSumRun.AppendChild<Text>(new Text(modifiedText));

            //témaszám kitöltése
            var temaszamRun = runs.Where(q => q.InnerText == "YYY").FirstOrDefault();
            string temaszamText = temaszamRun.InnerText;
            string modifiedText2 = Regex.Replace(temaszamText, "YYY", generateDto.Tan_Temanumber);
            temaszamRun.RemoveAllChildren<Text>();
            temaszamRun.AppendChild<Text>(new Text(modifiedText2));

            //dátum kitöltése
            string text = paragraphs.ElementAt(55).InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraphs.ElementAt(55).RemoveAllChildren<Run>();
            paragraphs.ElementAt(55).AppendChild<Run>(new Run(new Text(modifiedText)));

            //név kitöltése
            runs = paragraphs.ElementAt(58).Descendants<Run>();
            var nameRun = runs.Where(q => q.InnerText.Contains("Név")).FirstOrDefault();
            text = nameRun.InnerText;
            modifiedText = Regex.Replace(text, "Név", generateDto.PresidentName);
            nameRun.RemoveAllChildren<Text>();
            nameRun.AppendChild(new Text(modifiedText));

            //kari HÖK signature
            text = paragraphs.ElementAt(59).InnerText;
            modifiedText = generateDto.facultyName;
            paragraphs.ElementAt(59).RemoveAllChildren<Run>();
            paragraphs.ElementAt(59).AppendChild<Run>(new Run(new TabChar(), new Text(modifiedText)));

            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart, generateDto.facultyName.Split(" ").ElementAt(1));
            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, generateDto.facultyName.Split(" ").ElementAt(1));

            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid,
                $"OE {generateDto.facultyName.Split(" ").ElementAt(1)} Tanulmányi utalaskiserő {generateDto.Month}.docx");

            editeddocument.SaveAs(savepath).Close();
            return savepath;
        }
        public static string GenerateKözUtalas(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_utalas_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;

            var paragraphs = body.Descendants<Paragraph>();

            //ösztöndíj típusának kitöltése
            var runs = paragraphs.ElementAt(51).Descendants<Run>();
            var stipendTypeRun = runs.Where(q => q.InnerText.Contains("Utalás")).FirstOrDefault();
            string stipendTypeText = stipendTypeRun.InnerText;
            string modifiedText = Regex.Replace(stipendTypeText, "Utalás", "Közéleti");
            stipendTypeRun.RemoveAllChildren<Text>();
            stipendTypeRun.AppendChild<Text>(new Text(modifiedText));

            //ösztöndíjösszeg kitöltése
            var stipendSumRun = runs.Where(q => q.InnerText == "XXX ").FirstOrDefault();
            string stipendsumText = stipendSumRun.InnerText;
            modifiedText = Regex.Replace(stipendsumText, "XXX", generateDto.KözeletiSum.ToString("#,0", nfi));
            stipendSumRun.RemoveAllChildren<Text>();
            stipendSumRun.AppendChild<Text>(new Text(modifiedText));

            //témaszám kitöltése
            var temaszamRun = runs.Where(q => q.InnerText == "YYY").FirstOrDefault();
            string temaszamText = temaszamRun.InnerText;
            string modifiedText2 = Regex.Replace(temaszamText, "YYY", generateDto.Tan_Temanumber);
            temaszamRun.RemoveAllChildren<Text>();
            temaszamRun.AppendChild<Text>(new Text(modifiedText2));

            //dátum kitöltése
            string text = paragraphs.ElementAt(55).InnerText;
            modifiedText = text + DateTime.Now.ToString("yyyy.MM.dd");
            paragraphs.ElementAt(55).RemoveAllChildren<Run>();
            paragraphs.ElementAt(55).AppendChild<Run>(new Run(new Text(modifiedText)));

            //név kitöltése
            runs = paragraphs.ElementAt(58).Descendants<Run>();
            var nameRun = runs.Where(q => q.InnerText.Contains("Név")).FirstOrDefault();
            text = nameRun.InnerText;
            modifiedText = Regex.Replace(text, "Név", generateDto.PresidentName);
            nameRun.RemoveAllChildren<Text>();
            nameRun.AppendChild(new Text(modifiedText));

            //kari HÖK signature
            text = paragraphs.ElementAt(59).InnerText;
            modifiedText = generateDto.facultyName;
            paragraphs.ElementAt(59).RemoveAllChildren<Run>();
            paragraphs.ElementAt(59).AppendChild<Run>(new Run(new TabChar(), new Text(modifiedText)));

            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart, generateDto.facultyName.Split(" ").ElementAt(1));
            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, generateDto.facultyName.Split(" ").ElementAt(1));

            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid,
                $"OE {generateDto.facultyName.Split(" ").ElementAt(1)} Közéleti utalaskiserő {generateDto.Month}.docx");

            editeddocument.SaveAs(savepath).Close();
            return savepath;
        }
        public static string GenerateElnökiHatározatWord(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);
            string faculty = generateDto.facultyName.Split(" ").ElementAt(1);

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_elnoki_hat_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;

            //paragrafusok a teljes documentumban
            List<Paragraph> paras = body.Descendants<Paragraph>().ToList();

            //iktatószám
            paras.ElementAt(0).Append(new Run(new Text(generateDto.ElnokiIktato)));
            //dátum beszúrása
            paras.ElementAt(1).Append(new Run(new Text($"{DateTime.Now.ToString("yyyy.MM.dd.")}")));

            //Alulírott, YYY, mint az Óbudai Egyetem ZZZ Kari Hallgatói Részönkormányzatának elnöke
            //az elnökségi tagok AAA havi kötelességelmulasztás után járó szankciókról a
            //továbbiakban határozok.
            string text = paras.ElementAt(6).InnerText;
            string modifiedtext = Regex.Replace(text, "YYY", $"{generateDto.PresidentName}");
            string modifiedtext2 = Regex.Replace(modifiedtext, "AAA", $"{generateDto.MonthName}");
            string modtext = Regex.Replace(modifiedtext2, "ZZZ Kari", faculty);
            //text.Replace("Alulírott, XXX, ", $"Alulírott, {generateDto.PresidentName}, ").Replace("XXX havi", $"{generateDto.MonthName} havi").Replace("XXX Kari", rolename.Split(" ").ElementAt(0));
            paras.ElementAt(6).RemoveAllChildren<Run>();
            paras.ElementAt(6).AppendChild<Run>(new Run(new Text(modtext)));

            //TODO középre igazítani függőlegesen
            //itt kezdődik a táblázat és vízszintesen halad név-neptunkód-tisztség-szankció-összeg
            int payment = Convert.ToInt32(generateDto.ETPay);
            //alelnök
            paras.ElementAt(14).Append(new Run(new Text(generateDto.VicePresidentName)));
            paras.ElementAt(15).Append(new Run(new Text(generateDto.VicePresidentNeptun)));
            paras.ElementAt(17).Append(new Run(new Text(generateDto.VicePresidentPercent + "%")));
            paras.ElementAt(18).Append(new Run(new Text(payment * ((100 - Convert.ToInt32(generateDto.VicePresidentPercent)) / 100) + "Ft")));
            //gazdaságis
            paras.ElementAt(19).Append(new Run(new Text(generateDto.GazdName)));
            paras.ElementAt(20).Append(new Run(new Text(generateDto.GazdNeptun)));
            paras.ElementAt(22).Append(new Run(new Text(generateDto.GazdPercent + "%")));
            paras.ElementAt(23).Append(new Run(new Text(payment * ((100 - Convert.ToInt32(generateDto.GazdPercent)) / 100) + "Ft")));
            //éf
            paras.ElementAt(24).Append(new Run(new Text(generateDto.EfName)));
            paras.ElementAt(25).Append(new Run(new Text(generateDto.EfNeptun)));
            paras.ElementAt(27).Append(new Run(new Text(generateDto.EfPercent + "%")));
            paras.ElementAt(28).Append(new Run(new Text(payment * ((100 - Convert.ToInt32(generateDto.EfPercent)) / 100) + "Ft")));
            //kandó esetén éf egyébként törölni kell
            if (faculty.Contains("KVK"))
            {
                paras.ElementAt(29).Append(new Run(new Text(generateDto.Ef2Name)));
                paras.ElementAt(30).Append(new Run(new Text(generateDto.Ef2Neptun)));
                paras.ElementAt(32).Append(new Run(new Text(generateDto.Ef2Percent + "%")));
                paras.ElementAt(33).Append(new Run(new Text(payment * ((100 - Convert.ToInt32(generateDto.Ef2Percent)) / 100) + "Ft")));
            }
            else
            {
                body.Descendants<Table>().FirstOrDefault().Descendants<TableRow>().Where(q => q.InnerText.Contains("Érdekvédelmi")).LastOrDefault().Remove();
            }

            //pr
            paras.ElementAt(34).Append(new Run(new Text(generateDto.PrName)));
            paras.ElementAt(35).Append(new Run(new Text(generateDto.PrNeptun)));
            paras.ElementAt(37).Append(new Run(new Text(generateDto.PrPercent + "%")));
            paras.ElementAt(38).Append(new Run(new Text(payment * ((100 - Convert.ToInt32(generateDto.PrPercent)) / 100) + "Ft")));

            //keltezés kiegészítése az aktuális dátummal
            paras.ElementAt(45).Append(new Run(new Text(DateTime.Now.ToString("yyyy.MM.dd"))));

            paras.ElementAt(47).Append(new Run(new Text(generateDto.PresidentName)));
            //HÖK szerinti aláírás
            paras.ElementAt(48).Append(new Run(new Text(generateDto.facultyName)));

            //fejléc szerkesztése
            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart,faculty);

            //lábléc szerkesztése
            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, faculty);

            //fájl mentése
            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid,
                $"{generateDto.ElnokiIktato.Replace('/', '-')} elnöki határozat {generateDto.Month}.docx");

            editeddocument.SaveAs(savepath).Close();
            return savepath;
        }
        public static string GenerateÖCSI_Word(GenerateDto generateDto, List<StudentGenerateDto> students, string year)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_ocsi_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;

            
            //félév kitöltése
            var paragraphs = body.Descendants<Paragraph>();
            var felevutanParagraph = paragraphs.ElementAt(1);
            string text = felevutanParagraph.InnerText;
            string modifiedtext = Regex.Replace(text, "XXX ", $"{year}");
            felevutanParagraph.RemoveAllChildren<Run>();
            felevutanParagraph.Append(new Run(new Text(modifiedtext)));

            var keltParagraph = paragraphs.ElementAt(20);
            var nameParagraph = paragraphs.ElementAt(21);
            var facultyParagraph = paragraphs.ElementAt(22);

             //keltezés kitöltése
            text = keltParagraph.InnerText;
            modifiedtext = text + " Budapest, " + DateTime.Now.ToString("yyyy.MM.dd");
            keltParagraph.RemoveAllChildren<Run>();
            keltParagraph.RemoveAllChildren<ParagraphProperties>();
            keltParagraph.Append(new ParagraphProperties(new SpacingBetweenLines() { Before = "1400" }), new Run(new Text(modifiedtext)));

            //alelnök nevének kitöltése
            text = nameParagraph.InnerText;
            modifiedtext = Regex.Replace(text, "YYY", generateDto.VicePresidentName);
            nameParagraph.RemoveAllChildren<Run>();
            nameParagraph.RemoveAllChildren<ParagraphProperties>();
            nameParagraph.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(new Text(modifiedtext)));

            //hök és titulus kitöltése
            text = facultyParagraph.InnerText;
            modifiedtext = Regex.Replace(text, "ZZZ", generateDto.facultyName);
            facultyParagraph.RemoveAllChildren<Run>();
            facultyParagraph.RemoveAllChildren<ParagraphProperties>();
            facultyParagraph.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Run(new Text(modifiedtext)));

            //az első tábálázatot kiválasztjuk és feltöltjük a hallgatókkal
            Table table = body.Descendants<Table>().FirstOrDefault();
            table = WordEditor.FillÖcsiTableWithData(table, "ÖCSI", students);

            //fejléc szerkesztése
            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart, generateDto.facultyName.Split(" ").ElementAt(1));

            //lábléc szerkesztése
            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, generateDto.facultyName.Split(" ").ElementAt(1));

            //fájl mentése
            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid,
                $"{generateDto.facultyName} ÖCSI.docx");

            editeddocument.SaveAs(savepath).Close();

            return savepath;
        }
        public static string GenerateÖsztöndijMutató_Word(GenerateDto generateDto, KeyValuePair<string, List<StudentGenerateDto>> keyValuePair)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);
            string faculty = generateDto.facultyName.Split(" ").ElementAt(1);

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_empty_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;
            //kelt tárgy és ügyintéző hozzáadása a doksihoz
            var editedbody = WordEditor.CreateÖMDetailsPart(body, generateDto.VicePresidentName);
            doc.Body = editedbody;

            //TODO keyvaluePair.key -ekhez translatort kell megírnom hogy a szakok teljes nevét írja ki
            //Cím és alcím létrehozása
            body = WordEditor.AddTitle(body, "Ösztöndíj mutatók", "56", true);
            body = WordEditor.AddTitle(body, $"{keyValuePair.Key}", "40", false);

            body = WordEditor.AddTable(body, "Ösztöndíjmutató", keyValuePair.Value);

            body = WordEditor.AddSignature(body, faculty, JustificationValues.Center, generateDto, false, false);

            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart, faculty);

            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, faculty);

            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid,
                $"{generateDto.facultyName} Ösztöndíj mutató {keyValuePair.Key}.docx");
   
            editeddocument.SaveAs(savepath).Close();
            return savepath;
        }
        public static string GenerateTanulmanyiÖsszesito_Word(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_tanulmanyi_osszesito_tempalte.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;
            List<Paragraph> paras = body.Descendants<Paragraph>().ToList();

            //Tanulmányi iktatószám beszúrása
            paras.ElementAt(0).Append(new Run(new Text(generateDto.Tan_SummaryIktatoszam)));
            //Dátum beszúrása
            paras.ElementAt(1).Append(new Run(new Text($"{DateTime.Now.ToString("yyyy.MM.dd.")}")));

            //
            string text = paras.ElementAt(5).InnerText;
            string modifiedtext = Regex.Replace(text, "YYY", $"{generateDto.PresidentName}");
            string modifiedtext2 = Regex.Replace(modifiedtext, "AAA", $"{generateDto.MonthName}");
            string modtext = Regex.Replace(modifiedtext2,
                "ZZZ Kari",
                 WordEditor.ConvertFacultyNameToFacultyFullName(generateDto.facultyName.Split(" ").ElementAt(1)));

            paras.ElementAt(5).RemoveAllChildren<Run>();
            paras.ElementAt(5).AppendChild<Run>(new Run(new Text(modtext)));

            //dátum beszúrása
            text = paras.ElementAt(12).InnerText;
            modifiedtext = text + DateTime.Now.ToString("yyyy.MM.dd");
            paras.ElementAt(12).RemoveAllChildren<Run>();
            paras.ElementAt(12).AppendChild<Run>(new Run(new Text(modifiedtext)));

            //aláírás beszúrása az Elnök nevével
            body = WordEditor.AddSignature(body, generateDto.facultyName, JustificationValues.Center, generateDto, true, true);

            //a táblázat feltöltése az ösztöndíjat kapó hallgatókkal
            Table table = body.Descendants<Table>().FirstOrDefault();
            bool isDoubleAmmount = generateDto.Month.Split("-").Count() > 1 ? true : false;
            table = WordEditor.FillÖsszesitoWithData(table, generateDto.studentGetStipend, isDoubleAmmount, false,false);

            //fejléc szerkesztése
            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart, generateDto.facultyName.Split(" ").ElementAt(1));
            //lábléc szerkesztése
            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, generateDto.facultyName.Split(" ").ElementAt(1));

            //fájl mentése
            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid, $"{generateDto.Tan_SummaryIktatoszam.Replace('/','-')} Tanulmanyi Összesitő {generateDto.Month}.docx");

            editeddocument.SaveAs(savepath).Close();

            return savepath;
        }
        public static string GenerateKözeletiÖsszesitö(GenerateDto generateDto)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
                "Resources",
                generateDto.Uniid);

            WordprocessingDocument editeddocument = WordEditor.OpenFile(path, "oe_kozeleti_osszesito_template.docx");

            MainDocumentPart documentPart = editeddocument.MainDocumentPart;
            Document doc = documentPart.Document;
            Body body = doc.Body;
            List<Paragraph> paras = body.Descendants<Paragraph>().ToList();

            //Tanulmányi iktatószám beszúrása
            paras.ElementAt(0).Append(new Run(new Text(generateDto.Kozeleti_SummaryIktatoszam)));
            //Dátum beszúrása
            paras.ElementAt(1).Append(new Run(new Text($"{DateTime.Now.ToString("yyyy.MM.dd.")}")));

            //
            string text = paras.ElementAt(5).InnerText;
            string modifiedtext = Regex.Replace(text, "YYY", $"{generateDto.PresidentName}");
            string modifiedtext2 = Regex.Replace(modifiedtext, "AAA", $"{generateDto.MonthName}");
            string modtext = Regex.Replace(modifiedtext2,
                "ZZZ Kari",
                 WordEditor.ConvertFacultyNameToFacultyFullName(generateDto.facultyName.Split(" ").ElementAt(1)));

            paras.ElementAt(5).RemoveAllChildren<Run>();
            paras.ElementAt(5).AppendChild<Run>(new Run(new Text(modtext)));

            //dátum beszúrása
            text = paras.ElementAt(13).InnerText;
            modifiedtext = text + DateTime.Now.ToString("yyyy.MM.dd");
            paras.ElementAt(13).RemoveAllChildren<Run>();
            paras.ElementAt(13).AppendChild<Run>(new Run(new Text(modifiedtext)));

            //aláírás beszúrása az Elnök nevével
            body = WordEditor.AddSignature(body, generateDto.facultyName, JustificationValues.Center, generateDto, true, true);

            //a táblázat feltöltése az ösztöndíjat kapó hallgatókkal
            Table table = body.Descendants<Table>().FirstOrDefault();
            var studentsWithKözeleti = generateDto.studentGetStipend.Where(q => q.PublicStipAmmount != 0).ToList();
            var studentWithSinglePublic = generateDto.studentGetStipend.Where(q => q.SinglePublicStipAmmount != 0).ToList();
            table = WordEditor.FillÖsszesitoWithData(table, studentsWithKözeleti, false,true,false);
            table = WordEditor.FillÖsszesitoWithData(table, studentWithSinglePublic, false, true, true);

            //fejléc szerkesztése
            var ePart = documentPart.HeaderParts.ElementAt(1);
            WordEditor.EditHeader(ePart, generateDto.facultyName.Split(" ").ElementAt(1));
            //lábléc szerkesztése
            var fPart = documentPart.FooterParts.FirstOrDefault();
            WordEditor.EditFooter(fPart, generateDto.facultyName.Split(" ").ElementAt(1));

            //fájl mentése
            string savepath = WordEditor.SaveFile(path,
                generateDto.Roleid,
                generateDto.Uniid, $"{generateDto.Kozeleti_SummaryIktatoszam.Replace('/', '-')} Közéleti Összesitő {generateDto.Month}.docx");

            editeddocument.SaveAs(savepath).Close();

            return savepath;
        }
        private static string FacultyTranslator(string faculty)
        {
            switch (faculty.ToUpper())
            {
                case "BPROF": return "";
                case "MI": return "hok.banki.hu";
                case "KGK": return "kgk.hok.uni-obuda.hu/";
                case "KVK": return "kandohok.hu";
                case "NIK": return "nikhok.hu";
                case "RKK": return "rkkhok.hu";
                default: return "";
            }
        }
    }

}
