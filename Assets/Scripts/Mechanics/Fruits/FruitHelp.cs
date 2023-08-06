/// <summary>
/// Clase de ayuda para definir constantes importantes en el manejo de los frutos
/// Si se desea cambiar algún valor recurrir a esta clase.
/// </summary>
public class FruitHelp
{
    /// <summary>
    /// precio de venta de los frutos
    /// </summary>
    public class SalePrices
    {
        public static readonly int corn = 20, tsiuna = 40;
    }

    /// <summary>
    /// precio de compra de los frutos
    /// </summary>
    public class PurchasePrices
    {
        public static readonly int corn = 30;
    }

    /// <summary>
    /// Incremento de saciedad de los frutos
    /// </summary>
    public class SatietyIncrease
    {
        public static readonly int corn = 50, tsiuna = 20;
    }

    /// <summary>
    /// Durabilidad de los frutos
    /// </summary>
    public class Durability
    {
        public static readonly int corn = 1, tsiuna = 1;
    }

    /// <summary>
    /// Sonidos de los frutos
    /// </summary>
    public class Sounds
    {
        public static readonly int corn = 0, tsiuna = 1;
    }

    /// <summary>
    /// Nombres en español de los frutos
    /// </summary>
    public class SpanishNames
    {
        public static readonly string corn = "Fruto de Maíz", tsiuna = "Fruto de Tsiuna";
    }
}