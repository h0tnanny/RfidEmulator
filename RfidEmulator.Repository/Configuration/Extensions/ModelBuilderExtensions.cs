using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RfidEmulator.Repository.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var configurations = assembly.GetTypes()
            .Where(type =>
                type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .Select(Activator.CreateInstance)
            .ToList();

        foreach (dynamic? configuration in configurations)
        {
            modelBuilder.ApplyConfiguration(configuration);
        }
    }
}