using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Dev2.Common.Interfaces.Data;
using FontAwesome.WPF;

namespace Warewolf.Studio.Core
{
    public class ResourceTypeToIconConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            const string pathname = "/Warewolf.Studio.Themes.Luna;component/Images.xaml";
            ResourceDictionary dict = Application.LoadComponent(new Uri(pathname, System.UriKind.Relative)) as ResourceDictionary;           
            ResourceType resourceType;
            Brush brush = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
            if (value != null && Enum.TryParse(value.ToString(), out resourceType))
            {
                switch (resourceType)
                {
                    case ResourceType.WorkflowService:
                        return dict[CustomMenuIcons.WorkflowService] as DrawingImage;
                    case ResourceType.DbService:
                        return dict[CustomMenuIcons.DbService] as DrawingImage;
                    case ResourceType.PluginSource:
                        return dict[CustomMenuIcons.PluginSource] as DrawingImage;
                    case ResourceType.EmailSource:
                        return dict[CustomMenuIcons.EmailSource] as DrawingImage;
                    case ResourceType.WebService:
                        return dict[CustomMenuIcons.WebService] as DrawingImage;
                    case ResourceType.DbSource:
                        return dict[CustomMenuIcons.DbSource] as DrawingImage;
                    case ResourceType.PluginService:
                        return dict[CustomMenuIcons.PluginService] as DrawingImage;
                    case ResourceType.WebSource:
                        return dict[CustomMenuIcons.WebSource] as DrawingImage;
                    case ResourceType.SharepointServerSource:
                        return Application.Current.Resources["AddSharepointBlackLogo"];
                    case ResourceType.ServerSource:
                        return dict[CustomMenuIcons.ServerSource] as DrawingImage;
                    case ResourceType.Server:
                        return dict[CustomMenuIcons.Server] as DrawingImage;
                    case ResourceType.StartPage:
                        var imageSource = ImageAwesome.CreateImageSource(FontAwesomeIcon.Home, brush);
                        return imageSource;
                    case ResourceType.OauthSource:
                        return Application.Current.Resources["AddDropBoxBlackLogo"];
                    case ResourceType.Scheduler:
                        return ImageAwesome.CreateImageSource(FontAwesomeIcon.History, brush);
                    case ResourceType.Settings:
                        return ImageAwesome.CreateImageSource(FontAwesomeIcon.Cogs, brush);
                    case ResourceType.DependencyViewer:
                        return ImageAwesome.CreateImageSource(FontAwesomeIcon.Sitemap, brush);
                    case ResourceType.DeployViewer:
                        return ImageAwesome.CreateImageSource(FontAwesomeIcon.PaperPlane, brush);
                    default:
                        return dict[CustomMenuIcons.Folder] as DrawingImage;
                }
            }
            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}