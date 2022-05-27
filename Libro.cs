using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria
{
    class Libro
    {
        private string titulo = "";
        private string autor = "";
        private string editorial = "";
        private string isbn = "";
        private string edicion = "";
        private string anio = "";
        private string paginas = "";
        private string categoria = "";
        private string precio = "";
        private string stock = "";

        public Libro(string tit, string aut, string edit, string isb, string edic, string an, string pag, string cat, string prec, string st)
        {
            this.titulo = tit;
            this.autor = aut;
            this.editorial = edit;
            this.isbn = isb;
            this.edicion = edic;
            this.anio = an;
            this.paginas = pag;
            this.categoria = cat;
            this.precio = prec;
            this.stock = st;
        }

        public string Titulo { get => titulo;}
        public string Autor { get => autor; }
        public string Editorial { get => editorial; }
        public string Isbn { get => isbn; }
        public string Edicion { get => edicion; }
        public string Anio { get => anio; }
        public string Paginas { get => paginas; }
        public string Categoria { get => categoria; }
        public string Precio { get => precio; }
        public string Stock { get => stock; }
    }

    
}
