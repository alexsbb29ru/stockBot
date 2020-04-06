using BaseTypes;
using Init.Interfaces;
using Models;

namespace Init.Impl
{
    public class InitSettings : BaseController, IInitSettings
    {
        private string _token;
        private readonly ProxyModel _proxyModel;

        private string Token
        {
            get => _token;
            set => SetValue(ref _token, value);
        }

        public InitSettings()
        {
            Token = "1013415129:AAHhl4vTbVwjh89BM-xAkVZV6UOxIRvPMNU";
            _proxyModel = new ProxyModel()
            {
                Host = "mssg.me.pp.ru",
                Port = 443
            };
        }

        public string GetToken() => Token;

        public ProxyModel GetProxyData() => _proxyModel;
    }
}
