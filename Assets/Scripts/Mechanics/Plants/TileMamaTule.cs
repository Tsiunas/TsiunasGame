using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMamaTule : MonoBehaviour {

    private void Start()
    {
        DetectateNeighbors();
    }

    public List<GameObject> neighbors = new List<GameObject>();
    /// <summary>
    /// Sirve para detectar los Tiles vecinos en diferentes ángulos dependiendo del Tamaño
    /// </summary>
    public void DetectateNeighbors()
    {
        // Cambio el estado de ocupación de este objeto
        ChangeOccupationStatus();
        // Limpio la lista de vecinos
        neighbors.Clear();
        // Declaro una distancia para el Raycast
        float distance = 2.5f;
        // Realizo un ciclo por cada vez que se deba lanzar el rayo
        for (int i = 0; i < 15; i++)
        {
            // transformación para el ángulo obtenido dependiendo el tamaño de este objeto
            float rad = Mathf.Deg2Rad * (i * 45);

            distance = i % 4 == 0 ? 3.5f : 1.5f;
            // Cambio de coordenadas 2d a 3d, coseno: eje X, seno: eje Y
            Vector3 direction = transform.TransformDirection(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0));

            // Lanza un rayo y alamcena los objetos tocados
            RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, distance);
            foreach (RaycastHit h in hit)
            {
                // si el objeto tocado tiene Collider y no es este mism objeto
                if (h.collider != null && h.collider.gameObject != this.gameObject && !h.collider.gameObject.tag.Equals("ToolTarget") && h.collider.gameObject.layer != 9)
                {
                    // añado el objeto a la lista de vecinos
                    neighbors.Add(h.collider.gameObject);
                    // Obtengo el componente ToolTarget del vecino actaul y cambio su estado de ocupación
                    if (h.collider.gameObject.GetComponent<ToolTarget>() != null)
                        h.collider.gameObject.GetComponent<ToolTarget>().ChangeOccupationStatus(true, false);
                }
            }
        }
    }

    /// <summary>
    /// Sirve para cambiar el estado de ocupación XD (si este Tile está o no ocupado)
    /// </summary>
    public void ChangeOccupationStatus()
    {
        // Cachñe color
        Color _color = GetComponent<SpriteRenderer>().color;
        // Cambio de color dependiendo del valor de _toolTarget.Occupied
        _color = new Color(197.0f / 255.0f, 197.0f / 255.0f, 197.0f / 255.0f);
        // Asignación del nuevo color
        GetComponent<SpriteRenderer>().color = _color;

        // Si sameObject es false, habilite o inhabilite el Collider dependiendo de !_toolTarget.Occupied
        GetComponent<Collider>().enabled = false;
    }
}

