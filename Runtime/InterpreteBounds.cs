using Bounds.Cartas;
using Bounds.Infraestructura;
using Bounds.Infraestructura.Constantes;
using Bounds.Mazos;
using Bounds.Modulos.Cartas.Ilustradores;
using Bounds.Modulos.Cartas.Persistencia.Datos;
using Bounds.Persistencia;
using Ging1991.Core.Interfaces;
using Ging1991.Dialogos.Interpretes;
using Ging1991.Persistencia.Direcciones;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bounds.Dialogos {

	public class InterpreteBounds : Interprete<AccionBounds> {

		public CartaFisica ejemplo1, ejemplo2, ejemplo3;
		public DueloConstantes.Modo modoDuelo;
		public string modoDueloCadena;
		public IlustradorDeCartas ilustradorDeCartas;

		public override bool InterpretarAccionEspecial(AccionBounds accion) {

			if (accion.tipoBounds == "SET_CARTA_IMG") {
				SetImagenCarta(accion.indice, accion.cartaID);
			}

			if (accion.tipoBounds == "FINAL_HISTORIA") {
				Configuracion configuracion = new Configuracion(new DireccionDinamica("CONFIGURACION", "CONFIGURACION.json").Generar());
				configuracion.GuardarCapituloHistoria(1);
				ControlEscena.GetInstancia().CambiarEscena_menu();
			}

			if (accion.tipoBounds == "DUELO") {
				JugarDuelo(
					accion.dueloNombre1, accion.dueloAvatar1, accion.dueloLP1, accion.dueloMazo1,
					accion.dueloNombre2, accion.dueloAvatar2, accion.dueloLP2, accion.dueloMazo2);
				SetImagenCarta(accion.indice, accion.cartaID);
			}
			return accion.inmediato;
		}


		private void SetImagenCarta(int indice, int cartaID) {
			if (indice == 1) {
				ejemplo1.ColocarBocaArriba();
				ejemplo1.cartaImagen.MostrarCartaID(cartaID);
			}
			if (indice == 2) {
				ejemplo2.ColocarBocaArriba();
				ejemplo2.cartaImagen.MostrarCartaID(cartaID);
			}
			if (indice == 3) {
				ejemplo3.ColocarBocaArriba();
				ejemplo3.cartaImagen.MostrarCartaID(cartaID);
			}
		}

		private void JugarDuelo(
				string nombre1, string miniatura1, int LP1, string nombreMazo1,
				string nombre2, string miniatura2, int LP2, string nombreMazo2) {

			GlobalDuelo parametros = GlobalDuelo.GetInstancia();
			parametros.modo = modoDueloCadena;
			parametros.jugadorLP1 = LP1;
			parametros.jugadorLP2 = LP2;
			parametros.jugadorNombre1 = nombre1;
			parametros.jugadorNombre2 = nombre2;
			parametros.jugadorMiniatura1 = miniatura1;
			parametros.jugadorMiniatura2 = miniatura2;

			Mazo mazo1 = null;
			Mazo mazo2 = null;
			if (modoDuelo == DueloConstantes.Modo.TUTORIAL) {
				mazo1 = new MazoRecursos(new DireccionRecursos("MAZOS/TUTORIAL", nombreMazo1).Generar());
				mazo2 = new MazoRecursos(new DireccionRecursos("MAZOS/TUTORIAL", nombreMazo2).Generar());
				parametros.finalizarDuelo = new FinTutorial();
			}
			if (modoDuelo == DueloConstantes.Modo.HISTORIA) {
				mazo1 = new MazoRecursos(new DireccionRecursos("MAZOS/HISTORIA", nombreMazo1).Generar());
				mazo2 = new MazoRecursos(new DireccionRecursos("MAZOS/HISTORIA", nombreMazo2).Generar());
				parametros.finalizarDuelo = new FinHistoria();
			}
			CartaMazo vacioReal = new CartaMazo("0_A_N_1");
			parametros.mazo1 = mazo1.cartas;
			parametros.mazo2 = mazo2.cartas;
			parametros.mazoVacio1 = mazo1.principalVacio == null ? vacioReal : mazo1.principalVacio;
			parametros.mazoVacio2 = mazo2.principalVacio == null ? vacioReal : mazo2.principalVacio;
			parametros.protector1 = mazo1.protector;
			parametros.protector2 = mazo2.protector;

			SceneManager.LoadScene("DUELO");
		}

		public class FinTutorial : IFinalizarDuelo {
			public void FinalizarDuelo(int jugadorGanador) {
				if (jugadorGanador == 1) {
					Configuracion configuracion = new Configuracion(new DireccionDinamica("CONFIGURACION", "CONFIGURACION.json").Generar());
					Billetera billetera = new(new DireccionDinamica("CONFIGURACION", "BILLETERA.json").Generar());
					int capituloActual = configuracion.LeerCapituloLeccion();
					configuracion.GuardarCapituloLeccion(capituloActual + 1);
					billetera.GanarOro(500);
				}
				ControlEscena escena = ControlEscena.GetInstancia();
				escena.CambiarEscena("TUTORIAL");
			}

		}


		public class FinHistoria : IFinalizarDuelo {
			public void FinalizarDuelo(int jugadorGanador) {
				if (jugadorGanador == 1) {
					Configuracion configuracion = new Configuracion(new DireccionDinamica("CONFIGURACION", "CONFIGURACION.json").Generar());
					Billetera billetera = new(new DireccionDinamica("CONFIGURACION", "BILLETERA.json").Generar());
					billetera.GanarOro(500);
					int capituloActual = configuracion.LeerCapituloHistoria();
					configuracion.GuardarCapituloHistoria(capituloActual + 1);
				}
				ControlEscena escena = ControlEscena.GetInstancia();
				escena.CambiarEscena("HISTORIA");
			}

		}

	}

}