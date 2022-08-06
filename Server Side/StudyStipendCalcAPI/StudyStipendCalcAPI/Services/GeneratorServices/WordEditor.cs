using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using StudyStipendCalcAPI.DTOs.FileDtos;
using StudyStipendCalcAPI.DTOs.StudentDtos;

namespace StudyStipendCalcAPI.Services.FileServices
{
    public static class WordEditor
    {
        public static WordprocessingDocument OpenFile(string pathFolder, string filename)
        {
            string openpathFolder = Path.Combine(pathFolder, "Templates");
            if (!Directory.Exists(openpathFolder))
            {
                Directory.CreateDirectory(openpathFolder);
            }
            WordprocessingDocument document = WordprocessingDocument.Open(Path.Combine(openpathFolder, filename), false);
            WordprocessingDocument editeddocument = (WordprocessingDocument)document.Clone();
            document.Dispose();

            return editeddocument;

        }
        public static string SaveFile(string pathfolder,string roleid,string uniid,string filename)
        {
            string savedirectory = Path.Combine(pathfolder, roleid);
            string savepath = Path.Combine(savedirectory, filename);
            if (!Directory.Exists(savedirectory))
            {
                Directory.CreateDirectory(savedirectory);
            }
            return savepath;
        }
        public static Body CreateÖMDetailsPart(Body body, string ügyintezo)
        {
            CreateDetailsParagraphs(body.Descendants<Paragraph>().FirstOrDefault(), "Kelt: ", DateTime.Now.ToString("yyyy.MM.dd"));
            Paragraph paragraph1 = CreateDetailsParagraphs(new Paragraph(), "Tárgy: ", "Ösztöndíj mutatók");
            body.AppendChild<Paragraph>(paragraph1);
            Paragraph paragraph2 = CreateDetailsParagraphs(new Paragraph(), "Ügyintéző: ", ügyintezo);
            body.AppendChild<Paragraph>(paragraph2);
            return body;
        }
        public static Body AddTitle(Body body, string titleval, string titlesize, bool isTitle)
        {
            if (isTitle)
            {
                body.RemoveAllChildren<Paragraph>();
            }
            Paragraph paragraph = new Paragraph();
            ParagraphProperties pPr = new ParagraphProperties();
            Run run = new Run();
            RunProperties rPr = GetStyle($"{titlesize}", false);

            Justification justification = new Justification() { Val = JustificationValues.Center };
            SpacingBetweenLines spacingBefore = new SpacingBetweenLines() { Before = "1400" };
            SpacingBetweenLines spacingAfter = new SpacingBetweenLines() { After = "684" };
            if (isTitle)
            {

                pPr.Append(spacingBefore);
            }
            else
            {

                pPr.Append(spacingAfter);
            }
            pPr.Append(justification);
            run.AppendChild<RunProperties>(rPr);
            run.Append(new Text($"{titleval}"));
            paragraph.Append(pPr);
            paragraph.Append(run);
            body.AppendChild<Paragraph>(paragraph);

            return body;

        }
        public static RunProperties GetStyle(string size, bool isBold)
        {
            RunProperties runProp = new RunProperties();
            Bold bold;
            var runFont = new RunFonts() { Ascii = "Cambria" };
            if (isBold)
            {
                bold = new Bold();
                runProp.Append(bold);
            }
            var runsize = new FontSize() { Val = new StringValue($"{size}") };

            runProp.Append(runFont);
            runProp.Append(runsize);
            return runProp;
        }

