public class Program {
    public static void Main(string[] args) {
        int orderId = 0;
        bool isValidOrderId = false;
        int count = 1;
        while (!isValidOrderId || orderId <= 0)
        {
            if (count > 1)
            {
                Console.WriteLine("Invalid!");
            }
            Console.Write("Enter Order Id : ");
            isValidOrderId = int.TryParse(Console.ReadLine(), out orderId);
            count++;
        }


        string customerName = null;
        count = 1;
        while (customerName.IsWhiteSpace())
        {
            if (count > 1)
                Console.WriteLine("Invalid Customer Name");
            Console.Write("Enter Customer Name : ");
            customerName = Console.ReadLine();
            count++;
        }

        string productName = null;
        count = 1;
        while (productName.IsWhiteSpace())
        {
            if (count > 1)
                Console.WriteLine("Invalid Product Name");
            Console.Write("Enter Product Name : ");
            productName = Console.ReadLine();
            count++;
        }


        int quantity = 0;
        bool isValidQuantity = false;
        count = 1;
        while (!isValidQuantity || quantity <= 0)
        {
            if (count > 1)
                Console.WriteLine("Invalid quantity");
            Console.Write("Enter Quantity : ");
            isValidQuantity = int.TryParse(Console.ReadLine(), out quantity);
            count++;
        }

        decimal price = 0;
        bool isValidPrice = false;
        count= 1;
        while (!isValidPrice || price <= 0)
        {
            if (count > 1)
                Console.WriteLine("Invalid Price");
            Console.Write("Enter Price : ");
            isValidPrice = decimal.TryParse(Console.ReadLine(), out price);
            count++;
        }


        //Subtotal
        decimal subTotal = price * quantity;

        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("=======ORDER SUMMARY=======");
        Console.WriteLine("Order ID: " + orderId);
        Console.WriteLine("Customer Name: " + customerName);
        Console.WriteLine("Product Name: " + productName);
        Console.WriteLine("Quantity: " + quantity);
        Console.WriteLine("Unit Price: " + $"{price:F2}");
        Console.WriteLine("Subtotal: " + $"{subTotal:F2}");

        //order classification
        if (subTotal < 1000)
        {
            Console.Write("Order Classification: Small Order");
        }
        else if (subTotal < 5000)
        {
            Console.Write("Order Classification: Medium Order");
        }
        else if (subTotal < 10000)
        {
            Console.Write("Order Classification: Large Order");
        }
        else
        {
            Console.Write("Order Classification: Bulk Order");
        }

    }
}