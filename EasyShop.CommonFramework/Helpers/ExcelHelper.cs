using EasyShop.CommonFramework.Attributes;
using EasyShop.CommonFramework.Const;
using EasyShop.CommonFramework.Exception;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace EasyShop.CommonFramework.Helpers
{
    /// <summary>
    /// excel工具类
    /// </summary>
    public static class ExcelHelper
    {

        #region 导入
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static DataTable ImportExcel(IFormFile file, int sheetIndex = 0)
        {
            ISheet sheet;
            var fileExt = Path.GetExtension(file.FileName).ToLower();
            if (fileExt != ".xls" && fileExt != ".xlsx")
            {
                throw new CustomException("文件类型错误");
            }
            var stream = file.OpenReadStream();
            stream.Position = 0;
            if (fileExt == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                sheet = hssfwb.GetSheetAt(sheetIndex);
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                sheet = hssfwb.GetSheetAt(sheetIndex);
            }
            IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable { TableName = sheet.SheetName };
            bool flag = false;
            while (rows.MoveNext())
            {
                IRow row = (IRow)rows.Current;
                if (row.GetCell(0) != null && row.GetCell(0).CellType != CellType.Blank)
                {

                    flag = AddRow(row, dt, flag);
                }

            }
            return dt;
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileExt"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static DataTable ImportExcel(Stream stream, string fileExt, int sheetIndex = 0)
        {
            ISheet sheet;
            stream.Position = 0;
            if (fileExt == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                sheet = hssfwb.GetSheetAt(sheetIndex);
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                sheet = hssfwb.GetSheetAt(sheetIndex);
            }
            IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable { TableName = sheet.SheetName };
            bool flag = false;
            while (rows.MoveNext())
            {
                IRow row = (IRow)rows.Current;
                if (row.GetCell(0) != null && row.GetCell(0).CellType != CellType.Blank)
                {

                    flag = AddRow(row, dt, flag);
                }

            }
            return dt;
        }

        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dt"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private static bool AddRow(IRow row, DataTable dt, bool flag)
        {
            var cellNum = row.LastCellNum;
            DataRow dr = dt.NewRow();
            if (!flag)
            {
                for (int j = 0; j < cellNum; j++)
                {
                    ICell cell = row.GetCell(j);
                    if (!string.IsNullOrWhiteSpace(cell.GetCellValue()))
                    {
                        dt.Columns.Add(cell.GetCellValue(), typeof(string));
                    }

                }
                return true;
            }
            for (int i = 0; i < cellNum; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell == null)
                {
                    dr[i] = null;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(cell.GetCellValue()))
                    {
                        dr[i] = cell.GetCellValue();
                    }

                }
            }
            dt.Rows.Add(dr);
            return flag;
        }

        /// <summary>
        /// 获取单元格的值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns></returns>
        private static string GetCellValue(this ICell cell)
        {
            if (cell.CellType == CellType.Blank || cell.CellType == CellType.Formula || cell.CellType == CellType.String)
            {
                if (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric)
                {
                    return cell.NumericCellValue.ToString();
                }
                else
                {
                    return cell.StringCellValue;
                }
            }
            else
            {
                return DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue.ToString() : cell.NumericCellValue.ToString();
            }
        }

        #endregion


        #region 导出
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static byte[] Export<T>(List<T> dataSource) where T : new()
        {
            using (var memoryStream = new MemoryStream())
            {
                HSSFWorkbook workbook = new HSSFWorkbook();

                /*创建*/
                ISheet sheet = workbook.CreateSheet("Sheet1");

                /*类特性*/
                List<ExcelColumnAttribute> excelColumnAttributes = GetColumnAttributes<T>();

                /*创建表头*/
                IRow header_row = sheet.CreateRow(0);
                //行高
                header_row.Height = 500;
                CreateHeader(sheet, header_row, excelColumnAttributes, workbook);

                /*填充数据*/
                CreateDataRows(sheet, excelColumnAttributes, dataSource, workbook);

                workbook.Write(memoryStream);
                workbook.Close();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 获取列特性
        /// </summary>
        /// <returns></returns>
        public static List<ExcelColumnAttribute> GetColumnAttributes<T>()
        {
            List<ExcelColumnAttribute> excelColumnAttribute = new List<ExcelColumnAttribute>();
            Type t = typeof(T);
            PropertyInfo[] arryProperty = t.GetProperties();
            if (arryProperty.Any())
            {
                foreach (PropertyInfo p in arryProperty)
                {
                    ExcelColumnAttribute attribute = p.GetCustomAttribute<ExcelColumnAttribute>();
                    if (attribute != null)
                    {
                        excelColumnAttribute.Add(attribute.SetPropertyName(p.Name));
                    }
                }
            }
            return excelColumnAttribute.OrderBy(p => p.Sort).ToList();
        }

        /// <summary>
        /// 创建表头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="excelColumnAttributes"></param>
        /// <param name="workbook"></param>
        private static void CreateHeader(ISheet sheet, IRow row, List<ExcelColumnAttribute> excelColumnAttributes, HSSFWorkbook workbook)
        {
            var cellStyle = GetCellStyle(workbook);
            for (int i = 0; i < excelColumnAttributes.Count; i++)
            {
                var cell = row.CreateCell(i, CellType.String);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(excelColumnAttributes[i].ColumnName);
                sheet.SetColumnWidth(i, (int)((excelColumnAttributes[i].Width + 0.72) * 100));
            }
        }

        /// <summary>
        /// 单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static ICellStyle GetCellStyle(HSSFWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.WrapText = true;
            IFont font = workbook.CreateFont();
            //字体名称
            font.FontName = "等线";
            //字体POINTS
            font.FontHeightInPoints = 10;
            style.SetFont(font);
            return style;
        }

        /// <summary>
        /// 创建数据行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="excelColumnAttributes"></param>
        /// <param name="dataSource"></param>
        /// <param name="workbook"></param>
        private static void CreateDataRows<T>(ISheet sheet, List<ExcelColumnAttribute> excelColumnAttributes, List<T> dataSource, HSSFWorkbook workbook)
        {
            var cellStyle = GetCellStyle(workbook);

            var dateCellStyle = GetCellStyle(workbook);

            IDataFormat dataFormat = workbook.CreateDataFormat();

            var rowIndex = 1;
            foreach (var item in dataSource)
            {
                var row = sheet.CreateRow(rowIndex);
                for (int i = 0; i < excelColumnAttributes.Count; i++)
                {
                    var cell = row.CreateCell(i, CellType.String);
                    cell.CellStyle = cellStyle;

                    var value = item.GetType().GetProperty(excelColumnAttributes[i].PropertyName).GetValue(item);
                    cell.SetCellValue(value.ToString());

                    if(excelColumnAttributes[i].DataType== ExcelDataTypeConst.Photo)
                    {
                        AddPic(sheet,workbook,value.ToString(),cell.RowIndex,cell.ColumnIndex);
                    }
                }
                rowIndex++;
            }

        }


        /// <summary>
        /// 表格内渲染图片
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="workbook">工作簿</param>
        /// <param name="fileurl">图片地址（只能是本地服务器地址）</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private static void AddPic(ISheet sheet, HSSFWorkbook workbook, string fileurl, int row, int col)
        {
            byte[] bytes = null;
            //去掉域名和端口验证文件相对地址是否存在于当前服务器
            string newFileUrl= string.Join("/", fileurl.Split("//").LastOrDefault().Split("/").Skip(1).ToArray());
            if(File.Exists(newFileUrl))
            {
                //存在，代表当前文件是本地服务器文件
                bytes = File.ReadAllBytes(newFileUrl);
            }
            else
            {
                //当前文件是其他服务器文件，需要下载到当前服务器
                bytes = HttpHelper.DownloadFile(fileurl);
            }
            int picindex = workbook.AddPicture(bytes, PictureType.JPEG);
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, col, row, col + 1, row + 1);
            patriarch.CreatePicture(anchor, picindex);

        }

       
        #endregion
    }
}

