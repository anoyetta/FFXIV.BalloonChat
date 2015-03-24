using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using FFXIV.BalloonChat.Config;
using FFXIV.PluginCore;
using FFXIV.PluginCore.Controls;

namespace FFXIV.BalloonChat.Pages
{
    public partial class SettingsThemePage : UserControl
    {
        private bool enabledPreview;

        public SettingsThemePage()
        {
            this.InitializeComponent();

            this.Loaded += this.SettingsThemePage_Loaded;
            this.ThemesListBox.SelectionChanged += this.ThemesListBox_SelectionChanged;

            this.FontButton.Click += this.FontButton_Click;

            this.FontColorTextBox.LostFocus += (s, e) => this.PreviewBalloon();
            this.FontOutlineColorTextBox.LostFocus += (s, e) => this.PreviewBalloon();
            this.FontShadowColorTextBox.LostFocus += (s, e) => this.PreviewBalloon();
            this.BorderColorTextBox.LostFocus += (s, e) => this.PreviewBalloon();
            this.BorderThicknessTextBox.LostFocus += (s, e) => this.PreviewBalloon();
            this.BackgoundColorTextBox.LostFocus += (s, e) => this.PreviewBalloon();

            this.ApplyButton.Click += this.ApplyButton_Click;
        }

        private void SettingsThemePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ThemesListBox.Items.Count > 0)
            {
                this.ThemesListBox.SelectedIndex = 0;
            }

            this.enabledPreview = true;
        }

        private void ThemesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var theme = this.ThemesListBox.SelectedItem as BalloonChatTheme;
            if (theme == null)
            {
                this.DetailsPanel.Visibility = Visibility.Hidden;
                return;
            }

            this.DetailsPanel.Visibility = Visibility.Visible;

            try
            {
                this.enabledPreview = false;

                this.ThemeNameTextBox.Text = theme.Name;

                this.FontButton.Content =
                    theme.Font.FamilyName + ", " +
                    theme.Font.Size.ToString("N1") + "pt, " +
                    theme.Font.StyleString + "-" + theme.Font.WeightString + "-" + theme.Font.StretchString;
                this.FontButton.Tag = theme.Font;

                this.FontColorTextBox.Text = theme.FontColor;
                this.FontOutlineColorTextBox.Text = theme.FontOutlineColor;
                this.FontShadowColorTextBox.Text = theme.FontShadowColor;
                this.BorderColorTextBox.Text = theme.BorderColor;
                this.BorderThicknessTextBox.Text = theme.BorderThickness.ToString();
                this.BackgoundColorTextBox.Text = theme.BackgroundColor;

                this.Balloon.Theme = theme;
            }
            finally
            {
                this.enabledPreview = true;
            }
        }

        private void AddThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var theme = this.ThemesListBox.SelectedItem as BalloonChatTheme;

            var newTheme = new BalloonChatTheme()
            {
                ID = Guid.NewGuid(),
                Name = "New Theme",
            };

            if (theme != null)
            {
                newTheme.FontColor = theme.FontColor;
                newTheme.FontOutlineColor = theme.FontOutlineColor;
                newTheme.FontShadowColor = theme.FontShadowColor;
                newTheme.BorderColor = theme.BorderColor;
                newTheme.BorderThickness = theme.BorderThickness;
                newTheme.BackgroundColor = theme.BackgroundColor;
                newTheme.Font = theme.Font;
            }

            BalloonChatConfig.Default.Themes.Add(newTheme);
            BalloonChatConfig.Default.Save();

            this.ThemesListBox.Items.Refresh();
            this.ThemesListBox.SelectedItem = newTheme;

            TraceUtility.WriteLog("Added new theme.");
        }

        private void DeleteThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = this.ThemesListBox.SelectedIndex;

            var theme = this.ThemesListBox.SelectedItem as BalloonChatTheme;
            if (theme == null)
            {
                return;
            }

            BalloonChatConfig.Default.Themes.Remove(theme);
            BalloonChatConfig.Default.Save();

            this.ThemesListBox.Items.Refresh();

            if (this.ThemesListBox.Items.Count > 0)
            {
                this.ThemesListBox.SelectedIndex =
                    selectedIndex != 0 ? selectedIndex - 1 : 0;
            }

            TraceUtility.WriteLog("Deleted theme. [" + theme.Name + "]");
        }

        private void FontButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FontDialog()
            {
                Font = this.FontButton.Tag as FontInfo
            };

            if (dialog.ShowDialog(Window.GetWindow(this)).Value)
            {
                var font = dialog.Font;

                this.FontButton.Content =
                    font.FamilyName + ", " +
                    font.Size.ToString("N1") + "pt, " +
                    font.StyleString + "-" + font.WeightString + "-" + font.StretchString;
                this.FontButton.Tag = font;

                this.PreviewBalloon();
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var theme = this.ThemesListBox.SelectedItem as BalloonChatTheme;
            if (theme == null)
            {
                return;
            }

            var colorValidator = new Func<TextBox, string>(textBox =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = Colors.Gainsboro.ToString();
                }

                return textBox.Text;
            });

            var thicknessValidator = new Func<TextBox, double>(textBox =>
            {
                double d;
                if (!double.TryParse(textBox.Text, out d))
                {
                    d = 2.4d;
                }

                return d;
            });

            theme.Name = this.ThemeNameTextBox.Text;
            theme.Font = this.FontButton.Tag as FontInfo;
            theme.FontColor = colorValidator(this.FontColorTextBox);
            theme.FontOutlineColor = colorValidator(FontOutlineColorTextBox);
            theme.FontShadowColor = colorValidator(this.FontShadowColorTextBox);
            theme.BorderColor = colorValidator(this.BorderColorTextBox);
            theme.BorderThickness = thicknessValidator(this.BorderThicknessTextBox);
            theme.BackgroundColor = colorValidator(this.BackgoundColorTextBox);

            BalloonChatConfig.Default.Save();

            TraceUtility.WriteLog("Updated theme. [" + theme.Name + "]");
        }

        private void PreviewBalloon()
        {
            if (!this.enabledPreview)
            {
                return;
            }

            var colorValidator = new Func<TextBox, string>(textBox =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = Colors.Gainsboro.ToString();
                }

                return textBox.Text;
            });

            var thicknessValidator = new Func<TextBox, double>(textBox =>
            {
                double d;
                if (!double.TryParse(textBox.Text, out d))
                {
                    d = 2.4d;
                }

                return d;
            });

            var theme = this.Balloon.Theme;

            theme.Font = this.FontButton.Tag as FontInfo;
            theme.FontColor = colorValidator(this.FontColorTextBox);
            theme.FontOutlineColor = colorValidator(FontOutlineColorTextBox);
            theme.FontShadowColor = colorValidator(this.FontShadowColorTextBox);
            theme.BorderColor = colorValidator(this.BorderColorTextBox);
            theme.BorderThickness = thicknessValidator(this.BorderThicknessTextBox);
            theme.BackgroundColor = colorValidator(this.BackgoundColorTextBox);

            this.Balloon.Theme = theme;
        }
    }
}
