using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using UiPrueba1.Models;

namespace UiPrueba1.ViewModels
{
    public enum SyncStatus { Idle, Syncing, Synced, NotFound }

    public class ClienteViewModel : INotifyPropertyChanged
    {

        private string _clienteId = string.Empty;
        private string _tipoId = "Cédula Física";
        private string _nombre = string.Empty;
        private bool _esReceptor;
        private string _telefono = string.Empty;
        private string _email = string.Empty;
        private string? _provinciaSeleccionada;
        private string? _cantonSeleccionado;
        private string? _distritoSeleccionado;
        private string _direccion = string.Empty;
        private string _codActividad = string.Empty;
        private string _descActividad = string.Empty;
        private SyncStatus _syncStatus = SyncStatus.Idle;
        private string _mensajeValidacion = string.Empty;


        public string ClienteId
        {
            get => _clienteId;
            set { _clienteId = value; OnPropertyChanged(); }
        }

        public string TipoId
        {
            get => _tipoId;
            set
            {
                _tipoId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EsFisico));
                OnPropertyChanged(nameof(EsJuridico));
                OnPropertyChanged(nameof(EsExtranjero));
            }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public bool EsReceptor
        {
            get => _esReceptor;
            set
            {
                _esReceptor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MostrarDatosFiscales));
                OnPropertyChanged(nameof(NoEsReceptor));
            }
        }

        public string Telefono
        {
            get => _telefono;
            set { _telefono = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string? ProvinciaSeleccionada
        {
            get => _provinciaSeleccionada;
            set
            {
                _provinciaSeleccionada = value;
                OnPropertyChanged();
                CargarCantones(value);
                CantonSeleccionado = null;
                OnPropertyChanged(nameof(CanTenerCanton));
                OnPropertyChanged(nameof(CantonOpacity));
            }
        }

        public string? CantonSeleccionado
        {
            get => _cantonSeleccionado;
            set
            {
                _cantonSeleccionado = value;
                OnPropertyChanged();
                CargarDistritos(value);
                DistritoSeleccionado = null;
                OnPropertyChanged(nameof(CanTenerDistrito));
                OnPropertyChanged(nameof(DistritoOpacity));
            }
        }

        public string? DistritoSeleccionado
        {
            get => _distritoSeleccionado;
            set { _distritoSeleccionado = value; OnPropertyChanged(); }
        }

        public string Direccion
        {
            get => _direccion;
            set { _direccion = value; OnPropertyChanged(); }
        }

        public string CodActividad
        {
            get => _codActividad;
            set { _codActividad = value; OnPropertyChanged(); }
        }

        public string DescActividad
        {
            get => _descActividad;
            set
            {
                _descActividad = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TieneDescActividad));
            }
        }

        public SyncStatus SyncStatus
        {
            get => _syncStatus;
            set
            {
                _syncStatus = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SyncTexto));
                OnPropertyChanged(nameof(EstasSincronizando));
                OnPropertyChanged(nameof(NoEstasSincronizando));
                OnPropertyChanged(nameof(SyncButtonBackground));
                OnPropertyChanged(nameof(SyncButtonTextColor));
            }
        }

        public string MensajeValidacion
        {
            get => _mensajeValidacion;
            set
            {
                _mensajeValidacion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TieneMensajeValidacion));
            }
        }

        // ──────── Computed (bool) ────────

        public bool MostrarDatosFiscales => _esReceptor;
        public bool NoEsReceptor         => !_esReceptor;
        public bool EsFisico             => _tipoId == "Cédula Física";
        public bool EsJuridico           => _tipoId == "Cédula Jurídica";
        public bool EsExtranjero         => _tipoId == "Extranjero No Domiciliado";
        public bool EstasSincronizando   => _syncStatus == SyncStatus.Syncing;
        public bool NoEstasSincronizando => _syncStatus != SyncStatus.Syncing;
        public bool TieneMensajeValidacion => !string.IsNullOrEmpty(_mensajeValidacion);
        public bool TieneDescActividad   => !string.IsNullOrEmpty(_descActividad);
        public bool CanTenerCanton       => !string.IsNullOrEmpty(_provinciaSeleccionada);
        public bool CanTenerDistrito     => !string.IsNullOrEmpty(_cantonSeleccionado);

        // ──────── Computed (double) ────────

        public double CantonOpacity   => CanTenerCanton   ? 1.0 : 0.45;
        public double DistritoOpacity => CanTenerDistrito ? 1.0 : 0.45;

        // ──────── Computed (string) ────────

        public string SyncTexto => _syncStatus switch
        {
            SyncStatus.Syncing  => "Sincronizando...",
            SyncStatus.Synced   => "Sincronizado ✓",
            SyncStatus.NotFound => "No encontrado",
            _                   => "🔄 Sincronizar"
        };

        // ──────── Computed (Color) — Sync button ────────

        public Color SyncButtonBackground => _syncStatus switch
        {
            SyncStatus.Syncing  => Color.FromArgb("#9CA3AF"),
            SyncStatus.NotFound => Color.FromArgb("#EF4444"),
            _                   => Color.FromArgb("#22C55E")
        };
        public Color SyncButtonTextColor => Colors.White;

        // ──────── Collections ────────

        public ObservableCollection<string> Provincias { get; } = new();
        public ObservableCollection<string> Cantones   { get; } = new();
        public ObservableCollection<string> Distritos  { get; } = new();

        public IReadOnlyList<string> TiposIdentificacion { get; } = new[]
        {
            "Cédula Física",
            "Cédula Jurídica",
            "DIMEX",
            "NITE",
            "Extranjero No Domiciliado",
            "No Contribuyente"
        };

        // ──────── Commands ────────

        public ICommand SincronizarCommand      { get; }
        public ICommand GuardarCommand          { get; }
        public ICommand GuardarYVolverCommand   { get; }
        public ICommand CancelarCommand         { get; }
        public ICommand BuscarActividadCommand  { get; }

        public ClienteViewModel()
        {
            SincronizarCommand      = new Command(async () => await EjecutarSincronizar());
            GuardarCommand          = new Command(async () => await EjecutarGuardar());
            GuardarYVolverCommand   = new Command(async () => await EjecutarGuardarYVolver());
            CancelarCommand         = new Command(async () => await EjecutarCancelar());
            BuscarActividadCommand  = new Command(async () => await EjecutarBuscarActividad());

            CargarProvincias();
        }

        // ──────── Data Loaders ────────

        private void CargarProvincias()
        {
            foreach (var p in new[] { "San José", "Alajuela", "Cartago", "Heredia", "Guanacaste", "Puntarenas", "Limón" })
                Provincias.Add(p);
        }

        private void CargarCantones(string? provincia)
        {
            Cantones.Clear();
            if (string.IsNullOrEmpty(provincia)) return;

            var data = new Dictionary<string, string[]>
            {
                ["San José"]   = ["San José", "Escazú", "Desamparados", "Puriscal", "Tarrazú", "Aserrí", "Mora", "Goicoechea", "Santa Ana"],
                ["Alajuela"]   = ["Alajuela", "San Ramón", "Grecia", "San Mateo", "Atenas", "Naranjo", "Palmares", "Poás", "Orotina"],
                ["Cartago"]    = ["Cartago", "Paraíso", "La Unión", "Jiménez", "Turrialba", "Alvarado", "Oreamuno", "El Guarco"],
                ["Heredia"]    = ["Heredia", "Barva", "Santo Domingo", "Santa Bárbara", "San Rafael", "San Isidro", "Belén", "Flores"],
                ["Guanacaste"] = ["Liberia", "Nicoya", "Santa Cruz", "Bagaces", "Carrillo", "Cañas", "Abangares", "Tilarán"],
                ["Puntarenas"] = ["Puntarenas", "Esparza", "Buenos Aires", "Montes de Oro", "Osa", "Aguirre", "Golfito", "Coto Brus"],
                ["Limón"]      = ["Limón", "Pococí", "Siquirres", "Talamanca", "Matina", "Guácimo"]
            };

            if (data.TryGetValue(provincia, out var cantones))
                foreach (var c in cantones) Cantones.Add(c);
        }

        private void CargarDistritos(string? canton)
        {
            Distritos.Clear();
            if (string.IsNullOrEmpty(canton)) return;

            // TODO: Replace with real district data from API/DB
            foreach (var sufijo in new[] { "Centro", "Norte", "Sur", "Este", "Oeste" })
                Distritos.Add($"{canton} {sufijo}");
        }

        // ──────── Command Handlers ────────

        private async Task EjecutarSincronizar()
        {
            if (string.IsNullOrWhiteSpace(ClienteId)) return;

            SyncStatus = SyncStatus.Syncing;
            await Task.Delay(1500); // TODO: Replace with real Hacienda API call

            if (ClienteId.Length >= 9)
            {
                Nombre     = "Nombre Sugerido (Hacienda)";
                SyncStatus = SyncStatus.Synced;
            }
            else
            {
                SyncStatus = SyncStatus.NotFound;
            }
        }

        private async Task EjecutarBuscarActividad()
        {
            if (string.IsNullOrWhiteSpace(CodActividad)) return;

            await Task.Delay(300); // TODO: Replace with real activity lookup
            DescActividad = "Venta al por menor de alimentos, bebidas y tabaco";
        }

        private async Task EjecutarGuardar()
        {
            if (!Validar()) return;

            // TODO: Save via repository/service
            await Task.CompletedTask;
            await TryNavigateBack();
        }

        private async Task EjecutarGuardarYVolver()
        {
            if (!Validar()) return;

            // TODO: Save and navigate back to invoice screen
            await Task.CompletedTask;
            await TryNavigateBack();
        }

        private async Task EjecutarCancelar()
        {
            MensajeValidacion = string.Empty;
            await TryNavigateBack();
        }

        private static async Task TryNavigateBack()
        {
            try { await Shell.Current.GoToAsync(".."); }
            catch { /* Root page — no parent to navigate to */ }
        }

        private bool Validar()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(ClienteId))
                errores.Add("• El ID del cliente es requerido.");

            if (string.IsNullOrWhiteSpace(Nombre))
                errores.Add("• El nombre del cliente es requerido.");

            if (EsReceptor)
            {
                if (string.IsNullOrWhiteSpace(Telefono))
                    errores.Add("• El teléfono es requerido.");
                else if (!EsTelefonoValido(Telefono))
                    errores.Add("• El teléfono no es válido (ej: 8888-8888).");

                if (string.IsNullOrWhiteSpace(Email))
                    errores.Add("• El correo electrónico es requerido.");
                else if (!EsEmailValido(Email))
                    errores.Add("• El correo electrónico no es válido (ej: usuario@dominio.com).");

                if (string.IsNullOrWhiteSpace(ProvinciaSeleccionada))
                    errores.Add("• La provincia es requerida.");

                if (string.IsNullOrWhiteSpace(CantonSeleccionado))
                    errores.Add("• El cantón es requerido.");

                if (string.IsNullOrWhiteSpace(DistritoSeleccionado))
                    errores.Add("• El distrito es requerido.");

                if (string.IsNullOrWhiteSpace(Direccion))
                    errores.Add("• La dirección es requerida.");

                if (string.IsNullOrWhiteSpace(CodActividad))
                    errores.Add("• El código de actividad es requerido.");
            }

            MensajeValidacion = errores.Count > 0
                ? string.Join("\n", errores)
                : string.Empty;

            return errores.Count == 0;
        }

        private static readonly Regex _emailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex _telefonoRegex =
            new(@"^[\d\s\-\+\(\)]{7,15}$", RegexOptions.Compiled);

        private static bool EsEmailValido(string email)    => _emailRegex.IsMatch(email.Trim());
        private static bool EsTelefonoValido(string tel)   => _telefonoRegex.IsMatch(tel.Trim());

        // ──────── INotifyPropertyChanged ────────

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
