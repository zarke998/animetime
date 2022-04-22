using AnimeTime.WPF.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeTime.WPF.Views.Controls.VideoPlayerControls
{
    /// <summary>
    /// Interaction logic for FullscreenButton.xaml
    /// </summary>
    public partial class FullscreenButton : Button
    {
        private bool _isFullscreen;

        #region Depdendency Properties
        public bool IsFullscreen
        {
            get { return (bool)GetValue(IsFullscreenProperty); }
            set { SetValue(IsFullscreenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFullscreen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFullscreenProperty =
            DependencyProperty.Register("IsFullscreen", typeof(bool), typeof(FullscreenButton), new PropertyMetadata(false));



        public ICommand FullscreenToggleCommand
        {
            get { return (ICommand)GetValue(FullscreenToggleCommandProperty); }
            set { SetValue(FullscreenToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FullscreenToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FullscreenToggleCommandProperty =
            DependencyProperty.Register("FullscreenToggleCommand", typeof(ICommand), typeof(FullscreenButton), new PropertyMetadata(null));


        #endregion
        public FullscreenButton()
        {
            InitializeComponent();
        }

        protected override void OnClick()
        {
            base.OnClick();

            _isFullscreen = !IsFullscreen;

            this.SetCurrentValue(IsFullscreenProperty, _isFullscreen);
            FullscreenToggleCommand.TryExecute(IsFullscreen);
        }
    }
}
