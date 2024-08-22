namespace RDS.Web;

public static class ConfigurationWeb
{
    public const string HttpClientName = "RDS";
    
    public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3YjkyODdkNDkyYjcwOGU=";
    public static string Issuer = "www.rdsweb.mysoftwares.com.br";
    public static string Audience = "GeneralAudience";

    public const bool RenewToken = true;
    public const bool RenewTokenMessage = true;

    public static string StripePublicKey { get; set; } = "pk_test_qp8INYXhpnFxxdFYp2mh8T5r00qmy8cVdv";
    public static string BackendUrl { get; set; } = String.Empty;

    public static MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = ["Karla", "Poppins", "Roboto", "sans-serif"],
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = Colors.LightBlue.Darken2,
            Secondary = Colors.Teal.Darken2,
            Tertiary = Colors.Teal.Darken3,
            Background = Colors.Gray.Lighten1,
            AppbarBackground = Colors.LightBlue.Darken2,
            AppbarText = Colors.Shades.White,
            TextPrimary = Colors.Shades.Black,
            DrawerText = Colors.Shades.Black,
            DrawerBackground = Colors.Gray.Lighten1,
            PrimaryContrastText = Colors.Shades.White
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.Teal.Accent3,
            Secondary = Colors.LightGreen.Darken3,
            //Primary = Colors.BlueGray.Darken3,
            //Secondary = Colors.Cyan.Darken3,
            Tertiary = Colors.Cyan.Darken4,
            Background = Colors.Gray.Darken4,
            AppbarBackground = Colors.BlueGray.Darken4,
            AppbarText = Colors.Shades.White,
            TextPrimary = Colors.Shades.White,
            DrawerText = Colors.Shades.White,
            DrawerBackground = Colors.BlueGray.Darken3,
            PrimaryContrastText = Colors.Shades.White
        }

        //Primary = Colors.LightBlue.Accent3,
        //Secondary = Colors.LightBlue.Darken3,
        //Tertiary = Colors.LightBlue.Darken4,
        ////Background = Colors.Grey.Darken3,
        //AppbarBackground = Colors.LightBlue.Accent3,
        //AppbarText = Colors.Shades.Black,
        //PrimaryContrastText = new MudColor("#000000")

        //Detalhes das Cores
        //Primary: Colors.LightBlue.Darken2 - Um tom de azul claro escurecido para elementos principais, que é suave e profissional.
        //Secondary: Colors.Teal.Darken2 - Um tom escurecido de teal para elementos secundários, oferecendo um contraste elegante.
        //Tertiary: Colors.Teal.Darken3 - Um tom ainda mais escuro de teal para detalhes e elementos terciários.
        //Background: Colors.Grey.Lighten5 - Um cinza muito claro para o fundo, proporcionando um ambiente limpo e leve.
        //AppbarBackground: Colors.LightBlue.Darken2 - O mesmo tom principal para a barra de aplicativos, mantendo a consistência com o tema principal.
        //AppbarText: Colors.Shades.White - Branco para texto na barra de aplicativos, garantindo boa legibilidade.
        //TextPrimary: Colors.Shades.Black - Preto para texto primário,  proporcionando um contraste claro e legível.
        //DrawerText: Colors.Shades.Black - Preto para texto na gaveta de navegação, mantendo a legibilidade.
        //DrawerBackground: Colors.Grey.Lighten4 - Um cinza claro para o fundo da gaveta de navegação, criando uma aparência coesa e limpa.
        //PrimaryContrastText: Colors.Shades.White - Branco para o texto em botões ou outros elementos principais para contraste claro.
    };
}