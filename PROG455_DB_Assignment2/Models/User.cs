namespace PROG455_DB_Assignment2.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Location { get; set; }

        List<User>? Friends { get; set; }

        public User() { }

        public User(string name, string password)
        {
            ID = GUID();
            Name = name;
            Password = password;
            Location = "";
        }

        public User(int id, string name, string password, string location)
        {
            ID = id;
            Name = name;
            Password = password;
            Location = location;
        }

        int GUID()
        {
            // Generate a new GUID
            Guid guid = Guid.NewGuid();

            // Convert GUID to string and remove non-numeric characters
            string guidString = guid.ToString().Replace("-", "");
            string numericOnly = string.Empty;
            foreach (char c in guidString)
            {
                if (char.IsDigit(c))
                {
                    numericOnly += c;
                }
            }

            // Parse the numeric string to an integer
            char[] arr = numericOnly.Take(10).ToArray();
            string trimmed = new(arr);
            int num = Int32.Parse(trimmed);

            return num;
        }


        public void SetFriends(List<User> friends) => Friends = friends;
    }
}
