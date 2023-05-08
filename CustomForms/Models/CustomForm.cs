using SQLite;

namespace CustomForms.Models
{
    public class CustomForm
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string BaseName { get; set; }
    }
}
