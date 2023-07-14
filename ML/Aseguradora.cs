using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Aseguradora
    {
        public int IdAseguradora { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Nombre de la aseguradora: ")]
        public string Nombre { get; set; }

        [Display(Name = "Fecha de creación: ")]
        public string FechaCreacion { get; set; }

        public string FechaModificacion { get; set; }

        public List<object> Aseguradoras { get; set; }

        public bool Status { get; set; }

        public ML.Usuario? Usuario { get; set; }


    }
}
