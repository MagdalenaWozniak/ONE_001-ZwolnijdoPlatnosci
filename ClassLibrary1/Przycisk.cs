using Hydra;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CallbackAssemblyDescription(Name: "ONE-001 - Zwolnij do płatności",
                                           Description: "Aktualizacja Atrybutu na płatnościach",
                                           Author: "OneSupport",
                                           Version: "1.0",
                                           SystemVersion: "2017.2.1",
                                           Created: "19-06-2019")]


namespace AktualizujPlatnosciNMS
{
    [SubscribeProcedure(Procedures.Preliminarz, "Aktualizacja Atrybutu na płatnościach")]
    public class ButtonCLS : Callback
    {

        //własność i klasa ClaWindow

        /*ClaWindow Parent;
        ClaWindow preliminarz;
        ClaWindow przycisk;*/
        ClaWindow zakladkaPreliminarz;
        ClaWindow przyciskExecute;
        ClaWindow popGotowka;
        ClaWindow popFiltr;
        private int listID;

        public override void Init()
        {
            AddSubscription(false, 0, Events.OpenWindow, new TakeEventDelegate(OnOpenwindow));
            AddSubscription(false, 0, Events.ResizeWindow, new TakeEventDelegate(Resize));
        }
        //przeniesienie z pow
        public bool OnOpenwindow(Procedures ProcId, int ControlId, Events Event)
        {
            zakladkaPreliminarz = GetWindow().AllChildren["?PreliminarzTab"];
            Rectangle WndBnd = GetWindow().Bounds;
            przyciskExecute = zakladkaPreliminarz.Children.Add(ControlTypes.button);
            przyciskExecute.Visible = true;
            przyciskExecute.ToolTipRaw = "Oznaczenie dokumentów zwolnionych do płatności.";
            przyciskExecute.TextRaw = "Pla";

            //pobiera współrzędne przycisku gotówka
            popGotowka = GetWindow().AllChildren["?EksportButton"];
            przyciskExecute.Bounds = new Rectangle(360, popGotowka.Bounds.Y, 17, 15);
            listID = GetWindow().AllChildren["?PreliminarzBrowse"].Id;

            //naduszenie przycisku
            przyciskExecute.OnAfterAccepted += new TakeEventDelegate(listplat);

            popFiltr = GetWindow().AllChildren["?bUkryjFiltrDodatkowy"];
            //naduszenie przycisku
            popFiltr.OnAfterAccepted += new TakeEventDelegate(Resize);

            ////pupupButtonUkryjFiltr = this.GetWindow().get_AllChildren().get_Item("?bUkryjFiltrDodatkowy");
            // ISSUE: method pointer
            ////((ClaWindowRaw)this.pupupButtonUkryjFiltr).add_OnAfterAccepted(new TakeEventDelegate((object)this, __methodptr(OnResizeWindow)));

            /*  Parent = GetWindow();
              preliminarz = Parent.AllChildren["?PreliminarzBrowse"];
              //pobranie id okna
              prel = Parent.AllChildren["?PreliminarzTab"].Id;

              //pobiera wymiary okna
              Rectangle prost = GetWindow().Bounds;

              //dodaje i definiuje przycisk
              przycisk = preliminarz.AllChildren.Add(ControlTypes.button);
              przycisk.Visible = true;
              przycisk.TextRaw = "Atr";
              przycisk.ToolTipRaw = "Przycisk dodaje wartość do atrybutów wskazanych płatności.";
              przycisk.Bounds = new Rectangle(40, prost.Height - 100, 17, 15);

              AddSubscription(false, przycisk.Id, Events.Accepted, new TakeEventDelegate(listplat));
              */
            return true;
        }

        private bool Resize (Procedures ProcId, int ControlId, Events Event) {

            popGotowka = GetWindow().AllChildren["?EksportButton"];
            przyciskExecute.Bounds = new Rectangle(360, popGotowka.Bounds.Y, 17, 15);

            return true;
        }

        //przeniesienie z pow
        public bool listplat(Procedures ProcId, int ControlId, Events Event)
        {
           List<ListaPlatnosci> lista = new List<ListaPlatnosci>();
            ADODB._Recordset recordset = Runtime.WindowController.GetQueueMarked((int)ProcId, listID, GetCallbackThread());

            try
            {
                StringBuilder dokumenty = new StringBuilder();

                if (recordset != null)
                {
                    int count = recordset.RecordCount;

                    if (count != 0)
                    {
                        recordset.MoveFirst();

                        while (recordset.EOF == false)
                        {
                            ADODB.Fields fields = recordset.Fields;
                            lista.Add(new ListaPlatnosci()
                            {
                                TrP_GIDTyp = Convert.ToInt16(fields[0].Value),
                                TrP_GIDFirma = Convert.ToInt32(fields[1].Value),
                                TrP_GIDNumer = Convert.ToInt32(fields[2].Value),
                                TrP_GIDLp = Convert.ToInt16(fields[3].Value)
                            });
                            recordset.MoveNext();
                        }

                        WybierzDate wybierzDate = new WybierzDate(lista);
                        wybierzDate.sqlConnection = Runtime.ActiveRuntime.Repository.Connection;
                        int num1 = (int)wybierzDate.ShowDialog();

                    }
                    else
                    {
                        //Runtime.WindowController.UnlockThread();
                        MessageBox.Show("Nie zaznaczono dokumentów. Operacja anulowana.", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //Runtime.WindowController.LockThread();
                    }
                }

                else
                    {
                        //Runtime.WindowController.UnlockThread();
                        MessageBox.Show("Nie zaznaczono dokumentów. Operacja anulowana.", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //Runtime.WindowController.LockThread();
                    }
            }

            catch (Exception ex)
            {
                    //Runtime.WindowController.UnlockThread();
                    MessageBox.Show("Błąd pobierania zaznaczonych dokumentów --> " + ex.Message, "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Runtime.WindowController.LockThread();
            }

            return true;

        }


        public override void Cleanup()
        { }

    }

  
}
