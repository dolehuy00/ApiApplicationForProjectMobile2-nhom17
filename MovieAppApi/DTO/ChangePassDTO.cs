namespace MovieAppApi.DTO
{
    public class ChangePassDTO
    {
        public ChangePassDTO(string email, string newPassword, string oldPassword, string passwordConfirm)
        {
            Email = email;
            NewPassword = newPassword;
            OldPassword = oldPassword;
            PasswordConfirm = passwordConfirm;
        }

        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
