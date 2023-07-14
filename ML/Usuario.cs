using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        [Display(Name = "Usuario asegurado: ")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Nombre de usuario: ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se aceptan letras")]
        [Display(Name = "Nombre: ")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Solo se aceptan letras")]
        [Display(Name = "Apellido paterno: ")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Solo se aceptan letras")]
        [Display(Name = "Apellido materno: ")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [EmailAddressAttribute(ErrorMessage = "Dirección no válida")]
        [Display(Name = "E-Mail: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(18)]
        [Display(Name = "Contraseña: ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(1)]
        [RegularExpression("^[HM]$", ErrorMessage = "Solo se admiten las letras H y M mayusculas")]
        [Display(Name = "Sexo (H)Hombre (M)Mujer: ")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [PhoneAttribute(ErrorMessage = "Teléfono no válido")]
        [StringLength(10)]
        [Display(Name = "Número de teléfono: ")]
        public string Telefono { get; set; }

        [PhoneAttribute(ErrorMessage = "Celular no válido")]
        [StringLength(15)]
        [Display(Name = "Número de celular: ")]
        public string Celular { get; set; }

        [Display(Name = "Fecha de nacimiento: ")]
        public string FechaNacimiento { get; set; }

        [Display(Name = "CURP: ")]
        public string CURP { get; set; }

        public string Imagen { get; set; }
        public bool Status { get; set; }

        public ML.Rol Rol { get; set; } //Propiedad de navegacion para acceder a las FK

        public ML.Direccion Direccion { get; set; }
        public ML.Aseguradora Aseguradora { get; set; }

        public List<object> Usuarios { get; set; }

    }
}
