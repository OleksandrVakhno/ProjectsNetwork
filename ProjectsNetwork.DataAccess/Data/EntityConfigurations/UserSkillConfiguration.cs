using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectsNetwork.Models;


namespace ProjectsNetwork.DataAccess.Data.EntityConfigurations
{
    public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
    {
        public void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.HasKey(x => new { x.UserId, x.SkillId });

        }
    }
}
