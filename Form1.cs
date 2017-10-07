using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using SiebelLogScan.Properties;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    public partial class SiebelLogScan : Form
    {
        public DataSet LOGDS;
        public DataTable SQLDT;
        public DataTable EXECDT;
        public DataTable ERRDT;
        public DataTable TASKDT;
        public DataTable WFDT;
        private bool ValidLog;
        string Notepad = Settings.Default.Notepad.ToString();
        public SiebelLogScan()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Siebel Log|*.log";
            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            textBox1.Text = filename;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SQLAnalyzeFile();

            }
            }

        public void SQLAnalyzeFile()
        {
            this.CreateData();
            bool ValidLog = false;
            bool errflg = false;
            string filename = openFileDialog1.FileName;
            StreamReader sr = new StreamReader(filename);
            string str1 = sr.ReadLine();
            DateTime dateTime = new DateTime();
            int linecnt = 0;
            int linenbr = 0;
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str6 = "";
            string str7 = "";
            string str8 = "";
            string str9 = "";
            string str10 = "";
            string str11 = "";
            string str12 = "";
            string str13 = "";
            string str14 = "";
            string str15 = "";
            string str16 = "";
            string str17 = "";
            bool selectsql = false;
            bool isSQL = false;
            bool isBind = false;
            bool flag3 = false;
            object[] objRow;
            if (checked(str1.IndexOf(".log")) > 0)
            {
                ValidLog = true;
                linecnt = 1;
            }
            while ((str1 != null) && (ValidLog == true))
            {
                num1 = str1.IndexOf("\t");
                {

                    

                    //string str4 = str1.substring(0, str1.indexof("\t")); ///component name
                    //bool flag3 = true;

                    if (str1.IndexOf("Bind variable") > 0)
                    {
                        isSQL = false;
                        isBind = true;
                        str2 = str1.Substring(checked(str1.LastIndexOf(":") + 1)).Trim();
                        str4 = string.Concat(str4, str2, Environment.NewLine);
                    }
                    if (str1.IndexOf("SQL Statement") > 0 && isBind == true)
                    {
                        DataRowCollection newrow = this.SQLDT.Rows;
                        objRow = new object[] { str3, str15, str6, str4, linenbr };
                        newrow.Add(objRow);
                        str14 = str6;
                        isBind = false;
                        str6 = "";
                        str4 = "";
                        str3 = "";
                    }

                    if (isSQL)
                    { 
                        if(str1.IndexOf("WHERE")>0 && selectsql == true )
                        {
                            str15 = str6.Substring(checked(str6.LastIndexOf(".")+1)).Trim();
                           str15 = str15.Substring(0, str15.IndexOf(" "));
                        }
                        str6 = string.Concat(str6, str1.TrimEnd(new char[0]), Environment.NewLine); //reads sql
                        if (str1.IndexOf("INSERT INTO SIEBEL.") > 0 && selectsql == false)
                        {
                            str15 = str6.Substring(checked(str6.LastIndexOf(".")+1)).Trim();
                            str15 = str15.Substring(0, str15.IndexOf(" ("));
                        }
                        if (str1.IndexOf("UPDATE SIEBEL.") > 0 && selectsql == false)
                        {
                            str15 = str6.Substring(checked(str6.LastIndexOf(".")+1)).Trim();
                            str15 = str15.Substring(0, str15.IndexOf("SET"));
                        }
                    }
                    if (str1.IndexOf("SELECT statement with ID") > 0 || str1.IndexOf("INSERT/UPDATE statement with ID") > 0)
                    {
                        isSQL = true;
                        linenbr = linecnt;
                        str3 = str1.Substring(checked(str1.LastIndexOf(":") + 1)).Trim();
                        if(str1.IndexOf("SELECT statement with ID") > 0)
                        {
                            selectsql = true;
                        }
                        else
                        {
                            selectsql = false;
                        }
                    }
                    if (str1.IndexOf("SQL Statement") > 0)
                    {
                        linenbr = linecnt;
                        num5 = str1.LastIndexOf(":");
                        num3 = str1.IndexOf("SQL Statement");
                        str7 = str1.Substring((num3),num5-num3).Trim();
                        str8 = str1.Substring(checked(num5 + 1), checked(checked(str1.IndexOf(".") + 4) - num5));
                        DataRowCollection newrow = this.EXECDT.Rows;
                        objRow = new object[] { str7, str14, str8, linenbr };
                        newrow.Add(objRow);
                    }
                    if (num1 > 0 && (str1.Substring(checked(num1 + 1)).StartsWith("Error")) )
                    {
                        num4 = str1.IndexOf("SBL");
                        if (num4 > 0)
                        {
                            str9 = str1.Substring(checked(num4), 13).Trim(); ///error code
                            str10 = str1.Substring(checked(checked(num4 + 13) + 1)).Trim(); ///error description
                            num2 = idxofn(str1, '\t', 4) + 1;//index of date
                            if (num2 > 0 && DateTime.TryParseExact(str1.Substring(num2, 19), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                            {
                                str11 = dateTime.ToString("dd-MMM-yy HH:mm:ss");
                            }
                            str12 = "WARNING";
                            if (str1.Substring(checked(num1 + 1)).StartsWith("Error"))
                            {
                                str12 = "ERROR";
                            }
                            linenbr = linecnt;
                            DataRowCollection newrow = this.ERRDT.Rows;
                            objRow = new object[] { str11, str12, str9, str10, linenbr };
                            newrow.Add(objRow);
                            }
                        }
                    if (str12 == "ERROR")
                    {
                        DataRowCollection newrow = this.TASKDT.Rows;
                        objRow = new object[] { str9,str10, linenbr };
                        newrow.Add(objRow);
                        DataRowCollection newrow1 = this.WFDT.Rows;
                        objRow = new object[] { str16, str9,str10, linenbr };
                        newrow1.Add(objRow);
                        str12 = "";
                        str9 = "";
                    }
                    //Getting the Task details
                    if (str1.IndexOf("Task engine requested to navigate to next step:") > 0)
                    {
                        num2 = idxofn(str1, '\t', 4) + 1;//index of date
                        if (num2 > 0 && DateTime.TryParseExact(str1.Substring(num2, 19), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                        {
                            str11 = dateTime.ToString("dd-MMM-yy HH:mm:ss");
                        }
                        num6 = str1.IndexOf(": '")+3;
                        num7 = str1.LastIndexOf("'");                        
                        str13 = str1.Substring(checked(num6), num7 - num6).Trim();
                        linenbr = linecnt;
                        DataRowCollection newrow = this.TASKDT.Rows;
                        objRow = new object[] {str13, str11, linenbr };
                        newrow.Add(objRow);
                    }
                        //Getting the Workflow details
                        if(str1.IndexOf("Instantiating process definition") >0)
                    {
                        num8 = str1.IndexOf("'")+1;
                        num9 = str1.LastIndexOf("'");
                        str16 = str1.Substring(checked(num8), num9 - num8).Trim();
                    }
                        if (str1.IndexOf("Instantiating step definition") > 0)
                        {
                            num2 = idxofn(str1, '\t', 4) + 1;//index of date
                            if (num2 > 0 && DateTime.TryParseExact(str1.Substring(num2, 19), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                            {
                                str11 = dateTime.ToString("dd-MMM-yy HH:mm:ss");
                            }
                            num8 = str1.IndexOf("'")+1;
                            num9 = str1.LastIndexOf("'");
                            str17 = str1.Substring(checked(num8), num9 - num8).Trim();
                            linenbr = linecnt;
                            DataRowCollection newrow = this.WFDT.Rows;
                            objRow = new object[] { str16, str17, str11, linenbr };
                            newrow.Add(objRow);
                        }
                }
                str1 = sr.ReadLine();
                linecnt = linecnt + 1;
            }
            dataGridView1.DataSource = SQLDT;
            dataGridView2.DataSource = EXECDT;
            dataGridView3.DataSource = ERRDT;
            dataGridView4.DataSource = TASKDT;
            dataGridView5.DataSource = WFDT;
        }

        private void CreateData()
        {
            this.LOGDS = null;
            this.LOGDS = new DataSet();
            this.SQLDT = new DataTable("SQL");
            this.LOGDS.Tables.Add(this.SQLDT);
            this.SQLDT.Columns.Add("SQLID").Caption = "SQLID";
            this.SQLDT.Columns.Add("BASE TABLE").Caption = "BASE TABLE";
            this.SQLDT.Columns.Add("SQL").Caption = "SQL";
            this.SQLDT.Columns.Add("BIND VARIABLES").Caption = "BIND VARIABLES";
            this.SQLDT.Columns.Add("Line").Caption = "Line";
            this.EXECDT = new DataTable("EXEC");
            this.LOGDS.Tables.Add(this.EXECDT);
            this.EXECDT.Columns.Add("DESCRIPTION").Caption = "DECRIPTION";
            this.EXECDT.Columns.Add("SQL").Caption = "SQL";
            this.EXECDT.Columns.Add("TIME").Caption = "TIME";
            this.EXECDT.Columns.Add("LINE").Caption = "LINE";
            this.ERRDT = new DataTable("ERROR");
            this.LOGDS.Tables.Add(this.ERRDT);
            this.ERRDT.Columns.Add("TIMESTAMP").Caption = "TIMESTAMP";
            this.ERRDT.Columns.Add("SEVERITY").Caption = "SEVERITY";
            this.ERRDT.Columns.Add("ERROR CODE").Caption = "ERROR CODE";
            this.ERRDT.Columns.Add("ERROR DESC").Caption = "ERROR DESC";
            this.ERRDT.Columns.Add("LINE").Caption = "LINE";
            this.TASKDT = new DataTable("TASK");
            this.LOGDS.Tables.Add(this.TASKDT);
            this.TASKDT.Columns.Add("STEP NAME").Caption = "STEP NAME";
            this.TASKDT.Columns.Add("TIMESTAMP").Caption = "TIMESTAMP";
            this.TASKDT.Columns.Add("LINE").Caption = "LINE";
            this.WFDT = new DataTable("WF");
            this.LOGDS.Tables.Add(this.WFDT);
            this.WFDT.Columns.Add("WORKFLOW NAME").Caption = "WORKFLOW NAME";
            this.WFDT.Columns.Add("STEP NAME").Caption = "STEP NAME";
            this.WFDT.Columns.Add("TIMESTAMP").Caption = "TIMESTAMP";
            this.WFDT.Columns.Add("LINE").Caption = "LINE";
        }

        private int idxofn(string Sstr1, char Schar, int Sv2 = 0)
        {
            if (Sv2 <= 0)
                throw new ArgumentException("Can not find the zeroth index of substring in string. Must start with 1");
            int offset = Sstr1.IndexOf(Schar);
            for (int i = 1; i < Sv2; i++)
            {
                if (offset == -1) return -1;
                offset = Sstr1.IndexOf(Schar, offset + 1);
            }
            return offset;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Vijay Nadella to parse Siebel Logs\nVersion: 1.2\n For Bug Reporting Please Email: vijay.chowdary99@birlasoft.com",
   "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void notepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = "Notepad++*";
            openFileDialog2.Filter = "Notepad++|*.exe";
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                Notepad = openFileDialog2.FileName;
                Settings.Default.Notepad = Notepad;
                Settings.Default.Save();
            }
        }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex.Equals(4) && e.RowIndex != -1 && Settings.Default.Notepad.ToString() != "")
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
                {
                    string linenum = dataGridView1.CurrentCell.Value.ToString();
                    OpenNotepad(linenum);

                }
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1 && Settings.Default.Notepad.ToString() != "")
            {
                if (dataGridView2.CurrentCell != null && dataGridView2.CurrentCell.Value != null)
                {
                    string linenum = dataGridView2.CurrentCell.Value.ToString();
                    OpenNotepad(linenum);
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.CurrentCell.ColumnIndex.Equals(4) && e.RowIndex != -1 && Settings.Default.Notepad.ToString() != "")
            {
                if (dataGridView3.CurrentCell != null && dataGridView3.CurrentCell.Value != null)
                {
                    string linenum = dataGridView3.CurrentCell.Value.ToString();
                    OpenNotepad(linenum);
                }
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.CurrentCell.ColumnIndex.Equals(2) && e.RowIndex != -1 && Settings.Default.Notepad.ToString() != "")
            {
                if (dataGridView4.CurrentCell != null && dataGridView4.CurrentCell.Value != null)
                {
                    string linenum = dataGridView4.CurrentCell.Value.ToString();
                    OpenNotepad(linenum);
                }
            }
        }
        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView5.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1 && Settings.Default.Notepad.ToString() != "")
            {
                if (dataGridView5.CurrentCell != null && dataGridView5.CurrentCell.Value != null)
                {
                    string linenum = dataGridView5.CurrentCell.Value.ToString();
                    OpenNotepad(linenum);
                }
            }
        }

        private void OpenNotepad(string line)
        {
            Notepad = Settings.Default.Notepad.ToString();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.StartInfo.FileName = "notepad++";
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(Notepad);
            proc.StartInfo.Arguments = " -n" + line + " " + textBox1.Text + "";
            proc.Start();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SiebelLogScan_Load(object sender, EventArgs e)
        {

        }
    }
}
