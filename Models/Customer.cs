using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MDK._01._01_CourseProject.Models
{
    public class Customer
    {
        public int CustomerID;
        public string FullName;
        public string PassportData;
        public string Address;
        public DateTime? BirthDate;
        public bool? Gender;
        public string ContactDetails;
    }
}
