//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Job.PricesWF.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class WfcpRol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WfcpRol()
        {
            this.WfcpDocumento_Estatus = new HashSet<WfcpDocumento_Estatus>();
            this.WfcpUsuarioInRol = new HashSet<WfcpUsuarioInRol>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WfcpDocumento_Estatus> WfcpDocumento_Estatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WfcpUsuarioInRol> WfcpUsuarioInRol { get; set; }
    }
}
