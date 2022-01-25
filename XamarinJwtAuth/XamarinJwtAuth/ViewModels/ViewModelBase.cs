using System;
using Xamarin.CommunityToolkit;
using Xamarin.CommunityToolkit.ObjectModel;

namespace XamarinJwtAuth.ViewModels
{
    /// <summary>
    /// This is a *newer* version of the Base View Model that uses the Xamarin Community Toolkit
    /// but James Montemagmo's Base View Model as the XCT did not include a standard Base View Model
    /// and the BaseViewModel of this project is customized to the default shell applciation.
    /// </summary>
    public class ViewModelBase : ObservableObject 
    {
        public ViewModelBase()
        {
        }

		string? title = string.Empty;

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string? Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}

		string? subtitle = string.Empty;

		/// <summary>
		/// Gets or sets the subtitle.
		/// </summary>
		/// <value>The subtitle.</value>
		public string? Subtitle
		{
			get => subtitle;
			set => SetProperty(ref subtitle, value);
		}

		string? icon = string.Empty;

		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// <value>The icon.</value>
		public string? Icon
		{
			get => icon;
			set => SetProperty(ref icon, value);
		}

		bool isBusy;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
		public bool IsBusy
		{
			get => isBusy;
			set
			{
				if (SetProperty(ref isBusy, value))
					IsNotBusy = !isBusy;
			}
		}

		bool isNotBusy = true;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is not busy.
		/// </summary>
		/// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
		public bool IsNotBusy
		{
			get => isNotBusy;
			set
			{
				if (SetProperty(ref isNotBusy, value))
					IsBusy = !isNotBusy;
			}
		}

		bool canLoadMore = true;

		/// <summary>
		/// Gets or sets a value indicating whether this instance can load more.
		/// </summary>
		/// <value><c>true</c> if this instance can load more; otherwise, <c>false</c>.</value>
		public bool CanLoadMore
		{
			get => canLoadMore;
			set => SetProperty(ref canLoadMore, value);
		}


		string? header = string.Empty;

		/// <summary>
		/// Gets or sets the header.
		/// </summary>
		/// <value>The header.</value>
		public string? Header
		{
			get => header;
			set => SetProperty(ref header, value);
		}

		string? footer = string.Empty;

		/// <summary>
		/// Gets or sets the footer.
		/// </summary>
		/// <value>The footer.</value>
		public string? Footer
		{
			get => footer;
			set => SetProperty(ref footer, value);
		}
	}
}
