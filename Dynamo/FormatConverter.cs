using System.Collections;
using System.Data;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using Newtonsoft.Json;
using Path = System.IO.Path;
namespace Dynamo
{
    public class FormatConverter
    {

        public object valueChange;
        public FormatConverter()
        {
            
        }
       
        private void RemoveBlankRow(DataTable dt)
        {
            var rowsToRemove = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                bool blankrow = true;
                foreach (var item in row.ItemArray)
                {
               
                    if (!string.IsNullOrEmpty(item.ToString()))
                    {
                        blankrow = false;
                        break;
                    }

                }
                if (blankrow)
                {
                    rowsToRemove.Add(row);
                }

            }
            foreach (var row in rowsToRemove)
            {
                dt.Rows.Remove(row);
            }

        }

        private void FillBlankCell(DataTable dt)
        {
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {                
                if (dt.Rows[i][0] == "") 
                {
                    dt.Rows[i][0] = valueChange;
                }
                else
                {
                    valueChange = dt.Rows[i][0];
                }
            }
        }


        public DataTable PopulateGrid()
        {
            DataTable dt = new DataTable();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel files (*.xls*, *.xlsx)|*.xls*; *.xlsx";
            if (ofd.ShowDialog() == true)
            {
               
                string _sFileName = ofd.FileName;
                string _sPath = Path.GetFullPath(ofd.FileName);
                ReadExcel(_sPath, _sFileName, dt);
                RemoveBlankRow(dt);
                FillBlankCell(dt);
            }
            return dt;
        }

        private void ReadExcel(string path, string filename, DataTable dt)
        {
            
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                IEnumerable<Sheet> sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                dt.Clear();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(doc, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(doc, row.Descendants<Cell>().ElementAt(i));
                    }

                    dt.Rows.Add(tempRow);
                }

            }
            dt.Rows.RemoveAt(0);
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue == null? "" : cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }


        public string JsonConverter(DataTable dt, int h)
        {
            List<dynamic> cell = new List<dynamic>();

            for (int col = 0; col >= h; col++)
            {
                cell.Add(dt.Columns[col].ColumnName);
            }
            int i = 0;
            foreach (DataRow row in dt.Rows) 
            {

                if (row[1].ToString().ToLower().Trim() == "otros" || row[1].ToString().ToLower().Trim() == "otros") { continue; }
                object reason = new
                {
                    id = i++,
                    sector = row[0].ToString(),
                    description = row[1].ToString().Contains("(A)") ? row[1].ToString()?.Replace("(A)", "") : row[1].ToString(),
                    auto = row[1].ToString().Contains("(A)") ? true : false,
                    notify = row[3].ToString().ToLower().Trim().Equals("si") ? 3 : row[3].ToString().ToLower().Trim().Equals("no") ? 0 : row[3].ToString().ToLower().Trim().Contains("si (fin del") ? 2 : 3
                };

                cell.Add(reason);
                

            }
            var parse = JsonConvert.SerializeObject(cell, Formatting.Indented);

            return parse;
     
        }

  
    }
}
