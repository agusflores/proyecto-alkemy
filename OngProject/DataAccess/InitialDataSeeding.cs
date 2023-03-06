using Microsoft.EntityFrameworkCore;
using OngProject.Entities;
using System;

namespace OngProject.DataAccess
{
    public static class InitialDataSeeding
    {
        public static void Seeding(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasData(
                   new Category { Id = 1, Name = "category1", Image = "url img", Description = "description category1", IsDeleted = false, LastModified = DateTime.UtcNow },
                   new Category { Id = 2, Name = "category2", Image = "url img", Description = "description category2", IsDeleted = false, LastModified = DateTime.UtcNow },
                   new Category { Id = 3, Name = "category3", Image = "url img", Description = "description category3", IsDeleted = false, LastModified = DateTime.UtcNow },
                   new Category { Id = 4, Name = "category4", Image = "url img", Description = "description category4", IsDeleted = false, LastModified = DateTime.UtcNow },
                   new Category { Id = 5, Name = "category5", Image = "url img", Description = "description category5", IsDeleted = false, LastModified = DateTime.UtcNow }
                );



            modelBuilder.Entity<News>()
                .HasData(
                    new News { Id = 1, Name = "News01", Image = "url_img", Content = "this is a content - news 01", CategoryId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new News { Id = 2, Name = "News02", Image = "url_img", Content = "this is a content - news 02", CategoryId = 3, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new News { Id = 3, Name = "News03", Image = "url_img", Content = "this is a content - news 03", CategoryId = 3, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new News { Id = 4, Name = "News04", Image = "url_img", Content = "this is a content - news 04", CategoryId = 1, IsDeleted = false, LastModified = DateTime.UtcNow }
                );

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role { Id = 1, Name = "Admin", Description = "System Administrator", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Role { Id = 2, Name = "User", Description = "System User", IsDeleted = false, LastModified = DateTime.UtcNow }
                );


            modelBuilder.Entity<Members>()
                .HasData(
                    new Members { Id = 1, Name = "Members01", Image = "url_image_members01", Description = "Description members01", FacebookUrl = "url_facebook_members01", InstagramUrl = "url_instagram_members01", LinkedinUrl = "url_linkedIn_members01", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Members { Id = 2, Name = "Members02", Image = "url_image_members02", Description = "Description members02", FacebookUrl = "url_facebook_members02", InstagramUrl = "url_instagram_members02", LinkedinUrl = "url_linkedIn_members02", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Members { Id = 3, Name = "Members03", Image = "url_image_members03", Description = "Description members03", FacebookUrl = "url_facebook_members03", InstagramUrl = "url_instagram_members03", LinkedinUrl = "url_linkedIn_members03", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Members { Id = 4, Name = "Members04", Image = "url_image_members04", Description = "Description members04", FacebookUrl = "url_facebook_members04", InstagramUrl = "url_instagram_members04", LinkedinUrl = "url_linkedIn_members04", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Members { Id = 5, Name = "Members05", Image = "url_image_members05", Description = "Description members05", FacebookUrl = "url_facebook_members05", InstagramUrl = "url_instagram_members05", LinkedinUrl = "url_linkedIn_members05", IsDeleted = false, LastModified = DateTime.UtcNow }
              );

            modelBuilder.Entity<Organization>()
                .HasData(
                    new Organization { Id = 1, Name = "Organization 1", Email = "Email1@mail.com", Address = "Address 1", WelcomeText = "WelcomeText 1", AboutUsText = "AboutUsText 1", Image = "image_1", FacebookUrl = "facebook_url_1", InstagramUrl = "instagram_url_1", LinkedinUrl = "linkedin_url_1", Phone = 11111111, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Organization { Id = 2, Name = "Organization 2", Email = "Email2@mail.com", Address = "Address 2", WelcomeText = "WelcomeText 2", AboutUsText = "AboutUsText 2", Image = "image_2", FacebookUrl = "facebook_url_2", InstagramUrl = "instagram_url_2", LinkedinUrl = "linkedin_url_2", Phone = 22222222, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Organization { Id = 3, Name = "Organization 3", Email = "Email3@mail.com", Address = "Address 3", WelcomeText = "WelcomeText 3", AboutUsText = "AboutUsText 3", Image = "image_3", FacebookUrl = "facebook_url_3", InstagramUrl = "instagram_url_3", LinkedinUrl = "linkedin_url_3", Phone = 33333333, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Organization { Id = 4, Name = "Organization 4", Email = "Email4@mail.com", Address = "Address 4", WelcomeText = "WelcomeText 4", AboutUsText = "AboutUsText 4", Image = "image_4", FacebookUrl = "facebook_url_4", InstagramUrl = "instagram_url_4", LinkedinUrl = "linkedin_url_4", Phone = 44444444, IsDeleted = false, LastModified = DateTime.UtcNow }
                );

            modelBuilder.Entity<Activities>()
                .HasData(
                    new Activities { Id = 1, Name = "Activities 01", Image = "url_img", Content = "this is a content - activities 01", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Activities { Id = 2, Name = "Activities 02", Image = "url_img", Content = "this is a content - activities 02", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Activities { Id = 3, Name = "Activities 03", Image = "url_img", Content = "this is a content - activities 03", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Activities { Id = 4, Name = "Activities 04", Image = "url_img", Content = "this is a content - activities 04", IsDeleted = false, LastModified = DateTime.UtcNow }

                );

            modelBuilder.Entity<Testimonials>()
                .HasData(
                    new Testimonials { Id = 1, Name = "Testimonials01", Image = "url_image_testimonials01", Content = "Content testimonials01", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Testimonials { Id = 2, Name = "Testimonials02", Image = "url_image_testimonials02", Content = "Content testimonials02", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Testimonials { Id = 3, Name = "Testimonials03", Image = "url_image_testimonials03", Content = "Content testimonials03", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Testimonials { Id = 4, Name = "Testimonials04", Image = "url_image_testimonials04", Content = "Content testimonials04", IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Testimonials { Id = 5, Name = "Testimonials05", Image = "url_image_testimonials05", Content = "Content testimonials05", IsDeleted = false, LastModified = DateTime.UtcNow }
                );

            modelBuilder.Entity<Contacts>()
                .HasData(
                    new Contacts { Id = 1, Name = "ContactName01", Email = "contact01@mail.com", Message = "message01", Phone = 11111111, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Contacts { Id = 2, Name = "ContactName02", Email = "contact02@mail.com", Message = "message02", Phone = 22222222, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Contacts { Id = 3, Name = "ContactName03", Email = "contact03@mail.com", Message = "message03", Phone = 33333333, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Contacts { Id = 4, Name = "ContactName04", Email = "contact04@mail.com", Message = "message04", Phone = 44444444, IsDeleted = false, LastModified = DateTime.UtcNow }
                );


            // passwords = user01password, user02password, user03password, user04password, user05password, admin1password
            modelBuilder.Entity<User>()
                .HasData(
                    new User { Id = 1, FirstName = "UserFirstName01", LastName = "UserLastName01", Email = "user01@email.com", Password = "855f48ff90dfd663decf85f9d1423d05fe84c4ad84287b4a328d248aa44bbaeb", Photo = "user01photo.jpg", RoleId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new User { Id = 2, FirstName = "UserFirstName02", LastName = "UserLastName02", Email = "user02@email.com", Password = "3fb52b701d90223d8d103666adcc20c9e5b403ad15feaddc4523c67badb9abde", Photo = "user02photo.jpg", RoleId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new User { Id = 3, FirstName = "UserFirstName03", LastName = "UserLastName03", Email = "user03@email.com", Password = "806e06b9ba12aa0ec23c7a9acbc73edc42acc207bc1cfcd258d1746e011376dc", Photo = "user03photo.jpg", RoleId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new User { Id = 4, FirstName = "UserFirstName04", LastName = "UserLastName04", Email = "user04@email.com", Password = "b7ede7c12d6324f339cd49509fdfbeefde2ccecdaa53ea2f274602ff37280e01", Photo = "user04photo.jpg", RoleId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new User { Id = 5, FirstName = "UserFirstName05", LastName = "UserLastName05", Email = "user5@email.com", Password = "af43cca886229cc4ab166a723e7ca9e3834b9f4747f055cc61eca0fa09f35d08", Photo = "user05photo.jpg", RoleId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new User { Id = 6, FirstName = "AdminFirstName01", LastName = "AdminLastName01", Email = "admin1@email.com", Password = "e2f13fb5bc47424d6b27b3ac1c605d33f1f598c1db269b044c3f59338d1e583f", Photo = "user05photo.jpg", RoleId = 1, IsDeleted = false, LastModified = DateTime.UtcNow }
                );

            modelBuilder.Entity<Slides>()
                .HasData(
                    new Slides { Id = 1, Text = "Lorem ipsum lorem ipsum 1", ImageUrl = "www.sadasd.asdasd 1", Order = 1, OrganizationId = 1, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Slides { Id = 2, Text = "Lorem ipsum lorem ipsum 2", ImageUrl = "www.sadasd.asdasd 2", Order = 2, OrganizationId = 2, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Slides { Id = 3, Text = "Lorem ipsum lorem ipsum 3", ImageUrl = "www.sadasd.asdasd 3", Order = 3, OrganizationId = 3, IsDeleted = false, LastModified = DateTime.UtcNow },
                    new Slides { Id = 4, Text = "Lorem ipsum lorem ipsum 4", ImageUrl = "www.sadasd.asdasd 4", Order = 4, OrganizationId = 4, IsDeleted = false, LastModified = DateTime.UtcNow }
                );

            modelBuilder.Entity<Comments>()
               .HasData(
                   new Comments { Id = 1, UserId = 1, Body = "Body Comment 1", NewsId = 1, IsDeleted = false, LastModified = DateTime.UtcNow },
                   new Comments { Id = 2, UserId = 1, Body = "Body Comment 2", NewsId = 1, IsDeleted = false, LastModified = DateTime.UtcNow },
                   new Comments { Id = 3, UserId = 2, Body = "Body Comment 3", NewsId = 1, IsDeleted = false, LastModified = DateTime.UtcNow }
               );
        }
    }
}