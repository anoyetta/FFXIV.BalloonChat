using System.Windows;
using System.Windows.Controls;

using FirstFloor.ModernUI.Windows.Controls;

namespace FFXIV.PluginCore.Controls
{
    public class FontDialog
    {
        private FontDialogContent content = new FontDialogContent();

        public FontInfo Font
        {
            get { return this.content.FontInfo; }
            set { this.content.FontInfo = value; }
        }

        public bool? ShowDialog(
            Window owner = null)
        {
            var dialog = new ModernDialog
            {
                Title = "Plase select a font.",
                Content = this.content,
                Owner = owner,
                MaxWidth = 1280,
                MaxHeight = 768,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };

            dialog.Buttons = new Button[] { dialog.OkButton, dialog.CancelButton };
            dialog.OkButton.Click += this.content.OKBUtton_Click;

            return dialog.ShowDialog();
        }
    }
}
