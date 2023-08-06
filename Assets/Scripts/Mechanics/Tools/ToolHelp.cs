/// <summary>
/// Clase de ayuda para definir constantes importantes en el manejo de las herramientas
/// Si se desea cambiar algún valor recurrir a esta clase.
/// </summary>
public class ToolHelp {
    /// <summary>
    /// Costos de las herramientas
    /// </summary>
	public class Costs
	{
		public static readonly int hoe = 20, axe = 35, hammer = 35, machete = 25, watering_can = 20, hand = 0;
	}

    /// <summary>
    /// Durabilidad de las herramientas
    /// </summary>
	public class Durability
	{
        public static readonly int hoe = 10, axe = 10, hammer = 10, machete = 10, watering_can = 10, hand = 10, plusForGold = 10;
	}

    /// <summary>
    /// Sonidos de las herramientas
    /// </summary>
    public class Sounds
    {
        public static readonly int hoe = 0, axe = 1, hammer = 2, machete = 3, watering_can = 4, hand = 9;
    }

    /// <summary>
    /// Nombres en español de las herramientas
    /// </summary>
    public class SpanishNames
    {
        public static readonly string Hoe = "Azadón", Axe = "Hacha", Hammer = "Martillo", Machete = "Machete", Watering_can = "Regadera", hand = "Mano";
    }
}



 