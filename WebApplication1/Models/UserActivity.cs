using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserActivity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ActivityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual IEnumerable<ApplicationUser> Users { get; set; }
        public virtual IEnumerable<Activity> Activities { get; set; }

    }
}