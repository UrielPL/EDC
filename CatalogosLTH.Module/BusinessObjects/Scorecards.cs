using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Scorecards")]
    [XafDefaultProperty("Scorecards")]
    public class Scorecards : XPLiteObject
    {
        public Scorecards() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Scorecards(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Key(true)]
        public int OID { get; set; }

        private double _sc_autoauditoria;
        public double sc_autoauditoria
        {
            get { return _sc_autoauditoria; }
            set { SetPropertyValue("sc_autoauditoria", ref _sc_autoauditoria, value); }
        }

        private double _sc_auditoriaedc3;
        public double sc_auditoriaedc3
        {
            get { return _sc_auditoriaedc3; }
            set { SetPropertyValue("sc_auditoriaedc3", ref _sc_auditoriaedc3, value); }
        }

        private double _sc_tablafinanciera;
        public double sc_tablafinanciera
        {
            get { return _sc_tablafinanciera; }
            set { SetPropertyValue("sc_tablafinanciera", ref _sc_tablafinanciera, value); }
        }

        private double _sc_proyecto;
        public double sc_proyecto
        {
            get { return _sc_proyecto; }
            set { SetPropertyValue("sc_proyecto", ref _sc_proyecto, value); }
        }

        private double _sc_proyecto2;
        public double sc_proyecto2
        {
            get { return _sc_proyecto2; }
            set { SetPropertyValue("sc_proyecto2", ref _sc_proyecto2, value); }
        }

        private double _sc_proyecto3;
        public double sc_proyecto3
        {
            get { return _sc_proyecto3; }
            set { SetPropertyValue("sc_proyecto3", ref _sc_proyecto3, value); }
        }

        private double _scorecard;
        public double scorecard
        {
            get { return _scorecard; }
            set { SetPropertyValue("scorecard", ref _scorecard, value); }
        }


        private double _puntaje_final;
        public double puntaje_final
        {
            get { return _puntaje_final; }
            set { SetPropertyValue("puntaje_final", ref _puntaje_final, value); }
        }

        private mdl_distribuidor _distribuidor;
        public mdl_distribuidor distribuidor
        {
            get { return _distribuidor; }
            set { SetPropertyValue("distribuidor", ref _distribuidor, value); }
        }

        private DateTime _fechaCarga;
        public DateTime fechaCarga
        {
            get { return _fechaCarga; }
            set { SetPropertyValue("fechaCarga", ref _fechaCarga, value); }
        }

        [Action(Caption = "Importar Data")]
        public void importar()
        {
            var filePath = @"C:\Users\Administrator\Documents\Score card EDC.xlsx";
            List<model> lista = new List<model>();
            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Hoja1"];

                for(int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    model registro = new model();
                    for(int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        switch (col)
                        {
                            case 1:
                                registro.distribuidor = worksheet.Cells[row, col].Text;
                                break;
                            case 2:
                                registro.gerente = worksheet.Cells[row, col].Text;
                                break;
                            case 3:
                                registro.ejecutivo = worksheet.Cells[row, col].Text;
                                break;
                            case 4:
                                registro.scorecard = worksheet.Cells[row, col].Text;
                                break;
                            case 5:
                                registro.autoaudi = worksheet.Cells[row, col].Text;
                                break;
                            case 6:
                                registro.audedc3 = worksheet.Cells[row, col].Text;
                                break;
                            case 7:
                                registro.tablafinan = worksheet.Cells[row, col].Text;
                                break;
                            case 8:
                                registro.proyecto = worksheet.Cells[row, col].Text;
                                break;
                            case 9:
                                registro.proyecto2 = worksheet.Cells[row, col].Text;
                                break;
                            case 10:
                                registro.proyecto3 = worksheet.Cells[row, col].Text;
                                break;
                            case 11:
                                registro.scorecard = worksheet.Cells[row, col].Text;
                                break;
                        }
                    }

                    lista.Add(registro);
                }
            }
        }
    }

    public class model
    {
        public string distribuidor { get; set; }
        public string autoaudi { get; set; }
        public string audedc3 { get; set; }
        public string tablafinan { get; set; }
        public string proyecto { get; set; }
        public string proyecto2 { get; set; }
        public string proyecto3 { get; set; }
        public string puntaje { get; set; }
        public string scorecard { get; set; }
        public string gerente { get; set; }
        public string ejecutivo { get; set; }
    }
}
