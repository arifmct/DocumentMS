//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCPapperLess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Version
    {
        public int Id { get; set; }
        public string filename { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public string VersionNumber { get; set; }
        public Nullable<int> documentId { get; set; }
    
        public virtual Document Document { get; set; }
    }
}
