using System;
using System.Net.Http;

using System.Net.Http.Headers;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;

//token de la api
using Divisas.modulos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
//

namespace Divisas.Views
{
    public partial class AboutPage : ContentPage
    {
        public class Moneda
        {
            public bool isfill { get; set; } = false;
            public double change { get; set; }
            public string color { get; set; }
            public string image { get; set; }
            public string last_update { get; set; }
            public double percent { get; set; }
            public double price { get; set; }
            public double price_old { get; set; }
            public string symbol { get; set; }
            public string title { get; set; }
        }
        public class conversor
        {
            
            public double bcv { get; set; } = 0;
            public Moneda paralelo;
            public Moneda euroParalelo { get; set; }
            public Moneda euroBcv { get; set; }
            public double pesoCol { get; set; } = 0;

            public string cambiotop { get; set; } = "bcv";
            public string cambiodowm { get; set; } = "bs";
            
            

            public bool txtselect { get; set; } = false;

            public double dowm_a_top(double input)
            {
                //modulos.token_pass.pytoken= "d";//prueba

                switch (cambiodowm)
                {
                    case "bcv":

                        switch (cambiotop)
                        {
                            case "bs":
                                return bcv * input;
                            case "paralelo":
                                return (bcv * input) / paralelo.price ;
                            case "euro paralelo":
                                return (bcv * input) / euroParalelo.price ;
                            case "euro bcv":
                                return (bcv * input) / euroBcv.price ;
                            default:
                                return 0;
                        }


                    case "paralelo":
                        switch (cambiotop)
                        {
                            case "bs":
                                return paralelo.price * input;
                            case "bcv":
                                return (input * paralelo.price) / bcv ;
                            case "euro paralelo":
                                return (input * paralelo.price) / euroParalelo.price ;
                            case "euro bcv":
                                return (input * paralelo.price) / euroBcv.price ;
                            default:
                                break;
                        }
                        break;
                    case "euro paralelo":


                        switch (cambiotop)
                        {
                            case "bs":
                                return euroParalelo.price * input;
                            case "bcv":
                                return (input * euroParalelo.price) / bcv;
                            case "paralelo":
                                return (input * euroParalelo.price) / paralelo.price;
                            case "euro bcv":
                                return (input * euroParalelo.price) / euroBcv.price;
                            default:

                                break;
                        }
                        break;
                        
                    case "euro bcv":


                        switch (cambiotop)
                        {
                            case "bs":
                                return euroBcv.price * input;
                            case "bcv":
                                return (input * euroBcv.price) / bcv;
                            case "paralelo":
                                return (input * euroBcv.price) / paralelo.price;
                            case "euro paralelo":
                                return (input * euroBcv.price) / euroParalelo.price;
                            default:

                                break;
                        }
                        break;

                    case "bs":

                        switch (cambiotop)
                        {
                            case "bcv":
                                return input / bcv;
                            case "paralelo":
                                return input / paralelo.price;
                            case "euro paralelo":
                                return input / euroParalelo.price;
                            case "euro bcv":
                                return input / euroBcv.price;
                            default:
                                break;
                        }

                        break;
                    default:

                        break;
                }
                return -404;
            }




            // conversión arriba hacia abajo
            public double top_a_dowm(double input)
            {

                    switch (cambiotop)
                    {
                        case "bcv":

                        switch (cambiodowm)
                        {
                            case "bs":
                                return bcv * input;
                            case "paralelo":
                                return ( bcv * input) / paralelo.price;
                            case "euro paralelo":
                                return ( bcv * input) / euroParalelo.price;
                            case "euro bcv":
                                return ( bcv * input) / euroBcv.price;
                            default:
                                return 0;
                        }


                        case "paralelo":

                        switch (cambiodowm)
                        {
                            case "bs":
                                return paralelo.price * input;
                            case "bcv":
                                return (input * paralelo.price) / bcv;
                            case "euro paralelo":
                                return (input * paralelo.price) / euroParalelo.price;
                            case "euro bcv":
                                return (input * paralelo.price) / euroBcv.price;
                            default:

                                break;
                        }
                            break;

                        case "euro paralelo":


                        switch (cambiodowm)
                        {
                            case "bs":
                                return euroParalelo.price * input;
                            case "bcv":
                                return (input * euroParalelo.price) / bcv;
                            case "euro paralelo":
                                return (input * euroParalelo.price) / euroParalelo.price;
                            case "paralelo":
                                return (input * euroParalelo.price) / paralelo.price;
                            default:

                                break;
                        }
                        break;


                        

                        case "euro bcv":


                        switch (cambiodowm)
                        {
                            case "bs":
                                return euroBcv.price * input;
                            case "bcv":
                                return (input * euroBcv.price) / bcv;
                            case "euro paralelo":
                                return (input * euroBcv.price) / euroParalelo.price;
                            case "paralelo":
                                return (input * euroBcv.price) / paralelo.price;
                            default:

                                break;
                        }
                        break;



                    case "bs":

                        switch (cambiodowm)
                        {
                            case "bcv":
                                return input / bcv;
                            case "bs":
                                return input;
                            case "paralelo":
                                return input / paralelo.price;
                            case "euro paralelo":
                                return input / euroParalelo.price;
                            default:
                                break;
                        }
                            break;

                        default:

                            break;
                    }
                return -404;
            }

