
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomForms.Models
{
    public class Form : View
    {
        public static readonly BindableProperty NameProp = BindableProperty.Create("name", typeof(string), typeof(string), "");
        public string name
        {
            get => (string)GetValue(NameProp);
            set => SetValue(NameProp, value);
        }
        public static readonly BindableProperty ContentProp = BindableProperty.Create("content", typeof(string), typeof(string), "");
        public string content
        {
            get => (string)GetValue(ContentProp);
            set => SetValue(ContentProp, value);
        }
        public List<MyControl> list;
        public Form(List<MyControl> myList, string Name, string Content = "")
        {
            list = myList;
            name = Name;
            content = Content;
        }
    }
}
