using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DateDim
{
    public class DimDateGenerator
    {
        public void Generate(DateTime from, DateTime to)
        {
            if (from >= to)
                return;
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("MiladiDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ShamsiDate", typeof(string)));
            tbl.Columns.Add(new DataColumn("ShamsiYear", typeof(int)));
            tbl.Columns.Add(new DataColumn("ShamsiMonth", typeof(int)));
            tbl.Columns.Add(new DataColumn("ShamsiMonthName", typeof(string)));
            tbl.Columns.Add(new DataColumn("ShamsiDayInMonth", typeof(byte)));
            tbl.Columns.Add(new DataColumn("ShamsiSesion", typeof(byte)));
            System.Globalization.PersianCalendar pe = new System.Globalization.PersianCalendar();

            for (DateTime fromDt = from.Date; fromDt <= to.Date; fromDt = fromDt.AddDays(1))
            {
                DataRow dr = tbl.NewRow();
                dr["MiladiDate"] = fromDt;
                dr["ShamsiDate"] = $"{pe.GetYear(fromDt).ToString()}/{pe.GetMonth(fromDt).ToString()}/{pe.GetDayOfMonth(fromDt).ToString()}";
                dr["ShamsiYear"] = pe.GetYear(fromDt);
                dr["ShamsiMonth"] = pe.GetMonth(fromDt);
                dr["ShamsiMonthName"] = GetShamsiMonthName(pe.GetMonth(fromDt));
                dr["ShamsiDayInMonth"] = pe.GetDayOfMonth(fromDt);
                dr["ShamsiSesion"] = GetSesion(pe.GetMonth(fromDt));
                tbl.Rows.Add(dr);
            }

            SqlConnection con = new SqlConnection(AppConfig.GetConnectionString());
            //create object of SqlBulkCopy which help to insert  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            //assign Destination table name  
            objbulk.DestinationTableName = "DimDate";

            objbulk.ColumnMappings.Add("MiladiDate", "MiladiDate");
            objbulk.ColumnMappings.Add("ShamsiDate", "ShamsiDate");
            objbulk.ColumnMappings.Add("ShamsiYear", "ShamsiYear");
            objbulk.ColumnMappings.Add("ShamsiMonth", "ShamsiMonth");
            objbulk.ColumnMappings.Add("ShamsiMonthName", "ShamsiMonthName");
            objbulk.ColumnMappings.Add("ShamsiDayInMonth", "ShamsiDayInMonth");
            objbulk.ColumnMappings.Add("ShamsiSesion", "ShamsiSesion");


            try
            {

                con.Open();
                //insert bulk Records into DataBase.  
                objbulk.WriteToServer(tbl);
                con.Close();
                MessageBox.Show("با موفقیت انجام شد", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetSesion(int month)
        {
            switch (month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                case 5:
                case 6:
                    return 2;
                case 7:
                case 8:
                case 9:
                    return 3;
                case 10:
                case 11:
                case 12:
                    return 4;
                default:
                    return 0;
            }
        }

        private string GetShamsiMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "اذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return string.Empty;
            }
        }
    }
}
