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
    
    public partial class WfcpDocumento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WfcpDocumento()
        {
            this.WfcpDocumento_Estatus = new HashSet<WfcpDocumento_Estatus>();
            this.WfcpNuevaPartida = new HashSet<WfcpNuevaPartida>();
            this.WfcpCambioPrecio = new HashSet<WfcpCambioPrecio>();
        }
    
        public int id { get; set; }
        public int WorkflowId { get; set; }
        public long documento { get; set; }
        public string descripcion { get; set; }
        public System.DateTime fecha { get; set; }
        public string usuario { get; set; }
        public System.DateTime fecha_actualizacion { get; set; }
        public bool ZFM_SA_CREA_SOL { get; set; }
        public Nullable<bool> ZFM_SA_ACT_PRECIO { get; set; }
        public Nullable<bool> ZFM_SA_NEW_POS { get; set; }
    
        public virtual WfcpWorkflow WfcpWorkflow { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WfcpDocumento_Estatus> WfcpDocumento_Estatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WfcpNuevaPartida> WfcpNuevaPartida { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WfcpCambioPrecio> WfcpCambioPrecio { get; set; }
    }
}