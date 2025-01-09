using CommunityToolkit.Mvvm.ComponentModel;
using LoginForm.Enums;

namespace LoginForm.Models
{
    public partial class SingleSelectionButton : ObservableObject
    {
        public string Text { get; set; }

        [ObservableProperty]
        bool enabled = false;

        public UserType Type { get; set; }
    }
}
