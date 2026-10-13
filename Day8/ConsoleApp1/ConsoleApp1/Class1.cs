namespace classExample
{
    class Product
    {
        int Id { get; set; }

        public string name;

        public string Name
        {
            get; set
            {
                Name = value;
            }
        }
    }
}