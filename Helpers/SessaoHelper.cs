namespace SistemaLavanderia.Helpers
{
    public static class SessaoHelper
    {
        public static bool UsuarioLogado(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Session.GetString("UsuarioLogin"));
        }

        public static bool EhAdministrador(HttpContext context)
        {
            return context.Session.GetString("UsuarioPerfil") == "Administrador";
        }

        public static int? ObterUsuarioId(HttpContext context)
        {
            var valor = context.Session.GetString("UsuarioId");

            if (int.TryParse(valor, out int id))
                return id;

            return null;
        }
    }
}