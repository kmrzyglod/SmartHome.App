using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHome.Domain.Entities.Devices.Shared;

namespace SmartHome.Infrastructure.Persistence.Configurations
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasIndex(x => x.DeviceId).IsUnique();
        }
    }
}