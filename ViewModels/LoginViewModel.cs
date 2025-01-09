using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoginForm.Enums;
using LoginForm.Interfaces;
using LoginForm.Models;


namespace LoginForm.ViewModels
{
    public partial class LoginViewModel : ObservableObject, ILoginViewModel
    {
        public LoginViewModel()
        {
            var lastSelectedUserType = Preferences.Get("LastSelectedLoginType", "Default");

            StudentSingleSelectionButton = new()
            {
                Text = "Student",
                Enabled = (lastSelectedUserType == "Student") ? true : false,
                Type = UserType.Student,
            };

            LecturerSingleSelectionButton = new()
            {
                Text = "Lecturer",
                Enabled = (lastSelectedUserType == "Lecturer") ? true : false,
                Type = UserType.Lecturer,
            };

            LoginAsMessage = GetLoginAsMessage();

            if (lastSelectedUserType == "Default")
            {
                StudentSingleSelectionButton.Enabled = true;
                LoginAsMessage = "Login as Student";
            }

            Users = new()
            {
                new User()
                {
                    Email = "student@example.com",
                    Password = "123",
                    Type = UserType.Student,
                },
                new User()
                {
                    Email = "lecturer@example.com",
                    Password = "123",
                    Type = UserType.Lecturer,
                }
            };
        }

        [ObservableProperty]
        string
            loginAsMessage = string.Empty, 
            email = Preferences.Get("RememberedEmail", string.Empty), 
            password = Preferences.Get("RememberedPassword", string.Empty);

        [ObservableProperty]
        List<User> users;

        [ObservableProperty]
        SingleSelectionButton studentSingleSelectionButton;

        [ObservableProperty]
        SingleSelectionButton lecturerSingleSelectionButton;

        [ObservableProperty]
        bool rememberMeEnabled = Preferences.Get("RememberMeSelected", false);

        public string GetLoginAsMessage()
        {
            return "Login as " + Preferences.Get("LastSelectedLoginType", "Default");
        }

        [RelayCommand]
        public void SingleSelectionButtonClicked(SingleSelectionButton button)
        {
            button.Enabled = !button.Enabled;

            switch (button.Text)
            {
                case "Student":
                    LecturerSingleSelectionButton.Enabled = false;
                    break;
                case "Lecturer":
                    StudentSingleSelectionButton.Enabled = false;
                    break;
            }

            LoginAsMessage = "Login as " + button.Text;
        }

        [RelayCommand]
        public async Task Login()
        {
            string errorMessage = "Invalid data";
            string errorTitle = "Error";
            string cancel = "Cancel";

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert(errorTitle, errorMessage, cancel);
                return;
            };

            UserType selectedType = (StudentSingleSelectionButton.Enabled == true) ? StudentSingleSelectionButton.Type : LecturerSingleSelectionButton.Type;

            User? user = Users.FirstOrDefault(u => u.Email == Email && u.Password == Password && u.Type == selectedType);

            if (user == null)
            {
                await Shell.Current.DisplayAlert(errorTitle, errorMessage, cancel);
                return;
            }

            if (RememberMeEnabled == true)
            {
                Preferences.Set("RememberedEmail", Email);
                Preferences.Set("RememberedPassword", Password);
            }
            else
            {
                Preferences.Set("RememberedEmail", string.Empty);
                Preferences.Set("RememberedPassword", string.Empty);
            }

            Preferences.Set("RememberMeSelected", RememberMeEnabled);

            var a = user.Type.ToString();

            Preferences.Set("LastSelectedLoginType", user.Type.ToString());

            await Shell.Current.DisplayAlert("Succes", $"Welcome {user.Type}", "Proceed");
        }
    }
}