//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClaimReport.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClaimReult
    {
        public int id { get; set; }
        public int claimid { get; set; }
        public int coordinatorid { get; set; }
        public string description { get; set; }
        public Nullable<bool> status { get; set; }
    
        public virtual Claim Claim { get; set; }
        public virtual Coordinator Coordinator { get; set; }
    }
}