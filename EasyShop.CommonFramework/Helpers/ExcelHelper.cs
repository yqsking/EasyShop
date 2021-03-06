﻿using EasyShop.CommonFramework.Attributes;
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
        /// 导入excel,T中属性需要标注ExcelImportColumnAttribute特性
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static List<T> ImportExcel<T>(IFormFile file, int sheetIndex = 0) where T:new()
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
            return dt.ToList<T>(); ;
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileExt"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static List<T> ImportExcel<T>(Stream stream, string fileExt, int sheetIndex = 0) where T:new()
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
            return dt.ToList<T>();
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
                List<ExcelExportColumnAttribute> excelColumnAttributes = GetColumnAttributes<T>();

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
        public static List<ExcelExportColumnAttribute> GetColumnAttributes<T>()
        {
            List<ExcelExportColumnAttribute> excelColumnAttribute = new List<ExcelExportColumnAttribute>();
            Type t = typeof(T);
            PropertyInfo[] arryProperty = t.GetProperties();
            if (arryProperty.Any())
            {
                foreach (PropertyInfo p in arryProperty)
                {
                    ExcelExportColumnAttribute attribute = p.GetCustomAttribute<ExcelExportColumnAttribute>();
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
        private static void CreateHeader(ISheet sheet, IRow row, List<ExcelExportColumnAttribute> excelColumnAttributes, HSSFWorkbook workbook)
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
        private static void CreateDataRows<T>(ISheet sheet, List<ExcelExportColumnAttribute> excelColumnAttributes, List<T> dataSource, HSSFWorkbook workbook)
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

                }
                rowIndex++;
            }

        }

        #endregion
    }
}

