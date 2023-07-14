﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Colonia
    {
        [Required(ErrorMessage = "Seleccione una opción")]
        [Display(Name = "Colonia: ")]
        public int IdColonia { get; set; }

        public string Nombre { get; set; }

        public string CodigoPostal { get; set; }

        public ML.Municipio Municipio { get; set; }

        public List<object> Colonias { get; set; }

    }
}
