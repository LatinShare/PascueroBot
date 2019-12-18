using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PascueroBotSpace.Model
{
    public class Regalo
    {
        public Regalo()
        {

        }

        public Regalo(int id, string nombre, int edadMinima, int edadMaxima, string urlImagen, string descripcion, ComportamientoEnum.Comportamiento comportamiento, string mensajeRegalo)
        {
            ID = id;
            Nombre = nombre;
            EdadMinima = edadMinima;
            EdadMaxima = edadMaxima;
            UrlImagen = urlImagen;
            Descripcion = descripcion;
            Comportamiento = comportamiento;
            MensajeRegalo = mensajeRegalo ?? "";
        }

        public int ID { get; set; }
        public string Nombre { get; set; }
        public int EdadMaxima { get; set; }
        public int EdadMinima { get; set; }
        public string UrlImagen { get; set; }
        public string Descripcion { get; set; }
        public ComportamientoEnum.Comportamiento Comportamiento { get; set; }
        public string MensajeRegalo { get; set; }




    }
    public class Regalos
    {
        public Regalos()
        {
            this.regalos = new Regalo[] {
                new Regalo{
                        ID = 1,
                        Nombre = "Perro",
                        Descripcion = "",
                        UrlImagen = "https://static.boredpanda.com/blog/wp-content/uploads/2019/10/dog-thoughts-tweets-1.jpg",
                        EdadMinima = 1,
                        EdadMaxima = 17,
                        Comportamiento = ComportamientoEnum.Comportamiento.Bien,
                        MensajeRegalo = null
                },
                new Regalo{
                        ID = 2,
                        Nombre = "Gato",
                        Descripcion = "",
                        UrlImagen = "https://images-na.ssl-images-amazon.com/images/I/61YDY3QPTRL._SL1000_.jpg",
                        EdadMinima = 18,
                        EdadMaxima = 34,
                        Comportamiento = ComportamientoEnum.Comportamiento.Bien,
                        MensajeRegalo = null
                },
                new Regalo{
                        ID = 3,
                        Nombre = "Planta de tomates",
                        Descripcion = "Ideal para prepaparse un rico causeo",
                        UrlImagen = "http://laguaridadelfumeta.life/wp-content/uploads/2015/09/tomates.jpg",
                        EdadMinima = 35,
                        EdadMaxima = int.MaxValue,
                        Comportamiento = ComportamientoEnum.Comportamiento.Bien,
                        MensajeRegalo = null
                },
                //Regular
                new Regalo{
                        ID = 1,
                        Nombre = "Perro",
                        Descripcion = "",
                        UrlImagen = "https://static.boredpanda.com/blog/wp-content/uploads/2019/10/dog-thoughts-tweets-1.jpg",
                        EdadMinima = 1,
                        EdadMaxima = 17,
                        Comportamiento = ComportamientoEnum.Comportamiento.Regular,
                        MensajeRegalo = null
                },
                new Regalo{
                        ID = 2,
                        Nombre = "Gato",
                        Descripcion = "",
                        UrlImagen = "https://images-na.ssl-images-amazon.com/images/I/61YDY3QPTRL._SL1000_.jpg",
                        EdadMinima = 18,
                        EdadMaxima = 34,
                        Comportamiento = ComportamientoEnum.Comportamiento.Regular,
                        MensajeRegalo = null
                },
                new Regalo{
                        ID = 3,
                        Nombre = "Planta de tomates",
                        Descripcion = "",
                        UrlImagen = "http://laguaridadelfumeta.life/wp-content/uploads/2015/09/tomates.jpg",
                        EdadMinima = 35,
                        EdadMaxima = int.MaxValue,
                        Comportamiento = ComportamientoEnum.Comportamiento.Regular,
                        MensajeRegalo = null
                },
                // Mal
                new Regalo{
                        ID = 1,
                        Nombre = "Carbón",
                        Descripcion = "",
                        UrlImagen = "https://previews.123rf.com/images/patrickhastings/patrickhastings1210/patrickhastings121000010/15538322-sweet-coal-in-christmas-stocking-carbon-de-reyes.jpg",
                        EdadMinima = 1,
                        EdadMaxima = int.MaxValue,
                        Comportamiento = ComportamientoEnum.Comportamiento.Mal,
                        MensajeRegalo = null
                }


            };
        }
        public Regalo[] regalos { get; set; }
    }
}
