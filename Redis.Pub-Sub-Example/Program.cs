

using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1452");//redise bağlantımız sunucuda sıfre vb seyler var ıse opt=>{} ile içerisinde halledin

ISubscriber subscriber = connection.GetSubscriber();

while (true)
{
    Console.Write("Message: ");
    string message = Console.ReadLine();
    await subscriber.PublishAsync("mychanel", message);
}
