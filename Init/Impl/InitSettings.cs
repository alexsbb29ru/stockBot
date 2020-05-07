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
            //Test 1141495150:AAHBTDop4zGQWZ6S6-6k6Zb5lPulfE5c8QA
            //Prod 1013415129:AAHhl4vTbVwjh89BM-xAkVZV6UOxIRvPMNU
            Token = "1141495150:AAHBTDop4zGQWZ6S6-6k6Zb5lPulfE5c8QA"; //Test
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
