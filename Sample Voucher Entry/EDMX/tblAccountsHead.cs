//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sample_Voucher_Entry.EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblAccountsHead
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAccountsHead()
        {
            this.tblVoucherDetail = new HashSet<tblVoucherDetail>();
        }
    
        public int Accounts_Id { get; set; }
        public string Accounts_Name { get; set; }
        public string Accounts_Code { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVoucherDetail> tblVoucherDetail { get; set; }
    }
}
