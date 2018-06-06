using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample_Voucher_Entry.Models
{
    public class Transaction
    {
        public string AccountName { get; set; }

        public float Debit { get; set; }

        public float Credit { get; set; }
    }
}