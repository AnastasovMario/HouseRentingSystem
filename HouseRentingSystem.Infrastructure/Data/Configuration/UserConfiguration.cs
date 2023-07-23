using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Data.Configuration
{
	public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			//Подаваме списък с CreateUsers, и HasData го сийдва
			builder.HasData(CreateUsers());
		}

		private List<ApplicationUser> CreateUsers()
		{
			var users = new List<ApplicationUser>();
			var hasher = new PasswordHasher<ApplicationUser>();

			var user = new ApplicationUser()
			{
				Id = "dea12856-c198-4129-b3f3-b893d8395082",
				UserName = "agent@mail.com",
				NormalizedUserName = "agent@mail.com",
				Email = "agent@mail.com",
				NormalizedEmail = "agent@mail.com",
				FirstName = "Linda",
				LastName = "Michaels",
                IsActive = true
            };

			user.PasswordHash =
				 hasher.HashPassword(user, "agent123");

			users.Add(user);

			user= new ApplicationUser()
			{
				Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
				UserName = "guest@mail.com",
				NormalizedUserName = "guest@mail.com",
				Email = "guest@mail.com",
				NormalizedEmail = "guest@mail.com",
                FirstName = "Guest",
                LastName = "Guestov",
                IsActive = true
            };

			user.PasswordHash =
			hasher.HashPassword(user, "guest123");

			users.Add(user);

            user = new ApplicationUser()
            {
                Id = "6d4200ce-d726-4fc8-83d9-d6b3ac1f591e",
                UserName = "mario@mail.com",
                NormalizedUserName = "mario@mail.com",
                Email = "mario@mail.com",
                NormalizedEmail = "mario@mail.com",
                FirstName = "Mario",
                LastName = "Anastasov",
				IsActive = true
            };

            user.PasswordHash =
            hasher.HashPassword(user, "mario123");

            users.Add(user);

            return users;
		}

	}
}
