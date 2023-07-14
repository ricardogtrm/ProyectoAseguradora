using System;
using System.Collections.Generic;

namespace DL
{
    public partial class UsuarioEliminado
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apaterno { get; set; } = null!;
        public string Amaterno { get; set; } = null!;
        public DateTime FechaEliminacion { get; set; }
        public TimeSpan HoraEliminacion { get; set; }
    }
}
