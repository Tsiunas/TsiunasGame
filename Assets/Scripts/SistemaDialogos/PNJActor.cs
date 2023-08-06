
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tsiunas.SistemaDialogos
{
	[RequireComponent(typeof(UIAmistad))]
	[RequireComponent(typeof(PNJModelController))]
	public class PNJActor : PNJ, IGuardable<PNJActor.PNJActorDatos>
	{
		// Eduardo:
		// Se contabiliza cada vez que una situación es activada
		// Se verifica que la cantidad de situaciones activadas sea igual a la cantidad total de situaciones
		// En el momento de consultar ésta propiedad ya se debe haber terminado la situación actual para retornnar: TRUE
		private int situacionesActivadas;
		private bool puedeSerTsinuado = true;
		/// <summary>
		/// Retorna si el PNJActor puede ser Tsiunado.
		/// Esto es, si la situación activa no existe y si no tiene situación encolada
		/// </summary>
		public bool PuedeSerTsinuado {
			get { return situacionActiva == null && situacionEncolada == false; }
		}

		public bool _PuedeSerTsinuado
		{
			get { return puedeSerTsinuado; }
			private set { puedeSerTsinuado = value; }
		}

	  

		public NivelesAmistad nivelAmistad;
		public NivelesAmistad NivelAmistad
		{
			set
			{
				nivelAmistad = value;
				if (OnAmistadCambio != null) OnAmistadCambio((int)nivelAmistad);
			}
			get
			{
				return nivelAmistad;
			}
		}

		private PNJDatos.EstadosMachismo estadoMachismoActual;
		public PNJDatos.EstadosMachismo EstadoMachismoActual
		{
			get { return estadoMachismoActual; }
		}
		protected internal PNJDatos.EstadosMachismo CambiarEstadoMachismoActual {
			set { estadoMachismoActual = value; }
		}

		public PNJDatos.EstadosMachismo EstadoMachismoOriginal
		{
			get
			{
				return datosPNJ.estadoMachismo;
			}
		}

		/// <summary>
		/// Sisterma de Particulas de Tsiunas Isntanciado
		/// </summary>
		private GameObject spTsiunasInstanciado;
		/// <summary>
		/// El objeto Flama que se activa cuando se Tsiuna al personaje
		/// </summary>
		private GameObject flamaArmoniaIcono;


		public Action<int> OnAmistadCambio;
		public Action<string,object> OnActuar = delegate { };


		public PNJActor()
		{
			nivelAmistad = NivelesAmistad.REGULAR;
		}

		public static void AnimarHablar(string _id) {
			AnimationsManager.Instance.AnimarPNJ(_id, 1);
		}

		public static void AnimarIdle(string _id)
		{
			AnimationsManager.Instance.AnimarPNJ(_id, 0);
		}

		public static void AnimarCaminar(string _id)
		{
			AnimationsManager.Instance.AnimarPNJ(_id, 2);
		}
		
		protected Situacion situacionActiva;

		internal bool SaberPuedeSerTsinuado() { return GameManager.Instance.GetGameState == GameState.InGame ? PuedeSerTsinuado : _PuedeSerTsinuado; } 
		internal void EstablecerSiPuedeSerTsiunado(bool v) { _PuedeSerTsinuado = v; }
		
		private new void Awake()
		{
			base.Awake();

			if (GetComponent<PNJModelController>() == null)
				this.gameObject.AddComponent<PNJModelController>();

			if(datosPNJ != null)
			{
				if(datosPNJ.situaciones != null && datosPNJ.situaciones.Count > 0)
				{
					if(!situacionEncolada)
						situacionActiva = datosPNJ.situaciones[0];
				}
				this.estadoMachismoActual = datosPNJ.estadoMachismo;
			}

			//Si el PNJActor ya existe guardado en el Gestor de PNJ entonces lo cargamos
			if(!string.IsNullOrEmpty(id) && GestorPNJ.Instance.YaContiene(id))
			{
				
				PNJActorDatos p = GestorPNJ.Instance.CargarDatosPNJActor(id);
				this.SetFrom(p);
			}
			//Se instancia el Sistema de Particulas de Tsiunas
			spTsiunasInstanciado =  Instantiate(GestorPNJ.Instance.spTsiunasOriginal, this.transform);
			if(this.estadoMachismoActual > EstadoMachismoOriginal && this.estadoMachismoActual < PNJDatos.EstadosMachismo.Corresponsable)
				spTsiunasInstanciado.SetActive(true);
			//Se instancia la Flama de la Armonía
			InstanciarFlamaPNJ();

			DesplegadorDialogo.Instance.OnCerrado += ActivarIdle;
		}

		private void ActivarIdle()
		{
			AnimarIdle(id);            
		}

		private void InstanciarFlamaPNJ()
		{
			Transform canvas = null;
			try
			{
				canvas = GetComponentInChildren<Canvas>().gameObject.transform;
			}
			catch (NullReferenceException)
			{
				new TsiunasException("No se pudo instanciar la Flama de la Armonía").Tratar();
			}

			flamaArmoniaIcono =  Instantiate(GestorPNJ.Instance.flamaArmoniaOriginal, canvas);
			flamaArmoniaIcono.SetActive(this.flamado);
			
		}

		private new void Start()
		{
			if (OnAmistadCambio != null)
				OnAmistadCambio((int)nivelAmistad);
		}

		private void OnDestroy()
		{
			//Al salir de la escena se guarda el PNJActor para que sea persistente entre escenas
			if(GestorPNJ.Instance != null)
				GestorPNJ.Instance.GuardarDatosPNJActor(this);

			if (DesplegadorDialogo.Instance != null)
				DesplegadorDialogo.Instance.OnCerrado -= ActivarIdle;
		}

		public void EliminarSituacionActiva()
		{
			situacionActiva = null;
		}

		/// <summary>
		/// Se llama para Tsiunar este actor.
		/// </summary>
		internal void Tsiunarse()
		{
			//Si ya es corresponsable
			if(estadoMachismoActual == PNJDatos.EstadosMachismo.Corresponsable)
			{
				//Poner mensaje de agradecimiento por las Tsiunas y un mensaje de MamaTule
				DesplegadorDialogo.PNJDatosSpeeches datosPNJActor = new DesplegadorDialogo.PNJDatosSpeeches(this.datosPNJ, new Speech("!Sí Mama Tule, Gracias!"));
				//FIXME: PONER un verdadero mensaje de Mama Tule
				DesplegadorDialogo.PNJDatosSpeeches datosMamaTule = new DesplegadorDialogo.PNJDatosSpeeches(GestorPNJ.Instance.GetMamaTule(), new Speech("Tú también puedes encargate de llevar mis mensajes de amor y equidad"));

				DesplegadorDialogo.Instance.DesplegarTodos(datosMamaTule,datosPNJActor);
				return;
			}

			//Si no...
			if (estadoMachismoActual < PNJDatos.EstadosMachismo.Corresponsable)
				///Disminuir el valor del estado de machismo
				estadoMachismoActual++;
			//Si pasa a saer Regular
			if(estadoMachismoActual == PNJDatos.EstadosMachismo.Regular)
			{
				spTsiunasInstanciado.SetActive(true);
				SoundManager.PlayExito();
				DesplegadorDialogo.Instance.DesplegarMensajeMamaTuleAlTsiunar(this.datosPNJ);
			}
			if(estadoMachismoActual == PNJDatos.EstadosMachismo.Corresponsable)
			{
				DesplegadorDialogo.Instance.DesplegarMensajeMamaTuleAlTsiunar(this.datosPNJ);
				spTsiunasInstanciado.SetActive(false);
				SoundManager.PlayExito();
				//Activar Icono de Flama (con efecto gráfico)
				flamaArmoniaIcono.SetActive(true);
				flamado = true;
				//Ganar una Flama de la Armonía				
				TrackerSystem.Instance.SendTrackingData("user", "earned", "item", "flama|"+this.name+"|éxito");
				TrackerSystem.Instance.SendTrackingData("user", "progressed", "serious-game", "flama|user|éxito");
				HarmonyFlamesManager.Instance.IncreaseHarmonyFlamesLevel();
        		TrackerSystem.Instance.SendTrackingData("user", "increased", "item", "flama|user|éxito");
				
			}
		}



		private bool situacionEncolada = false;
		/// <summary>
		/// Variable booleana que es true cuando este personaje ha sido flamado (con él se ganó flama)
		/// </summary>
		private bool flamado;

		public void EstablecerSituacionActiva(String id)
		{
			if (string.IsNullOrEmpty(id))
			{
				situacionEncolada = true;
				situacionActiva = null;
				return;
			}
			situacionActiva = datosPNJ.situaciones.Find(s => s.id == id);
			situacionEncolada = true;
			if (situacionActiva == null)
				throw new TsiunasException("Cuidado! Se ha establecido una situacion con id '"+id+"' pero no se encontró esta situación en el PNJdatos");


		}

		public void EstablecerSituacionActiva(int sitActivar)
		{
			if(datosPNJ.situaciones.Count > sitActivar)
			{
				EstablecerSituacionActiva(datosPNJ.situaciones[sitActivar].id);
			}
		}



		protected override void Hablar()
		{
			TrackerSystem.Instance.SendTrackingData("user", "interacted", "character",this.name+"|"+this.name+"|éxito");
			 
			if(situacionActiva!=null && situacionActiva.TieneIntervenciones)
			{
				situacionActiva.IdPNJOwner = this.id;
				situacionActiva.OnSituacionTerminada += TerminarSituacion;
				situacionEncolada = false;
				situacionActiva.Iniciar();
			}
			else
			{
				PNJActor.AnimarHablar(id);
				//Si está flamado (se le han dado todas las tisunas a un personaje para convertirlo)
				if (flamado)
				{
					string texto;
					if (string.IsNullOrEmpty(datosPNJ.textoFlamado))
						texto = "Gracias por las Tsiunas que me diste... Ahora comprendo.";
					else
						texto = datosPNJ.textoFlamado;
					DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, new Speech(texto));
				}
				else
				{
					//Si no hay situación entonces despliega el texto por defecto o el texto dependiendo de su nivel de amistad
					if (nivelAmistad == NivelesAmistad.REGULAR || datosPNJ.speechesPorAmistad == null || datosPNJ.speechesPorAmistad.Count == 0)
					{
						DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, this.datosPNJ.speechPorDefecto);
						return;
					}
					if (nivelAmistad == NivelesAmistad.EMPATICO || nivelAmistad == NivelesAmistad.AMISTAD_ENTABLADA)
					{
						DesplegarSegunNivel(NivelesAmistad.EMPATICO);
					}
					else if (nivelAmistad == NivelesAmistad.DISGUSTO)
					{
						DesplegarSegunNivel(NivelesAmistad.DISGUSTO);
					}
					else
						DesplegarSegunNivel(NivelesAmistad.DESDEN);
				}



				return;
			}
			

		}

		/// <summary>
		/// Envia unmensaje a este objeto con el evento actuar. Lo cual ejecuta la actuación de este personaje por defecto.
		/// </summary>
		internal void ActuarActuacion()
		{
			this.ActuarActuacion("Actuar",null);
		}

		internal void ActuarActuacion(string mensaje)
		{
			this.ActuarActuacion(mensaje, null);
		}

		internal void ActuarActuacion(string mensaje, object obj)
		{
			this.OnActuar(mensaje,obj);
		}
		private void DesplegarSegunNivel(NivelesAmistad n)
		{
			string s = datosPNJ.speechesPorAmistad[(int)n].texto;
			if (!string.IsNullOrEmpty(s))
				DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, this.datosPNJ.speechesPorAmistad[(int)n]);
			else
			{
				string texto = "(null)";
				switch (n)
				{
					case NivelesAmistad.DESDEN: texto = GestorPNJ.TEXTO_DESDEN; break;
					case NivelesAmistad.DISGUSTO: texto = GestorPNJ.TEXTO_DISGUSTO; break;
					case NivelesAmistad.EMPATICO: texto = GestorPNJ.TEXTO_EMPATICO; break;
					case NivelesAmistad.AMISTAD_ENTABLADA: texto = GestorPNJ.TEXTO_AMISTOSO; break;
				}
				DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, new Speech(texto));
			}
				
		}

		/// <summary>
		/// Handler que se ejcuta cuando se termina una situación activa
		/// Por defecto consume la situación asingándola a nulo
		/// </summary>
		private void TerminarSituacion()
		{
			if (situacionActiva != null)
			{
				if (!situacionActiva.noConsumir)
				{
					situacionesActivadas++;
					situacionActiva = null;
				}
				else if (situacionEncolada)
				{
					situacionEncolada = false;
				}
			}
			else
			{
				throw new TsiunasException("Se terminó una situación en este PNJActor, pero la situacionActual es nula");
			}
		}

		public NivelesAmistad SetAmistad(NivelesAmistad nuevaAmistad)
		{
			nivelAmistad = nuevaAmistad;
			if (OnAmistadCambio != null)
				OnAmistadCambio((int)nivelAmistad);
			return nivelAmistad;
		}
		public NivelesAmistad SubirAmistad()
		{
			nivelAmistad = nivelAmistad != NivelesAmistad.AMISTAD_ENTABLADA ? ++nivelAmistad : nivelAmistad;
			if (OnAmistadCambio != null) OnAmistadCambio((int)nivelAmistad);
			switch (this.name)
			{
				
				case "PNJ_DIEGO": TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|"+this.name+"|fallo"); Debug.Log("user "+ "selected "+ "dialog-tree "+ "machista|"+this.name+"|fallo"); break;
				case "PNJ_NAHUEL": TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|"+this.name+"|fallo"); Debug.Log("user "+ "selected "+ "dialog-tree "+ "machista|"+this.name+"|fallo"); break;
				case "PNJ_ANIBAL": TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|"+this.name+"|fallo"); Debug.Log("user "+ "selected "+ "dialog-tree "+ "machista|"+this.name+"|fallo"); break;
				case "PNJ_SILVANA": TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|"+this.name+"|fallo"); Debug.Log("user "+ "selected "+ "dialog-tree "+ "machista|"+this.name+"|fallo"); break;
				case "PNJ_YIDID": TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|"+this.name+"|fallo"); Debug.Log("user "+ "selected "+ "dialog-tree "+ "machista|"+this.name+"|fallo"); break;
				
				
				default:break;
			}
			return nivelAmistad;
			
		}

		public NivelesAmistad BajarAmistad()
		{
			nivelAmistad = nivelAmistad != NivelesAmistad.DESDEN ? --nivelAmistad : nivelAmistad;
			if (OnAmistadCambio != null) OnAmistadCambio((int)nivelAmistad);
			switch (this.name)
			{
				
				case "PNJ_DON_ARCENIO": TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|"+this.name+"|fallo"); Debug.Log("user "+ "selected "+ "dialog-tree "+ "machista|"+this.name+"|fallo"); break;
				
				default:break;
			}
			return nivelAmistad;
		}

	   



		#region Implementación IGuardable
		public PNJActorDatos GetDatos()
		{
			return new PNJActorDatos(this);
		}
		#endregion

		public void SetFrom(PNJActorDatos datos)
		{
			this.id = datos.id;
			this.nivelAmistad = datos.nivelAmistad;
			this.estadoMachismoActual = datos.nivelMachismo;
			this.flamado = datos.flamado;
			if (!string.IsNullOrEmpty(datos.idSituacionActual))
				this.EstablecerSituacionActiva(datos.idSituacionActual);
			else
				situacionActiva = null;
		}

		#region Clase para guardar
		[System.Serializable]
		public class PNJActorDatos: IEquatable<PNJActorDatos>
		{
			public string id;
			public NivelesAmistad nivelAmistad;
			public string idSituacionActual;
			public PNJDatos.EstadosMachismo nivelMachismo;
			public bool flamado;
			public PNJActorDatos(PNJActor original)
			{
				this.id = original.id;
				this.nivelAmistad = original.nivelAmistad;
				this.nivelMachismo = original.estadoMachismoActual;
				this.flamado = original.flamado;
				if(original.situacionActiva != null)
					if(original.situacionActiva.id != "SIT_")
						this.idSituacionActual = original.situacionActiva.id;
			}

			bool IEquatable<PNJActorDatos>.Equals(PNJActorDatos other)
			{
				return this.id == other.id;
			}
		}
		#endregion
	}

   
}