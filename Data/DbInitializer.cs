namespace ELib_IDSFintech_Internship.Data
{
    public static class DbInitializer
    {
        public static void Initialize(StudentSystemContext context)
        {
            if (context.Students.Any()
                && context.Locations.Any()
                /*&& context.Tasks.Any()
                && context.SubTasks.Any()*/)
            {
                return;   //DB has been seeded
            }

            var location = new Location
            {
                Id = 1,
                City = "Beirut",
                Country = "Lebanon",
                Address = "First street idk what",
                TimeStamp = DateTime.Now,

            };

            var student = new Student
            {
                FirstName = "James",
                LastName = "AK",
                Email = "user1@example.com",
                Location = location,
                LocationId = location.Id,
                TimeStamp = DateTime.Now,
            };

            location.Students = new List<Student> { student };


            // Add entities to context
            context.Students.Add(student);
            context.Locations.Add(location);


            // Save changes to database
            context.SaveChanges();
        }
    }
}
