using Mapster;
using RfidEmulator.Domain.DTOs;
using RfidEmulator.Domain.Entity;

namespace RfidEmulator.Api.ServiceExtensions;

public static class MapsterConfig
{
    public static IServiceCollection  RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<ReaderDto, Reader>
            .NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.Config, src => src.Config)
            .Map(dest => dest.Antennas, src => src.Antennas);

        TypeAdapterConfig<ReaderConfigDto, ReaderConfig>
            .NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid());
        
        TypeAdapterConfig<AntennaDto, Antenna>
            .NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid());
        
        return services;
    }
}