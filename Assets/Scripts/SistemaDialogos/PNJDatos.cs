using UnityEngine;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using System;

[CreateAssetMenu()]
public class PNJDatos : ScriptableObject
{
    public enum EstadosMachismo { Machista, Regular, Corresponsable};
    

    //TODO: Eliminar pero aprender de esto, esto es para crear instancias de archivos de script
    //Esto se usará para las consecuencias
    //public MonoScript prueba;
    
	public PNJDatos()
    {
        situaciones = new List<Situacion>();
        speechPorDefecto = new Speech();                    
        estadoMachismo = EstadosMachismo.Regular;

    }

    /*TODO: IDEM
    public void Instanciar(GameObject go)
    {
        go.AddComponent(prueba.GetClass());
        
    }
    */
	public Sprite avatar;
	public string id;
	public string nombre;
    public List<Situacion> situaciones;
    public bool esAnonimo;
    public Speech speechPorDefecto;
    public List<Speech> speechesPorAmistad;
    public EstadosMachismo estadoMachismo;
    public string mensajeMamaTule;
    public string textoFlamado;
}