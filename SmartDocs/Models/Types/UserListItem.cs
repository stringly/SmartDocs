namespace SmartDocs.Models.Types
{
    public class UserListItem
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }

        public UserListItem()
        {
        }

        public UserListItem(SmartUser user)
        {
            UserId = user.UserId;
            DisplayName = user.DisplayName;
        }
    }
}
