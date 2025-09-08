using System.Globalization;

namespace Practica_5_Imagenes.Converters;

/// <summary>
/// Converter que convierte un string a bool para mostrar/ocultar elementos en la UI.
/// Devuelve true si el string no es null, vacío o solo espacios en blanco.
/// 
/// Uso típico: IsVisible="{Binding ErrorMessage, Converter={StaticResource StringToBoolConverter}}"
/// </summary>
public class StringToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !string.IsNullOrWhiteSpace(value?.ToString());
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack no está soportado para StringToBoolConverter");
    }
}

/// <summary>
/// Converter que convierte un bool a Color.
/// Útil para cambiar colores de botones según el estado.
/// 
/// Uso típico: BackgroundColor="{Binding IsCacheEnabled, Converter={StaticResource BoolToColorConverter}}"
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    /// <summary>
    /// Color cuando el valor es true (por defecto: Verde)
    /// </summary>
    public Color TrueColor { get; set; } = Colors.Green;

    /// <summary>
    /// Color cuando el valor es false (por defecto: Gris)
    /// </summary>
    public Color FalseColor { get; set; } = Colors.Gray;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? TrueColor : FalseColor;
        }
        return FalseColor;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack no está soportado para BoolToColorConverter");
    }
}

/// <summary>
/// Converter que formatea el tamaño de archivo en bytes a una representación legible.
/// Ejemplo: 1024 -> "1.0 KB", 1048576 -> "1.0 MB"
/// 
/// Uso típico: Text="{Binding FileSize, Converter={StaticResource FileSizeConverter}}"
/// </summary>
public class FileSizeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long bytes)
            return "0 B";

        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int suffixIndex = 0;
        double size = bytes;

        while (size >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            size /= 1024;
            suffixIndex++;
        }

        return $"{size:F1} {suffixes[suffixIndex]}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack no está soportado para FileSizeConverter");
    }
}