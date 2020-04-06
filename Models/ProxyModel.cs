using BaseTypes;

namespace Models
{
    public class ProxyModel : BaseController
    {
		private string _host;
		private int _port;

		public string Host
		{
			get => _host;
			set => SetValue(ref _host, value, nameof(Host));
		}

		public int Port
		{
			get => _port;
			set => SetValue(ref _port, value, nameof(_port));
		}


	}
}
