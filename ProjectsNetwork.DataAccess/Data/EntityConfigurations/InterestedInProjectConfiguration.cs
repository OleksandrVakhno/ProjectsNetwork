using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Data.EntityConfigurations
{
    public class InterestedInProjectConfiguration : IEntityTypeConfiguration<InterestedInProject>
    {
        public void Configure(EntityTypeBuilder<InterestedInProject> builder)
        {
            builder.HasKey(x => new { x.UserId, x.ProjectId });
        }
    }
}
