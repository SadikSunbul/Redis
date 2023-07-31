
using StackExchange.Redis;

ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync("localhost:1452");//redise bağlantımız sunucuda sıfre vb seyler var ıse opt=>{} ile içerisinde halledin

ISubscriber subscriber = connection.GetSubscriber();

//bası mychanel sonu onemsız bırsekılde olanlardan okuycaktır verıyı 
await subscriber.SubscribeAsync("mychanel.*", (channel, mesage) =>
{
    Console.WriteLine(mesage);
});

Console.ReadLine();