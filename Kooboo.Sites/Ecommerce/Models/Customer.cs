using Kooboo.Sites.Models;
using System;

namespace Kooboo.Sites.Ecommerce.Models
{
    public class Customer : CoreObject
    {
        public Customer()
        {
            this.ConstType = ConstObjectType.Customer;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long MembershipNumber { get; set; }

        public string Password { get; set; }

        private string _emailaddress;
        public string EmailAddress
        {
            get
            {
                return _emailaddress;
            }
            set
            {
                _emailaddress = value;
                EmailHash = default(Guid);
            }
        }

        private Guid _emailhash;

        public Guid EmailHash
        {
            get
            {
                if (_emailhash == default(Guid))
                {
                    if (!string.IsNullOrWhiteSpace(_emailaddress))
                    {
                        _emailhash = Lib.Security.Hash.ComputeGuidIgnoreCase(_emailaddress);
                    }
                }
                return _emailhash;
            }
            set
            {
                _emailhash = value;
            }
        }

        public string Telephone { get; set; }

        private Guid _telhash;

        public Guid TelHash 
        {
            get
            {
                if (_telhash == default)
                {
                    if (!string.IsNullOrWhiteSpace(Telephone))
                    {
                        _telhash = Lib.Security.Hash.ComputeGuidIgnoreCase(Telephone);
                    }
                }
                return _telhash;
            }
            set
            {
                _telhash = value;
            }
        }

        public Guid DefaultShippingAddress { get; set; }

        public Guid DefaultBankCard { get; set; }

        /// <summary>
        ///  this is a temp user. 
        /// </summary>
        public bool NoLogin { get; set; }

        public override int GetHashCode()
        {
            string unique = this.Name + this.FirstName + this.LastName + this.EmailAddress + this.Telephone;
            unique += this.DefaultBankCard.ToString() + this.DefaultShippingAddress.ToString();
            unique += this.Password;
            return Lib.Security.Hash.ComputeIntCaseSensitive(unique);
        }
    }
}