            //////////////////gets cambios
            /// <summary>
            /// 

            public Label lbp;

            public async Task getparalelo()
            {
                lbp.Text = "cargando";
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = "https://pydolarve.org/api/v1/dollar?page=enparalelovzla&monitor=enparalelovzla";
                        string jsonResponse = await client.GetStringAsync(url);
                        paralelo = JsonConvert.DeserializeObject<Moneda>(jsonResponse);
                        paralelo.isfill = true;
                        lbp.Text = paralelo.price.ToString() + " = $ paralelo";
                        await App.Current.MainPage.DisplayAlert("Precio del dolar paralelo", paralelo.price.ToString(), "OK");

                    }
                    // Actualiza la interfaz de usuario después de cargar los datos
                    //lbp.Text = cambio?.price.ToString() + "bcv" ?? "No data bcv "; // Asegúrate de que cambio no sea nulo
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }

            }










            private static readonly HttpClient client = new HttpClient();




            public async Task GetEuroParalelo()
            {
                string url = "https://pydolarve.org/api/v1/euro?monitor=enparalelovzla";
                string token = modulos.token_pass.pytoken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                     euroParalelo = JsonConvert.DeserializeObject<Moneda>(jsonResponse);
                    euroParalelo.isfill = true;
                    lbp.Text = euroParalelo.price.ToString() + " = € Paralelo";
                    await App.Current.MainPage.DisplayAlert("Precio del Euro paralelo", euroParalelo.price.ToString(), "OK");
                    
                }
                catch (HttpRequestException e)
                {
                    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }






            public async Task GetEuroBcv()
            {
                string url = "https://pydolarve.org/api/v1/euro?monitor=bcv";
                string token = modulos.token_pass.pytoken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    euroBcv = JsonConvert.DeserializeObject<Moneda>(jsonResponse);
                    euroBcv.isfill = true;
                    lbp.Text = euroBcv.price.ToString() + " = € BCV";
                    await App.Current.MainPage.DisplayAlert("Precio del Euro BCV", euroBcv.price.ToString(), "OK");


                }
                catch (HttpRequestException e)
                {
                    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }


            public Entry txtdowm;
            public Entry txttop;


