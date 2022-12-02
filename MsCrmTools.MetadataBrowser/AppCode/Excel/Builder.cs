using Microsoft.Xrm.Sdk.Metadata;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MsCrmTools.MetadataBrowser.AppCode.Excel
{
    internal class Builder
    {
        public void BuildFile(string path, ListView list, string sheetName, Control parent = null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage innerWorkBook = new ExcelPackage();
            ExcelWorksheet sheet = innerWorkBook.Workbook.Worksheets.Add(sheetName);

            int index = 1;
            foreach (ColumnHeader column in list.Columns)
            {
                sheet.Cells[1, index++].Value = column.Text;
            }

            ExcelRange r = sheet.Cells["1:1"];

            r.Style.Font.Bold = true;
            r.Style.Fill.PatternType = ExcelFillStyle.Solid;
            r.Style.Fill.BackgroundColor.SetColor(Color.Navy);
            r.Style.Font.Color.SetColor(Color.White);

            int row = 2;
            foreach (ListViewItem item in list.Items)
            {
                index = 1;

                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    sheet.Cells[row, index].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    string text = subItem.Text;

                    if (subItem.Tag != null)
                    {
                        if (subItem.Tag is CascadeConfigurationInfo cci)
                        {
                            text = $"Assign: {cci.Assign}\rDelete: {cci.Delete}\rMerge: {cci.Merge}\rReparent: {cci.Reparent}\rRollupView: {cci.RollupView}\rShare: {cci.Share}\rUnshare: {cci.Unshare}";

                            sheet.Column(index).Width = 30;
                            sheet.Cells[row, index].Style.WrapText = true;
                        }
                        else if (subItem.Tag is AssociatedMenuConfigurationInfo amci)
                        {
                            text = $"Behavior: {amci.Behavior}";
                            if (amci.Behavior == AssociatedMenuBehavior.UseLabel)
                            {
                                text += $"\r\nLabel: {amci.Label?.UserLocalizedLabel?.Label} ({amci.Label?.UserLocalizedLabel?.LanguageCode})";
                            }
                            text += $"\r\nGroup: {amci.Group}";
                            text += $"\r\nOrder: {amci.Order}";

                            sheet.Column(index).Width = 30;
                            sheet.Cells[row, index].Style.WrapText = true;
                        }
                    }

                    sheet.Cells[row, index].Value = text;
                    sheet.Column(index++).AutoFit();
                }

                row++;
            }

            innerWorkBook.SaveAs(new FileInfo(path));

            if (parent != null)
            {
                if (DialogResult.Yes == MessageBox.Show(parent,
                        $"File saved to {path}!\n\nWould you like to open it now? (Requires Microsoft Excel)",
                        "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    Process.Start("Excel.exe", $"\"{path}\"");
                }
            }
        }
    }
}