        public static void EditHeader(HeaderPart headerPart, string facultyCode)
        {
            var paragraphs = headerPart.Header.Descendants<Paragraph>();

            var HÖKtext = paragraphs.ElementAt(1).InnerText;
            var KHÖKtext = ConvertFacultyNameToFacultyFullName(facultyCode) + "i Részönkormányzat";
            paragraphs.ElementAt(1).RemoveAllChildren<Run>();
            paragraphs.ElementAt(1).Append(new Run(
                new RunProperties(new FontSize() { Val = new StringValue("22") },
                                new RunFonts() { Ascii = "Arial" },
                                new Color() { Val = "#152a50" },
                                new Justification() { Val = JustificationValues.Right }),
                new Text(HÖKtext), new Break(), new Text(KHÖKtext)));
        }
        public static void EditFooter(FooterPart footerPart, string facultyCode)
        {
            var paragraphs = footerPart.Footer.Descendants<Paragraph>();

            paragraphs.ElementAt(1).RemoveAllChildren<Run>();
            paragraphs.ElementAt(1).Append(new Run(
                new RunProperties(new FontSize() { Val = new StringValue("15") },
                                new RunFonts() { Ascii = "Arial" },
                                new Color() { Val = "#12234d" }),
                new Text(GetCity(facultyCode))));

            paragraphs.ElementAt(2).RemoveAllChildren<Run>();
            paragraphs.ElementAt(2).Append(new Run(
                new RunProperties(new FontSize() { Val = new StringValue("15") },
                                new RunFonts() { Ascii = "Arial" },
                                new Color() { Val = "#12234d" }),
                new Text(GetPhone(facultyCode))));

            var link = paragraphs.ElementAt(3).GetFirstChild<Hyperlink>();
            link.RemoveAllChildren<Run>();
            link.Append(new Run(
                new RunProperties(new FontSize() { Val = new StringValue("15") },
                                new RunFonts() { Ascii = "Arial" },
                                new Color() { Val = "#12234d" }),
                new Text(GetMail(facultyCode))));

            paragraphs.ElementAt(4).RemoveAllChildren<Run>();
            paragraphs.ElementAt(4).Append(new Run(
                new RunProperties(new FontSize() { Val = new StringValue("15") },
                                new RunFonts() { Ascii = "Arial" },
                                new Color() { Val = "#12234d" }),
                new Text(GetStreet(facultyCode))));

            link = paragraphs.ElementAt(6).GetFirstChild<Hyperlink>();
            link.RemoveAllChildren<Run>();
            link.Append(new Run(
                new RunProperties(new FontSize() { Val = new StringValue("15") },
                                new RunFonts() { Ascii = "Arial" },
                                new Color() { Val = "#12234d" }),
                new Text(GetWeb(facultyCode))));

        }
        public static Table FillÖsszesitoWithData(Table table, List<StudentGenerateDto> studs, bool isDoubleAmmount, bool isPublic, bool isSinglePublic)
        {
            if (!isSinglePublic)
            {
                
            table.RemoveAllChildren<TableRow>();
            table.RemoveAllChildren<TableGrid>();
            TableRow firstRow = new TableRow();

            // Create a cell.
            TableCell FirstR1C = new TableCell();

            // Specify the width property of the table cell.
            FirstR1C.Append(new TableCellProperties(
                new TableHeader() { Val = OnOffOnlyValues.On }));

            // Specify the table cell content.
            Run FirstCloumnRun = new Run();
            Text FirstColumnText = new Text("Neptun kód");
            FirstCloumnRun.PrependChild(GetStyle("22", true));
            FirstCloumnRun.Append(FirstColumnText);
            FirstR1C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(FirstCloumnRun));
            // Append the table cell to the table row.
            firstRow.Append(FirstR1C);

            // Create a second table cell.
            TableCell FirstRowSecondColumn = new TableCell();
            Run SecondColumnRun = new Run();
            Text SecondColumnText = new Text("Képzés kód");
            SecondColumnRun.PrependChild(GetStyle("22", true));
            SecondColumnRun.Append(SecondColumnText);
            FirstRowSecondColumn.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(SecondColumnRun));

            // Append the table cell to the table row.
            firstRow.Append(FirstRowSecondColumn);

            TableCell FirstRow3Column = new TableCell();
            Run ThirdColumnRun = new Run();
            Text ThirdColumnText = new Text("Jogcím");
            ThirdColumnRun.PrependChild(GetStyle("22", true));
            ThirdColumnRun.Append(ThirdColumnText);
            FirstRow3Column.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(ThirdColumnRun));

            // Append the table cell to the table row.
            firstRow.Append(FirstRow3Column);

            if (isPublic)
            {
                TableCell FirstRowReasonColumn = new TableCell();
                Run ReasonColumnRun = new Run();
                Text ReasonColumnText = new Text("Indoklás");
                ReasonColumnRun.PrependChild(GetStyle("22", true));
                ReasonColumnRun.Append(ReasonColumnText);
                FirstRowReasonColumn.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(ReasonColumnRun));

