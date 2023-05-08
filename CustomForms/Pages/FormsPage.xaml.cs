using CustomForms.Models;
using CustomForms.Pages;
using System.Collections.ObjectModel;

namespace CustomForms;

public partial class FormsPage : ContentPage
{
	public ObservableCollection<Form> Forms;
    CollectionView myColv;
    CustomFormDB database;
    async void Load(object sender = null)
    {
        /*string path = Path.Combine(FileSystem.Current.AppDataDirectory, "forms");
        string[] fileEntries = Directory.GetFiles(path);
        Forms = new ObservableCollection<Form>();
        foreach (string fileName in fileEntries)
        {
            Forms.Add(new Form(new List<MyControl>(), fileName.Substring(path.Count() + 1).Replace(".txt", "")));
        }*/
        Forms = new ObservableCollection<Form>();
        List<CustomForm> cforms = await database.GetBaseForms();
        foreach (CustomForm cform in cforms)
        {
            Forms.Add(new Form(new List<MyControl>(), cform.BaseName));
        }
        myColv.SetBinding(ItemsView.ItemsSourceProperty, "Forms");
        myColv.ItemsSource = Forms;
    }
	public FormsPage()
    {
        InitializeComponent();
        database = new CustomFormDB();
        Forms = new ObservableCollection<Form>();
        MessagingCenter.Subscribe<FormPage>(this, "reload", (sender) => { Load(sender); });
        MessagingCenter.Subscribe<AddFormPage>(this, "addedform", (sender) => { Load(sender); });
        /*if (!Directory.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, "forms")))
            Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "forms"));
        string path = Path.Combine(FileSystem.Current.AppDataDirectory, "forms");
        string[] fileEntries = Directory.GetFiles(path);
        Forms = new ObservableCollection<Form>();
        foreach (string fileName in fileEntries)
        {
            Forms.Add(new Form(new List<MyControl>(), fileName.Substring(path.Count()+1).Replace(".txt","")));
        }*/
        Load();
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
            frame.BackgroundColor = Color.FromHex("#512BD4");
            Label nameLbl = new Label();
            nameLbl.SetBinding(Label.TextProperty, "name");
            nameLbl.TextColor = Color.FromHex("#DFD8F7");
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s,e) => {
                CustomForm form = new CustomForm();
                form.BaseName = ((Label)((Frame)s).Content).Text;
                form.Name = "";
                CustomForm cform = await database.GetForm(form);
                List<MyControl> controls = await database.GetControls(cform.ID);
                await Navigation.PushAsync(new FormPage(controls, cform.ID));
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);
            Label countLbl = new Label();
            frame.Content = nameLbl;
            colvVertSL.Add(frame);
            return colvVertSL;
        });
        vertSL.Add(myColv);
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
    private async void testgotoform(object sender, EventArgs e)
    {
        CustomForm form = new CustomForm();
        form.Name = ((Label)((Frame)sender).Content).Text;
        CustomForm cform = await database.GetForm(form);
        List<MyControl> controls = await database.GetControls(cform.ID);
        await Navigation.PushAsync(new FormPage(controls, cform.ID));
        //await Navigation.PushAsync(new FormPage("MyFirstForm.txt"));
    }
}