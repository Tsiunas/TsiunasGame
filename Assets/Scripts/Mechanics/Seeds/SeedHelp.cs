/// <summary>
/// Clase de ayuda para definir constantes importantes en el manejo de las semillas
/// Si se desea cambiar algún valor recurrir a esta clase.
/// </summary>
public class SeedHelp
{
    /// <summary>
    /// precio de venta de las semillas
    /// </summary>
    public class SalePrices
    {
        public static readonly int corn = 5, tsiuna = 10;
    }

    /// <summary>
    /// Incremento de saciedad de las semillas
    /// </summary>
    public class SatietyIncrease
    {
        public static readonly int corn = 5, tsiuna = 10;
    }

    /// <summary>
    /// Durabilidad de las semillas
    /// </summary>
    public class Durability
    {
        public static readonly int corn = 1, tsiuna = 1;
    }

    /// <summary>
    /// Sonidos de las semillas
    /// </summary>
    public class Sounds
    {
        public static readonly int corn = 0, tsiuna = 1;
    }

    /// <summary>
    /// Nombres en español de las semillas
    /// </summary>
    public class SpanishNames
    {
        public static readonly string corn = "Semilla de Maíz", tsiuna = "Semilla de Tsiuna";
    }
}