using System;
using System.Data;
using System.IO;
using SQLite;
using System.Collections.Generic;

namespace WebRequestTutorial.SQLite
{
    public class DBRepo
    {
        public string CreateDB()
        {
            try
            {
                var output = "";
                output += "Creating Database if it doesnt exist.";
                string dbPath = Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.Personal), "sqliteDB.db3");
                var db = new SQLiteConnection(dbPath);
                output += "\nDatabse Created...";
                return output;
            }
            catch (Exception e)
            {

                return "Error: " + e.Message;
            }
            
        }

        public string CreateTable()
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "sqliteDB.db3");
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<Contact>();
                string result = "Tabel was created";
                return result;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
        
        public string InsertContact (Contact contact)
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "sqliteDB.db3");
                var db = new SQLiteConnection(dbPath);

                Contact item = new Contact();
                item = contact;
                db.Insert(item);
                return "Contact Inserted";

            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        public string DeleteContact(int id)
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "sqliteDB.db3");
                var db = new SQLiteConnection(dbPath);

                var item = db.Get<Contact>(id);
                db.Delete(item);
                return "Contact Deleted";

            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        public List<Contact> GetAllContacts()
        {
            try
            {
                string dbPath = Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.Personal), "sqliteDB.db3");
                var db = new SQLiteConnection(dbPath);

                List<Contact> rContacts = new List<Contact>();

                var table = db.Table<Contact>();
                foreach (var item in table)
                {
                    rContacts.Add(item);
                }

                return rContacts;

            }
            catch
            {
                throw;
            }
        }

    }
}