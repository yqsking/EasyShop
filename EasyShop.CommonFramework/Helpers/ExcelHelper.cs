using EasyShop.CommonFramework.Exception;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Data;
using System.IO;

namespace EasyShop.CommonFramework.Helpers
{
    /// <summary>
    /// excel工具类
    /// </summary>
    public  static class ExcelHelper
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
                sheet= hssfwb.GetSheetAt(sheetIndex);
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                sheet= hssfwb.GetSheetAt(sheetIndex);
            }
            IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable { TableName = sheet.SheetName };
            bool flag = false;
            while (rows.MoveNext())
            {
                IRow row = (IRow)rows.Current;
                if ( row.GetCell(0) != null && row.GetCell(0).CellType != CellType.Blank)
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
        public static DataTable ImportExcel(Stream stream,string fileExt, int sheetIndex = 0)
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
                    if(!string.IsNullOrWhiteSpace(cell.GetCellValue()))
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
                    if(!string.IsNullOrWhiteSpace(cell.GetCellValue()))
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
            if (cell.CellType == CellType.Blank || cell.CellType == CellType.Formula|| cell.CellType == CellType.String)
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


    }
}
