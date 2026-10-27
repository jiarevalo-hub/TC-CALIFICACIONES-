namespace jairos4.Views;

public partial class calculo : ContentPage
{
    private List<string> listaEstudiantes = new List<string>()
    {
        "Carlos Pérez",
        "María Gómez",
        "Juan Rodríguez",
        "Ana Torres",
        "Luis Mendoza"
    };

    public calculo()
    {
        InitializeComponent();

        pickerEstudiantes.ItemsSource = listaEstudiantes;
        datePickerFecha.Date = DateTime.Today;
    }

    // Limpia automáticamente los campos al cambiar de estudiante en el selector
    private void OnEstudianteSelectedIndexChanged(object sender, EventArgs e)
    {
        txtSeguimiento1.Text = string.Empty;
        txtExamen1.Text = string.Empty;
        txtSeguimiento2.Text = string.Empty;
        txtExamen2.Text = string.Empty;
        txtSeguimiento3.Text = string.Empty;
        txtExamen3.Text = string.Empty;
        
        datePickerFecha.Date = DateTime.Today;
    }

    private async void OnCalcularClicked(object sender, EventArgs e)
    {
        // 1. Validar selección
        if (pickerEstudiantes.SelectedIndex == -1)
        {
            await DisplayAlert("Advertencia", "Por favor seleccione un estudiante del listado.", "OK");
            return;
        }

        // 2. Validar campos vacíos (3 parciales = 6 entradas)
        if (string.IsNullOrWhiteSpace(txtSeguimiento1.Text) || string.IsNullOrWhiteSpace(txtExamen1.Text) ||
            string.IsNullOrWhiteSpace(txtSeguimiento2.Text) || string.IsNullOrWhiteSpace(txtExamen2.Text) ||
            string.IsNullOrWhiteSpace(txtSeguimiento3.Text) || string.IsNullOrWhiteSpace(txtExamen3.Text))
        {
            await DisplayAlert("Error", "Todos los campos de notas de los tres parciales son obligatorios.", "OK");
            return;
        }

        // 3. Validar formato numérico
        if (!double.TryParse(txtSeguimiento1.Text, out double seg1) || !double.TryParse(txtExamen1.Text, out double ex1) ||
            !double.TryParse(txtSeguimiento2.Text, out double seg2) || !double.TryParse(txtExamen2.Text, out double ex2) ||
            !double.TryParse(txtSeguimiento3.Text, out double seg3) || !double.TryParse(txtExamen3.Text, out double ex3))
        {
            await DisplayAlert("Error de Formato", "Ingrese únicamente valores numéricos válidos.", "OK");
            return;
        }

        // 4. Validar rangos (0 a 10)
        if (seg1 < 0 || seg1 > 10 || ex1 < 0 || ex1 > 10 ||
            seg2 < 0 || seg2 > 10 || ex2 < 0 || ex2 > 10 ||
            seg3 < 0 || seg3 > 10 || ex3 < 0 || ex3 > 10)
        {
            await DisplayAlert("Error de Rango", "Todas las calificaciones deben estar entre 0 y 10.", "OK");
            return;
        }

        // 5. Cálculos académicos para los 3 parciales (Seguimiento 30% + Examen 20% por parcial)
        double notaParcial1 = (seg1 * 0.3) + (ex1 * 0.7); // Cada parcial da máximo 5 puntos según los porcentajes (3 + 2 = 5)
        double notaParcial2 = (seg2 * 0.3) + (ex2 * 0.7);
        double notaParcial3 = (seg3 * 0.3) + (ex3 * 0.7);

        // CORRECCIÓN: Nota Final como el promedio de los tres parciales (Suma de los 3 parciales dividida para 3)
        double notaFinal = (notaParcial1 + notaParcial2 + notaParcial3) / 3.0;

        // 6. Estado del estudiante basado en la nota final sobre 10:
        // >= 7: Aprobado | 5 a 6.9: Complementario | < 5: Reprobado
        string estado = "";
        if (notaFinal >= 7)
        {
            estado = "APROBADO";
        }
        else if (notaFinal >= 5 && notaFinal <= 6.9)
        {
            estado = "COMPLEMENTARIO";
        }
        else
        {
            estado = "REPROBADO";
        }

        string nombreEstudiante = pickerEstudiantes.SelectedItem.ToString();
        string fechaSeleccionada = datePickerFecha.Date.Value.ToString("dd/MM/yyyy");

        // 7. Mensaje final con los 3 parciales
        string mensaje = $"Nombre: {nombreEstudiante}\n" +
                         $"Fecha: {fechaSeleccionada}\n" +
                         $"Nota Parcial 1: {notaParcial1:F2}\n" +
                         $"Nota Parcial 2: {notaParcial2:F2}\n" +
                         $"Nota Parcial 3: {notaParcial3:F2}\n" +
                         $"Nota Final: {notaFinal:F2}\n" +
                         $"Estado: {estado}";

        await DisplayAlert("Resultado del Estudiante", mensaje, "OK");
    }
}