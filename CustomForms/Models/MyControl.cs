using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace CustomForms.Models
{
    public class MyControl
    {
        [PrimaryKey, AutoIncrement]
        public int otherid { get; set; }
        public int FormID { get; set; }
        public int ID { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public double size { get; set; }
        public MyControl() { }
        public MyControl(string Type) 
        {
            type = Type;
        }
        public MyControl(string Type, string Value, double Size = 12)
        {
            type = Type; 
            value = Value; 
            size = Size;
        }
    }
}
