using CV_Website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CV_Website.Data
{
public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var context = serviceProvider.GetRequiredService<CVContext>();

            //Börjar med att seeda användare, om det går bra börjar den seeda resten
            try
            {
                await SeedUsersAsync(userManager, logger);
                await SeedRelationshipsAsync(context, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedUsersAsync(UserManager<User> userManager, ILogger logger)
        {
            var users = new[]
            {
                new User { UserName = "alice@example.com", Email = "alice@example.com", PhoneNumber = "+46701234567", Name = "Alice", Address = "Drottninggatan 10", Private = false },
                new User { UserName = "bob@example.com", Email = "bob@example.com", PhoneNumber = "0701234567", Name = "Bob", Address = "Storgatan 5", Private = true },
                new User { UserName = "charlie@example.com", Email = "charlie@example.com", PhoneNumber = "+46761234567", Name = "Charlie", Address = "Kungsgatan 18", Private = false },
                new User { UserName = "dave@example.com", Email = "dave@example.com", PhoneNumber = "0761234567", Name = "Dave", Address = "Östra Långgatan 22", Private = false },
                new User { UserName = "eve@example.com", Email = "eve@example.com", PhoneNumber = "+46731234567", Name = "Eve", Address = "Västra Vallgatan 4", Private = true },
                new User { UserName = "frank@example.com", Email = "frank@example.com", PhoneNumber = "0731234567", Name = "Frank", Address = "Kyrkogatan 8", Private = false },
                new User { UserName = "grace@example.com", Email = "grace@example.com", PhoneNumber = "0707654321", Name = "Grace", Address = "Långholmsgatan 12", Private = false },
                new User { UserName = "hannah@example.com", Email = "hannah@example.com", PhoneNumber = "0738765432", Name = "Hannah", Address = "Mäster Samuelsgatan 3", Private = true },
                new User { UserName = "ian@example.com", Email = "ian@example.com", PhoneNumber = "0765432109", Name = "Ian", Address = "Hornsgatan 9", Private = false },
                new User { UserName = "julia@example.com", Email = "julia@example.com", PhoneNumber = "0709876543", Name = "Julia", Address = "Tegelbacken 15", Private = true }
            };

            foreach (var user in users)
            {
                try
                {
                    //Kollar så att användaren inte finns
                    if (await userManager.FindByEmailAsync(user.Email) == null)
                    {
                        var result = await userManager.CreateAsync(user, "DefaultPassword123!");
                        if (result.Succeeded)
                        {
                            logger.LogInformation($"User {user.Email} created successfully.");
                        }
                        else
                        {
                            logger.LogWarning($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while creating user {user.Email}");
                }
            }
        }

        private static async Task SeedRelationshipsAsync(CVContext context, ILogger logger)
        {
            try
            {
                if (!context.Skills.Any() && !context.Experience.Any() && !context.CVs.Any())
                {
                    var skills = new[]
                    {
                        new Skills { Name = "C#" },
                        new Skills { Name = "ASP.NET" },
                        new Skills { Name = "JavaScript" },
                        new Skills { Name = "SQL" },
                        new Skills { Name = "Python" },
                        new Skills { Name = "HTML & CSS" },
                        new Skills { Name = "React" },
                        new Skills { Name = "Angular" },
                        new Skills { Name = "Node.js" },
                        new Skills { Name = "Docker" },
                        new Skills { Name = "Kubernetes" }
                    };

                    var experiences = new[]
                    {
                        new Experience { Name = "Software Developer" },
                        new Experience { Name = "Full Stack Developer" },
                        new Experience { Name = "Data Analyst" },
                        new Experience { Name = "DevOps Engineer" },
                        new Experience { Name = "UI/UX Designer" },
                        new Experience { Name = "Mobile App Developer" },
                        new Experience { Name = "Machine Learning Engineer" }
                    };

                    var educations = new[]
                    {
                        new Education { Name = "Computer Science" },
                        new Education { Name = "Information Technology" },
                        new Education { Name = "Software Engineering" },
                        new Education { Name = "Data Science" },
                        new Education { Name = "Cyber Security" }
                    };

                    var cvs = new[]
                    {
                        new CV { UserId = 1, Skills = new List<Skills> { skills[0], skills[1] }, Experience = new List<Experience> { experiences[0] }, Education = new List<Education> { educations[0] } },
                        new CV { UserId = 2, Skills = new List<Skills> { skills[2], skills[3] }, Experience = new List<Experience> { experiences[1] }, Education = new List<Education> { educations[1] } },
                        new CV { UserId = 3, Skills = new List<Skills> { skills[4], skills[5] }, Experience = new List<Experience> { experiences[2] }, Education = new List<Education> { educations[2] } },
                        new CV { UserId = 4, Skills = new List<Skills> { skills[6], skills[7] }, Experience = new List<Experience> { experiences[3] }, Education = new List<Education> { educations[3] } },
                        new CV { UserId = 5, Skills = new List<Skills> { skills[8], skills[9] }, Experience = new List<Experience> { experiences[4] }, Education = new List<Education> { educations[4] } },
                        new CV { UserId = 6, Skills = new List<Skills> { skills[10] }, Experience = new List<Experience> { experiences[5] }, Education = new List<Education> { educations[0] } },
                        new CV { UserId = 7, Skills = new List<Skills> { skills[0], skills[2] }, Experience = new List<Experience> { experiences[6] }, Education = new List<Education> { educations[1] } },
                        new CV { UserId = 8, Skills = new List<Skills> { skills[1], skills[3] }, Experience = new List<Experience> { experiences[0] }, Education = new List<Education> { educations[2] } },
                        new CV { UserId = 9, Skills = new List<Skills> { skills[4], skills[5] }, Experience = new List<Experience> { experiences[1] }, Education = new List<Education> { educations[3] } },
                        new CV { UserId = 10, Skills = new List<Skills> { skills[6], skills[7] }, Experience = new List<Experience> { experiences[2] }, Education = new List<Education> { educations[4] } }
                    };


                    var projects = new[]
                    {
                        new Project { Title = "Portfolio Website", Description = "A showcase of personal projects.", CreatorId = 1, Users = new List<User> { context.Users.Find(1), context.Users.Find(2) } },
                        new Project { Title = "E-Commerce Platform", Description = "An online shopping platform.", CreatorId = 2, Users = new List<User> { context.Users.Find(3), context.Users.Find(4) } },
                        new Project { Title = "AI Chatbot", Description = "A chatbot powered by AI.", CreatorId = 3, Users = new List<User> { context.Users.Find(5), context.Users.Find(6) } },
                        new Project { Title = "Fitness App", Description = "An app for tracking fitness.", CreatorId = 4, Users = new List<User> { context.Users.Find(7), context.Users.Find(8) } }
                    };


                    var messages = new[]
                    {
                        new Message { SenderId = 1, ReceiverId = 2, SenderName = "Alice", MessageText = "Hi Bob, how are you?", Read = true },
                        new Message { SenderId = 2, ReceiverId = 1, SenderName = "Bob", MessageText = "I'm good, thanks Alice!", Read = false },
                        new Message { SenderId = 3, ReceiverId = 4, SenderName = "Charlie", MessageText = "Hey Dave, how's the project?", Read = true },
                        new Message { SenderId = 4, ReceiverId = 3, SenderName = "Dave", MessageText = "It's going well, thanks Charlie!", Read = true },
                        new Message { SenderId = 5, ReceiverId = 6, SenderName = "Eve", MessageText = "Frank, can you share the updates?", Read = false },
                        new Message { SenderId = 6, ReceiverId = 5, SenderName = "Frank", MessageText = "Sure, I'll send them over.", Read = true },
                        new Message { SenderId = 7, ReceiverId = 8, SenderName = "Grace", MessageText = "Hannah, did you finish the report?", Read = false },
                        new Message { SenderId = 8, ReceiverId = 7, SenderName = "Hannah", MessageText = "Yes, I'll email it to you.", Read = true },
                        new Message { SenderId = 9, ReceiverId = 10, SenderName = "Ian", MessageText = "Julia, your designs are great!", Read = false },
                        new Message { SenderId = 10, ReceiverId = 9, SenderName = "Julia", MessageText = "Thanks Ian!", Read = true }
                    };

                    context.Skills.AddRange(skills);
                    context.Experience.AddRange(experiences);
                    context.Education.AddRange(educations);
                    context.CVs.AddRange(cvs);
                    context.Project.AddRange(projects);
                    context.Messages.AddRange(messages);

                    await context.SaveChangesAsync();
                    logger.LogInformation("Relationships seeded successfully.");
                }
                else
                {
                    logger.LogInformation("Database already seeded.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding relationships.");
                throw;
            }
        }
    }
}
