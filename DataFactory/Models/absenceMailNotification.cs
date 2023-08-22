using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Captureit.DataFactory.Models
{
    public class absenceAlert
    {
        public List<absenceMailNotification> absentStudentRecords { get; set; }
       
        public absenceAlert()
        {
            absentStudentRecords = new List<absenceMailNotification>();
        }
    }


    public class absenceMailNotification
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string RollNo { get; set; }
        public string StudentMail { get; set; }
        public string ParentMail { get; set; }
    }
}
