using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Direccion
    {
        [Display(Name = "Dirección: ")]
        public int IdDireccion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Calle: ")]
        public string Calle { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Número interior: ")]
        public string NumeroInterior { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Número exterior: ")]
        public string NumeroExterior { get; set; }
        public ML.Colonia Colonia { get; set; }
        public List<object> Direcciones { get; set; }

    }
}
