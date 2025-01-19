using System.Reflection;

namespace ChatApplication.Server.Domain.DTOs.Common
{
    public static class ApplicationAssemblyReference
    {
        public static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
    }
}
