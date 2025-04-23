using CommunityToolkit.Maui.Views;

namespace TeslaAppMovilFinal2._0.Popups;

public partial class ImagePopup : Popup
{
    public ImagePopup(string imageUrl)
    {
        InitializeComponent();
        PopupImage.Source = imageUrl;
    }
}
