using Bounds.Cartas;
using Bounds.Dialogos;
using Bounds.Modulos.Cartas.Ilustradores;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Musica;
using Bounds.Persistencia;
using Bounds.Persistencia.Parametros;
using Bounds.Persistencia.proveedores;
using Ging1991.Core.Interfaces;
using Ging1991.Dialogos;
using Ging1991.Dialogos.Persistencia;
using Ging1991.Persistencia.Direcciones;
using Ging1991.Persistencia.Lectores;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bounds.Tutorial {

	public class TutorialControl : MonoBehaviour {

		public CartaFisica ejemplo1, ejemplo2, ejemplo3;
		public Dialogo<AccionBounds> dialogo;
		private Configuracion configuracion;
		public InterpreteBounds interprete;
		public ParametrosControl parametrosControl;
		public MusicaDeFondo musicaDeFondo;
		public ControlUIBounds personalizarUI;
		public CartaGenerador cartaGenerador;

		void Start() {
			parametrosControl.Inicializar();
			ParametrosEscena parametros = parametrosControl.parametros;
			personalizarUI.Personalizar(parametros.direcciones["SISTEMA"], parametros.direcciones["COLORES"]);
			configuracion = new(parametros.direcciones["CONFIGURACION"]);
			musicaDeFondo.Inicializar(parametros.direcciones["MUSICA_TIENDA"]);
			IProveedor<int, CartaBD> proveedorCartas = new LectorCartas(new DireccionRecursos(parametrosControl.parametros.direcciones["CARTAS_DATOS"]));

			LectorLista<AccionBounds> lector = new LectorLista<AccionBounds>(
				new DireccionRecursos(parametros.direcciones["TUTORIAL"], $"CAPITULO{configuracion.LeerCapituloLeccion()}").Generar(),
				Ging1991.Persistencia.Lectores.TipoLector.RECURSOS
			);

			dialogo.Inicializar(
				lector.GetLista(),
				new Ging1991.Dialogos.Persistencia.LectorImagenes(new DireccionRecursos("PERSONAJES/MINIATURAS")),
				new Ging1991.Dialogos.Persistencia.LectorImagenes(new DireccionRecursos("PERSONAJES/AVATARES"))
			);

			proveedorCartas = new LectorCartas(new DireccionRecursos(parametrosControl.parametros.direcciones["CARTAS_DATOS"]));

			IlustradorDeCartas ilustradorDeCartas = new IlustradorDeCartas(
				parametrosControl.parametros.direcciones["CARTAS_RECURSO"],
				parametrosControl.parametros.direcciones["CARTAS_DINAMICA"]
			);

			cartaGenerador.Inicializar(
				ilustradorDeCartas,
				proveedorCartas,
				new ProveedorColores(
					parametrosControl.parametros.direcciones["COLORES"],
					TipoLector.RECURSOS
				)
			);

			ejemplo1.cartaImagen.generador = cartaGenerador;
			ejemplo2.cartaImagen.generador = cartaGenerador;
			ejemplo3.cartaImagen.generador = cartaGenerador;

			ejemplo1.ColocarBocaAbajo();
			ejemplo2.ColocarBocaAbajo();
			ejemplo3.ColocarBocaAbajo();
		}


		public void IrMenu() {
			SceneManager.LoadScene("Menu");
		}


		public void IrVideo() {
			string videoId = "RI69hhCVYIc";
			Application.OpenURL("https://www.youtube.com/watch?v=" + videoId);
		}


		public void SaltarLeccion() {
			configuracion.GuardarCapituloLeccion(configuracion.LeerCapituloLeccion() + 1);
			SceneManager.LoadScene("TUTORIAL");
		}


	}

}