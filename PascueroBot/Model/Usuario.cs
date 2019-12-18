
namespace PascueroBotSpace.Model
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string IdUsuario { get; set; }
        public string TenantId { get; set; }
        public bool RegaloSolicitado { get; set; }
        public int Edad { get; set; }
        public ComportamientoEnum.Comportamiento Comportamiento { get; set; }
    }
}
