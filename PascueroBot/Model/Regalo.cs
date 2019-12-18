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
                        Nombre = "Viaje al caribe",
                        Descripcion = "",
                        UrlImagen = "data:image/jpeg;base64,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",
                        EdadMinima = 1,
                        EdadMaxima = 17,
                        Comportamiento = ComportamientoEnum.Comportamiento.Bien,
                        MensajeRegalo = null
                },
                new Regalo{
                        ID = 2,
                        Nombre = "Viaje a Pichulemu",
                        Descripcion = "",
                        UrlImagen = "https://chile.travel/wp-content/uploads/bfi_thumb/Pichilemu-surf-shutterstock-DST109-mpo6a81og7u8975prqoel1xudfieq5u1qg32g7v8vk.jpg",
                        EdadMinima = 18,
                        EdadMaxima = 34,
                        Comportamiento = ComportamientoEnum.Comportamiento.Bien,
                        MensajeRegalo = null
                },
                new Regalo{
                        ID = 3,
                        Nombre = "Viaje a Pomaire",
                        Descripcion = "",
                        UrlImagen = "https://i3.visitchile.com/img/GalleryContent/572/slider/Pomaire.jpg",
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
