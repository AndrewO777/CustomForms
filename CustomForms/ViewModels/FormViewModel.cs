using CustomForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomForms.ViewModels;
public class FormViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    /*Form myForm;
    public Form MyForm { get => myForm; 
        set
        {
            if (myForm != value)
            {
                myForm = value;
                OnPropertyChanged(nameof(MyForm));
            }
        } 
    }*/
    string name;
    public string Name { get => name; 
        set
        { 
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        } 
    }
}
