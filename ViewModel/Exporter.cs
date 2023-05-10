using OfficeOpenXml;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace AppDB.ViewModel
{
    internal class Exporter
    {
        public void ExportAsExcel<T>(DbSet<T> DbSet, string tableName, int rowsNumber = 50) where T : class
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string name = (DbSet as IQueryable).ElementType.Name;
            saveFileDialog.Title = $"Сохранение {name}";
            saveFileDialog.FileName = $"{name}.xlsx";
            saveFileDialog.Filter = "Excel файл (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromCollection(DbSet, true);
                    package.SaveAs(new FileInfo(saveFileDialog.FileName));
                }
            }
        }

        public void ExportAsExcel(System.Windows.Controls.DataGrid dataGrid, string tableName, int rowsNumber = 50)
        {
            if (dataGrid is null)
            {
                System.Windows.MessageBox.Show($@"DataGrid ""{dataGrid}"" is NULL", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string name = $"{tableName}";
            saveFileDialog.Title = $"Сохранение {name}";
            saveFileDialog.FileName = $"{name}.xlsx";
            saveFileDialog.Filter = "Excel файл (*.xlsx)|*.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    DataTable dataTable = new DataTable();
                    int columnCount = 0;
                    // Считывание названий колонок
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        if (dataGrid.Columns[i] is DataGridTextColumn column)
                        {
                            if (column.Header != null)
                            {
                                dataTable.Columns.Add(column.Header.ToString());
                                columnCount++;
                            }
                        }
                    }

                    for (int i = 0; i < dataGrid.Items.Count; i++) // N - количество строк для считывания
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            var cellContent = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]);
                            if (cellContent is TextBlock textBlock)
                            {
                                dataRow[j] = textBlock.Text;
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                    worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    package.SaveAs(new FileInfo(saveFileDialog.FileName));
                }
            }
        }
    }
}
