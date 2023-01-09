namespace Library.Helpers
{
    public static class EnumExtensionMethods
    {
        public static List<MySelectList> GetEnumList(int value)
        {
            var enumList = (from d in Enum.GetValues(typeof(Categories)).Cast<Categories>()
                            select new MySelectList
                            {
                                Id = Convert.ToInt16(d).ToString(),
                                Text = d.ToString(),
                                Selected = d.HasFlag(d)
                            }).ToList();
            return enumList;
        }
    }

    public enum Categories
    {
        Action = 1,
        Adventure = 2,
        Classics = 4,
        Mystery = 8,
        Fantasy = 16,
        Historical = 32,
        Horror = 64
    }
    public enum UserType
    {
        Admin = 1,
        User = 2
    }
}
