using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ej1.Models
{
    //Clases POCO. Plain Old C# Object. Solo tienen campos y propiedades
    public class Libro
    {
        public string Titulo { get; set; } = "";
        public string Autor { get; set; } = "";
        public string Portada { get; set; } = "";
    }
}
