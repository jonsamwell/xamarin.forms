using System;
using Redbridge.Forms.Markup;
using Redbridge.Validation;
using Xamarin.Forms;

namespace Redbridge.Forms
{
	public abstract class NavigationControllerViewModel<TStartupPageModel> : NavigationControllerViewModel
		where TStartupPageModel : INavigationPageModel
	{
		public NavigationControllerViewModel(IViewModelFactory viewModelFactory)
		{
			if (viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
			CurrentPage = viewModelFactory.CreateModel<TStartupPageModel>();
		}
	}

    [View(typeof(NavigationControllerPage))]
	public abstract class NavigationControllerViewModel : ViewModel, INavigationControllerViewModel, IDisposable
	{
		private INavigationPageModel _currentPage;
		private Color _navigationBarColour;
		private Color _navigationBarTextColour;
		private ImageSource _navigationBarIcon;

		public NavigationControllerViewModel() : base() 
		{
			if (RedbridgeThemeManager.Current != null)
			{
				NavigationBarTextColour = RedbridgeThemeManager.Current.NavigationTextColour;
				NavigationBarColour = RedbridgeThemeManager.Current.NavigationBarColour;
			}
		}

		public NavigationControllerViewModel(INavigationPageModel rootPage) : this()
		{
            CurrentPage = rootPage ?? throw new ArgumentNullException(nameof(rootPage));
		}

		public Color NavigationBarColour
		{
			get { return _navigationBarColour; }
			set 
			{
				if (_navigationBarColour != value)
				{
					_navigationBarColour = value;
					OnPropertyChanged(nameof(NavigationBarColour));
				}
			}
		}

		public Color NavigationBarTextColour
		{
			get { return _navigationBarTextColour; }
			set
			{
				if (_navigationBarTextColour != value)
				{
					_navigationBarTextColour = value;
					OnPropertyChanged(nameof(NavigationBarTextColour));
				}
			}
		}

		public ImageSource NavigationBarIcon
		{
			get { return _navigationBarIcon; }
			set
			{
				if (_navigationBarIcon != value)
				{
					_navigationBarIcon = value;
					OnPropertyChanged(nameof(NavigationBarIcon));
				}
			}
		}

        // Called when the primary navigation controller is popped.
        public void OnNavigateBack () {}

		public INavigationPageModel CurrentPage
		{
			get { return _currentPage; }
			set 
			{
				if (_currentPage != value)
				{
                    _currentPage = value;

					if (_currentPage != null)
					{
						NavigationBarTextColour = _currentPage.NavigationBarTextColour;
						NavigationBarColour = _currentPage.NavigationBarColour;
					}

					OnPropertyChanged(nameof(CurrentPage));
				}
			}
		}

        public ValidationResultCollection Validate()
        {
            return OnValidate();
        }

        protected virtual ValidationResultCollection OnValidate()
        {
            return new ValidationResultCollection(true);
        }

        public void Dispose()
        {
            CurrentPage?.Dispose();
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