            public conversor()
            {
                paralelo = new Moneda {price=0};
                euroBcv = new Moneda { price = 0 };
                euroParalelo = new Moneda { price = 0 };
            }

        }
        public static conversor calculador = new conversor {};
        public static Moneda cambio;

        public bool EsNumero(string texto)
        {
            
            double numero;
            return double.TryParse(texto, out numero);
        }

        public AboutPage()
        {
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario
                                   // Definir las opciones del Picker
            List<string> opciones = new List<string>
            {
                "Dólar paralelo",
                "Dólar bcv",
                "Euro paralelo",
                "Euro bcv",
                "Bolívares"
            };

            List<string> opcionesDowm = new List<string>
            {
                "paralelo",
                "euro paralelo",
                "bcv",
                "bs"
            };
            calculador.lbp = lbp;
            calculador.txtdowm = txt_dowm;
            calculador.txttop = txt_top;

            // Asignar la lista al ItemsSource del Picker
            cboxtop.ItemsSource = opciones;
            cboxdowm.ItemsSource = opciones;
            LoadData(); // Llama al método para cargar datos

            cboxtop.SelectedItem = "Dólar bcv";
            cboxdowm.SelectedItem = "Bolívares";

        }

        private async void LoadData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://pydolarve.org/api/v1/dollar?page=bcv&monitor=usd";
                    string jsonResponse = await client.GetStringAsync(url);
                    DeserializacionJson(jsonResponse);
                }

                // Actualiza la interfaz de usuario después de cargar los datos
                lbp.Text = cambio?.price.ToString() + "bcv" ?? "No data bcv "; // Asegúrate de que cambio no sea nulo
            }
            catch (Exception ex)
            {
                // Manejo de errores
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public static void DeserializacionJson(string jsonString)
        {
            cambio = JsonConvert.DeserializeObject<Moneda>(jsonString);
            cambio.isfill = true;
            calculador.bcv = cambio.price;
            calculador.cambiotop = "bcv";
            calculador.cambiodowm = "bs";
        }






        public void uplb(string txt)
        {
            lbp.Text = txt;
        }

        private void txt_dowm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (calculador.txtselect == true)
            {
                if(EsNumero(txt_dowm.Text))
                {
                    txt_top.Text = (calculador.dowm_a_top(double.Parse(txt_dowm.Text))).ToString("N2");
                }

            }

        }

        public void txt_top_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (calculador.txtselect == false) {
                if (EsNumero(txt_top.Text))
                {
                    txt_dowm.Text = (calculador.top_a_dowm(double.Parse(txt_top.Text))).ToString("N2");
                }
            }
        }


        public void txt_top_Focused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = false;
        }

        public void txt_dowm_Focused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = true;
        }

        public void txt_dowm_Unfocused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = false;
        }

        public void txt_top_Unfocused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = true;
        }

        private async void msgbox(string txt)
        {
            await DisplayAlert("message system",txt,"okey");
        }

        private void cboxtop_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DisplayAlert("selecionado", cboxtop.SelectedItem.ToString(), "ok");


            switch (cboxtop.SelectedItem.ToString())
            {
                case "Dólar bcv":
                    calculador.cambiotop = "bcv";
                    lbp.Text = calculador.bcv + " bcv";
                    lb_moneda_top.Text = "bcv";

                    break;
                case "Dólar paralelo":
                    lbp.Text = "Cargando";
                    calculador.getparalelo();
                    calculador.cambiotop = "paralelo";
                    lb_moneda_top.Text = "$";
                    break;
                case "Euro paralelo":
                    //msgbox("!Estamos trabajando en incluir esta funcionalidad pronto!");
                    lbp.Text = "Cargando";
                    calculador.GetEuroParalelo();
                    calculador.cambiotop = "euro paralelo";
                    lb_moneda_top.Text = "€";
                    //cboxtop.SelectedItem = calculador.cambiotop;
                    break;
                case "Euro bcv":
                    //msgbox("!Estamos trabajando en incluir esta funcionalidad pronto!");
                    lbp.Text = "Cargando";
                    calculador.GetEuroBcv();
                    calculador.cambiotop = "euro bcv";
                    lb_moneda_top.Text = "€";
                    //cboxtop.SelectedItem = calculador.cambiotop;
                    break;
                default:
                    calculador.cambiotop = "bs";
                    lb_moneda_top.Text ="bs";
                    break;
            }
            txt_top.Text = "0";

        }

        private void cboxdowm_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch (cboxdowm.SelectedItem.ToString())
            {
                case "Dólar bcv":
                    calculador.cambiodowm = "bcv";
                    lbp.Text = calculador.bcv + " bcv";
                    lb_moneda_top.Text = "bcv";
                    
                    break;
                case "Dólar paralelo":
                    lbp.Text = "Cargando";
                    calculador.getparalelo();
                    calculador.cambiodowm = "paralelo";
                    lb_moneda_dowm.Text ="$";
                    break;
                case "Euro paralelo":
                    //msgbox("!Estamos trabajando en incluir esta funcionalidad pronto!");
                    lbp.Text = "Cargando";
                    calculador.GetEuroParalelo();
                    calculador.cambiodowm = "euro paralelo";
                    lb_moneda_dowm.Text = "€";
                    //cboxtop.SelectedItem = calculador.cambiotop;
                    break;
                case "Euro bcv":
                    //msgbox("!Estamos trabajando en incluir esta funcionalidad pronto!");
                    lbp.Text = "Cargando";
                    calculador.GetEuroBcv();
                    calculador.cambiodowm = "euro bcv";
                    lb_moneda_dowm.Text = "€";
                    //cboxtop.SelectedItem = calculador.cambiotop;
                    break;
                default:
                    calculador.cambiodowm = "bs";
                    lb_moneda_dowm.Text ="bs";
                    break;
            }
            txt_dowm.Text = "0";

        }
    }
}