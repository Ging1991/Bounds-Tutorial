using System;
using Ging1991.Dialogos.Persistencia;

namespace Bounds.Dialogos {

	[Serializable]
	public class AccionBounds : IAccionEspecial {

		public string tipo;
		public string texto;
		public string nombre;
		public string imagen;
		public float escalaX;
		public float escalaY;
		public int posX;
		public int posY;
		public int alto;
		public int ancho;
		public bool inmediato;

		public string tipoBounds;
		public int indice;
		public int cartaID;
		public string dueloNombre1;
		public string dueloNombre2;
		public string dueloAvatar1;
		public string dueloAvatar2;
		public string dueloMazo1;
		public string dueloMazo2;
		public int dueloLP1;
		public int dueloLP2;

		public AccionEstandar GetAccionEstandar() {
			if (tipo == "ESPECIAL")
				return null;

			return new AccionEstandar {
				tipo = tipo,
				texto = texto,
				nombre = nombre,
				imagen = imagen,
				escalaX = escalaX,
				escalaY = escalaY,
				posX = posX,
				posY = posY,
				alto = alto,
				ancho = ancho,
				inmediato = inmediato
			};
		}

	}

}