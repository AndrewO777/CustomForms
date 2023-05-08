
using CustomForms.Models;
using Microsoft.Maui.Controls.Compatibility;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CustomForms.Pages;

public partial class GalleryPage : ContentPage
{
    ObservableCollection<Form> Forms;
    CustomFormDB database;
    CollectionView myColv;
    async void Load(object sender = null)
    {

        /*if (!Directory.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents")))
            Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents"));
        string path = Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents");
        string[] fileEntries = Directory.GetFiles(path);
        Forms = new ObservableCollection<Form>();
        foreach (string contentName in fileEntries)
        {
            string file = File.ReadAllText(contentName);
            string formname = "";
            for (int i = 0; i < file.Count() - 11; ++i)
            {
                if (file.Substring(i, 12) == ",.?endtag?.,")
                {
                    formname = file.Substring(0, i);
                    break;
                }
            }
            Forms.Add(new Form(new List<MyControl>(), formname.Replace(".txt", ""), contentName.Substring(path.Count() + 1).Replace(".txt", "")));
        }*/
        Forms = new ObservableCollection<Form>();
        List<CustomForm> cforms = await database.GetForms();
        foreach (CustomForm cform in cforms)
        {
            Forms.Add(new Form(new List<MyControl>(), cform.BaseName, cform.Name));
        }
        myColv.SetBinding(ItemsView.ItemsSourceProperty, "Forms");
        myColv.ItemsSource = Forms;
    }
    private void SearchChanged(object sender, EventArgs e)
    {
        ObservableCollection<Form> forms = new ObservableCollection<Form>();
        for (int i = 0; i < Forms.Count(); ++i)
        {
            if (Forms[i].name.ToLower().Contains(searchEntry.Text.ToLower()) || Forms[i].content.ToLower().Contains(searchEntry.Text.ToLower()))
            {
                forms.Add(new Form(new List<MyControl>(), Forms[i].name, Forms[i].content));
            }
        }
        myColv.ItemsSource = forms;
    }
	public GalleryPage()
	{
		InitializeComponent();

        database = new CustomFormDB();
        Forms = new ObservableCollection<Form>();
        MessagingCenter.Subscribe<FormPage>(this, "addedcontent", (sender) => { Load(sender); });
        Load();
        /*if (!Directory.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents")))
            Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents"));
        string path = Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents");
        string[] fileEntries = Directory.GetFiles(path);
        Forms = new ObservableCollection<Form>();
        foreach(string contentName in fileEntries)
        {
            Console.WriteLine((contentName.Substring(path.Count() + 1)));
            string file = File.ReadAllText(contentName);
            string formname = "";
            for (int i = 0; i < file.Count() - 11; ++i)
            {
                if (file.Substring(i, 12) == ",.?endtag?.,")
                {
                    formname = file.Substring(0,i);
                    break;
                }
            }
            Forms.Add(new Form(new List<MyControl>(), formname.Replace(".txt",""), contentName.Substring(path.Count() + 1).Replace(".txt", "")));
        }*/

        /*foreach (string fileName in fileEntries)
        {
            Forms.Add(new Form(new List<MyControl>(), fileName, fileName.Substring(path.Count() + 1).Replace(".txt", "")));
        }*/
        myColv = new CollectionView();
        myColv.SetBinding(ItemsView.ItemsSourceProperty, "Forms");
        myColv.ItemsSource = Forms;
        myColv.ItemTemplate = new DataTemplate(() =>
        {
            VerticalStackLayout colvVertSL = new VerticalStackLayout();
            HorizontalStackLayout colvHorzSL = new HorizontalStackLayout();
            Frame frame = new Frame();
            frame.BorderColor = Colors.Gray;
            frame.CornerRadius = 10;
            frame.Margin = 10;
            Label nameLbl = new Label();
            nameLbl.SetBinding(Label.TextProperty, "name");
            nameLbl.TextColor = Color.FromHex("#DFD8F7");
            Label contentLbl = new Label();
            contentLbl.SetBinding(Label.TextProperty, "content");
            contentLbl.Margin = new Thickness(10,0,0,0);
            contentLbl.TextColor = Color.FromHex("#DFD8F7");
            colvHorzSL.Children.Add(nameLbl);
            colvHorzSL.Children.Add(contentLbl);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                CustomForm form = new CustomForm();
                form.BaseName = ((Label)((HorizontalStackLayout)((Frame)s).Content).Children[0]).Text;
                form.Name = ((Label)((HorizontalStackLayout)((Frame)s).Content).Children[1]).Text;
                CustomForm cform = await database.GetForm(form);
                List<MyControl> controls = await database.GetControls(cform.ID);
                await Navigation.PushAsync(new FormPage(controls, cform.ID));
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);
            frame.BackgroundColor = Color.FromHex("#512BD4");
            Label countLbl = new Label();
            frame.Content = colvHorzSL;
            colvVertSL.Add(frame);
            return colvVertSL;
        });
        vertSL.Add(myColv);
    }
}