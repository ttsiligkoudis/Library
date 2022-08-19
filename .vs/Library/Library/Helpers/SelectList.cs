namespace Library.Helpers
{
    public class MySelectList : IEquatable<MySelectList>
    {
        #pragma warning disable

        public MySelectList()
        {
            Disabled = false;
            Selected = false;
        }
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }

        public bool Equals(MySelectList other)
        {
            return Id == other.Id;
        }
        #pragma warning restore

    }
}