                // Append the table cell to the table row.
                firstRow.Append(FirstRowReasonColumn);
            }

            TableCell FirstRow4Column = new TableCell();
            Run FirstRow4ColumnRun = new Run();
            Text FirstRow4ColumnText = new Text("Kiutalandó összeg");
            FirstRow4ColumnRun.PrependChild(GetStyle("22", true));
            FirstRow4ColumnRun.Append(FirstRow4ColumnText);
            FirstRow4Column.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(FirstRow4ColumnRun));

            // Append the table cell to the table row.
            firstRow.Append(FirstRow4Column);

            // Append the table row to the table.
            table.Append(firstRow);
        }

            foreach (var item in studs)
            {
                TableRow tableRow = new TableRow();

                TableCell R1C = new TableCell();
                Run r1 = new Run();
                Text t1 = new Text(item.NeptunCode);
                r1.PrependChild(GetStyle("22", false));
                r1.Append(t1);
                R1C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r1));


                TableCell R2C = new TableCell();
                Run r2 = new Run();
                Text t2 = new Text(item.ModulCode.ToString());
                r2.PrependChild(GetStyle("22", false));
                r2.Append(t2);
                R2C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r2));


                TableCell R3C = new TableCell();
                Run r3 = new Run();
                Text t3 = isPublic ? new Text("Közéleti ösztöndíj") : new Text("Tanulmányi ösztöndíj");
                r3.PrependChild(GetStyle("22", false));
                r3.Append(t3);
                R3C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r3));

                TableCell R4C = new TableCell();
                Run r4 = new Run();
                Text t4;
                if (isDoubleAmmount)
                {
                    int ammount = item.StipendAmmount * 2;
                    t4 = new Text(ammount.ToString());
                }
                else
                {
                    t4 = !isPublic ? new Text(item.StipendAmmount.ToString()) : !isSinglePublic ? new Text(item.PublicStipAmmount.ToString()) : new Text(item.SinglePublicStipAmmount.ToString());
                }
                r4.PrependChild(GetStyle("22", false));
                r4.Append(t4);
                R4C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r4));

                if (isSinglePublic || isPublic)
                {
                    TableCell Reason3C = new TableCell();
                    Run reasonRun = new Run();
                    Text reasonText = new Text(item.PublicStipReason);
                    reasonRun.PrependChild(GetStyle("22", false));
                    reasonRun.Append(reasonText);
                    Reason3C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(reasonRun));

                    tableRow.Append(R1C, R2C, R3C, Reason3C, R4C);
                }
                else
                {
                    tableRow.Append(R1C, R2C, R3C, R4C);
                }
                table.Append(tableRow);
            }

            return table;
        }
        public static Table FillÖcsiTableWithData(Table table, string valuetitle, List<StudentGenerateDto> students)
        {
            table.RemoveAllChildren<TableRow>();
            table.RemoveAllChildren<TableGrid>();
            TableRow firstRow = new TableRow();

            // Create a cell.
            TableCell FirstR1C = new TableCell();

            // Specify the width property of the table cell.
            FirstR1C.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" },
                new TableHeader() { Val = OnOffOnlyValues.On }));

            // Specify the table cell content.
            Run FirstCloumnRun = new Run();
            Text FirstColumnText = new Text("Neptun kód");
            FirstCloumnRun.PrependChild(GetStyle("22", true));
            FirstCloumnRun.Append(FirstColumnText);
            FirstR1C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(FirstCloumnRun));
            // Append the table cell to the table row.
            firstRow.Append(FirstR1C);

            // Create a second table cell.
            TableCell FirstRowSecondColumn = new TableCell();
            Run SecondColumnRun = new Run();
            Text SecondColumnText = new Text(valuetitle);
            SecondColumnRun.PrependChild(GetStyle("22", true));
            SecondColumnRun.Append(SecondColumnText);
            FirstRowSecondColumn.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(SecondColumnRun));

            // Append the table cell to the table row.
            firstRow.Append(FirstRowSecondColumn);

            // Append the table row to the table.
            table.Append(firstRow);
            foreach (var item in students)
            {
                TableRow tableRow = new TableRow();

                TableCell R1C = new TableCell();
                Run r1 = new Run();
                Text t1 = new Text(item.NeptunCode);
                r1.PrependChild(GetStyle("22", false));
                r1.Append(t1);
                R1C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r1));


                TableCell R2C = new TableCell();
                Run r2 = new Run();
                Text t2 = new Text(item.GroupIndex.ToString());
                r1.PrependChild(GetStyle("22", false));
                r2.Append(t2);
                R2C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r2));
                tableRow.Append(R1C, R2C);

                table.Append(tableRow);
            }
            return table;
        }
        public static Body AddTable(Body body, string valuetitle, List<StudentGenerateDto> studentGroup)
        {
            // Create an empty table.
            Table table = new Table();

            // Create a TableProperties object and specify its border information.
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 12 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 12 },
                    //new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                    //new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Dashed), Size = 24 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 12 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.BasicThinLines), Size = 12 }
                ),
                new TableWidth() { Width = "1950", Type = TableWidthUnitValues.Pct },
                new TableRowHeight() { Val = Convert.ToUInt32("10"), HeightType = HeightRuleValues.Auto },
                new Justification() { Val = JustificationValues.Center },
                new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }
            );

            // Append the TableProperties object to the empty table.
            table.AppendChild<TableProperties>(tblProp);
            // Create a row.
            TableRow firstRow = new TableRow();

            // Create a cell.
            TableCell FirstR1C = new TableCell();

            // Specify the width property of the table cell.
            FirstR1C.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" },
                new TableHeader() { Val = OnOffOnlyValues.On }));

            // Specify the table cell content.
            Run FirstCloumnRun = new Run();
            Text FirstColumnText = new Text("Neptun kód");
            FirstCloumnRun.PrependChild(GetStyle("22", true));
            FirstCloumnRun.Append(FirstColumnText);
            FirstR1C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(FirstCloumnRun));
            // Append the table cell to the table row.
            firstRow.Append(FirstR1C);

            // Create a second table cell.
            TableCell FirstRowSecondColumn = new TableCell();
            Run SecondColumnRun = new Run();
            Text SecondColumnText = new Text(valuetitle);
            SecondColumnRun.PrependChild(GetStyle("22", true));
            SecondColumnRun.Append(SecondColumnText);
            FirstRowSecondColumn.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(SecondColumnRun));

            // Append the table cell to the table row.
            firstRow.Append(FirstRowSecondColumn);

            // Append the table row to the table.
            table.Append(firstRow);
            foreach (var item in studentGroup)
            {
                TableRow tableRow = new TableRow();

                TableCell R1C = new TableCell();
                Run r1 = new Run();
                Text t1 = new Text(item.NeptunCode);
                r1.PrependChild(GetStyle("22", false));
                r1.Append(t1);
                R1C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r1));


                TableCell R2C = new TableCell();
                Run r2 = new Run();
                Text t2 = new Text(item.StipendIndex.ToString());
                r1.PrependChild(GetStyle("22", false));
                r2.Append(t2);
                R2C.Append(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }), new Paragraph(r2));
                tableRow.Append(R1C, R2C);

                table.Append(tableRow);
            }


            // Append the table to the document.
            body.Append(table);
            return body;
        }
        public static Body AddSignature(Body body, string rolename, JustificationValues justificationValues, GenerateDto dto, bool president, bool needName)
        {
            Paragraph signatureLine = new Paragraph(new ParagraphProperties(new Justification() { Val = justificationValues }, new SpacingBetweenLines() { Before = "1400" }), new Run(new Text("_______________________")));
            body.Append(signatureLine);
            if (needName)
            {
                if (president)
                {
                    Paragraph Name = new Paragraph(new ParagraphProperties(new Justification() { Val = justificationValues }), new Run(new Text($"{dto.PresidentName }")));
                    body.Append(Name);
                }
                else
                {
                    Paragraph Name = new Paragraph(new ParagraphProperties(new Justification() { Val = justificationValues }), new Run(new Text($"{dto.VicePresidentName }")));
                    body.Append(Name);
                }
            }
            Paragraph signature = new Paragraph(new ParagraphProperties(new Justification() { Val = justificationValues }), new Run(new Text(dto.facultyName)));
            body.Append(signature);
            if (president)
            {
                Paragraph signature2 = new Paragraph(new ParagraphProperties(new Justification() { Val = justificationValues }), new Run(new Text("Elnök")));
                body.Append(signature2);
                return body;
            }
            else
            {
                Paragraph signature2 = new Paragraph(new ParagraphProperties(new Justification() { Val = justificationValues }), new Run(new Text("Alelnök")));
                body.Append(signature2);
                return body;
            }


        }

        public static string ConvertFacultyNameToFacultyFullName(string name)
        {
            switch (name)
            {
                case "AMK": return "Alba Regia Műszaki Kar";
                case "BGK": return "Bánki Donát Gépész és Biztonságtechnikai Mérnöki Kar";
                case "KGK": return "Keleti Károly Gazdasági Kar";
                case "KVK": return "Kandó Kálmán Villamosmérnöki Kar";
                case "NIK": return "Neumann János Informatikai Kar";
                case "RKK": return "Rejtő Sándor Könnyűipari és Környezetmérnöki Kar";

                default: return "";
            }
        }
        public static string GetCity(string facultyCode)
        {
            switch (facultyCode)
            {
                case "AMK": return "8000 Székesfehérvár";
                case "BGK": return "1081 Budapest";
                case "KGK": return "1084 Budapest";
                case "KVK": return "1084 Budapest";
                case "NIK": return "1034 Budapest";
                case "RKK": return "1034 Budapest";

                default: return "";
            }
        }
        public static string GetStreet(string facultyCode)
        {
            switch (facultyCode)
            {
                case "AMK": return "Budai út 45.";
                case "BGK": return "Népszínház utca 8.";
                case "KGK": return "Tavaszmező utca 16-18";
                case "KVK": return "Tavaszmező utca 17.";
                case "NIK": return "Bécsi út 96/b 1.11";
                case "RKK": return "Doberdó út 6.";

                default: return "";
            }
        }
        public static string GetWeb(string facultyCode)
        {
            switch (facultyCode)
            {
                case "AMK": return "";
                case "BGK": return "hok.banki.hu";
                case "KGK": return "kgk.hok.uni-obuda.hu/";
                case "KVK": return "kandohok.hu";
                case "NIK": return "nikhok.hu";
                case "RKK": return "rkkhok.hu";

                default: return "";
            }
        }
        public static string GetMail(string facultyCode)
        {
            switch (facultyCode)
            {
                case "AMK": return "amkelnokseg@cl.uni-obuda.hu";
                case "BGK": return "info@hok.banki.hu";
                case "KGK": return "kgkhk@cl.uni-obuda.hu";
                case "KVK": return "elnokseg@kandohok.hu";
                case "NIK": return "nik@hok.uni-obuda.hu";
                case "RKK": return "rkk@hok.uni-obuda.hu";

                default: return "";
            }
        }
        public static string GetPhone(string facultyCode)
        {
            switch (facultyCode)
            {
                case "AMK": return "";
                case "BGK": return "+36 (1) 666 5305";
                case "KGK": return "+36 (1) 666 5044";
                case "KVK": return "+36 (1) 666-5014";
                case "NIK": return "+36 (1) 666-5591";
                case "RKK": return "";

                default: return "";
            }
        }

        static Paragraph CreateDetailsParagraphs(Paragraph paragraph, string details, string value)
        {
            if (paragraph.Descendants<ParagraphProperties>().ToList().Count == 0)
            {
                ParagraphProperties pPr = new ParagraphProperties();
                pPr.Indentation = new Indentation() { Left = "6700" };
                paragraph.Append(pPr);
            }
            else
            {
                var pPr = paragraph.Descendants<ParagraphProperties>().FirstOrDefault();
                pPr.Indentation = new Indentation() { Left = "6700" };
            }
            Run keltRun = new Run();

            keltRun.PrependChild(GetStyle("24", true));
            keltRun.Append(new Text($"{details} "));
            paragraph.AppendChild<Run>(keltRun);

            Run dateRun = new Run();
            dateRun.Append(new Text($" {value}"));

            paragraph.AppendChild<Run>(dateRun);

            return paragraph;
        }
    }
}
