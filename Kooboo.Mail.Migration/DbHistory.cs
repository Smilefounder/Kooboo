using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Kooboo.Mail
{
    public class DbHistory
    {
        [MaxLength(255)]
        public string Id { get; set; }
    }
}
