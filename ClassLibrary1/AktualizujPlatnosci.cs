using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AktualizujPlatnosciNMS
{
        public class WybierzDate : Form
        {
            private IContainer components = (IContainer)null;
            private List<ListaPlatnosci> ListaPlatnosci;
            public SqlConnection sqlConnection;
            private string Atr_Wartosc;
            private ListBox lbResult;
            private Button btnExecute;
            private Button btnCancel;
            private Panel pnBottom;
            private RadioButton rbYes;
            private RadioButton rbNo;
            private Panel pnTop;

            public WybierzDate(List<ListaPlatnosci> list)
            {
                ListaPlatnosci = list;
                InitializeComponent();
            }

            private void BrnCancelClick(object sender, EventArgs e)
            {
                Close();
            }

            private void BtnExecuteClick(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                lbResult.Items.Clear();
                btnExecute.Enabled = false;
                Atr_Wartosc = !rbNo.Checked ? "TAK" : "NIE";
                foreach (ListaPlatnosci traPlat in ListaPlatnosci)
                {
                    lbResult.Items.Add(("PRLV_GIDNumer = " + traPlat.TrP_GIDTyp + ":" + traPlat.TrP_GIDNumer));
                    lbResult.Items.Add(("-- " + this.Update((int)traPlat.TrP_GIDTyp, traPlat.TrP_GIDNumer, this.Atr_Wartosc) + " --"));
                }
                lbResult.Items.Add((object)"Zadanie wykonano pomyślnie!");
                btnCancel.Text = "Zamknij";
                Cursor.Current = Cursors.Default;


            }

            private string Update(int TrP_GIDTyp, int TrP_GIDNumer, string wartosc)
            {
                string str = "OK";
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "\r\nUPDATE CDN.Atrybuty SET\r\nAtr_Wartosc = '" + wartosc + "' \r\nWHERE \r\nAtr_ObiTyp = " + TrP_GIDTyp + " AND Atr_ObiNumer = " + TrP_GIDNumer + " AND Atr_AtkId = 6; UPDATE CDN.Atrybuty SET Atr_Wartosc = '" + wartosc + "' WHERE Atr_ObiTyp = 12300 AND Atr_ObiNumer = " + TrP_GIDNumer + " AND Atr_AtkId = 6 and 7684 = " + TrP_GIDTyp + " ";
                try
                {
                    if (this.sqlConnection.State == ConnectionState.Closed)
                        this.sqlConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    str = "Błąd SQL. " + ex.Message;
                }
                finally
                {
                    this.sqlConnection.Close();
                }
                return str;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && this.components != null)
                    this.components.Dispose();
                base.Dispose(disposing);
            }


            private void InitializeComponent()
            {
                pnTop = new Panel();
                rbNo = new RadioButton();
                rbYes = new RadioButton();
                pnBottom = new Panel();
                btnCancel = new Button();
                btnExecute = new Button();
                lbResult = new ListBox();
                pnTop.SuspendLayout();
                pnBottom.SuspendLayout();
                SuspendLayout();
                pnTop.Controls.Add((Control)rbNo);
                pnTop.Controls.Add((Control)rbYes);
                pnTop.Dock = DockStyle.Top;
                pnTop.Location = new Point(0, 0);
                pnTop.Name = "pnTop";
                pnTop.Size = new Size((int)byte.MaxValue, 39);
                pnTop.TabIndex = 0;
                rbNo.Dock = DockStyle.Fill;
                rbNo.Location = new Point(104, 0);
                rbNo.Name = "rbNo";
                rbNo.Size = new Size(151, 39);
                rbNo.TabIndex = 1;
                rbNo.Text = "NIE";
                rbNo.UseVisualStyleBackColor = true;
                rbYes.Checked = true;
                rbYes.Dock = DockStyle.Left;
                rbYes.Location = new Point(0, 0);
                rbYes.Name = "rbYes";
                rbYes.Size = new Size(104, 39);
                rbYes.TabIndex = 0;
                rbYes.TabStop = true;
                rbYes.Text = "TAK";
                rbYes.UseVisualStyleBackColor = true;
                pnBottom.Controls.Add((Control)btnCancel);
                pnBottom.Controls.Add((Control)btnExecute);
                pnBottom.Dock = DockStyle.Bottom;
                pnBottom.Location = new Point(0, 161);
                pnBottom.Name = "pnBottom";
                pnBottom.Size = new Size((int)byte.MaxValue, 27);
                pnBottom.TabIndex = 1;
                btnCancel.Dock = DockStyle.Right;
                btnCancel.Location = new Point(180, 0);
                btnCancel.Name = "btnCancel";
                btnCancel.Size = new Size(75, 27);
                btnCancel.TabIndex = 1;
                btnCancel.Text = "Anuluj";
                btnCancel.UseVisualStyleBackColor = true;
                btnCancel.Click += new EventHandler(BrnCancelClick);
                btnExecute.Dock = DockStyle.Left;
                btnExecute.Location = new Point(0, 0);
                btnExecute.Name = "btnExecute";
                btnExecute.Size = new Size(75, 27);
                btnExecute.TabIndex = 0;
                btnExecute.Text = "Wykonaj";
                btnExecute.UseVisualStyleBackColor = true;
                btnExecute.Click += new EventHandler(BtnExecuteClick);
                lbResult.Dock = DockStyle.Fill;
                lbResult.FormattingEnabled = true;
                lbResult.Location = new Point(0, 39);
                lbResult.Name = "lbResult";
                lbResult.Size = new Size((int)byte.MaxValue, 122);
                lbResult.TabIndex = 2;
                AutoScaleDimensions = new SizeF(6f, 13f);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new Size((int)byte.MaxValue, 188);
                Controls.Add((Control)lbResult);
                Controls.Add((Control)pnBottom);
                Controls.Add((Control)pnTop);
                Name = "AktualizacjaAtrybutow";
                StartPosition = FormStartPosition.CenterScreen;
                Text = "Aktualizacja atrybutów";
                pnTop.ResumeLayout(false);
                pnBottom.ResumeLayout(false);
                ResumeLayout(false);
            }

        }

        

       

        
    }
