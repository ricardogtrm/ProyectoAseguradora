using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Aseguradora
    {
        public int IdAseguradora { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public bool? Status { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }

        //Propiedades adicionales
        public string NombreUsuario { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
    }
}
