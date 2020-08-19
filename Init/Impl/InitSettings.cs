using BaseTypes;
using Init.Interfaces;

namespace Init.Impl
{
    public class InitSettings : BaseController, IInitSettings
    {
        private string _token;

        private string Token
        {
            get => _token;
            set => _token = value;
        }

        public InitSettings()
        {
            //Test 1141495150:AAHBTDop4zGQWZ6S6-6k6Zb5lPulfE5c8QA
            //Prod 1013415129:AAHhl4vTbVwjh89BM-xAkVZV6UOxIRvPMNU
            //Release 1245406455:AAGVuJZkdgNVNWdn1YrZ4Yx42Xrdsxubo0E
            Token = "1141495150:AAHBTDop4zGQWZ6S6-6k6Zb5lPulfE5c8QA";//Test
        }

        public string GetToken() => Token;
    }
}
