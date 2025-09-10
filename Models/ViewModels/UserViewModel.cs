namespace star_events.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public int LoyaltyPoints { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

