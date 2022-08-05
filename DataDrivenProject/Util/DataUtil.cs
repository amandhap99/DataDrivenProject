using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenProject.Util
{
    public class DataUtil
    {
        public static object[] getTestData(ExcelReaderFile xls, string testCaseName)
        {
            string sheetName = "Data";

            int testStartRowNum = 1;

            while (!xls.getCellData(sheetName, 0, testStartRowNum).Equals(testCaseName))
            {
                testStartRowNum++;
            }
            Console.WriteLine("Test starts from row : " + testStartRowNum);

            int colStartRowNum = 1 + testStartRowNum;
            int dataStartRowNum = 2 + testStartRowNum;

            //calculate rows of data
            int rows = 0;
            while (!xls.getCellData(sheetName, 0, dataStartRowNum + rows).Equals(""))
            {
                rows++;

            }
            Console.WriteLine("Total rows :" + rows);

            //calculate total cols of data
            int cols = 0;
            while (!xls.getCellData(sheetName, cols, colStartRowNum).Equals(""))
            {
                cols++;
            }
            Console.WriteLine("Total cols :" + cols);

            //read the data

            object[][] data = new object[rows][];
            int dataRow = 0;
            Dictionary<string, string> table = null;

            for (int rNum = dataStartRowNum; rNum < dataStartRowNum + rows; rNum++)
            {
                data[rNum - dataStartRowNum] = new object[1];
                table = new Dictionary<string, string>();
                for (int cNum = 0; cNum < cols; cNum++)
                {
                    string key = xls.getCellData(sheetName, cNum, colStartRowNum);
                    string value = xls.getCellData(sheetName, cNum, rNum);
                    table.Add(key, value);
                }
                data[dataRow][0] = table;
                dataRow++;
                //0,0 0,1 0,2
                //1,0 1,1 1,2
            }
            return data;
        }

        public static bool isTestRunnable(string testCaseName, ExcelReaderFile xls)
        {
            string sheetName = "TestCases";
            int rows = xls.getRowCount(sheetName);
            for (int r = 2; r <= rows; r++)
            {
                string tName = xls.getCellData(sheetName, "TCID", r);
                if (tName.Equals(testCaseName))
                {
                    string runmode = xls.getCellData(sheetName, "Runmode", r);
                    if (runmode.Equals("Y"))
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
