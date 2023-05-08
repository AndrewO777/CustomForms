using CustomForms.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomForms
{
    public class CustomFormDB
    {
        SQLiteAsyncConnection Database;
        public CustomFormDB()
        {

        }
        async Task Init()
        {
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<CustomForm>();
            var othertable = await Database.CreateTableAsync<MyControl>();
            //await Database.DropTableAsync<MyControl>();
            //await Database.DropTableAsync<CustomForm>();
        }
        public async Task<List<CustomForm>> GetForms()
        {
            await Init();
            return await Database.Table<CustomForm>().Where(i => i.Name != "").ToListAsync();
        }
        public async Task<List<CustomForm>> GetBaseForms()
        {
            await Init();
            return await Database.QueryAsync<CustomForm>("SELECT * FROM [CustomForm] WHERE [Name] = \"\"");
        }
        public async Task<CustomForm> GetForm(int id)
        {
            await Init();
            return await Database.Table<CustomForm>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
        public async Task<CustomForm> GetForm(CustomForm form)
        {
            await Init();
            return await Database.Table<CustomForm>().Where(i => i.BaseName == form.BaseName && i.Name == form.Name).FirstOrDefaultAsync();
        }
        public async Task<List<MyControl>> GetControls(int id)
        {
            await Init();
            return await Database.Table<MyControl>().Where(i => i.FormID == id).ToListAsync();
        }
        public async Task<int> SaveForm(CustomForm form)
        {
            await Init();
            if (form.ID != 0 && form.ID != -1)
                return await Database.UpdateAsync(form);
            else if (form.ID == -1)
            {
                CustomForm cform = new CustomForm();
                cform.BaseName = form.BaseName; 
                cform.Name = form.Name;
                return await Database.InsertAsync(cform);
            }
            else
                return await Database.InsertAsync(form);
        }
        public async Task<int> SaveControl(MyControl control)
        {
            await Init();
            if (control.otherid != 0)
                return await Database.UpdateAsync(control);
            else
                return await Database.InsertAsync(control);
        }
        public async Task<int> DeleteForm(CustomForm form)
        {
            await Init();
            return await Database.DeleteAsync(form);
        }
        public async Task<int> DeleteControl(MyControl control)
        {
            await Init();
            return await Database.DeleteAsync(control);
        }
    }
}
