using Bounds.Dialogos;
using Bounds.Modulos.Cartas;
using Bounds.Modulos.Cartas.Persistencia;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Modulos.Persistencia;
using Bounds.Musica;
using Bounds.Persistencia;
using Bounds.Persistencia.Parametros;
using Ging1991.Core.Interfaces;
using Ging1991.Dialogos;
using Ging1991.Dialogos.Persistencia;
using Ging1991.Persistencia.Direcciones;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bounds.Tutorial {

	public class TutorialControl : MonoBehaviour {

		public GameObject cartaEjemplo1, cartaEjemplo2, cartaEjemplo3;
		public Dialogo<AccionBounds> dialogo;
		private Configuracion configuracion;
		public InterpreteBounds interprete;
		public ParametrosControl parametrosControl;
		public MusicaDeFondo musicaDeFondo;

		void Start() {
			parametrosControl.Inicializar();
			ParametrosEscena parametros = parametrosControl.parametros;
			configuracion = new(parametros.direcciones["CONFIGURACION"]);
			musicaDeFondo.Inicializar(parametros.direcciones["MUSICA_TIENDA"]);
			IProveedor<int, CartaBD> proveedorCartas = new LectorCartas(new DireccionRecursos(parametrosControl.parametros.direcciones["CARTAS_DATOS"]));

			interprete.InicializarCartas(proveedorCartas);

			LectorLista<AccionBounds> lector = new LectorLista<AccionBounds>(
				new DireccionRecursos("TUTORIAL", $"CAPITULO{configuracion.LeerCapituloLeccion()}").Generar(),
				Ging1991.Persistencia.Lectores.TipoLector.RECURSOS
			);

			dialogo.Inicializar(
				lector.GetLista(),
				new LectorImagenes(new DireccionRecursos("PERSONAJES/MINIATURAS")),
				new LectorImagenes(new DireccionRecursos("PERSONAJES/AVATARES"))
			);

			cartaEjemplo1.GetComponent<CartaFisica>().ColocarBocaAbajo();
			cartaEjemplo2.GetComponent<CartaFisica>().ColocarBocaAbajo();
			cartaEjemplo3.GetComponent<CartaFisica>().ColocarBocaAbajo();

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