
using CustomForms.Models;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomForms.Pages;

public partial class FormPage : ContentPage
{
	string mypath;
	string Name;
	CustomFormDB database;
	CustomForm me;
	string myContent;
	int ID;
	List<string> elements;
	public async void Init()
	{
		me = await database.GetForm(ID);
	}
	public FormPage(List<MyControl> controls, int id)//string name, string content = "")
	{
		InitializeComponent();
		database = new CustomFormDB();
		ID = id;
		Init();
		foreach (MyControl control in controls)
		{
			if (control.type == "Label")
				vertSL.Add(new Label
				{
					FontSize = control.size,
					Text = control.value,
                    Margin = new Thickness(10, 5),
					HorizontalOptions = LayoutOptions.Center,
					TextColor = (Color)Application.Current.Resources["PrimaryColor"]
				});
			else if (control.type == "Entry")
				vertSL.Add(new Entry
				{
					Text = control.value,
                    BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"],
				    TextColor = (Color)Application.Current.Resources["PrimaryColor"],
				    HorizontalOptions = LayoutOptions.Fill,
				    MinimumHeightRequest = 20,
				    Margin = new Thickness(10, 5),
				});
			else if (control.type == "Editor")
				vertSL.Add(new Editor
				{
					Text = control.value,
                    HorizontalOptions = LayoutOptions.Fill,
					Margin = new Thickness(10, 5),
					MinimumHeightRequest = 100,
					AutoSize = EditorAutoSizeOption.TextChanges,
					TextColor = (Color)Application.Current.Resources["PrimaryColor"],
					BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"]
				});
		}
		/*Name = name;
		myContent = content;
        mypath = Path.Combine(FileSystem.Current.AppDataDirectory, "forms", name);
		string formdata = File.ReadAllText(mypath);
		List<string> contentdata = new List<string>();
		if (content != "")
		{
			deleteBtn.Text = "Delete Content File";
			string mycontent = File.ReadAllText(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents", content));
			int j = 0;
			mycontent = mycontent.Replace(name + ",.?endtag?.,", "");
            for (int i = 0; i < mycontent.Count()-11;++i)
			{
				Console.WriteLine(mycontent.Substring(i, 12));
				if (mycontent.Substring(i, 12) == ",.?endtag?.,")
				{
					contentdata.Add(mycontent.Substring(j, i-j));
					j = i+12;
				}
			}
		}
		elements = new List<string>();
		int iter = 0;
		//LabelEditorEntry
		for (int i = 0; i < formdata.Count();)
		{
			if (i < formdata.Count() - 5)
			{
				if (formdata[i] == 'E' && formdata[i+1] == 'd')
				{
					//Editor here
					elements.Add("Editor");
                    Editor newEditor = new Editor();
					newEditor.HorizontalOptions = LayoutOptions.Fill;
					if (content != "")
					{
						newEditor.Text = contentdata[iter];
						++iter;
					}
                    newEditor.Margin = new Thickness(10, 5);
                    newEditor.MinimumHeightRequest = 100;
                    newEditor.AutoSize = EditorAutoSizeOption.TextChanges;
                    newEditor.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
                    newEditor.BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"];
					vertSL.Add(newEditor);
                    i += 6;
				}
			}
			if (i < formdata.Count() - 4)
			{
				if (formdata[i] == 'L')
				{
					//label
					elements.Add("Label");
					Label myLbl = new Label();
					for (int j = i + 5; j < formdata.Count(); ++j)
					{
						if (formdata.Substring(j,5) == ",num,")
						{
							string lbltxt = formdata.Substring(i+5,j-(i+5));
							myLbl.Text = lbltxt;
							j += 5;
							int x = j;
							for (; formdata[x] != 'E' && formdata[x] != 'L' && x < formdata.Count() - 1; ++x)
							{}
							if (x == formdata.Count() - 1)
								++x;
							string sizestr = formdata.Substring(j, x-j);
							double.TryParse(sizestr, out double size);
							myLbl.FontSize = size;
                            myLbl.Margin = new Thickness(10, 5);
                            myLbl.HorizontalOptions = LayoutOptions.Center;
                            myLbl.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
							vertSL.Add(myLbl);
							i = x;
							j = formdata.Count();
                        }
					}
				}
				else if (formdata[i] == 'E' && formdata[i + 1] == 'n')
				{
					//entry
					elements.Add("Entry");
                    Entry newEntry = new Entry();
					if (content != "")
					{
						newEntry.Text = contentdata[iter];
						++iter;
					}
                    newEntry.BackgroundColor = (Color)Application.Current.Resources["SecondaryColor"];
                    newEntry.TextColor = (Color)Application.Current.Resources["PrimaryColor"];
                    newEntry.HorizontalOptions = LayoutOptions.Fill;
					newEntry.MinimumHeightRequest = 20;
                    newEntry.Margin = new Thickness(10, 5);
					vertSL.Add(newEntry);
                    i += 5;
				}
			}
		}*/
        /*string path = Path.Combine(FileSystem.Current.AppDataDirectory,"forms");
		string[] fileEntries = Directory.GetFiles(path);
		foreach (string fileName in fileEntries)
		{
			Console.WriteLine(fileName);
			Console.WriteLine(File.ReadAllText(fileName));
		}*/
    }
	private async void DeleteClicked(object sender, EventArgs e)
	{
		CustomForm form = await database.GetForm(ID);
		List<MyControl> controls = await database.GetControls(ID);
		foreach (MyControl control in controls)
		{
			await database.DeleteControl(control);
		}
		await database.DeleteForm(form);
        MessagingCenter.Send<FormPage>(this, "reload");
        MessagingCenter.Send<FormPage>(this, "addedcontent");

        await Navigation.PopAsync();

        //if (myContent == "")
        //{
        /*if (!Directory.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents")))
            Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents"));
        string path = Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents");
        string[] fileEntries = Directory.GetFiles(path);
        bool flag = false;
        foreach (string contentName in fileEntries)
        {
            string file = File.ReadAllText(contentName);
            string formname = "";
            for (int i = 0; i < file.Count() - 11; ++i)
            {
                if (file.Substring(i, 12) == ",.?endtag?.,")
                {
                    formname = file.Substring(0, i);
                    if (formname == Name)
                    {
                        flag = true;
                        break;
                    }
                }
            }
        }
        if (!flag)
        {
            File.Delete(mypath);
            MessagingCenter.Send<FormPage>(this, "reload");

            await Navigation.PopAsync();
        }
        else
        {
            cantdeleteframe.IsVisible = true;
        }*/
        //}
        /*else
		{
			File.Delete(Path.Combine(FileSystem.Current.AppDataDirectory, "forms", "formcontents", myContent));
			MessagingCenter.Send<FormPage>(this, "addedcontent");

            await Navigation.PopAsync();
        }*/
    }
	private async void CantDeletePopover(object sender,EventArgs e)
	{
		cantdeleteframe.IsVisible = !cantdeleteframe.IsVisible;
	}
	private async void SaveClicked(object sender, EventArgs e)
	{
		if (me.Name != "")
		{
			List<MyControl> controls = await database.GetControls(me.ID);
			controls = controls.OrderBy(o=>o.ID).ToList();
            for (int i = 0; i < vertSL.Count -2; ++i)
            {
                if (vertSL.Children[i+2].GetType() == typeof(Label))
                {
					controls[i].value=((Label)vertSL.Children[i+2]).Text;
                }
                else if (vertSL.Children[i+2].GetType() == typeof(Entry))
                {
                    controls[i].value = ((Entry)vertSL.Children[i+2]).Text;
                }
                else if (vertSL.Children[i+2].GetType() == typeof(Editor))
                {
                    controls[i].value = ((Editor)vertSL.Children[i+2]).Text;
                }
            }
			foreach (MyControl control in controls)
			{
                await database.SaveControl(control);
			}
			await database.SaveForm(me);
            MessagingCenter.Send<FormPage>(this, "addedcontent");
        }
		else
		{
			me.ID = -1;
			me.Name = fileNameEntry.Text;
			await database.SaveForm(me);
			CustomForm form = await database.GetForm(new CustomForm { BaseName=me.BaseName,Name=me.Name });
            List<MyControl> controls = new List<MyControl>();
            foreach (object element in vertSL.Children)
            {
                if (element.GetType() == typeof(Label))
                {
                    controls.Add(new MyControl("Label", ((Label)element).Text, ((Label)element).FontSize));
                }
                else if (element.GetType() == typeof(Entry))
                {
                    controls.Add(new MyControl("Entry", ((Entry)element).Text));
                }
                else if (element.GetType() == typeof(Editor))
                {
                    controls.Add(new MyControl("Editor", ((Editor)element).Text));
                }
            }
            int index = 0;
            foreach (MyControl control in controls)
            {
                control.FormID = form.ID;
                control.ID = index;
                ++index;
                await database.SaveControl(control);
            }
            MessagingCenter.Send<FormPage>(this, "addedcontent");
            await Navigation.PopAsync();
        }

        /*bool flag = false;
		if (fileNameEntry.Text != null)
		{
			if (fileNameEntry.Text.Count() > 0)
			{
				for (int i = 0; i < fileNameEntry.Text.Count(); ++i)
				{
					if (fileNameEntry.Text[i] != ' ')
					{
						flag = true;
					}
				}
			}
		}
		if (flag || myContent != "") {
			string path = FileSystem.Current.AppDataDirectory;
			if (!Directory.Exists(Path.Combine(path, "forms", "formcontents")))
				Directory.CreateDirectory(Path.Combine(path, "forms", "formcontents"));
			string fullPath;
			if (myContent == "")
			{
				fullPath = Path.Combine(path, "forms", "formcontents", fileNameEntry.Text + ".txt");
				for (int i = 0; File.Exists(fullPath); ++i)
				{
					fullPath = Path.Combine(path, "forms", "formcontents", fileNameEntry.Text + i + ".txt");
				}
			}
			else
			{
				fullPath = Path.Combine(path, "forms", "formcontents", myContent);
				File.Delete(fullPath);
			}
			/*for (int i = 0; File.Exists(fullPath); ++i)
			{
				fullPath = Path.Combine(path, Name.Replace("0.txt", "") + i + ".txt");
			}*//*
			string formdata = Name + ",.?endtag?.,";
			for (int i = 2; i < vertSL.Children.Count; ++i)
			{
				if (elements[i - 2] == "Editor")
				{
					formdata += ((Editor)vertSL.Children[i]).Text + ",.?endtag?.,";
				} else if (elements[i - 2] == "Entry")
				{
					formdata += ((Entry)vertSL.Children[i]).Text + ",.?endtag?.,";
				}
			}
			File.WriteAllText(fullPath, formdata);
			MessagingCenter.Send<FormPage>(this, "addedcontent");
			if (myContent == "")
			{
				await Navigation.PopAsync();
			}
		}
		else
		{
			errorLbl.IsVisible = true;
		}*/
    }
	private void SavePopoverClicked(object sender, EventArgs e)
	{
		if (me.Name == "")
		{
			addcontentframe.IsVisible = !addcontentframe.IsVisible;
		}
		else
		{
			SaveClicked(sender, e);
		}
	}
}