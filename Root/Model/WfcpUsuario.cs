//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Root.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class WfcpUsuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WfcpUsuario()
        {
            this.WfcpUsuarioInRol = new HashSet<WfcpUsuarioInRol>();
        }
    
        public int id { get; set; }
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public bool activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WfcpUsuarioInRol> WfcpUsuarioInRol { get; set; }
    }
}
