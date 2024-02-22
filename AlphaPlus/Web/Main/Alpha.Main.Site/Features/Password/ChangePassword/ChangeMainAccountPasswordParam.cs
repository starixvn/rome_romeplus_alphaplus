namespace Alpha.Main.Site.Features.Password
{
    public class ChangeMainAccountPasswordParam : ResetSubAccountPasswordParam
    {
        public string CurrentPassword { get; set; }

        public string NickName { get; set; }
    }
}
