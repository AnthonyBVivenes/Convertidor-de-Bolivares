using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
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
            public double euro { get; set; } = 0;
            public double pesoCol { get; set; } = 0;

            public string cambiotop { get; set; } = "bcv";
            public string cambiodowm { get; set; } = "bs";
            
            

            public bool txtselect { get; set; } = false;

            public double dowm_a_top(double input)
            {

                switch (cambiodowm)
                {
                    case "bcv":

                        switch (cambiotop)
                        {
                            case "bs":
                                return bcv * input;
                                break;
                            case "paralelo":
                                return paralelo.price / input;
                                break;
                            default:
                                return 0;
                                break;
                        }

                        break;
                    case "paralelo":


                        switch (cambiotop)
                        {
                            case "bs":
                                return paralelo.price * input;
                            case "bcv":
                                return bcv / input;
                                break;
                            default:

                                break;
                        }
                        break;
                    case "euro":

                        break;
                    case "bs":

                        switch (cambiotop)
                        {
                            case "bcv":
                                return input / bcv;
                                break;
                            case "paralelo":
                                return input / paralelo.price;
                                break;
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
                                break;
                            case "paralelo":
                                return ( bcv * input) / paralelo.price;
                                break;
                            default:
                                return 0;
                                break;
                        }

                            break;
                        case "paralelo":

                        switch (cambiodowm)
                        {
                            case "bs":
                                return paralelo.price * input;
                            case "bcv":
                                return (input * paralelo.price) / bcv;
                                break;
                            default:

                                break;
                        }
                            break;
                        case "euro":

                            break;
                        case "bs":
                        switch (cambiodowm)
                        {
                            case "bcv":
                                return input / bcv;
                                break;
                            case "paralelo":
                                return input / paralelo.price;
                                break;
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

            public async void getparalelo()
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

                        lbp.Text = paralelo.price.ToString() + " paralelo";
                    }

                    // Actualiza la interfaz de usuario después de cargar los datos
                    //lbp.Text = cambio?.price.ToString() + "bcv" ?? "No data bcv "; // Asegúrate de que cambio no sea nulo
                }
                catch (Exception ex)
                {

                    // Manejo de errores
                    
                    //await DisplayAlert("Error", ex.Message, "OK");
                }



            }


            public conversor()
            {
                paralelo = new Moneda {price=0};

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
                "paralelo",
                "euro",
                "bcv",
                "bs"
            };
            List<string> opcionesDowm = new List<string>
            {
                "paralelo",
                "euro",
                "bcv",
                "bs"
            };
            calculador.lbp = lbp;

            // Asignar la lista al ItemsSource del Picker
            cboxtop.ItemsSource = opciones;
            cboxdowm.ItemsSource = opciones;
            LoadData(); // Llama al método para cargar datos
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

        private void txt_top_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (calculador.txtselect == false) {
                if (EsNumero(txt_top.Text))
                {
                    txt_dowm.Text = (calculador.top_a_dowm(double.Parse(txt_top.Text))).ToString("N2");
                }
            }
        }

        private void txt_top_Focused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = false;
        }

        private void txt_dowm_Focused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = true;
        }

        private void txt_dowm_Unfocused(object sender, FocusEventArgs e)
        {
            calculador.txtselect = false;
        }

        private void txt_top_Unfocused(object sender, FocusEventArgs e)
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
                case "bcv":
                    calculador.cambiotop = "bcv";
                    lbp.Text = calculador.bcv + " bcv";
                    lb_moneda_top.Text = "bcv";
                    break;
                case "paralelo":
                    lbp.Text = "Cargando";
                    calculador.getparalelo();
                    calculador.cambiotop = "paralelo";
                    lb_moneda_top.Text = "$";
                    break;
                case "euro":
                    msgbox("!Estamos trabajando en incluir esta funcionalidad pronto!");
                    cboxtop.SelectedItem = calculador.cambiotop;
                    break;
                default:
                    calculador.cambiotop = "bs";
                    lb_moneda_top.Text ="bs";
                    break;
            }

        }

        private void cboxdowm_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (cboxdowm.SelectedItem.ToString())
            {
                case "bcv":
                    calculador.cambiodowm = "bcv";
                    lbp.Text = calculador.bcv + " bcv";
                    lb_moneda_top.Text = "bcv";
                    break;
                case "paralelo":
                    lbp.Text = "Cargando";
                    calculador.getparalelo();
                    calculador.cambiodowm = "paralelo";
                    lb_moneda_dowm.Text ="$";
                    break;
                case "euro":
                    msgbox("!Estamos trabajando en incluir esta funcionalidad pronto!");
                    cboxdowm.SelectedItem = calculador.cambiodowm;
                    break;
                default:
                    calculador.cambiodowm = "bs";
                    lb_moneda_dowm.Text ="bs";
                    break;
            }

        }
    }
}