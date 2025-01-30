using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using System.IO;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Documentos")]
    [XafDefaultProperty("Nombre")]
    public class Documento : XPObject
    {
        public Documento() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Documento(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public string Nombre { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string Descripcion { get; set; }

        private CategoriaDocumento categoria;
        [Association("Categoria-Documentos")]
        [XafDisplayName("Categoría")]
        public CategoriaDocumento Categoria
        {
            get { return categoria; }
            set { SetPropertyValue("Categoria", ref categoria, value); }
        }

        FileData muestra1;
        [ImmediatePostData]
        public FileData Muestra1
        {
            get { return muestra1; }
            set { SetPropertyValue("Muestra1", ref muestra1, value); }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\Archivos\\";

            if (Muestra1 != null)
            {
                int pos = Muestra1.FileName.IndexOf('.');
                string ext = Muestra1.FileName.Substring(pos, (Muestra1.FileName.Length - pos));
                string newname = Nombre + "Documento" + ext;
                FileStream fileStream = System.IO.File.Create(ruta + newname);
                Muestra1.SaveToStream(fileStream);
                fileStream.Close();
            }
        }
    }

}