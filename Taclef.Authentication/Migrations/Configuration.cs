using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Taclef.Authentication.LoginProviders;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Taclef.Authentication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Taclef.Authentication.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

	        context.Database.ExecuteSqlCommand(
		        "ALTER TABLE LoginProviders ALTER COLUMN DerivedKey nvarchar(max) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL");
			context.Database.ExecuteSqlCommand(
				"ALTER TABLE ConsumerApplications ALTER COLUMN SecretKey nvarchar(max) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
			context.Database.ExecuteSqlCommand(
				"ALTER TABLE UserLogins ALTER COLUMN Credentials nvarchar(max) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL");


			context.LoginProviders.AddOrUpdate(
				p => p.Name,
				new LoginProvider
				{
					Name = "d2l-cforp", 
					DisplayName = "D2L CFORP",
					DerivedKey = Serializer.SerializeToBase64(new Desire2LearnDerivedKey { TcProfileUrl = "https://cforp.desire2learn.com/d2l/api/ext/1.0/lti/tcservices" }),
					ProviderType = "D2L-LTI",
					WebsiteUrl = "https://cforp.desire2learn.com/",
					Uuid = Guid.Parse("7C701197-EBB9-416B-B951-65C0C4AAE8C2")
				}
				);

			context.ConsumerApplications.AddOrUpdate(
				p => p.Name,
				new ConsumerApplication
				{
					Name = "taclef-quiz",
					DisplayName = "Auto-formation TACLEF",
					SecretKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPnYxLzlraWlvREwyWFE0cHd1KzI3UUR2NVpRek5JRVdRWks1Vzk0SjV4aDdyMFdkQ2RzUFJNNldabVpDcFR2clB5OVNQaTVEQlNya0FMWG9pMkkxWmhHM3I4Z3MweEFCcUtCSG1tc0N4eUZPMEVGZmhGSjR5SzBMeGdMRm1vWmROTFpIK2hheUMrNDBNYWUwZkllWVZ2Vk9hY2NpaXJqa0tpWWtqMktRVnhKaz08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPiszUHlrYkFmbnczS0FNRkdPTWVWR09rYWRxQnBKdVBYcWFwWDJ6d0hRZ1h1Ky8vMXk5dlRETmpiN0JCSzFVL1N4bHloV3QrRjZkWEVhMks5Yi85Njh3PT08L1A+PFE+d3RYc2VqWU9jSmkxSlA5eUpaMS82VnpyRHFnczc1R3JJc1RiM1B0QXo5SXY5YWh2a1BQZHJ6K0hXeDdaVlRzeW1kRTNmYmlaWUc5Ri9zbkRMeXpOUXc9PTwvUT48RFA+eUZmQlZNSjI3MHNWUHpTK3RrNk9SS0sxS3Y3enlNQ1ArK1oweVlBeWFEQkNFUllHeUw0RWtaL1cyV2JCdU9NNGxFTUpiRlhiWkV1bGk4bklGWXdBY1E9PTwvRFA+PERRPmNKTkFsd0NpNlVZaTdHUzRwS2xUWGhnMUxQdlpNdmRkQmJMdlFzamVFdlpraGJHei9QSWJOZ1lrQkc5WHJocU5TemtLc2hhMmNIVDY0WlBjdG1aUmx3PT08L0RRPjxJbnZlcnNlUT5BQVVlK09JRThwMTB2VjQ0UjkvdHJBd1VDeGlUenB6SnRXY0wxMHR5cEVqdnB1NWM4S0ZWVmpZZTFGWk9LQnlWT24yYmJTSU1WK3VVUWlsODlLeVFPZz09PC9JbnZlcnNlUT48RD5QTExpOStFNlQvUXdUb1ZSOVdGVlEyM2lUOGtzSklDNzZCc2R6VnBPYW1ielQ5TXJFd1ZQek14SStCcVlnTXJ2Ym95SVRQR0lSVDJCVDJ0T014WStIa083cWIvZGVwUkhVc1EzemVKMHhtbGMrRVRNaGxGaUpxS1QrSHNPcUJ1NndYMWNZOW96bVZiR0YzZTVNMmF3L0dCU2dQMmRzN0VLNWlONXV5cXZyYnM9PC9EPjwvUlNBS2V5VmFsdWU+",
					WebsiteUrl = "http://dev.cforp.ca/taclef-quiz/",
					SuccessUrl = "http://dev.cforp.ca/taclef-quiz/acces", 
					FailedUrl = "http://dev.cforp.ca/taclef-quiz/aucun-acces", 
					Uuid = Guid.Parse("DE842F46-D174-4F0C-89FB-F5DACEC88477")
				},
				new ConsumerApplication
				{
					Name = "taclef-quiz-localhost",
					DisplayName = "Auto-formation TACLEF (localhost)",
					SecretKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPnYxLzlraWlvREwyWFE0cHd1KzI3UUR2NVpRek5JRVdRWks1Vzk0SjV4aDdyMFdkQ2RzUFJNNldabVpDcFR2clB5OVNQaTVEQlNya0FMWG9pMkkxWmhHM3I4Z3MweEFCcUtCSG1tc0N4eUZPMEVGZmhGSjR5SzBMeGdMRm1vWmROTFpIK2hheUMrNDBNYWUwZkllWVZ2Vk9hY2NpaXJqa0tpWWtqMktRVnhKaz08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPiszUHlrYkFmbnczS0FNRkdPTWVWR09rYWRxQnBKdVBYcWFwWDJ6d0hRZ1h1Ky8vMXk5dlRETmpiN0JCSzFVL1N4bHloV3QrRjZkWEVhMks5Yi85Njh3PT08L1A+PFE+d3RYc2VqWU9jSmkxSlA5eUpaMS82VnpyRHFnczc1R3JJc1RiM1B0QXo5SXY5YWh2a1BQZHJ6K0hXeDdaVlRzeW1kRTNmYmlaWUc5Ri9zbkRMeXpOUXc9PTwvUT48RFA+eUZmQlZNSjI3MHNWUHpTK3RrNk9SS0sxS3Y3enlNQ1ArK1oweVlBeWFEQkNFUllHeUw0RWtaL1cyV2JCdU9NNGxFTUpiRlhiWkV1bGk4bklGWXdBY1E9PTwvRFA+PERRPmNKTkFsd0NpNlVZaTdHUzRwS2xUWGhnMUxQdlpNdmRkQmJMdlFzamVFdlpraGJHei9QSWJOZ1lrQkc5WHJocU5TemtLc2hhMmNIVDY0WlBjdG1aUmx3PT08L0RRPjxJbnZlcnNlUT5BQVVlK09JRThwMTB2VjQ0UjkvdHJBd1VDeGlUenB6SnRXY0wxMHR5cEVqdnB1NWM4S0ZWVmpZZTFGWk9LQnlWT24yYmJTSU1WK3VVUWlsODlLeVFPZz09PC9JbnZlcnNlUT48RD5QTExpOStFNlQvUXdUb1ZSOVdGVlEyM2lUOGtzSklDNzZCc2R6VnBPYW1ielQ5TXJFd1ZQek14SStCcVlnTXJ2Ym95SVRQR0lSVDJCVDJ0T014WStIa083cWIvZGVwUkhVc1EzemVKMHhtbGMrRVRNaGxGaUpxS1QrSHNPcUJ1NndYMWNZOW96bVZiR0YzZTVNMmF3L0dCU2dQMmRzN0VLNWlONXV5cXZyYnM9PC9EPjwvUlNBS2V5VmFsdWU+",
					WebsiteUrl = "http://localhost:20399/",
					SuccessUrl = "http://localhost:20399/acces",
					FailedUrl = "http://localhost:20399/aucun-acces",
					Uuid = Guid.Parse("5B88527F-0ABE-4396-9E79-56EDC546B704")
				}
				);

			context.ApplicationRoles.AddOrUpdate(
				p => p.Name,
				new ApplicationRole { Name = "Admin", DisplayName = "Administrateur du système", Uuid = Guid.Parse("8647E3BE-24D2-4B0B-AFFA-B105242868BF") ,IsStandardRole = false},
				new ApplicationRole { Name = "Teacher", DisplayName = "Enseignant/e", Uuid = Guid.Parse("653E76A9-9F6E-4ADA-BC35-2D1408F9C3C5"),IsStandardRole = true},
				new ApplicationRole { Name = "SchoolAdmin", DisplayName = "Administrateur d'école", Uuid = Guid.Parse("429F3544-40F9-4078-ADA8-4D8EB9070E58"),IsStandardRole =  true},
                new ApplicationRole { Name = "BoardAdmin", DisplayName = "Administrateur de conseil", Uuid = Guid.Parse("7CC265A1-37AF-4590-8688-D665DE131055"), IsStandardRole = true },
                new ApplicationRole { Name = "SuperUser", DisplayName = "Super User", Uuid = Guid.Parse("53C5AEFE-A15B-497F-BBD1-F8B54E8531D9"), IsStandardRole = false },
                new ApplicationRole { Name = "SecurityAdmin", DisplayName = "Administrateur des accès", Uuid = Guid.Parse("B2980E44-7581-4211-A178-B6E7C4CE5780"), IsStandardRole = false }
				);

            context.SaveChanges();

			context.Boards.AddOrUpdate(
				p => p.ShortName,
				new SchoolBoard
				{
					Name = "Conseil Scolaire Catholique Providence",
					ShortName = "CSCP",
					Uuid = Guid.Parse("365D823B-400F-4EFA-B7BD-4D6824CBABC2")
				},
                new SchoolBoard
                {
                    Name = "Conseil Scolaire Viamonde",
                    ShortName = "CSVM",
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new SchoolBoard
                {
                    Name = "Conseil Scolaire de District Catholique Centre Sud",
                    ShortName = "CSDCCS",
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }

                );


            context.SaveChanges();
           
            int schoolNum = 704717;
			context.Schools.AddOrUpdate(
				p => p.ShortName,
				new School
				{
					Name = "École secondaire E.J. Lajeunesse",
					ShortName = "ESJ Lajeunesse",
					MinistryUniqueId = schoolNum++,
					Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSCP"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                },
                new School
                {
                    Name = "École élemenaire Jeane Lajoie",
                    ShortName = "EE J Lajoie",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSVM"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                },new School
                {
                    Name = "École élemenaire La Moraine",
                    ShortName = "EE La Moraine",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSVM"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new School
                {
                    Name = "École élemenaire Laure Rièse",
                    ShortName = "EE L Rièse",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSVM"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new School
                {
                    Name = "École élemenaire La Mosaique",
                    ShortName = "EE Lamosaique",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSVM"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new School
                {
                    Name = "École élemenaire St Jean de Lalonde",
                    ShortName = "EE SJ de Lalonde",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSDCCS"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new School
                {
                    Name = "École élémentaire catholique Ange-Gabriel",
                    ShortName = "EEC Ange-Gabriel",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSDCCS"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new School
                {
                    Name = "École élémentaire catholique Brampton-Ouest",
                    ShortName = "EEC Brampton-Ouest",
                    MinistryUniqueId = schoolNum++,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSDCCS"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                }, new School
                {
                    Name = "École élémentaire catholique Cardinal-Léger",
                    ShortName = "EEC Cardinal-Leger",
                    MinistryUniqueId = schoolNum,
                    Board = context.Boards.FirstOrDefault(b => b.ShortName == "CSDCCS"),
                    Uuid = Guid.Parse(Guid.NewGuid().ToString())
                });

            context.SaveChanges();
            List<string> lastNameList = new List<string>();
            
            lastNameList.Add("Smith");
            lastNameList.Add("Johnson");
            lastNameList.Add("Williams");
            lastNameList.Add("Jones");
            lastNameList.Add("Brown");
            lastNameList.Add("Davis");
            lastNameList.Add("Miller");
            lastNameList.Add("Wilson");
            lastNameList.Add("Moore");
            lastNameList.Add("Taylor");
            lastNameList.Add("Anderson");
            lastNameList.Add("Thomas");
            lastNameList.Add("Jackson");
            lastNameList.Add("White");
            lastNameList.Add("Harris");
            lastNameList.Add("Martin");
            lastNameList.Add("Thompson");
            lastNameList.Add("Garcia");
            lastNameList.Add("Martinez");
            lastNameList.Add("Robinson");
            lastNameList.Add("Clark");
            lastNameList.Add("Rodriguez");
            lastNameList.Add("Lewis");
            lastNameList.Add("Lee");
            lastNameList.Add("Walker");
            lastNameList.Add("Hall");
            lastNameList.Add("Allen");
            lastNameList.Add("Young");
            lastNameList.Add("Hernandez");
            lastNameList.Add("King");
            lastNameList.Add("Wright");
            lastNameList.Add("Lopez");
            lastNameList.Add("Hill");
            lastNameList.Add("Scott");
            lastNameList.Add("Green");
            lastNameList.Add("Adams");
            lastNameList.Add("Baker");
            lastNameList.Add("Gonzalez");
            lastNameList.Add("Nelson");
            lastNameList.Add("Carter");
            lastNameList.Add("Mitchell");
            lastNameList.Add("Perez");
            lastNameList.Add("Roberts");
            lastNameList.Add("Turner");
            lastNameList.Add("Phillips");
            lastNameList.Add("Campbell");
            lastNameList.Add("Parker");
            lastNameList.Add("Evans");
            lastNameList.Add("Edwards");
            lastNameList.Add("Collins");
            lastNameList.Add("Stewart");
            lastNameList.Add("Sanchez");
            lastNameList.Add("Morris");
            lastNameList.Add("Rogers");
            lastNameList.Add("Reed");
            lastNameList.Add("Cook");
            lastNameList.Add("Morgan");
            lastNameList.Add("Bell");
            lastNameList.Add("Murphy");
            lastNameList.Add("Bailey");
            lastNameList.Add("Rivera");
            lastNameList.Add("Cooper");
            lastNameList.Add("Richardson");
            lastNameList.Add("Cox");
            lastNameList.Add("Howard");
            lastNameList.Add("Ward");
            lastNameList.Add("Torres");
            lastNameList.Add("Peterson");
            lastNameList.Add("Gray");
            lastNameList.Add("Ramirez");
            lastNameList.Add("James");
            lastNameList.Add("Watson");
            lastNameList.Add("Brooks");
            lastNameList.Add("Kelly");
            lastNameList.Add("Sanders");
            lastNameList.Add("Price");
            lastNameList.Add("Bennett");
            lastNameList.Add("Wood");
            lastNameList.Add("Barnes");
            lastNameList.Add("Ross");
            lastNameList.Add("Henderson");
            lastNameList.Add("Coleman");
            lastNameList.Add("Jenkins");
            lastNameList.Add("Perry");
            lastNameList.Add("Powell");
            lastNameList.Add("Long");
            lastNameList.Add("Patterson");
            lastNameList.Add("Hughes");
            lastNameList.Add("Flores");
            lastNameList.Add("Washington");
            lastNameList.Add("Butler");
            lastNameList.Add("Simmons");
            lastNameList.Add("Foster");
            lastNameList.Add("Gonzales");
            lastNameList.Add("Bryant");
            lastNameList.Add("Alexander");
            lastNameList.Add("Russell");
            lastNameList.Add("Griffin");
            lastNameList.Add("Diaz");
            lastNameList.Add("Hayes");
            List<string> firstNameList = new List<string>();
            firstNameList.Add("Aiden");
            firstNameList.Add("Jackson");
            firstNameList.Add("Mason");
            firstNameList.Add("Liam");
            firstNameList.Add("Jacob");
            firstNameList.Add("Jayden");
            firstNameList.Add("Ethan");
            firstNameList.Add("Noah");
            firstNameList.Add("Lucas");
            firstNameList.Add("Logan");
            firstNameList.Add("Caleb");
            firstNameList.Add("Caden");
            firstNameList.Add("Jack");
            firstNameList.Add("Ryan");
            firstNameList.Add("Connor");
            firstNameList.Add("Michael");
            firstNameList.Add("Elijah");
            firstNameList.Add("Brayden");
            firstNameList.Add("Benjamin");
            firstNameList.Add("Nicholas");
            firstNameList.Add("Alexander");
            firstNameList.Add("William");
            firstNameList.Add("Matthew");
            firstNameList.Add("James");
            firstNameList.Add("Landon");
            firstNameList.Add("Nathan");
            firstNameList.Add("Dylan");
            firstNameList.Add("Evan");
            firstNameList.Add("Luke");
            firstNameList.Add("Andrew");
            firstNameList.Add("Gabriel");
            firstNameList.Add("Gavin");
            firstNameList.Add("Joshua");
            firstNameList.Add("Owen");
            firstNameList.Add("Daniel");
            firstNameList.Add("Carter");
            firstNameList.Add("Tyler");
            firstNameList.Add("Cameron");
            firstNameList.Add("Christian");
            firstNameList.Add("Wyatt");
            firstNameList.Add("Henry");
            firstNameList.Add("Eli");
            firstNameList.Add("Joseph");
            firstNameList.Add("Max");
            firstNameList.Add("Isaac");
            firstNameList.Add("Samuel");
            firstNameList.Add("Anthony");
            firstNameList.Add("Grayson");
            firstNameList.Add("Zachary");
            firstNameList.Add("David");
            firstNameList.Add("Christopher");
            firstNameList.Add("John");
            firstNameList.Add("Isaiah");
            firstNameList.Add("Levi");
            firstNameList.Add("Jonathan");
            firstNameList.Add("Oliver");
            firstNameList.Add("Chase");
            firstNameList.Add("Cooper");
            firstNameList.Add("Tristan");
            firstNameList.Add("Colton");
            firstNameList.Add("Austin");
            firstNameList.Add("Colin");
            firstNameList.Add("Charlie");
            firstNameList.Add("Dominic");
            firstNameList.Add("Parker");
            firstNameList.Add("Hunter");
            firstNameList.Add("Thomas");
            firstNameList.Add("Alex");
            firstNameList.Add("Ian");
            firstNameList.Add("Jordan");
            firstNameList.Add("Cole");
            firstNameList.Add("Julian");
            firstNameList.Add("Aaron");
            firstNameList.Add("Carson");
            firstNameList.Add("Miles");
            firstNameList.Add("Blake");
            firstNameList.Add("Brody");
            firstNameList.Add("Adam");
            firstNameList.Add("Sebastian");
            firstNameList.Add("Adrian");
            firstNameList.Add("Nolan");
            firstNameList.Add("Sean");
            firstNameList.Add("Riley");
            firstNameList.Add("Bentley");
            firstNameList.Add("Xavier");
            firstNameList.Add("Hayden");
            firstNameList.Add("Jeremiah");
            firstNameList.Add("Jason");
            firstNameList.Add("Jake");
            firstNameList.Add("Asher");
            firstNameList.Add("Micah");
            firstNameList.Add("Jace");
            firstNameList.Add("Brandon");
            firstNameList.Add("Josiah");
            firstNameList.Add("Hudson");
            firstNameList.Add("Nathaniel");
            firstNameList.Add("Bryson");
            firstNameList.Add("Ryder");
            firstNameList.Add("Justin");
            firstNameList.Add("Bryce");

            //—————female

            firstNameList.Add("Sophia");
            firstNameList.Add("Emma");
            firstNameList.Add("Isabella");
            firstNameList.Add("Olivia");
            firstNameList.Add("Ava");
            firstNameList.Add("Lily");
            firstNameList.Add("Chloe");
            firstNameList.Add("Madison");
            firstNameList.Add("Emily");
            firstNameList.Add("Abigail");
            firstNameList.Add("Addison");
            firstNameList.Add("Mia");
            firstNameList.Add("Madelyn");
            firstNameList.Add("Ella");
            firstNameList.Add("Hailey");
            firstNameList.Add("Kaylee");
            firstNameList.Add("Avery");
            firstNameList.Add("Kaitlyn");
            firstNameList.Add("Riley");
            firstNameList.Add("Aubrey");
            firstNameList.Add("Brooklyn");
            firstNameList.Add("Peyton");
            firstNameList.Add("Layla");
            firstNameList.Add("Hannah");
            firstNameList.Add("Charlotte");
            firstNameList.Add("Bella");
            firstNameList.Add("Natalie");
            firstNameList.Add("Sarah");
            firstNameList.Add("Grace");
            firstNameList.Add("Amelia");
            firstNameList.Add("Kylie");
            firstNameList.Add("Arianna");
            firstNameList.Add("Anna");
            firstNameList.Add("Elizabeth");
            firstNameList.Add("Sophie");
            firstNameList.Add("Claire");
            firstNameList.Add("Lila");
            firstNameList.Add("Aaliyah");
            firstNameList.Add("Gabriella");
            firstNameList.Add("Elise");
            firstNameList.Add("Lillian");
            firstNameList.Add("Samantha");
            firstNameList.Add("Makayla");
            firstNameList.Add("Audrey");
            firstNameList.Add("Alyssa");
            firstNameList.Add("Ellie");
            firstNameList.Add("Alexis");
            firstNameList.Add("Isabelle");
            firstNameList.Add("Savannah");
            firstNameList.Add("Evelyn");
            firstNameList.Add("Leah");
            firstNameList.Add("Keira");
            firstNameList.Add("Allison");
            firstNameList.Add("Maya");
            firstNameList.Add("Lucy");
            firstNameList.Add("Sydney");
            firstNameList.Add("Taylor");
            firstNameList.Add("Molly");
            firstNameList.Add("Lauren");
            firstNameList.Add("Harper");
            firstNameList.Add("Scarlett");
            firstNameList.Add("Brianna");
            firstNameList.Add("Victoria");
            firstNameList.Add("Liliana");
            firstNameList.Add("Aria");
            firstNameList.Add("Kayla");
            firstNameList.Add("Annabelle");
            firstNameList.Add("Gianna");
            firstNameList.Add("Kennedy");
            firstNameList.Add("Stella");
            firstNameList.Add("Reagan");
            firstNameList.Add("Julia");
            firstNameList.Add("Bailey");
            firstNameList.Add("Alexandra");
            firstNameList.Add("Jordyn");
            firstNameList.Add("Nora");
            firstNameList.Add("Carolin");
            firstNameList.Add("Mackenzie");
            firstNameList.Add("Jasmine");
            firstNameList.Add("Jocelyn");
            firstNameList.Add("Kendall");
            firstNameList.Add("Morgan");
            firstNameList.Add("Nevaeh");
            firstNameList.Add("Maria");
            firstNameList.Add("Eva");
            firstNameList.Add("Juliana");
            firstNameList.Add("Abby");
            firstNameList.Add("Alexa");
            firstNameList.Add("Summer");
            firstNameList.Add("Brooke");
            firstNameList.Add("Penelope");
            firstNameList.Add("Violet");
            firstNameList.Add("Kate");
            firstNameList.Add("Hadley");
            firstNameList.Add("Ashlyn");
            firstNameList.Add("Sadie");
            firstNameList.Add("Paige");
            firstNameList.Add("Katherine");
            firstNameList.Add("Sienna");
            firstNameList.Add("Piper");

            string firstName,lastName;
            int iFistIndex = 0;
            int iLastIndex = 0;
            for (int i = 0; i < 2000; i++)
            {
                if (iFistIndex < firstNameList.Count)
                {
                    firstName = firstNameList[iFistIndex++];
                    lastName = lastNameList[iLastIndex];
                }
                else
                {
                    iFistIndex = 0;
                    firstName = firstNameList[iFistIndex++];
                    lastName = lastNameList[iLastIndex++];

                }
               
                context.UserLogins.Add(new UserLogin
                {
                    Credentials =
                        Desire2LearnLtiLoginProvider.CreateCredentials(string.Format("{0}.{1}", firstName, lastName)),
                    Id = Guid.NewGuid().ToString(),
                    Provider = context.LoginProviders.FirstOrDefault(),
                    User = new ApplicationUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        EmailAddress = string.Format("{0}.{1}@taclef.on.ca", firstName, lastName),
                        IsDeleted = false,
                        IsLocked = false,
                        Id = Guid.NewGuid().ToString(),
                        Roles = new List<ApplicationRole> { context.ApplicationRoles.Where(r=>r.IsStandardRole).FirstOrDefault()},
                        School =  context.Schools.FirstOrDefault()
                            


                        

                    }

                });
            }
            context.SaveChanges();
            
        }
    }
}
