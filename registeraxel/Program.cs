using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace TheRegister
{
    public partial class LoginFrm : Form
    {
        UserFrm frmUser = new UserFrm();
        static Dictionary<String, String> userPasswdDict = new Dictionary<string, string>();
        static Dictionary<String, Animal> userAnimalDict = new Dictionary<string, Animal>();

        public LoginFrm()
        {
            InitializeComponent();
            Setup();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string errorMessage = Login(editUsrname.Text, editPasswd.Text);
            lblErrorMessage.Text = errorMessage;

            if (errorMessage == "")
            {
                this.Hide();
                frmUser.Text = editUsrname.Text;
                if (frmUser.ShowDialog() == DialogResult.OK)
                {
                    lblErrorMessage.Text = "";
                    editPasswd.Text = "";
                    this.Show();
                }
            }
        }

        private string Login(string user, string passwd)
        {
            string errorMessage = "";
            string registeredPasswd = "";


            bool userExist = userPasswdDict.TryGetValue(user, out registeredPasswd);
            if (userExist)
            {

                if (passwd.CompareTo(registeredPasswd) == 0)
                {
                    Animal registredAnimal;
                    bool animalExist = userAnimalDict.TryGetValue(user, out registredAnimal);
                    if (animalExist)
                    {
                        frmUser.initializeAnimal(registredAnimal);
                        Console.WriteLine("This is your animal:{0}", registredAnimal.Show());
                    }
                    else
                    {
                        frmUser.initializeAnimal(null);
                    }
                }
                else
                {
                    errorMessage = "Invalid username or password!";
                }
            }
            else
            {
                errorMessage = string.Format("The user {0} is not found", user);
            }
            return errorMessage;
        }


        private static void Setup()
        {
            String path = @"C:\Temp\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            String urFilename = path + "user_register.txt";
            if (!File.Exists(urFilename))
            {
                FileStream f = File.Create(urFilename);
                f.Close();
                userPasswdDict.Add("Ackemo", "1337");
                userPasswdDict.Add("Olle", "1234");
                userPasswdDict.Add("Kajsa", "2341");

                string jsonuserPasswd = JsonConvert.SerializeObject(userPasswdDict, Formatting.Indented);
                File.WriteAllText(urFilename, jsonuserPasswd);
            }
            else
            {
                string json = File.ReadAllText(urFilename);
                userPasswdDict = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);
            }

            String arFilename = path + "animal_register.txt";
            if (!File.Exists(arFilename))
            {
                FileStream f = File.Create(arFilename);
                f.Close();
                userAnimalDict.Add("Ackemo", new Animal("Horse", "Humpe", "RawrxD", false));
                userAnimalDict.Add("Olle", new Animal("Dragon", "Heindrich", "Arf", true));

                string jsonUserAnimal = JsonConvert.SerializeObject(userAnimalDict, Formatting.Indented);
                File.WriteAllText(arFilename, jsonUserAnimal);
            }
            else
            {
                string json = File.ReadAllText(arFilename);
                userAnimalDict = JsonConvert.DeserializeObject<Dictionary<String, Animal>>(json);
            }

        }

        private void LoginFrm_Load(object sender, EventArgs e)
        {

        }
    }
}