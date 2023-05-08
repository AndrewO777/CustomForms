using CustomForms.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomForms;

public partial class AddFormPage : ContentPage
{
	CustomFormDB database;
	public AddFormPage(CustomFormDB customFormDB)
	{
		InitializeComponent();
		database = customFormDB;
	}
	private void AddLabelClicked(object sender, EventArgs e)
	{
		addLabelFrame.IsVisible= !addLabelFrame.IsVisible;
	}
	private void AddEntryClicked(object sender, EventArgs e)
	{
		Entry newEntry = new Entry();
		newEntry.BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"];
        newEntry.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
        newEntry.HorizontalOptions= LayoutOptions.Center;
		newEntry.WidthRequest = previewSL.Width - 20;
		newEntry.Margin = new Thickness(10,5);
		previewSL.Add(newEntry);
	}
	private void FontSizeChanged(object sender, EventArgs e)
	{
		fontSizeSlider.Value = Math.Round(fontSizeSlider.Value);
		fontSizeLbl.Text = fontSizeSlider.Value.ToString();
	}

    private void AddLabelSubmitClicked(object sender, EventArgs e)
	{
		Label newLabel = new Label();
        bool flag = false;
        if (labelTextEntry.Text != null)
        {
            if (labelTextEntry.Text.Count() > 0)
            {
                for (int i = 0; i < labelTextEntry.Text.Count(); ++i)
                {
                    if (labelTextEntry.Text[i] != ' ')
                    {
                        flag = true;
                    }
                }
            }
        }
        if (flag)
		{
			addLabelErrorLbl.IsVisible = false;
			newLabel.Text = labelTextEntry.Text;
			newLabel.Margin = new Thickness(10,5);
            newLabel.HorizontalOptions = LayoutOptions.Center;
			newLabel.FontSize = fontSizeSlider.Value;
			newLabel.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
			previewSL.Add(newLabel);
            addLabelFrame.IsVisible = false;
        }
		else
		{
			addLabelErrorLbl.IsVisible= true;
		}
	}
	private void AddTextboxClicked(object sender, EventArgs e)
	{
		Editor newEditor = new Editor();
		newEditor.WidthRequest = previewSL.Width - 20;
		newEditor.Margin = new Thickness(10,5);
		newEditor.MinimumHeightRequest = 100;
		newEditor.AutoSize = EditorAutoSizeOption.TextChanges;
		newEditor.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
        newEditor.BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"];
        previewSL.Add(newEditor);
	}
	private void SaveFormClicked(object sender, EventArgs e)
	{
		addFormFrame.IsVisible = !addFormFrame.IsVisible;
	}
	private async void SaveFormPopoverClicked(object sender, EventArgs e)
	{
        bool flag = false;
        if (formNameEntry.Text != null)
        {
            if (formNameEntry.Text.Count() > 0)
            {
                for (int i = 0; i < formNameEntry.Text.Count(); ++i)
                {
                    if (formNameEntry.Text[i] != ' ')
                    {
                        flag = true;
                    }
                }
            }
        }
        if (flag) {
			List<MyControl> list = new List<MyControl>();
			for (int i = 0; i < previewSL.Count; ++i)
			{
				if (previewSL.Children[i].GetType() == typeof(Label))
				{
					list.Add(new MyControl("Label", ((Label)previewSL.Children[i]).Text, ((Label)previewSL.Children[i]).FontSize));
				}
				else if (previewSL.Children[i].GetType() == typeof(Entry))
				{
					list.Add(new MyControl("Entry", ((Entry)previewSL.Children[i]).Text));
				}
				else if (previewSL.Children[i].GetType() == typeof(Editor))
				{
					list.Add(new MyControl("Editor", ((Editor)previewSL.Children[i]).Text));
				}
			}
			Form newForm = new Form(list, formNameEntry.Text);
			addFormFrame.IsVisible = false;
			//Add code to upload to database here
			CustomForm form = new CustomForm();
			form.BaseName = newForm.name;
			form.Name = "";
			await database.SaveForm(form);
			CustomForm cform = await database.GetForm(form);
			int index = 0;
			foreach (MyControl item in newForm.list)
			{
				item.FormID = cform.ID;
				item.ID = index;
				++index;
				await database.SaveControl(item);
			}
			//Old Text File Code
			/*string path = FileSystem.Current.AppDataDirectory;
			if (!Directory.Exists(Path.Combine(path, "forms")))
				Directory.CreateDirectory(Path.Combine(path, "forms"));
			string fullPath = Path.Combine(path, "forms", newForm.name + ".txt");
			string formListText = "";
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i].type == "Label")
				{
					formListText += list[i].type;
					formListText += list[i].value;
					formListText += ",num,";
					formListText += list[i].size;
				}
				else
				{
					formListText += list[i].type;
				}
			}
			for (int i = 0; File.Exists(fullPath); ++i)
			{
				fullPath = Path.Combine(path, newForm.name + i + ".txt");
			}
			File.WriteAllText(fullPath, formListText);*/
			MessagingCenter.Send<AddFormPage>(this, "addedform");
			await Navigation.PopAsync();
		}
		else
		{
			saveFormLblerr.IsVisible = true;
		}
	}
	private void RemoveClicked(object sender, EventArgs e)
	{
		if (previewSL.Children.Count() > 0)
		{
			previewSL.Children.RemoveAt(previewSL.Children.Count() - 1);
		}
	}
}