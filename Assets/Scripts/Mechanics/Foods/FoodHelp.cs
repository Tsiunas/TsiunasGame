/// <summary>
/// Clase de ayuda para definir constantes importantes en el manejo de las comidas
/// Si se desea cambiar algún valor recurrir a esta clase.
/// </summary>
public class FoodHelp
{
    /// <summary>
    /// Costos de compra de las comidas
    /// </summary>
    public class Costs
    {
        public static readonly int bread = 5, riceWithBeans = 20, soup = 10, coffe = 2, milk = 3, water = 1, chontaduro = 5, fish = 10;
    }

    /// <summary>
    /// Incremento de saciedad de las comidas
    /// </summary>
    public class SatietyIncrease
    {
        public static readonly int bread = 5, riceWithBeans = 20, soup = 10, coffe = 2, milk = 3, water = 1, chontaduro = 5, fish = 10;
    }

    /// <summary>
    /// Durabilidad de las comidas
    /// </summary>
    public class Durability
    {
        public static readonly int bread = 1, riceWithBeans = 1, soup = 1, coffe = 1, milk = 1, water = 1, chontaduro = 1, fish = 1;
    }

    /// <summary>
    /// Sonidos de las comidas
    /// </summary>
    public class Sounds
    {
        public static readonly int bread = 0, riceWithBeans = 1, soup = 2, coffe = 3, milk = 4, water = 5, chontaduro = 6, fish = 7;
    }

    /// <summary>
    /// Nombres en español de las comidas
    /// </summary>
    public class SpanishNames
    {
        public static readonly string bread = "Pan", riceWithBeans = "Arroz con fríjoles", soup = "Sopa", coffe = "Café", milk = "Leche", water = "Agua", chontaduro = "Chontaduro", fish = "Pescado";
    }
}