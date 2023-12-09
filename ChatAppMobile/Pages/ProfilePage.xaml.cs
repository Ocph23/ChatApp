using ChatAppMobile.Validator;
using ChatAppMobile.ViewModels;
using OcphApiAuth.Client;
using Shared;
using System.Text.Json;

namespace ChatAppMobile.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel();
    }
}


public class ProfileViewModel : BaseViewModel
{

    ProfileValidator validator = new ProfileValidator(); 
    public ProfileViewModel()
    {
        ChangeAvatarCommand = new Command( async(x) =>await ChangeAvatarCommandAction(x));
        UpdateCommand = new Command(UpdateAction, UpdateValidate);
        _ = Load();
        this.PropertyChanged += (o, p) =>
        {
            if(p.PropertyName!= "UpdateCommand")
            {
                UpdateCommand.ChangeCanExecute();
            }
        };

    }

    private async void UpdateAction(object obj)
    {
        try
        {
            var accountService = ServiceHelper.GetService<IAccountService>();
            var model = new UserDTO() { Active = Active, Email = Email, Nama = Name, Telepon = Telepon, Id = userid, Photo = Photo };
            var result = await accountService.UpdateUser(model);

            Preferences.Set("user",JsonSerializer.Serialize(model));

            await AppHelper.ShowMessage("Success");
        }
        catch (Exception ex)
        {
           await AppHelper.ShellDisplayError(ex.Message);
        }
        

    }

    private bool UpdateValidate(object arg)
    {
        var validate = validator.Validate(this);
        return validate.IsValid;
    }

    private async Task Load()
    {
        try
        {
            UserDTO user;
            var userDTOString = Preferences.Get("user", null);
            if (string.IsNullOrEmpty(userDTOString))
            {
                var accountService = ServiceHelper.GetService<IAccountService>();
                user = await accountService.GetProfile();
                if (user == null)
                {
                    Preferences.Set("user", JsonSerializer.Serialize(user));
                }

            }
            else
            {
                user = JsonSerializer.Deserialize<UserDTO>(userDTOString);
            }

            if (user != null)
            {
                this.userid=user.Id;
                this.Name = user.Nama;
                this.Email = user.Email;
                this.Telepon = user.Telepon;
                this.Active = user.Active;
                this.Photo = user.Photo;
            }
        }
        catch (Exception)
        {
            throw;
        }

    }

    private Command updateCommand;

    public Command UpdateCommand
    {
        get { return updateCommand; }
        set { SetProperty(ref updateCommand , value); }
    }


    public Command ChangeAvatarCommand { get; private set; }

    private async Task ChangeAvatarCommandAction(object obj)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }


    private string name;
    private string userid;

    public string Name
    {
        get { return name; }
        set { SetProperty(ref name, value); }
    }



    private string telepon;

    public string Telepon
    {
        get { return telepon; }
        set { SetProperty(ref telepon, value); }
    }


    private string email;

    public string Email
    {
        get { return email; }
        set { SetProperty(ref email, value); }
    }



    private bool active;

    public bool Active
    {
        get { return active; }
        set { SetProperty(ref active , value); }
    }



    private string? photo;

    public string? Photo
    {
        get { return photo; }
        set { SetProperty(ref photo, value); }
    }
}